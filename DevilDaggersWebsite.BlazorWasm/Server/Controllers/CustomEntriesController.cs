using DevilDaggersWebsite.BlazorWasm.Server.Controllers.Attributes;
using DevilDaggersWebsite.Caches.SpawnsetHash;
using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Extensions;
using DevilDaggersWebsite.HostedServices.DdInfoDiscordBot;
using DevilDaggersWebsite.Singletons;
using DevilDaggersWebsite.Transients;
using DevilDaggersWebsite.Utils;
using DSharpPlus.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DevilDaggersWebsite.BlazorWasm.Server.Controllers
{
	[Route("api/custom-entries")]
	[ApiController]
	public class CustomEntriesController : ControllerBase
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly IToolHelper _toolHelper;
		private readonly DiscordLogger _discordLogger;
		private readonly SpawnsetHashCache _spawnsetHashCache;

		public CustomEntriesController(ApplicationDbContext dbContext, IToolHelper toolHelper, DiscordLogger discordLogger, SpawnsetHashCache spawnsetHashCache)
		{
			_dbContext = dbContext;
			_toolHelper = toolHelper;
			_discordLogger = discordLogger;
			_spawnsetHashCache = spawnsetHashCache;
		}

		[HttpPost("submit")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[EndpointConsumer(EndpointConsumers.Ddcl)]
		public async Task<ActionResult<Dto.UploadSuccess>> SubmitScore([FromBody] Dto.UploadRequest uploadRequest)
		{
			try
			{
				return await ProcessUploadRequest(uploadRequest);
			}
			catch (Exception ex)
			{
				ex.Data[nameof(uploadRequest.ClientVersion)] = uploadRequest.ClientVersion;
				ex.Data[nameof(uploadRequest.OperatingSystem)] = uploadRequest.OperatingSystem;
				ex.Data[nameof(uploadRequest.BuildMode)] = uploadRequest.BuildMode;
				await _discordLogger.TryLogException($"Upload failed for user `{uploadRequest.PlayerName}` (`{uploadRequest.PlayerId}`) for `{GetSpawnsetHashOrName(uploadRequest.SurvivalHashMd5, null)}`.", ex);
				throw;
			}
		}

		[ApiExplorerSettings(IgnoreApi = true)]
		[NonAction]
		public async Task<ActionResult<Dto.UploadSuccess>> ProcessUploadRequest(Dto.UploadRequest uploadRequest)
		{
			// Check if the submission actually came from DDCL.
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
				uploadRequest.HomingDaggersEaten,
				uploadRequest.IsReplay ? 1 : 0,
				uploadRequest.SurvivalHashMd5.ByteArrayToHexString(),
				string.Join(",", new int[3] { uploadRequest.LevelUpTime2, uploadRequest.LevelUpTime3, uploadRequest.LevelUpTime4 }));
			if (await DecryptValidation(uploadRequest.Validation) != check)
			{
				const string errorMessage = "Invalid submission.";
				await TryLog(uploadRequest, null, errorMessage, "rotating_light");
				return new BadRequestObjectResult(new ProblemDetails { Title = errorMessage });
			}

			// Add the player or update the username. Also check for banned user immediately.
			Player? player = _dbContext.Players.FirstOrDefault(p => p.Id == uploadRequest.PlayerId);
			if (player != null)
			{
				if (player.IsBannedFromDdcl)
				{
					const string errorMessage = "Banned.";
					await TryLog(uploadRequest, null, errorMessage, "rotating_light");
					return new BadRequestObjectResult(new ProblemDetails { Title = errorMessage });
				}

				player.PlayerName = uploadRequest.PlayerName;
			}
			else
			{
				player = new()
				{
					Id = uploadRequest.PlayerId,
					PlayerName = uploadRequest.PlayerName,
				};
				_dbContext.Players.Add(player);
			}

			// Check for required version.
			Dto.Tool tool = _toolHelper.GetToolByName("DevilDaggersCustomLeaderboards");
			Version clientVersionParsed = Version.Parse(uploadRequest.ClientVersion);
			if (clientVersionParsed < tool.VersionNumberRequired)
			{
				const string errorMessage = "You are using an unsupported and outdated version of DDCL. Please update the program.";
				await TryLog(uploadRequest, null, errorMessage);
				return new BadRequestObjectResult(new ProblemDetails { Title = errorMessage });
			}

			// Check for existing spawnset.
			SpawnsetHashCacheData? spawnsetHashData = await _spawnsetHashCache.GetSpawnset(uploadRequest.SurvivalHashMd5);
			string? spawnsetName = spawnsetHashData?.Name;
			if (string.IsNullOrEmpty(spawnsetName))
			{
				const string errorMessage = "This spawnset doesn't exist on DevilDaggers.info.";
				await TryLog(uploadRequest, spawnsetName, errorMessage);
				return new BadRequestObjectResult(new ProblemDetails { Title = errorMessage });
			}

			// Check for existing leaderboard.
			CustomLeaderboard? customLeaderboard = _dbContext.CustomLeaderboards.Include(cl => cl.SpawnsetFile).ThenInclude(sf => sf.Player).FirstOrDefault(cl => cl.SpawnsetFile.Name == spawnsetName);
			if (customLeaderboard == null)
			{
				const string errorMessage = "This spawnset exists on DevilDaggers.info, but doesn't have a leaderboard.";
				await TryLog(uploadRequest, spawnsetName, errorMessage);
				return new BadRequestObjectResult(new ProblemDetails { Title = errorMessage });
			}

			// Temporary workaround until TimeAttack works in DDCL.
			if (customLeaderboard.Category == Enumerators.CustomLeaderboardCategory.TimeAttack)
			{
				const string errorMessage = "TimeAttack leaderboards are not supported right now.";
				await TryLog(uploadRequest, spawnsetName, errorMessage);
				return new BadRequestObjectResult(new ProblemDetails { Title = errorMessage });
			}

			bool isAscending = customLeaderboard.Category.IsAscending();

			// At this point, the submission is accepted.
			if (!uploadRequest.IsReplay)
			{
				// Update leaderboard statistics.
				customLeaderboard.DateLastPlayed = DateTime.UtcNow;
				customLeaderboard.TotalRunsSubmitted++;
			}
			else if (!isAscending)
			{
				// Due to a bug in the game, we need to subtract a couple ticks if the run is a replay, so replays don't overwrite the actual score if submitted twice.
				// The amount of overflowing ticks varies between 0 and 3 (the longer the run the higher the amount), so subtract 4 ticks for now.
				uploadRequest.Time -= 667;
			}

			// Make sure HomingDaggers is not negative (happens rarely).
			uploadRequest.HomingDaggers = Math.Max(0, uploadRequest.HomingDaggers);

			// Calculate the new rank.
			List<CustomEntry> entries = FetchEntriesFromDatabase(customLeaderboard, isAscending);
			int rank = isAscending ? entries.Count(e => e.Time <= uploadRequest.Time) + 1 : entries.Count(e => e.Time >= uploadRequest.Time) + 1;
			int totalPlayers = entries.Count;

			CustomEntry? customEntry = entries.Find(e => e.PlayerId == uploadRequest.PlayerId);
			if (customEntry == null)
			{
				// Add new custom entry to this leaderboard.
				CustomEntry newCustomEntry = uploadRequest.ToCustomEntryEntity(customLeaderboard);
				_dbContext.CustomEntries.Add(newCustomEntry);

				CustomEntryData newCustomEntryData = new() { CustomEntry = newCustomEntry };
				Populate(newCustomEntryData, uploadRequest.GameStates);
				_dbContext.CustomEntryData.Add(newCustomEntryData);

				_dbContext.SaveChanges();

				// Fetch the entries again after having modified the leaderboard.
				entries = FetchEntriesFromDatabase(customLeaderboard, isAscending);
				totalPlayers = entries.Count;

				await TrySendLeaderboardMessage(customLeaderboard, $"`{uploadRequest.PlayerName}` just entered the `{spawnsetName}` leaderboard!", rank, totalPlayers, uploadRequest.Time);
				await TryLog(uploadRequest, spawnsetName);
				return new Dto.UploadSuccess
				{
					Message = $"Welcome to the {spawnsetName} leaderboard!",
					TotalPlayers = totalPlayers,
					Leaderboard = customLeaderboard.ToDto(),
					Category = customLeaderboard.Category,
					Entries = entries.ConvertAll(e => e.ToDto()),
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
					HomingDaggersEaten = uploadRequest.HomingDaggersEaten,
					LevelUpTime2 = uploadRequest.LevelUpTime2,
					LevelUpTime3 = uploadRequest.LevelUpTime3,
					LevelUpTime4 = uploadRequest.LevelUpTime4,
				};
			}

			// User is already on the leaderboard, but did not get a better score.
			if (isAscending && customEntry.Time <= uploadRequest.Time || !isAscending && customEntry.Time >= uploadRequest.Time)
			{
				_dbContext.SaveChanges();

				// Fetch the entries again after having modified the leaderboard.
				entries = FetchEntriesFromDatabase(customLeaderboard, isAscending);

				await TryLog(uploadRequest, spawnsetName);
				return new Dto.UploadSuccess
				{
					Message = $"No new highscore for {customLeaderboard.SpawnsetFile.Name}.",
					TotalPlayers = totalPlayers,
					Leaderboard = customLeaderboard.ToDto(),
					Category = customLeaderboard.Category,
					Entries = entries.ConvertAll(e => e.ToDto()),
					IsNewPlayerOnThisLeaderboard = false,
				};
			}

			// User got a better score.

			// Calculate the old rank.
			int oldRank = isAscending ? entries.Count(e => e.Time < customEntry.Time) + 1 : entries.Count(e => e.Time > customEntry.Time) + 1;

			int rankDiff = oldRank - rank;
			int timeDiff = uploadRequest.Time - customEntry.Time;
			int gemsCollectedDiff = uploadRequest.GemsCollected - customEntry.GemsCollected;
			int enemiesKilledDiff = uploadRequest.EnemiesKilled - customEntry.EnemiesKilled;
			int daggersFiredDiff = uploadRequest.DaggersFired - customEntry.DaggersFired;
			int daggersHitDiff = uploadRequest.DaggersHit - customEntry.DaggersHit;
			int enemiesAliveDiff = uploadRequest.EnemiesAlive - customEntry.EnemiesAlive;
			int homingDaggersDiff = uploadRequest.HomingDaggers - customEntry.HomingDaggers;
			int homingDaggersEatenDiff = uploadRequest.HomingDaggersEaten - customEntry.HomingDaggersEaten;
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
			customEntry.HomingDaggersEaten = uploadRequest.HomingDaggersEaten;
			customEntry.GemsDespawned = uploadRequest.GemsDespawned;
			customEntry.GemsEaten = uploadRequest.GemsEaten;
			customEntry.GemsTotal = uploadRequest.GemsTotal;
			customEntry.DeathType = uploadRequest.DeathType;
			customEntry.LevelUpTime2 = uploadRequest.LevelUpTime2;
			customEntry.LevelUpTime3 = uploadRequest.LevelUpTime3;
			customEntry.LevelUpTime4 = uploadRequest.LevelUpTime4;
			customEntry.SubmitDate = DateTime.UtcNow;
			customEntry.ClientVersion = uploadRequest.ClientVersion;

			// Update the entry data.
			CustomEntryData? customEntryData = _dbContext.CustomEntryData.FirstOrDefault(ced => ced.CustomEntryId == customEntry.Id);
			if (customEntryData == null)
			{
				customEntryData = new()
				{
					CustomEntryId = customEntry.Id,
				};
				Populate(customEntryData, uploadRequest.GameStates);
				_dbContext.CustomEntryData.Add(customEntryData);
			}
			else
			{
				Populate(customEntryData, uploadRequest.GameStates);
			}

			_dbContext.SaveChanges();

			// Fetch the entries again after having modified the leaderboard.
			entries = FetchEntriesFromDatabase(customLeaderboard, isAscending);

			await TrySendLeaderboardMessage(customLeaderboard, $"`{uploadRequest.PlayerName}` just got {FormatTimeString(uploadRequest.Time)} seconds on the `{spawnsetName}` leaderboard, beating their previous highscore of {FormatTimeString(uploadRequest.Time - timeDiff)} by {FormatTimeString(Math.Abs(timeDiff))} seconds!", rank, totalPlayers, uploadRequest.Time);
			await TryLog(uploadRequest, spawnsetName);
			return new Dto.UploadSuccess
			{
				Message = $"NEW HIGHSCORE for {customLeaderboard.SpawnsetFile.Name}!",
				TotalPlayers = totalPlayers,
				Leaderboard = customLeaderboard.ToDto(),
				Category = customLeaderboard.Category,
				Entries = entries.ConvertAll(e => e.ToDto()),
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
				HomingDaggersEaten = uploadRequest.HomingDaggersEaten,
				HomingDaggersEatenDiff = homingDaggersEatenDiff,
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

		private List<CustomEntry> FetchEntriesFromDatabase(CustomLeaderboard? customLeaderboard, bool isAscending)
		{
			// Use tracking to update player score.
			return _dbContext.CustomEntries
				.Include(ce => ce.Player)
				.Where(e => e.CustomLeaderboard == customLeaderboard)
				.OrderByMember(nameof(CustomEntry.Time), isAscending)
				.ThenByMember(nameof(CustomEntry.SubmitDate), true)
				.ToList();
		}

		private async Task TrySendLeaderboardMessage(CustomLeaderboard customLeaderboard, string message, int rank, int totalPlayers, int time)
		{
			try
			{
				DiscordEmbedBuilder builder = new()
				{
					Title = message,
					Color = customLeaderboard.GetDaggerFromTime(time).GetDiscordColor(),
					Url = Uri.EscapeUriString($"https://devildaggers.info/CustomLeaderboards/Leaderboard?spawnsetName={customLeaderboard.SpawnsetFile.Name}"),
				};
				builder.AddFieldObject("Score", FormatTimeString(time), true);
				builder.AddFieldObject("Rank", $"{rank}/{totalPlayers}", true);
				await _discordLogger.TryLog(Channel.CustomLeaderboards, null, builder.Build());
			}
			catch (Exception ex)
			{
				await _discordLogger.TryLogException("Error while attempting to send leaderboard message.", ex);
			}
		}

		private static string FormatTimeString(int time)
			=> (time / 10000.0).ToString("0.0000");

		private static string GetSpawnsetHashOrName(byte[] spawnsetHash, string? spawnsetName)
			=> string.IsNullOrEmpty(spawnsetName) ? BitConverter.ToString(spawnsetHash).Replace("-", string.Empty) : spawnsetName;

		private async Task<string> DecryptValidation(string validation)
		{
			try
			{
				return Secrets.EncryptionWrapper.DecodeAndDecrypt(HttpUtility.HtmlDecode(validation));
			}
			catch (Exception ex)
			{
				await _discordLogger.TryLogException($"Could not decrypt validation: `{validation}`", ex);

				return string.Empty;
			}
		}

		private async Task TryLog(Dto.UploadRequest uploadRequest, string? spawnsetName, string? errorMessage = null, string? errorEmoteNameOverride = null)
		{
			try
			{
				string spawnsetIdentification = GetSpawnsetHashOrName(uploadRequest.SurvivalHashMd5, spawnsetName);

				string replayString = uploadRequest.IsReplay ? " | `Replay`" : string.Empty;
				string ddclInfo = $"(`{uploadRequest.ClientVersion}` | `{uploadRequest.OperatingSystem}` | `{uploadRequest.BuildMode}`{replayString})";

				if (!string.IsNullOrEmpty(errorMessage))
					await _discordLogger.TryLog(Channel.MonitoringCustomLeaderboard, $":{errorEmoteNameOverride ?? "warning"}: Upload failed for user `{uploadRequest.PlayerName}` (`{uploadRequest.PlayerId}`) for `{spawnsetIdentification}`. {ddclInfo}\n**{errorMessage}**");
				else
					await _discordLogger.TryLog(Channel.MonitoringCustomLeaderboard, $":white_check_mark: `{uploadRequest.PlayerName}` just submitted a score of `{uploadRequest.Time / 10000f:0.0000}` to `{spawnsetIdentification}`. {ddclInfo}");
			}
			catch
			{
				// Ignore exceptions that occurred while attempting to log.
			}
		}

		private void Populate(CustomEntryData ced, List<Dto.GameState> gameStates)
		{
			ced.GemsCollectedData = CompressProperty(gs => gs.GemsCollected);
			ced.EnemiesKilledData = CompressProperty(gs => gs.EnemiesKilled);
			ced.DaggersFiredData = CompressProperty(gs => gs.DaggersFired);
			ced.DaggersHitData = CompressProperty(gs => gs.DaggersHit);
			ced.EnemiesAliveData = CompressProperty(gs => gs.EnemiesAlive);
			ced.HomingDaggersData = CompressProperty(gs => gs.HomingDaggers);
			ced.HomingDaggersEatenData = CompressProperty(gs => gs.HomingDaggersEaten);
			ced.GemsDespawnedData = CompressProperty(gs => gs.GemsDespawned);
			ced.GemsEatenData = CompressProperty(gs => gs.GemsEaten);
			ced.GemsTotalData = CompressProperty(gs => gs.GemsTotal);

			ced.Skull1sAliveData = CompressProperty(gs => gs.Skull1sAlive);
			ced.Skull2sAliveData = CompressProperty(gs => gs.Skull2sAlive);
			ced.Skull3sAliveData = CompressProperty(gs => gs.Skull3sAlive);
			ced.SpiderlingsAliveData = CompressProperty(gs => gs.SpiderlingsAlive);
			ced.Skull4sAliveData = CompressProperty(gs => gs.Skull4sAlive);
			ced.Squid1sAliveData = CompressProperty(gs => gs.Squid1sAlive);
			ced.Squid2sAliveData = CompressProperty(gs => gs.Squid2sAlive);
			ced.Squid3sAliveData = CompressProperty(gs => gs.Squid3sAlive);
			ced.CentipedesAliveData = CompressProperty(gs => gs.CentipedesAlive);
			ced.GigapedesAliveData = CompressProperty(gs => gs.GigapedesAlive);
			ced.Spider1sAliveData = CompressProperty(gs => gs.Spider1sAlive);
			ced.Spider2sAliveData = CompressProperty(gs => gs.Spider2sAlive);
			ced.LeviathansAliveData = CompressProperty(gs => gs.LeviathansAlive);
			ced.OrbsAliveData = CompressProperty(gs => gs.OrbsAlive);
			ced.ThornsAliveData = CompressProperty(gs => gs.ThornsAlive);
			ced.GhostpedesAliveData = CompressProperty(gs => gs.GhostpedesAlive);
			ced.SpiderEggsAliveData = CompressProperty(gs => gs.SpiderEggsAlive);

			ced.Skull1sKilledData = CompressProperty(gs => gs.Skull1sKilled);
			ced.Skull2sKilledData = CompressProperty(gs => gs.Skull2sKilled);
			ced.Skull3sKilledData = CompressProperty(gs => gs.Skull3sKilled);
			ced.SpiderlingsKilledData = CompressProperty(gs => gs.SpiderlingsKilled);
			ced.Skull4sKilledData = CompressProperty(gs => gs.Skull4sKilled);
			ced.Squid1sKilledData = CompressProperty(gs => gs.Squid1sKilled);
			ced.Squid2sKilledData = CompressProperty(gs => gs.Squid2sKilled);
			ced.Squid3sKilledData = CompressProperty(gs => gs.Squid3sKilled);
			ced.CentipedesKilledData = CompressProperty(gs => gs.CentipedesKilled);
			ced.GigapedesKilledData = CompressProperty(gs => gs.GigapedesKilled);
			ced.Spider1sKilledData = CompressProperty(gs => gs.Spider1sKilled);
			ced.Spider2sKilledData = CompressProperty(gs => gs.Spider2sKilled);
			ced.LeviathansKilledData = CompressProperty(gs => gs.LeviathansKilled);
			ced.OrbsKilledData = CompressProperty(gs => gs.OrbsKilled);
			ced.ThornsKilledData = CompressProperty(gs => gs.ThornsKilled);
			ced.GhostpedesKilledData = CompressProperty(gs => gs.GhostpedesKilled);
			ced.SpiderEggsKilledData = CompressProperty(gs => gs.SpiderEggsKilled);

			byte[] CompressProperty(Func<Dto.GameState, int> propertySelector)
				=> IntegerArrayCompressor.CompressData(gameStates.Select(propertySelector).ToArray());
		}
	}
}
