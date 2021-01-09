using DevilDaggersCore.Extensions;
using DevilDaggersCore.Spawnsets;
using DevilDaggersWebsite.Core.Clients;
using DevilDaggersWebsite.Core.Entities;
using DevilDaggersWebsite.Core.Extensions;
using DevilDaggersWebsite.Core.Transients;
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
		private readonly ApplicationDbContext _context;
		private readonly IWebHostEnvironment _env;
		private readonly ToolHelper _toolHelper;

		private readonly Dictionary<int, string> _usernames;

		public CustomLeaderboardsController(ApplicationDbContext dbContext, IWebHostEnvironment env, ToolHelper toolHelper)
		{
			_context = dbContext;
			_env = env;
			_toolHelper = toolHelper;

			_usernames = dbContext.Players.Select(p => new KeyValuePair<int, string>(p.Id, p.Username)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<List<Dto.CustomLeaderboard>> GetCustomLeaderboards()
		{
			return _context.CustomLeaderboards
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
			CustomLeaderboard? customLeaderboard = _context.CustomLeaderboards
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
				if (spawnset.GetHashString() == uploadRequest.SpawnsetHash)
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

			CustomLeaderboard leaderboard = _context.CustomLeaderboards.Include(cl => cl.SpawnsetFile).ThenInclude(sf => sf.Player).FirstOrDefault(cl => cl.SpawnsetFile.Name == spawnsetName);
			if (leaderboard == null)
			{
				const string errorMessage = "This spawnset exists on DevilDaggers.info, but doesn't have a leaderboard.";
				await TryLog(uploadRequest, spawnsetName, errorMessage);
				return new BadRequestObjectResult(new ProblemDetails { Title = errorMessage });
			}

			if (leaderboard.Category == Enumerators.CustomLeaderboardCategory.Challenge && Version.Parse(uploadRequest.ClientVersion) <= new Version(0, 10, 4, 0))
			{
				const string errorMessage = "Challenge leaderboards are not supported on version 0.10.4.0 or lower.";
				await TryLog(uploadRequest, spawnsetName, errorMessage);
				return new BadRequestObjectResult(new ProblemDetails { Title = errorMessage });
			}

			// At this point, the submission is accepted.

			// Add the player or update the username.
			Player player = _context.Players.FirstOrDefault(p => p.Id == uploadRequest.PlayerId);
			if (player == null)
			{
				player = new Player
				{
					Id = uploadRequest.PlayerId,
					Username = await GetUsername(uploadRequest),
				};
				_context.Players.Add(player);
			}
			else
			{
				player.Username = await GetUsername(uploadRequest);
			}

			// Update the date this leaderboard was submitted to.
			leaderboard.DateLastPlayed = DateTime.Now;
			leaderboard.TotalRunsSubmitted++;

			// Calculate the new rank.
			IEnumerable<CustomEntry> entries = _context.CustomEntries.Where(e => e.CustomLeaderboard == leaderboard).OrderByMember(nameof(CustomEntry.Time), leaderboard.IsAscending()).ToArray();

			int rank = leaderboard.IsAscending() ? entries.Count(e => e.Time < uploadRequest.Time) + 1 : entries.Count(e => e.Time > uploadRequest.Time) + 1;
			int totalPlayers = entries.Count();

			CustomEntry? entry = _context.CustomEntries.FirstOrDefault(e => e.PlayerId == uploadRequest.PlayerId && e.CustomLeaderboardId == leaderboard.Id);
			if (entry == null)
			{
				// Add new user to this leaderboard.
				_context.CustomEntries.Add(uploadRequest.ToCustomEntryEntity(leaderboard));
				_context.SaveChanges();

				// Fetch the entries again after having modified the leaderboard.
				entries = _context.CustomEntries.Where(e => e.CustomLeaderboard == leaderboard).OrderByMember(nameof(CustomEntry.Time), leaderboard.IsAscending());
				totalPlayers = entries.Count();

				await TryLog(uploadRequest, spawnsetName);
				return new Dto.UploadSuccess
				{
					Message = $"Welcome to the leaderboard for {spawnsetName}.",
					TotalPlayers = totalPlayers,
					Leaderboard = leaderboard.ToDto(),
					Category = leaderboard.Category,
					Entries = entries
						.Select(e => e.ToDto(GetUsernameFromCache(e)))
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
			if (leaderboard.IsAscending() && entry.Time <= uploadRequest.Time || !leaderboard.IsAscending() && entry.Time >= uploadRequest.Time)
			{
				_context.SaveChanges();

				// Fetch the entries again after having modified the leaderboard.
				entries = _context.CustomEntries.Where(e => e.CustomLeaderboard == leaderboard).OrderByMember(nameof(CustomEntry.Time), leaderboard.IsAscending()).ToArray();

				await TryLog(uploadRequest, spawnsetName);
				return new Dto.UploadSuccess
				{
					Message = $"No new highscore for {leaderboard.SpawnsetFile.Name}.",
					TotalPlayers = totalPlayers,
					Leaderboard = leaderboard.ToDto(),
					Category = leaderboard.Category,
					Entries = entries
						.Select(e => e.ToDto(GetUsernameFromCache(e)))
						.ToList(),
					IsNewUserOnThisLeaderboard = false,
				};
			}

			// User got a better score.

			// Calculate the old rank.
			int oldRank = leaderboard.IsAscending() ? entries.Count(e => e.Time < entry.Time) + 1 : entries.Count(e => e.Time > entry.Time) + 1;

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
			entry.ClientVersion = uploadRequest.ClientVersion;
			entry.GemsData = string.Join(",", uploadRequest.GameStates.Select(gs => gs.Gems));
			entry.KillsData = string.Join(",", uploadRequest.GameStates.Select(gs => gs.Kills));
			entry.HomingData = string.Join(",", uploadRequest.GameStates.Select(gs => gs.Homing));
			entry.EnemiesAliveData = string.Join(",", uploadRequest.GameStates.Select(gs => gs.EnemiesAlive));
			entry.DaggersFiredData = string.Join(",", uploadRequest.GameStates.Select(gs => gs.DaggersFired));
			entry.DaggersHitData = string.Join(",", uploadRequest.GameStates.Select(gs => gs.DaggersHit));

			_context.SaveChanges();

			// Fetch the entries again after having modified the leaderboard.
			entries = _context.CustomEntries.Where(e => e.CustomLeaderboard == leaderboard).OrderByMember(nameof(CustomEntry.Time), leaderboard.IsAscending()).ToArray();

			await TryLog(uploadRequest, spawnsetName);
			return new Dto.UploadSuccess
			{
				Message = $"NEW HIGHSCORE for {leaderboard.SpawnsetFile.Name}!",
				TotalPlayers = totalPlayers,
				Leaderboard = leaderboard.ToDto(),
				Category = leaderboard.Category,
				Entries = entries
					.Select(e => e.ToDto(GetUsernameFromCache(e)))
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

		private string GetUsernameFromCache(CustomEntry e)
			=> _usernames.FirstOrDefault(u => u.Key == e.PlayerId).Value ?? "[Player not found]";

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

				string ddclInfo = $"(`{uploadRequest.ClientVersion}` | `{uploadRequest.OperatingSystem}` | `{uploadRequest.BuildMode}`)";

				if (!string.IsNullOrEmpty(errorMessage))
					await BotLogger.Instance.TryLog($"Upload failed for user `{uploadRequest.Username}` (`{uploadRequest.PlayerId}`) for `{spawnsetIdentification}`. {ddclInfo}\n{errorMessage}");
				else
					await BotLogger.Instance.TryLog($"`{uploadRequest.Username}` just submitted a score of `{uploadRequest.Time / 10000f:0.0000}` to `{spawnsetIdentification}`. {ddclInfo}");
			}
			catch
			{
				// Ignore exceptions that occurred while attempting to log.
			}
		}
	}
}