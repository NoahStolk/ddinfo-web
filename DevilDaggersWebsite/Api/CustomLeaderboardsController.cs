using DevilDaggersCore.Extensions;
using DevilDaggersCore.Spawnsets;
using DevilDaggersWebsite.Clients;
using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Extensions;
using DevilDaggersWebsite.Transients;
using DiscordBotDdInfo.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using BotLogger = DiscordBotDdInfo.Logging.DiscordLogger;

namespace DevilDaggersWebsite.Api
{
	[Route("api/custom-leaderboards")]
	[ApiController]
	public class CustomLeaderboardsController : ControllerBase
	{
		private static readonly bool _enableData;

		private readonly ApplicationDbContext _dbContext;
		private readonly IWebHostEnvironment _env;
		private readonly ToolHelper _toolHelper;

		private readonly Dictionary<int, string> _usernames;

		public CustomLeaderboardsController(ApplicationDbContext dbContext, IWebHostEnvironment env, ToolHelper toolHelper)
		{
			_dbContext = dbContext;
			_env = env;
			_toolHelper = toolHelper;

			_usernames = dbContext.Players.Select(p => new KeyValuePair<int, string>(p.Id, p.PlayerName)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<List<Dto.CustomLeaderboard>> GetCustomLeaderboards()
		{
			return _dbContext.CustomLeaderboards
				.AsNoTracking()
				.Include(cl => cl.SpawnsetFile)
					.ThenInclude(sf => sf.Player)
				.Select(cl => cl.ToDto())
				.ToList();
		}

		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<Dto.CustomLeaderboard> GetCustomLeaderboard(int id)
		{
			CustomLeaderboard? customLeaderboard = _dbContext.CustomLeaderboards
				.AsNoTracking()
				.Include(cl => cl.SpawnsetFile)
					.ThenInclude(sf => sf.Player)
				.FirstOrDefault(cl => cl.Id == id);

			if (customLeaderboard == null)
				return new NotFoundObjectResult(new ProblemDetails { Title = $"Leaderboard with {nameof(id)} '{id}' was not found." });

			return customLeaderboard.ToDto();
		}

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
				ex.Data[nameof(uploadRequest.ClientVersion)] = uploadRequest.ClientVersion;
				ex.Data[nameof(uploadRequest.OperatingSystem)] = uploadRequest.OperatingSystem;
				ex.Data[nameof(uploadRequest.BuildMode)] = uploadRequest.BuildMode;
				await BotLogger.Instance.TryLogException($"Upload failed for user `{uploadRequest.PlayerName}` (`{uploadRequest.PlayerId}`) for `{GetSpawnsetNameOrHash(uploadRequest, null)}`.", ex);
				throw;
			}

			IEnumerable<(string Name, Spawnset Spawnset)> GetSpawnsets()
			{
				foreach (string spawnsetPath in Directory.GetFiles(Path.Combine(_env.WebRootPath, "spawnsets")))
				{
					if (Spawnset.TryParse(System.IO.File.ReadAllBytes(spawnsetPath), out Spawnset spawnset))
						yield return (Path.GetFileName(spawnsetPath), spawnset);
				}
			}
		}

		[ApiExplorerSettings(IgnoreApi = true)]
		[NonAction]
		public async Task<ActionResult<Dto.UploadSuccess>> ProcessUploadRequest(Dto.UploadRequest uploadRequest, IEnumerable<(string Name, Spawnset Spawnset)> spawnsets)
		{
			Version clientVersionParsed = Version.Parse(uploadRequest.ClientVersion);

			if (clientVersionParsed < _toolHelper.GetToolByName("DevilDaggersCustomLeaderboards").VersionNumberRequired)
			{
				const string errorMessage = "You are using an unsupported and outdated version of DDCL. Please update the program.";
				await TryLog(uploadRequest, null, errorMessage);
				return new BadRequestObjectResult(new ProblemDetails { Title = errorMessage });
			}

			string spawnsetName = string.Empty;
			foreach ((string name, Spawnset spawnset) in spawnsets)
			{
				// TODO: Use cache.
				if (!spawnset.TryGetBytes(out byte[] bytes))
					throw new("Could not get bytes from spawnset.");

				byte[] spawnsetHash = MD5.HashData(bytes);
				bool found = true;
				for (int i = 0; i < 16; i++)
				{
					if (spawnsetHash[i] != uploadRequest.SurvivalHashMd5[i])
					{
						found = false;
						break;
					}
				}

				if (found)
				{
					spawnsetName = name;
					break;
				}
			}

			if (string.IsNullOrEmpty(spawnsetName))
			{
				const string errorMessage = "This spawnset does not exist on DevilDaggers.info.";
				await TryLog(uploadRequest, spawnsetName, errorMessage);
				return new BadRequestObjectResult(new ProblemDetails { Title = errorMessage });
			}

			string check = string.Join(
				";",
				uploadRequest.PlayerId,
				uploadRequest.Time,
				uploadRequest.GemsCollected,
				uploadRequest.GemsDespawned,
				uploadRequest.GemsEaten,
				uploadRequest.GemsTotal,
				uploadRequest.EnemiesKilled,
				uploadRequest.DeathType,
				uploadRequest.DaggersHit,
				uploadRequest.DaggersFired,
				uploadRequest.EnemiesAlive,
				uploadRequest.HomingDaggers,
				ByteArrayToHexString(uploadRequest.SurvivalHashMd5),
				string.Join(",", new int[3] { uploadRequest.LevelUpTime2, uploadRequest.LevelUpTime3, uploadRequest.LevelUpTime4 }));
			if (await DecryptValidation(uploadRequest.Validation) != check)
			{
				const string errorMessage = "Invalid submission.";
				await TryLog(uploadRequest, spawnsetName, errorMessage);
				return new BadRequestObjectResult(new ProblemDetails { Title = errorMessage });
			}

			CustomLeaderboard? customLeaderboard = _dbContext.CustomLeaderboards.Include(cl => cl.SpawnsetFile).ThenInclude(sf => sf.Player).FirstOrDefault(cl => cl.SpawnsetFile.Name == spawnsetName);
			if (customLeaderboard == null)
			{
				const string errorMessage = "This spawnset exists on DevilDaggers.info, but doesn't have a leaderboard.";
				await TryLog(uploadRequest, spawnsetName, errorMessage);
				return new BadRequestObjectResult(new ProblemDetails { Title = errorMessage });
			}

			if (customLeaderboard.Category == Enumerators.CustomLeaderboardCategory.Challenge && Version.Parse(uploadRequest.ClientVersion) <= new Version(0, 14, 1, 0))
			{
				const string errorMessage = "Challenge leaderboards are not supported on version 0.14.1.0 or lower.";
				await TryLog(uploadRequest, spawnsetName, errorMessage);
				return new BadRequestObjectResult(new ProblemDetails { Title = errorMessage });
			}

			// At this point, the submission is accepted.

			// Add the player or update the username.
			Player? player = _dbContext.Players.FirstOrDefault(p => p.Id == uploadRequest.PlayerId);
			if (player == null)
			{
				player = new Player
				{
					Id = uploadRequest.PlayerId,
					PlayerName = await GetUsername(uploadRequest),
				};
				_dbContext.Players.Add(player);
			}
			else
			{
				player.PlayerName = await GetUsername(uploadRequest);
			}

			// Update the date this leaderboard was submitted to.
			customLeaderboard.DateLastPlayed = DateTime.Now;
			customLeaderboard.TotalRunsSubmitted++;

			// Calculate the new rank.
			IEnumerable<CustomEntry> entries = _dbContext.CustomEntries.Where(e => e.CustomLeaderboard == customLeaderboard).OrderByMember(nameof(CustomEntry.Time), customLeaderboard.IsAscending()).ToArray();

			int rank = customLeaderboard.IsAscending() ? entries.Count(e => e.Time < uploadRequest.Time) + 1 : entries.Count(e => e.Time > uploadRequest.Time) + 1;
			int totalPlayers = entries.Count();

			CustomEntry? customEntry = _dbContext.CustomEntries.FirstOrDefault(e => e.PlayerId == uploadRequest.PlayerId && e.CustomLeaderboardId == customLeaderboard.Id);
			if (customEntry == null)
			{
				// Add new custom entry to this leaderboard.
				CustomEntry newCustomEntry = uploadRequest.ToCustomEntryEntity(customLeaderboard);
				_dbContext.CustomEntries.Add(newCustomEntry);

				if (_enableData)
				{
					CustomEntryData newCustomEntryData = new() { CustomEntryId = newCustomEntry.Id, };
					newCustomEntryData.Populate(uploadRequest.GameStates);
					_dbContext.CustomEntryData.Add(newCustomEntryData);
				}

				_dbContext.SaveChanges();

				// Fetch the entries again after having modified the leaderboard.
				entries = _dbContext.CustomEntries.Where(e => e.CustomLeaderboard == customLeaderboard).OrderByMember(nameof(CustomEntry.Time), customLeaderboard.IsAscending());
				totalPlayers = entries.Count();

				await TryLog(uploadRequest, spawnsetName);
				return new Dto.UploadSuccess
				{
					Message = $"Welcome to the leaderboard for {spawnsetName}.",
					TotalPlayers = totalPlayers,
					Leaderboard = customLeaderboard.ToDto(),
					Category = customLeaderboard.Category,
					Entries = entries
						.Select(e => e.ToDto(GetUsernameFromCache(e)))
						.ToList(),
					IsNewPlayerOnThisLeaderboard = true,
					Rank = rank,
					Time = uploadRequest.Time,
					EnemiesKilled = uploadRequest.EnemiesKilled,
					GemsCollected = uploadRequest.GemsCollected,
					GemsDespawned = uploadRequest.GemsDespawned,
					GemsEaten = uploadRequest.GemsEaten,
					DaggersHit = uploadRequest.DaggersHit,
					DaggersFired = uploadRequest.DaggersFired,
					EnemiesAlive = uploadRequest.EnemiesAlive,
					HomingDaggers = uploadRequest.HomingDaggers,
					LevelUpTime2 = uploadRequest.LevelUpTime2,
					LevelUpTime3 = uploadRequest.LevelUpTime3,
					LevelUpTime4 = uploadRequest.LevelUpTime4,
				};
			}

			// User is already on the leaderboard, but did not get a better score.
			if (customLeaderboard.IsAscending() && customEntry.Time <= uploadRequest.Time || !customLeaderboard.IsAscending() && customEntry.Time >= uploadRequest.Time)
			{
				_dbContext.SaveChanges();

				// Fetch the entries again after having modified the leaderboard.
				entries = _dbContext.CustomEntries.Where(e => e.CustomLeaderboard == customLeaderboard).OrderByMember(nameof(CustomEntry.Time), customLeaderboard.IsAscending()).ToArray();

				await TryLog(uploadRequest, spawnsetName);
				return new Dto.UploadSuccess
				{
					Message = $"No new highscore for {customLeaderboard.SpawnsetFile.Name}.",
					TotalPlayers = totalPlayers,
					Leaderboard = customLeaderboard.ToDto(),
					Category = customLeaderboard.Category,
					Entries = entries
						.Select(e => e.ToDto(GetUsernameFromCache(e)))
						.ToList(),
					IsNewPlayerOnThisLeaderboard = false,
				};
			}

			// User got a better score.

			// Calculate the old rank.
			int oldRank = customLeaderboard.IsAscending() ? entries.Count(e => e.Time < customEntry.Time) + 1 : entries.Count(e => e.Time > customEntry.Time) + 1;

			int rankDiff = oldRank - rank;
			int timeDiff = uploadRequest.Time - customEntry.Time;
			int gemsCollectedDiff = uploadRequest.GemsCollected - customEntry.GemsCollected;
			int enemiesKilledDiff = uploadRequest.EnemiesKilled - customEntry.EnemiesKilled;
			int daggersFiredDiff = uploadRequest.DaggersFired - customEntry.DaggersFired;
			int daggersHitDiff = uploadRequest.DaggersHit - customEntry.DaggersHit;
			int enemiesAliveDiff = uploadRequest.EnemiesAlive - customEntry.EnemiesAlive;
			int homingDaggersDiff = uploadRequest.HomingDaggers - customEntry.HomingDaggers;
			int gemsDespawnedDiff = uploadRequest.GemsDespawned - customEntry.GemsDespawned;
			int gemsEatenDiff = uploadRequest.GemsEaten - customEntry.GemsEaten;
			int gemsTotalDiff = uploadRequest.GemsTotal - customEntry.GemsTotal;
			int levelUpTime2Diff = uploadRequest.LevelUpTime2 - customEntry.LevelUpTime2;
			int levelUpTime3Diff = uploadRequest.LevelUpTime3 - customEntry.LevelUpTime3;
			int levelUpTime4Diff = uploadRequest.LevelUpTime4 - customEntry.LevelUpTime4;

			// Update the entry.
			customEntry.Time = uploadRequest.Time;
			customEntry.EnemiesKilled = uploadRequest.EnemiesKilled;
			customEntry.GemsCollected = uploadRequest.GemsCollected;
			customEntry.DaggersFired = uploadRequest.DaggersFired;
			customEntry.DaggersHit = uploadRequest.DaggersHit;
			customEntry.EnemiesAlive = uploadRequest.EnemiesAlive;
			customEntry.HomingDaggers = uploadRequest.HomingDaggers;
			customEntry.GemsDespawned = uploadRequest.GemsDespawned;
			customEntry.GemsEaten = uploadRequest.GemsEaten;
			customEntry.DeathType = uploadRequest.DeathType;
			customEntry.LevelUpTime2 = uploadRequest.LevelUpTime2;
			customEntry.LevelUpTime3 = uploadRequest.LevelUpTime3;
			customEntry.LevelUpTime4 = uploadRequest.LevelUpTime4;
			customEntry.SubmitDate = DateTime.Now;
			customEntry.ClientVersion = uploadRequest.ClientVersion;

			// Update the entry data.
			if (_enableData)
			{
				CustomEntryData? customEntryData = _dbContext.CustomEntryData.FirstOrDefault(ced => ced.CustomEntryId == customEntry.Id);
				if (customEntryData == null)
				{
					customEntryData = new()
					{
						CustomEntryId = customEntry.Id,
					};
					customEntryData.Populate(uploadRequest.GameStates);
					_dbContext.CustomEntryData.Add(customEntryData);
				}
				else
				{
					customEntryData.Populate(uploadRequest.GameStates);
				}
			}

			_dbContext.SaveChanges();

			// Fetch the entries again after having modified the leaderboard.
			entries = _dbContext.CustomEntries.Where(e => e.CustomLeaderboard == customLeaderboard).OrderByMember(nameof(CustomEntry.Time), customLeaderboard.IsAscending()).ToArray();

			await TryLog(uploadRequest, spawnsetName);
			return new Dto.UploadSuccess
			{
				Message = $"NEW HIGHSCORE for {customLeaderboard.SpawnsetFile.Name}!",
				TotalPlayers = totalPlayers,
				Leaderboard = customLeaderboard.ToDto(),
				Category = customLeaderboard.Category,
				Entries = entries
					.Select(e => e.ToDto(GetUsernameFromCache(e)))
					.ToList(),
				IsNewPlayerOnThisLeaderboard = false,
				Rank = rank,
				RankDiff = rankDiff,
				Time = uploadRequest.Time,
				TimeDiff = timeDiff,
				GemsCollected = uploadRequest.GemsCollected,
				GemsCollectedDiff = gemsCollectedDiff,
				EnemiesKilled = uploadRequest.EnemiesKilled,
				EnemiesKilledDiff = enemiesKilledDiff,
				DaggersFired = uploadRequest.DaggersFired,
				DaggersFiredDiff = daggersFiredDiff,
				DaggersHit = uploadRequest.DaggersHit,
				DaggersHitDiff = daggersHitDiff,
				EnemiesAlive = uploadRequest.EnemiesAlive,
				EnemiesAliveDiff = enemiesAliveDiff,
				HomingDaggers = uploadRequest.HomingDaggers,
				HomingDaggersDiff = homingDaggersDiff,
				GemsDespawned = uploadRequest.GemsDespawned,
				GemsDespawnedDiff = gemsDespawnedDiff,
				GemsEaten = uploadRequest.GemsEaten,
				GemsEatenDiff = gemsEatenDiff,
				GemsTotal = uploadRequest.GemsTotal,
				GemsTotalDiff = gemsTotalDiff,
				LevelUpTime2 = uploadRequest.LevelUpTime2,
				LevelUpTime2Diff = levelUpTime2Diff,
				LevelUpTime3 = uploadRequest.LevelUpTime3,
				LevelUpTime3Diff = levelUpTime3Diff,
				LevelUpTime4 = uploadRequest.LevelUpTime4,
				LevelUpTime4Diff = levelUpTime4Diff,
			};
		}

		private string GetUsernameFromCache(CustomEntry e)
			=> _usernames.FirstOrDefault(u => u.Key == e.PlayerId).Value ?? "[Player not found]";

		private static string GetSpawnsetNameOrHash(Dto.UploadRequest uploadRequest, string? spawnsetName)
			=> string.IsNullOrEmpty(spawnsetName) ? BitConverter.ToString(uploadRequest.SurvivalHashMd5).Replace("-", string.Empty) : spawnsetName;

		private static async Task<string> GetUsername(Dto.UploadRequest uploadRequest)
		{
			if (uploadRequest.PlayerName.EndsWith("med fragger", StringComparison.InvariantCulture))
				return (await DdHasmodaiClient.GetUserById(uploadRequest.PlayerId))?.Username ?? uploadRequest.PlayerName;
			return uploadRequest.PlayerName;
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

				string ddclInfo = $"(`{uploadRequest.ClientVersion}` | `{uploadRequest.OperatingSystem}` | `{uploadRequest.BuildMode}`)";

				if (!string.IsNullOrEmpty(errorMessage))
					await BotLogger.Instance.TryLog(LoggingChannel.CustomLeaderboard, $"Upload failed for user `{uploadRequest.PlayerName}` (`{uploadRequest.PlayerId}`) for `{spawnsetIdentification}`. {ddclInfo}\n{errorMessage}");
				else
					await BotLogger.Instance.TryLog(LoggingChannel.CustomLeaderboard, $"`{uploadRequest.PlayerName}` just submitted a score of `{uploadRequest.Time / 10000f:0.0000}` to `{spawnsetIdentification}`. {ddclInfo}");
			}
			catch
			{
				// Ignore exceptions that occurred while attempting to log.
			}
		}

		private static string ByteArrayToHexString(byte[] byteArray)
			=> BitConverter.ToString(byteArray).Replace("-", string.Empty);
	}
}
