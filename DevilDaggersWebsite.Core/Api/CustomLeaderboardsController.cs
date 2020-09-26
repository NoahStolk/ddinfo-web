using DevilDaggersCore.Extensions;
using DevilDaggersCore.Spawnsets;
using DevilDaggersWebsite.Core.Clients;
using DevilDaggersWebsite.Core.Entities;
using DevilDaggersWebsite.Core.Tools;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using BotLogger = DiscordBotDdInfo.DiscordLogger;

namespace DevilDaggersWebsite.Core.Api
{
	[Route("api/custom-leaderboards")]
	[ApiController]
	public class CustomLeaderboardsController : ControllerBase
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly IWebHostEnvironment _env;

		private readonly Dictionary<int, string> _usernames;

		public CustomLeaderboardsController(ApplicationDbContext dbContext, IWebHostEnvironment env)
		{
			_dbContext = dbContext;
			_env = env;

			_usernames = dbContext.Players.Select(p => new KeyValuePair<int, string>(p.Id, p.Username)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<List<Dto.CustomLeaderboard>> GetCustomLeaderboards()
			=> _dbContext.CustomLeaderboards
				.Select(cl => new Dto.CustomLeaderboard
				{
					SpawnsetAuthorName = cl.SpawnsetFile.Player.Username,
					SpawnsetName = cl.SpawnsetFile.Name,
					Bronze = cl.Bronze,
					Silver = cl.Silver,
					Golden = cl.Golden,
					Devil = cl.Devil,
					Homing = cl.Homing,
					DateLastPlayed = cl.DateLastPlayed,
					DateCreated = cl.DateCreated,
				})
				.ToList();

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<Dto.UploadSuccess>> UploadScore([FromBody] Dto.UploadRequest uploadRequest)
		{
			try
			{
				return await ProcessUploadRequest(uploadRequest, GetSpawnsets());
			}
			catch (Exception ex)
			{
				await BotLogger.Instance.TryLogException($"Upload failed for user `{uploadRequest.Username}` (`{uploadRequest.PlayerId}`) for `{GetSpawnsetNameOrHash(uploadRequest, null)}`.", ex);
				throw;
			}
		}

		private IEnumerable<(string name, Spawnset spawnset)> GetSpawnsets()
		{
			foreach (string spawnsetPath in Directory.GetFiles(Path.Combine(_env.WebRootPath, "spawnsets")))
			{
				if (Spawnset.TryParse(System.IO.File.ReadAllBytes(spawnsetPath), out Spawnset spawnset))
					yield return (Path.GetFileName(spawnsetPath), spawnset);
			}
		}

		[ApiExplorerSettings(IgnoreApi = true)]
		[NonAction]
		public async Task<ActionResult<Dto.UploadSuccess>> ProcessUploadRequest(Dto.UploadRequest uploadRequest, IEnumerable<(string name, Spawnset spawnset)> spawnsets)
		{
			Version clientVersionParsed = Version.Parse(uploadRequest.DdclClientVersion);
			if (clientVersionParsed < ToolList.DevilDaggersCustomLeaderboards.VersionNumberRequired)
			{
				string errorMessage = "You are using an unsupported and outdated version of DDCL. Please update the program.";
				await TryLog(uploadRequest, null, errorMessage);
				return new BadRequestObjectResult(new ProblemDetails { Title = errorMessage });
			}

			string spawnsetName = string.Empty;
			foreach ((string name, Spawnset spawnset) in spawnsets)
			{
				if (spawnset.GetHashString() == uploadRequest.SpawnsetHash)
				{
					spawnsetName = name;
					break;
				}
			}

			if (string.IsNullOrEmpty(spawnsetName))
			{
				string errorMessage = "This spawnset does not exist on DevilDaggers.info.";
				await TryLog(uploadRequest, spawnsetName, errorMessage);
				return new BadRequestObjectResult(new ProblemDetails { Title = errorMessage });
			}

			string check = string.Join(
				";",
				uploadRequest.PlayerId,
				uploadRequest.Time,
				uploadRequest.Gems,
				uploadRequest.Kills,
				uploadRequest.DeathType,
				uploadRequest.DaggersHit,
				uploadRequest.DaggersFired,
				uploadRequest.EnemiesAlive,
				uploadRequest.Homing,
				string.Join(",", new int[3] { uploadRequest.LevelUpTime2, uploadRequest.LevelUpTime3, uploadRequest.LevelUpTime4 }));
			if (await DecryptValidation(uploadRequest.Validation) != check)
			{
				string errorMessage = "Invalid submission.";
				await TryLog(uploadRequest, spawnsetName, errorMessage);
				return new BadRequestObjectResult(new ProblemDetails { Title = errorMessage });
			}

			CustomLeaderboard leaderboard = _dbContext.CustomLeaderboards.Include(cl => cl.Category).Include(cl => cl.SpawnsetFile).ThenInclude(sf => sf.Player).FirstOrDefault(cl => cl.SpawnsetFile.Name == spawnsetName);
			if (leaderboard == null)
			{
				string errorMessage = "This spawnset exists on DevilDaggers.info, but doesn't have a leaderboard.";
				await TryLog(uploadRequest, spawnsetName, errorMessage);
				return new BadRequestObjectResult(new ProblemDetails { Title = errorMessage });
			}

			// At this point, the submission is accepted.

			// Add the player or update the username.
			Player player = _dbContext.Players.FirstOrDefault(p => p.Id == uploadRequest.PlayerId);
			if (player == null)
			{
				player = new Player
				{
					Id = uploadRequest.PlayerId,
					Username = await GetUsername(uploadRequest),
				};
				_dbContext.Players.Add(player);
			}
			else
			{
				player.Username = await GetUsername(uploadRequest);
			}

			// Update the date this leaderboard was submitted to.
			leaderboard.DateLastPlayed = DateTime.Now;
			leaderboard.TotalRunsSubmitted++;

			// Calculate the new rank.
			IEnumerable<CustomEntry> entries = _dbContext.CustomEntries.Where(e => e.CustomLeaderboard == leaderboard).OrderByMember(leaderboard.Category.SortingPropertyName, leaderboard.Category.Ascending).ToArray();

			// TODO: Use reflection to use Category.SortingPropertyName.
			int rank = leaderboard.Category.Ascending ? entries.Count(e => e.Time < uploadRequest.Time) + 1 : entries.Count(e => e.Time > uploadRequest.Time) + 1;
			int totalPlayers = entries.Count();

			CustomEntry? entry = _dbContext.CustomEntries.FirstOrDefault(e => e.PlayerId == uploadRequest.PlayerId && e.CustomLeaderboardId == leaderboard.Id);
			if (entry == null)
			{
				// Add new user to this leaderboard.
				_dbContext.CustomEntries.Add(new CustomEntry
				{
					PlayerId = uploadRequest.PlayerId,
					Time = uploadRequest.Time,
					Gems = uploadRequest.Gems,
					Kills = uploadRequest.Kills,
					DeathType = uploadRequest.DeathType,
					DaggersHit = uploadRequest.DaggersHit,
					DaggersFired = uploadRequest.DaggersFired,
					EnemiesAlive = uploadRequest.EnemiesAlive,
					Homing = uploadRequest.Homing,
					LevelUpTime2 = uploadRequest.LevelUpTime2,
					LevelUpTime3 = uploadRequest.LevelUpTime3,
					LevelUpTime4 = uploadRequest.LevelUpTime4,
					SubmitDate = DateTime.Now,
					ClientVersion = uploadRequest.DdclClientVersion,
					CustomLeaderboard = leaderboard,
					GemsData = string.Join(",", uploadRequest.GameStates.Select(gs => gs.Gems)),
					KillsData = string.Join(",", uploadRequest.GameStates.Select(gs => gs.Kills)),
					HomingData = string.Join(",", uploadRequest.GameStates.Select(gs => gs.Homing)),
					EnemiesAliveData = string.Join(",", uploadRequest.GameStates.Select(gs => gs.EnemiesAlive)),
					DaggersFiredData = string.Join(",", uploadRequest.GameStates.Select(gs => gs.DaggersFired)),
					DaggersHitData = string.Join(",", uploadRequest.GameStates.Select(gs => gs.DaggersHit)),
				});
				_dbContext.SaveChanges();

				// Fetch the entries again after having modified the leaderboard.
				entries = _dbContext.CustomEntries.Where(e => e.CustomLeaderboard == leaderboard).OrderByMember(leaderboard.Category.SortingPropertyName, leaderboard.Category.Ascending);
				totalPlayers = entries.Count();

				await TryLog(uploadRequest, spawnsetName);
				return new Dto.UploadSuccess
				{
					Message = $"Welcome to the leaderboard for {spawnsetName}.",
					TotalPlayers = totalPlayers,
					Leaderboard = new Dto.CustomLeaderboard
					{
						SpawnsetName = spawnsetName,
						SpawnsetAuthorName = leaderboard.SpawnsetFile.Player.Username,
						Bronze = leaderboard.Bronze,
						Silver = leaderboard.Silver,
						Golden = leaderboard.Golden,
						Devil = leaderboard.Devil,
						Homing = leaderboard.Homing,
						DateCreated = leaderboard.DateCreated,
						DateLastPlayed = leaderboard.DateLastPlayed,
					},
					Category = new Dto.CustomLeaderboardCategory
					{
						Ascending = leaderboard.Category.Ascending,
						LayoutPartialName = leaderboard.Category.LayoutPartialName,
						Name = leaderboard.Category.Name,
						SortingPropertyName = leaderboard.Category.SortingPropertyName,
					},
					Entries = entries
						.Select(e => new Dto.CustomEntry
						{
							PlayerId = e.PlayerId,
							Username = _usernames.FirstOrDefault(u => u.Key == e.PlayerId).Value ?? "[Player not found]",
							ClientVersion = e.ClientVersion,
							DeathType = e.DeathType,
							EnemiesAlive = e.EnemiesAlive,
							Gems = e.Gems,
							Homing = e.Homing,
							Kills = e.Kills,
							LevelUpTime2 = e.LevelUpTime2,
							LevelUpTime3 = e.LevelUpTime3,
							LevelUpTime4 = e.LevelUpTime4,
							DaggersFired = e.DaggersFired,
							DaggersHit = e.DaggersHit,
							SubmitDate = e.SubmitDate,
							Time = e.Time,
						})
						.ToList(),
					IsNewUserOnThisLeaderboard = true,
					Rank = rank,
					Time = uploadRequest.Time,
					Kills = uploadRequest.Kills,
					Gems = uploadRequest.Gems,
					DaggersHit = uploadRequest.DaggersHit,
					DaggersFired = uploadRequest.DaggersFired,
					EnemiesAlive = uploadRequest.EnemiesAlive,
					Homing = uploadRequest.Homing,
					LevelUpTime2 = uploadRequest.LevelUpTime2,
					LevelUpTime3 = uploadRequest.LevelUpTime3,
					LevelUpTime4 = uploadRequest.LevelUpTime4,
				};
			}

			// User is already on the leaderboard, but did not get a better score.
			// TODO: Use reflection to use Category.SortingPropertyName.
			if (leaderboard.Category.Ascending && entry.Time <= uploadRequest.Time || !leaderboard.Category.Ascending && entry.Time >= uploadRequest.Time)
			{
				_dbContext.SaveChanges();

				// Fetch the entries again after having modified the leaderboard.
				entries = _dbContext.CustomEntries.Where(e => e.CustomLeaderboard == leaderboard).OrderByMember(leaderboard.Category.SortingPropertyName, leaderboard.Category.Ascending).ToArray();

				await TryLog(uploadRequest, spawnsetName);
				return new Dto.UploadSuccess
				{
					Message = $"No new highscore for {leaderboard.SpawnsetFile.Name}.",
					TotalPlayers = totalPlayers,
					Leaderboard = new Dto.CustomLeaderboard
					{
						SpawnsetName = leaderboard.SpawnsetFile.Name,
						SpawnsetAuthorName = leaderboard.SpawnsetFile.Player.Username,
						Bronze = leaderboard.Bronze,
						Silver = leaderboard.Silver,
						Golden = leaderboard.Golden,
						Devil = leaderboard.Devil,
						Homing = leaderboard.Homing,
						DateCreated = leaderboard.DateCreated,
						DateLastPlayed = leaderboard.DateLastPlayed,
					},
					Category = new Dto.CustomLeaderboardCategory
					{
						Ascending = leaderboard.Category.Ascending,
						LayoutPartialName = leaderboard.Category.LayoutPartialName,
						Name = leaderboard.Category.Name,
						SortingPropertyName = leaderboard.Category.SortingPropertyName,
					},
					Entries = entries
						.Select(e => new Dto.CustomEntry
						{
							PlayerId = e.PlayerId,
							Username = _usernames.FirstOrDefault(u => u.Key == e.PlayerId).Value ?? "[Player not found]",
							ClientVersion = e.ClientVersion,
							DeathType = e.DeathType,
							EnemiesAlive = e.EnemiesAlive,
							Gems = e.Gems,
							Homing = e.Homing,
							Kills = e.Kills,
							LevelUpTime2 = e.LevelUpTime2,
							LevelUpTime3 = e.LevelUpTime3,
							LevelUpTime4 = e.LevelUpTime4,
							DaggersFired = e.DaggersFired,
							DaggersHit = e.DaggersHit,
							SubmitDate = e.SubmitDate,
							Time = e.Time,
						})
						.ToList(),
					IsNewUserOnThisLeaderboard = false,
				};
			}

			// User got a better score.

			// Calculate the old rank.
			int oldRank = leaderboard.Category.Ascending ? entries.Count(e => e.Time < entry.Time) + 1 : entries.Count(e => e.Time > entry.Time) + 1;

			int rankDiff = oldRank - rank;
			int timeDiff = uploadRequest.Time - entry.Time;
			int killsDiff = uploadRequest.Kills - entry.Kills;
			int gemsDiff = uploadRequest.Gems - entry.Gems;
			int shotsHitDiff = uploadRequest.DaggersHit - entry.DaggersHit;
			int shotsFiredDiff = uploadRequest.DaggersFired - entry.DaggersFired;
			int enemiesAliveDiff = uploadRequest.EnemiesAlive - entry.EnemiesAlive;
			int homingDiff = uploadRequest.Homing - entry.Homing;
			int levelUpTime2Diff = uploadRequest.LevelUpTime2 - entry.LevelUpTime2;
			int levelUpTime3Diff = uploadRequest.LevelUpTime3 - entry.LevelUpTime3;
			int levelUpTime4Diff = uploadRequest.LevelUpTime4 - entry.LevelUpTime4;

			entry.Time = uploadRequest.Time;
			entry.Kills = uploadRequest.Kills;
			entry.Gems = uploadRequest.Gems;
			entry.DeathType = uploadRequest.DeathType;
			entry.DaggersHit = uploadRequest.DaggersHit;
			entry.DaggersFired = uploadRequest.DaggersFired;
			entry.EnemiesAlive = uploadRequest.EnemiesAlive;
			entry.Homing = uploadRequest.Homing;
			entry.LevelUpTime2 = uploadRequest.LevelUpTime2;
			entry.LevelUpTime3 = uploadRequest.LevelUpTime3;
			entry.LevelUpTime4 = uploadRequest.LevelUpTime4;
			entry.SubmitDate = DateTime.Now;
			entry.ClientVersion = uploadRequest.DdclClientVersion;
			entry.GemsData = string.Join(",", uploadRequest.GameStates.Select(gs => gs.Gems));
			entry.KillsData = string.Join(",", uploadRequest.GameStates.Select(gs => gs.Kills));
			entry.HomingData = string.Join(",", uploadRequest.GameStates.Select(gs => gs.Homing));
			entry.EnemiesAliveData = string.Join(",", uploadRequest.GameStates.Select(gs => gs.EnemiesAlive));
			entry.DaggersFiredData = string.Join(",", uploadRequest.GameStates.Select(gs => gs.DaggersFired));
			entry.DaggersHitData = string.Join(",", uploadRequest.GameStates.Select(gs => gs.DaggersHit));

			_dbContext.SaveChanges();

			// Fetch the entries again after having modified the leaderboard.
			entries = _dbContext.CustomEntries.Where(e => e.CustomLeaderboard == leaderboard).OrderByMember(leaderboard.Category.SortingPropertyName, leaderboard.Category.Ascending).ToArray();

			await TryLog(uploadRequest, spawnsetName);
			return new Dto.UploadSuccess
			{
				Message = $"NEW HIGHSCORE for {leaderboard.SpawnsetFile.Name}!",
				TotalPlayers = totalPlayers,
				Leaderboard = new Dto.CustomLeaderboard
				{
					SpawnsetName = leaderboard.SpawnsetFile.Name,
					SpawnsetAuthorName = leaderboard.SpawnsetFile.Player.Username,
					Bronze = leaderboard.Bronze,
					Silver = leaderboard.Silver,
					Golden = leaderboard.Golden,
					Devil = leaderboard.Devil,
					Homing = leaderboard.Homing,
					DateCreated = leaderboard.DateCreated,
					DateLastPlayed = leaderboard.DateLastPlayed,
				},
				Category = new Dto.CustomLeaderboardCategory
				{
					Ascending = leaderboard.Category.Ascending,
					LayoutPartialName = leaderboard.Category.LayoutPartialName,
					Name = leaderboard.Category.Name,
					SortingPropertyName = leaderboard.Category.SortingPropertyName,
				},
				Entries = entries
					.Select(e => new Dto.CustomEntry
					{
						PlayerId = e.PlayerId,
						Username = _usernames.FirstOrDefault(u => u.Key == e.PlayerId).Value ?? "[Player not found]",
						ClientVersion = e.ClientVersion,
						DeathType = e.DeathType,
						EnemiesAlive = e.EnemiesAlive,
						Gems = e.Gems,
						Homing = e.Homing,
						Kills = e.Kills,
						LevelUpTime2 = e.LevelUpTime2,
						LevelUpTime3 = e.LevelUpTime3,
						LevelUpTime4 = e.LevelUpTime4,
						DaggersFired = e.DaggersFired,
						DaggersHit = e.DaggersHit,
						SubmitDate = e.SubmitDate,
						Time = e.Time,
					})
					.ToList(),
				IsNewUserOnThisLeaderboard = false,
				Rank = rank,
				RankDiff = rankDiff,
				Time = uploadRequest.Time,
				TimeDiff = timeDiff,
				Kills = uploadRequest.Kills,
				KillsDiff = killsDiff,
				Gems = uploadRequest.Gems,
				GemsDiff = gemsDiff,
				DaggersHit = uploadRequest.DaggersHit,
				DaggersHitDiff = shotsHitDiff,
				DaggersFired = uploadRequest.DaggersFired,
				DaggersFiredDiff = shotsFiredDiff,
				EnemiesAlive = uploadRequest.EnemiesAlive,
				EnemiesAliveDiff = enemiesAliveDiff,
				Homing = uploadRequest.Homing,
				HomingDiff = homingDiff,
				LevelUpTime2 = uploadRequest.LevelUpTime2,
				LevelUpTime2Diff = levelUpTime2Diff,
				LevelUpTime3 = uploadRequest.LevelUpTime3,
				LevelUpTime3Diff = levelUpTime3Diff,
				LevelUpTime4 = uploadRequest.LevelUpTime4,
				LevelUpTime4Diff = levelUpTime4Diff,
			};
		}

		private static string GetSpawnsetNameOrHash(Dto.UploadRequest uploadRequest, string? spawnsetName)
			=> string.IsNullOrEmpty(spawnsetName) ? uploadRequest.SpawnsetHash : spawnsetName;

		private static async Task<string> GetUsername(Dto.UploadRequest uploadRequest)
		{
			if (uploadRequest.Username.EndsWith("med fragger", StringComparison.InvariantCulture))
				return (await DdHasmodaiClient.GetUserById(uploadRequest.PlayerId))?.Username ?? uploadRequest.Username;
			return uploadRequest.Username;
		}

		private static async Task<string> DecryptValidation(string validation)
		{
			try
			{
				return Secrets.EncryptionWrapper.DecodeAndDecrypt(HttpUtility.HtmlDecode(validation));
			}
			catch (Exception ex)
			{
				await BotLogger.Instance.TryLogException($"Could not decrypt validation: `{validation}`", ex);

				return string.Empty;
			}
		}

		private static async Task TryLog(Dto.UploadRequest uploadRequest, string? spawnsetName, string? errorMessage = null)
		{
			try
			{
				string spawnsetIdentification = GetSpawnsetNameOrHash(uploadRequest, spawnsetName);

				if (!string.IsNullOrEmpty(errorMessage))
					await BotLogger.Instance.TryLog($"Upload failed for user `{uploadRequest.Username}` (`{uploadRequest.PlayerId}`) for `{spawnsetIdentification}`.\n{errorMessage}");
				else
					await BotLogger.Instance.TryLog($"`{uploadRequest.Username}` just submitted a score of `{uploadRequest.Time / 10000f:0.0000}` to `{spawnsetIdentification}`.");
			}
			catch
			{
				// Ignore exceptions that occurred while attempting to log.
			}
		}
	}
}