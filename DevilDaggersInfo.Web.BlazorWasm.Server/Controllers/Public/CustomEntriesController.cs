using DevilDaggersInfo.Core.Encryption;
using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.SpawnsetHashes;
using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.SpawnsetSummaries;
using DevilDaggersInfo.Web.BlazorWasm.Server.Converters.Public;
using DevilDaggersInfo.Web.BlazorWasm.Server.HostedServices.DdInfoDiscordBot;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomEntries;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Utils;
using DSharpPlus.Entities;
using System.Web;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Controllers.Public;

[Route("api/custom-entries")]
[ApiController]
public class CustomEntriesController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;
	private readonly ILogger<CustomEntriesController> _logger;
	private readonly SpawnsetHashCache _spawnsetHashCache;
	private readonly SpawnsetSummaryCache _spawnsetSummaryCache;
	private readonly IFileSystemService _fileSystemService;

	private readonly AesBase32Wrapper _encryptionWrapper;

	public CustomEntriesController(ApplicationDbContext dbContext, ILogger<CustomEntriesController> logger, SpawnsetHashCache spawnsetHashCache, SpawnsetSummaryCache spawnsetSummaryCache, IFileSystemService fileSystemService, IConfiguration configuration)
	{
		_dbContext = dbContext;
		_logger = logger;
		_spawnsetHashCache = spawnsetHashCache;
		_spawnsetSummaryCache = spawnsetSummaryCache;
		_fileSystemService = fileSystemService;

		IConfigurationSection section = configuration.GetRequiredSection("CustomLeaderboardSecrets");
		_encryptionWrapper = new(section["InitializationVector"], section["Password"], section["Salt"]);
	}

	[HttpGet("{id}/replay")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public ActionResult GetCustomEntryReplayById([Required] int id)
	{
		string path = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.CustomEntryReplays), $"{id}.ddreplay");
		if (!IoFile.Exists(path))
			return NotFound();

		var customEntry = _dbContext.CustomEntries
			.AsNoTracking()
			.Select(ce => new
			{
				ce.Id,
				ce.CustomLeaderboard.SpawnsetId,
				SpawnsetName = ce.CustomLeaderboard.Spawnset.Name,
				ce.PlayerId,
				ce.Player.PlayerName,
			})
			.FirstOrDefault(ce => ce.Id == id);
		if (customEntry == null)
			return NotFound();

		string fileName = $"{customEntry.SpawnsetId}-{customEntry.SpawnsetName}-{customEntry.PlayerId}-{customEntry.PlayerName}.ddreplay";
		return File(IoFile.ReadAllBytes(path), MediaTypeNames.Application.Octet, fileName);
	}

	[HttpGet("{id}/data")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public ActionResult<GetCustomEntryData> GetCustomEntryDataById([Required] int id)
	{
		CustomEntryEntity? customEntry = _dbContext.CustomEntries
			.AsNoTracking()
			.Include(ce => ce.Player)
			.Include(ce => ce.CustomLeaderboard)
				.ThenInclude(cl => cl.Spawnset)
			.FirstOrDefault(cl => cl.Id == id);
		if (customEntry == null)
			return NotFound();

		CustomEntryDataEntity? customEntryData = _dbContext.CustomEntryData
			.AsNoTracking()
			.FirstOrDefault(ced => ced.CustomEntryId == id);

		SpawnsetSummary ss = _spawnsetSummaryCache.GetSpawnsetSummaryByFilePath(Path.Combine(_fileSystemService.GetPath(DataSubDirectory.Spawnsets), customEntry.CustomLeaderboard.Spawnset.Name));
		EffectivePlayerSettings eps = SpawnsetBinary.GetEffectivePlayerSettings(ss.HandLevel, ss.AdditionalGems);
		return customEntry.ToGetCustomEntryData(customEntryData, eps.HandLevel);
	}

	[HttpGet("player-stats")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<List<GetCustomLeaderboardStatisticsForPlayer>> GetCustomLeaderboardStatisticsByPlayerId([Required] int playerId)
	{
		var customEntries = _dbContext.CustomEntries
			.AsNoTracking()
			.Include(ce => ce.CustomLeaderboard)
			.Where(cl => cl.PlayerId == playerId)
			.Select(ce => new
			{
				ce.Time,
				ce.CustomLeaderboardId,
				ce.CustomLeaderboard.Category,
				ce.CustomLeaderboard.TimeLeviathan,
				ce.CustomLeaderboard.TimeDevil,
				ce.CustomLeaderboard.TimeGolden,
				ce.CustomLeaderboard.TimeSilver,
				ce.CustomLeaderboard.TimeBronze,
			})
			.ToList();

		List<GetCustomLeaderboardStatisticsForPlayer> stats = new();
		foreach (CustomLeaderboardCategory category in Enum.GetValues<CustomLeaderboardCategory>())
		{
			var customEntriesByCategory = customEntries.Where(c => c.Category == category);
			if (!customEntriesByCategory.Any())
				continue;

			int leviathanDaggers = 0;
			int devilDaggers = 0;
			int goldenDaggers = 0;
			int silverDaggers = 0;
			int bronzeDaggers = 0;
			int defaultDaggers = 0;
			int played = 0;
			foreach (var customEntry in customEntriesByCategory)
			{
				played++;
				switch (CustomLeaderboardUtils.GetDaggerFromTime(category, customEntry.Time, customEntry.TimeLeviathan, customEntry.TimeDevil, customEntry.TimeGolden, customEntry.TimeSilver, customEntry.TimeBronze))
				{
					case CustomLeaderboardDagger.Leviathan: leviathanDaggers++; break;
					case CustomLeaderboardDagger.Devil: devilDaggers++; break;
					case CustomLeaderboardDagger.Golden: goldenDaggers++; break;
					case CustomLeaderboardDagger.Silver: silverDaggers++; break;
					case CustomLeaderboardDagger.Bronze: bronzeDaggers++; break;
					default: defaultDaggers++; break;
				}
			}

			stats.Add(new()
			{
				CustomLeaderboardCategory = category,
				LeviathanDaggerCount = leviathanDaggers,
				DevilDaggerCount = devilDaggers,
				GoldenDaggerCount = goldenDaggers,
				SilverDaggerCount = silverDaggers,
				BronzeDaggerCount = bronzeDaggers,
				DefaultDaggerCount = defaultDaggers,
				LeaderboardsPlayedCount = played,
			});
		}

		return stats;
	}

	[HttpPost("submit")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<GetUploadSuccess>> SubmitScoreForDdcl([FromBody] AddUploadRequest uploadRequest)
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
			_logger.LogError(ex, "Upload failed for user `{playerName}` (`{playerId}`) for `{spawnset}`.", uploadRequest.PlayerName, uploadRequest.PlayerId, uploadRequest.SurvivalHashMd5.ByteArrayToHexString());
			throw;
		}
	}

	// TODO: Refactor and move method to a new service.
	[ApiExplorerSettings(IgnoreApi = true)]
	[NonAction]
	public async Task<ActionResult<GetUploadSuccess>> ProcessUploadRequest(AddUploadRequest uploadRequest)
	{
		// Check if the submission actually came from an allowed program.
		string expected = string.Join(
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

		string actual = DecryptValidation(uploadRequest.Validation);
		if (actual != expected)
		{
			string errorMessage = $"Invalid submission for {uploadRequest.Validation}.\nExpected: {expected}\nActual:   {actual}";
			await TryLog(uploadRequest, null, errorMessage, "rotating_light");
			return new BadRequestObjectResult(new ProblemDetails { Title = errorMessage });
		}

		// Add the player or update the username. Also check for banned user immediately.
		PlayerEntity? player = _dbContext.Players.FirstOrDefault(p => p.Id == uploadRequest.PlayerId);
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
		string clientName = uploadRequest.Client is "ddstats-rust" or "1" ? "ddstats-rust" : "DevilDaggersCustomLeaderboards";
		ToolEntity? tool = _dbContext.Tools.AsNoTracking().FirstOrDefault(t => t.Name == clientName);
		if (tool == null)
			throw new($"Could not find tool with name {clientName} in database.");

		Version clientVersionParsed = Version.Parse(uploadRequest.ClientVersion);
		if (clientVersionParsed < Version.Parse(tool.RequiredVersionNumber))
		{
			string errorMessage = $"You are using an unsupported and outdated version of {clientName}. Please update the program.";
			await TryLog(uploadRequest, null, errorMessage);
			return new BadRequestObjectResult(new ProblemDetails { Title = errorMessage });
		}

		// Reject local replays as they can easily be manipulated.
		if (uploadRequest.Status == 8)
		{
			const string errorMessage = "Local replays cannot be validated.";
			await TryLog(uploadRequest, null, errorMessage, "rotating_light");
			return new BadRequestObjectResult(new ProblemDetails { Title = errorMessage });
		}

		// Check for existing spawnset.
		SpawnsetHashCacheData? spawnsetHashData = _spawnsetHashCache.GetSpawnset(uploadRequest.SurvivalHashMd5);
		string? spawnsetName = spawnsetHashData?.Name;
		if (string.IsNullOrEmpty(spawnsetName))
		{
			const string errorMessage = "This spawnset doesn't exist on DevilDaggers.info.";
			await TryLog(uploadRequest, spawnsetName, errorMessage);
			return new BadRequestObjectResult(new ProblemDetails { Title = errorMessage });
		}

		// Check for existing leaderboard.
		CustomLeaderboardEntity? customLeaderboard = _dbContext.CustomLeaderboards.Include(cl => cl.Spawnset).ThenInclude(sf => sf.Player).FirstOrDefault(cl => cl.Spawnset.Name == spawnsetName);
		if (customLeaderboard == null)
		{
			const string errorMessage = "This spawnset exists on DevilDaggers.info, but doesn't have a leaderboard.";
			await TryLog(uploadRequest, spawnsetName, errorMessage);
			return new BadRequestObjectResult(new ProblemDetails { Title = errorMessage });
		}

		// Temporary workaround until TimeAttack works in DDCL (if ever).
		if (customLeaderboard.Category == CustomLeaderboardCategory.TimeAttack)
		{
			const string errorMessage = "TimeAttack leaderboards are not supported right now.";
			await TryLog(uploadRequest, spawnsetName, errorMessage);
			return new BadRequestObjectResult(new ProblemDetails { Title = errorMessage });
		}

		bool isAscending = customLeaderboard.Category.IsAscending();

		// At this point, the submission is accepted.

		// Make sure HomingDaggers is not negative (happens rarely as a bug, and also for spawnsets with homing disabled which we don't want to display values for anyway).
		uploadRequest.HomingDaggers = Math.Max(0, uploadRequest.HomingDaggers);
		foreach (AddGameState addGameState in uploadRequest.GameStates.Where(gs => gs.HomingDaggers < 0))
			addGameState.HomingDaggers = 0;

		// Calculate the new rank.
		List<CustomEntryEntity> entries = FetchEntriesFromDatabase(customLeaderboard, isAscending);
		int rank = isAscending ? entries.Count(e => e.Time <= uploadRequest.Time) + 1 : entries.Count(e => e.Time >= uploadRequest.Time) + 1;
		int totalPlayers = entries.Count;

		CustomEntryEntity? customEntry = entries.Find(e => e.PlayerId == uploadRequest.PlayerId);
		if (customEntry == null)
		{
			// Add new custom entry to this leaderboard.
			CustomEntryEntity newCustomEntry = new()
			{
				PlayerId = uploadRequest.PlayerId,
				Time = uploadRequest.Time,
				GemsCollected = uploadRequest.GemsCollected,
				GemsDespawned = uploadRequest.GemsDespawned,
				GemsEaten = uploadRequest.GemsEaten,
				GemsTotal = uploadRequest.GemsTotal,
				EnemiesKilled = uploadRequest.EnemiesKilled,
				DeathType = uploadRequest.DeathType,
				DaggersHit = uploadRequest.DaggersHit,
				DaggersFired = uploadRequest.DaggersFired,
				EnemiesAlive = uploadRequest.EnemiesAlive,
				HomingStored = uploadRequest.HomingDaggers,
				HomingEaten = uploadRequest.HomingDaggersEaten,
				LevelUpTime2 = uploadRequest.LevelUpTime2,
				LevelUpTime3 = uploadRequest.LevelUpTime3,
				LevelUpTime4 = uploadRequest.LevelUpTime4,
				SubmitDate = DateTime.UtcNow,
				ClientVersion = uploadRequest.ClientVersion,
				Client = GetClientFromString(uploadRequest.Client),
				CustomLeaderboard = customLeaderboard,
			};
			_dbContext.CustomEntries.Add(newCustomEntry);

			CustomEntryDataEntity newCustomEntryData = new() { CustomEntry = newCustomEntry };
			Populate(newCustomEntryData, uploadRequest.GameStates);
			_dbContext.CustomEntryData.Add(newCustomEntryData);

			UpdateLeaderboardStatistics(customLeaderboard);

			_dbContext.SaveChanges();

			if (uploadRequest.ReplayData != null)
				await WriteReplayFile(newCustomEntry.Id, uploadRequest.ReplayData);

			// Fetch the entries again after having modified the leaderboard.
			entries = FetchEntriesFromDatabase(customLeaderboard, isAscending);
			totalPlayers = entries.Count;

			await TrySendLeaderboardMessage(customLeaderboard, $"`{uploadRequest.PlayerName}` just entered the `{spawnsetName}` leaderboard!", rank, totalPlayers, uploadRequest.Time);
			await TryLog(uploadRequest, spawnsetName);

			return new GetUploadSuccess
			{
				Message = $"Welcome to the {spawnsetName} leaderboard!",
				TotalPlayers = totalPlayers,
				Leaderboard = customLeaderboard.ToGetCustomLeaderboardDdcl(),
				Category = customLeaderboard.Category,
				Entries = entries.ConvertAll(e => e.ToGetCustomEntryDdcl()),
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

		// Due to a bug in the game, we need to manually fix the request's time. The time gains a couple extra ticks if the run is a replay.
		// We don't want replays to overwrite the real score (this spams messages and is incorrect).
		// The amount of overflowing ticks varies between 0 and 3 (the longer the run the higher the amount).
		// Simply reset the time to the original when all data is the same.
		// TODO: Also apply this to ascending leaderboards.
		const int timeThreshold = 1000; // 0.1 seconds (or 6 ticks).
		const int gemThreshold = 2;
		const int killThreshold = 5;
		bool isHighscoreByUnderASecond = uploadRequest.Time > customEntry.Time && uploadRequest.Time < customEntry.Time + timeThreshold;
		bool gemsAlmostTheSame = uploadRequest.GemsCollected >= customEntry.GemsCollected - gemThreshold && uploadRequest.GemsCollected <= customEntry.GemsCollected + gemThreshold;
		bool killsAlmostTheSame = uploadRequest.EnemiesKilled >= customEntry.EnemiesKilled - killThreshold && uploadRequest.EnemiesKilled <= customEntry.EnemiesKilled + killThreshold;
		bool deathTypeTheSame = uploadRequest.DeathType == customEntry.DeathType;
		if (uploadRequest.IsReplay && !isAscending && isHighscoreByUnderASecond && gemsAlmostTheSame && killsAlmostTheSame && deathTypeTheSame)
			uploadRequest.Time = customEntry.Time;

		// User is already on the leaderboard, but did not get a better score.
		if (isAscending && customEntry.Time <= uploadRequest.Time || !isAscending && customEntry.Time >= uploadRequest.Time)
		{
			if (!uploadRequest.IsReplay)
				UpdateLeaderboardStatistics(customLeaderboard);

			_dbContext.SaveChanges();

			// Fetch the entries again after having modified the leaderboard.
			entries = FetchEntriesFromDatabase(customLeaderboard, isAscending);

			await TryLog(uploadRequest, spawnsetName);

			return new GetUploadSuccess
			{
				Message = $"No new highscore for {customLeaderboard.Spawnset.Name}.",
				TotalPlayers = totalPlayers,
				Leaderboard = customLeaderboard.ToGetCustomLeaderboardDdcl(),
				Category = customLeaderboard.Category,
				Entries = entries.ConvertAll(e => e.ToGetCustomEntryDdcl()),
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
		int homingDaggersDiff = uploadRequest.HomingDaggers - customEntry.HomingStored;
		int homingDaggersEatenDiff = uploadRequest.HomingDaggersEaten - customEntry.HomingEaten;
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
		customEntry.HomingStored = uploadRequest.HomingDaggers;
		customEntry.HomingEaten = uploadRequest.HomingDaggersEaten;
		customEntry.GemsDespawned = uploadRequest.GemsDespawned;
		customEntry.GemsEaten = uploadRequest.GemsEaten;
		customEntry.GemsTotal = uploadRequest.GemsTotal;
		customEntry.DeathType = uploadRequest.DeathType;
		customEntry.LevelUpTime2 = uploadRequest.LevelUpTime2;
		customEntry.LevelUpTime3 = uploadRequest.LevelUpTime3;
		customEntry.LevelUpTime4 = uploadRequest.LevelUpTime4;
		customEntry.SubmitDate = DateTime.UtcNow;
		customEntry.ClientVersion = uploadRequest.ClientVersion;
		customEntry.Client = GetClientFromString(uploadRequest.Client);

		// Update the entry data.
		CustomEntryDataEntity? customEntryData = _dbContext.CustomEntryData.FirstOrDefault(ced => ced.CustomEntryId == customEntry.Id);
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

		UpdateLeaderboardStatistics(customLeaderboard);

		_dbContext.SaveChanges();

		if (uploadRequest.ReplayData != null)
			await WriteReplayFile(customEntry.Id, uploadRequest.ReplayData);

		// Fetch the entries again after having modified the leaderboard.
		entries = FetchEntriesFromDatabase(customLeaderboard, isAscending);

		await TrySendLeaderboardMessage(customLeaderboard, $"`{uploadRequest.PlayerName}` just got {FormatTimeString(uploadRequest.Time)} seconds on the `{spawnsetName}` leaderboard, beating their previous highscore of {FormatTimeString(uploadRequest.Time - timeDiff)} by {FormatTimeString(Math.Abs(timeDiff))} seconds!", rank, totalPlayers, uploadRequest.Time);
		await TryLog(uploadRequest, spawnsetName);

		return new GetUploadSuccess
		{
			Message = $"NEW HIGHSCORE for {customLeaderboard.Spawnset.Name}!",
			TotalPlayers = totalPlayers,
			Leaderboard = customLeaderboard.ToGetCustomLeaderboardDdcl(),
			Category = customLeaderboard.Category,
			Entries = entries.ConvertAll(e => e.ToGetCustomEntryDdcl()),
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

	private async Task WriteReplayFile(int customEntryId, byte[] replayData)
	{
		await IoFile.WriteAllBytesAsync(Path.Combine(_fileSystemService.GetPath(DataSubDirectory.CustomEntryReplays), $"{customEntryId}.ddreplay"), replayData);
	}

	private static void UpdateLeaderboardStatistics(CustomLeaderboardEntity customLeaderboard)
	{
		customLeaderboard.DateLastPlayed = DateTime.UtcNow;
		customLeaderboard.TotalRunsSubmitted++;
	}

	private List<CustomEntryEntity> FetchEntriesFromDatabase(CustomLeaderboardEntity? customLeaderboard, bool isAscending)
	{
		// Use tracking to update player score.
		return _dbContext.CustomEntries
			.Include(ce => ce.Player)
			.Where(e => e.CustomLeaderboard == customLeaderboard)
			.OrderByMember(nameof(CustomEntryEntity.Time), isAscending)
			.ThenByMember(nameof(CustomEntryEntity.SubmitDate), true)
			.ToList();
	}

	private async Task TrySendLeaderboardMessage(CustomLeaderboardEntity customLeaderboard, string message, int rank, int totalPlayers, int time)
	{
		try
		{
			DiscordEmbedBuilder builder = new()
			{
				Title = message,
				Color = customLeaderboard.GetDaggerFromTime(time).GetDiscordColor(),
				Url = $"https://devildaggers.info/custom/leaderboard/{customLeaderboard.Id}",
			};
			builder.AddFieldObject("Score", FormatTimeString(time), true);
			builder.AddFieldObject("Rank", $"{rank}/{totalPlayers}", true);

			// TODO: Don't log in development.
			DiscordChannel? channel = DevilDaggersInfoServerConstants.Channels[Channel.CustomLeaderboards].DiscordChannel;
			if (channel == null)
				return;

			await channel.SendMessageAsyncSafe(null, builder.Build());
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error while attempting to send leaderboard message.");
		}
	}

	private static string FormatTimeString(int time)
		=> time.ToSecondsTime().ToString("0.0000");

	private static string GetSpawnsetHashOrName(byte[] spawnsetHash, string? spawnsetName)
		=> string.IsNullOrEmpty(spawnsetName) ? BitConverter.ToString(spawnsetHash).Replace("-", string.Empty) : spawnsetName;

	private string DecryptValidation(string validation)
	{
		try
		{
			return _encryptionWrapper.DecodeAndDecrypt(HttpUtility.HtmlDecode(validation));
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Could not decrypt validation: `{validation}`", validation);

			return string.Empty;
		}
	}

	private static async Task TryLog(AddUploadRequest uploadRequest, string? spawnsetName, string? errorMessage = null, string? errorEmoteNameOverride = null)
	{
		try
		{
			string spawnsetIdentification = GetSpawnsetHashOrName(uploadRequest.SurvivalHashMd5, spawnsetName);

			string replayData = uploadRequest.ReplayData == null ? "Replay data not included" : $"Replay data {uploadRequest.ReplayData.Length:N0} bytes";
			string replayString = uploadRequest.IsReplay ? " | `Replay`" : string.Empty;
			string localReplayString = uploadRequest.Status == 8 ? $" | `Local replay from {uploadRequest.ReplayPlayerId}`" : string.Empty;
			string requestInfo = $"(`{uploadRequest.ClientVersion}` | `{uploadRequest.OperatingSystem}` | `{uploadRequest.BuildMode}` | `{uploadRequest.Client}`{replayString}{localReplayString} | `{replayData}` | `Status {uploadRequest.Status}`)";

			// TODO: Don't log in development.
			DiscordChannel? channel = DevilDaggersInfoServerConstants.Channels[Channel.MonitoringCustomLeaderboard].DiscordChannel;
			if (channel == null)
				return;

			if (!string.IsNullOrEmpty(errorMessage))
				await channel.SendMessageAsyncSafe($":{errorEmoteNameOverride ?? "warning"}: Upload failed for user `{uploadRequest.PlayerName}` (`{uploadRequest.PlayerId}`) for `{spawnsetIdentification}`. {requestInfo}\n**{errorMessage}**");
			else
				await channel.SendMessageAsyncSafe($":white_check_mark: `{uploadRequest.PlayerName}` just submitted a score of `{FormatTimeString(uploadRequest.Time)}` to `{spawnsetIdentification}`. {requestInfo}");
		}
		catch
		{
			// Ignore exceptions that occurred while attempting to log.
		}
	}

	private static void Populate(CustomEntryDataEntity ced, List<AddGameState> gameStates)
	{
		ced.GemsCollectedData = CompressProperty(gs => gs.GemsCollected);
		ced.EnemiesKilledData = CompressProperty(gs => gs.EnemiesKilled);
		ced.DaggersFiredData = CompressProperty(gs => gs.DaggersFired);
		ced.DaggersHitData = CompressProperty(gs => gs.DaggersHit);
		ced.EnemiesAliveData = CompressProperty(gs => gs.EnemiesAlive);
		ced.HomingStoredData = CompressProperty(gs => gs.HomingDaggers);
		ced.HomingEatenData = CompressProperty(gs => gs.HomingDaggersEaten);
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

		byte[] CompressProperty(Func<AddGameState, int> propertySelector)
			=> IntegerArrayCompressor.CompressData(gameStates.Select(propertySelector).ToArray());
	}

	private static CustomLeaderboardsClient GetClientFromString(string clientString) => clientString switch
	{
		"DevilDaggersCustomLeaderboards" or "0" => CustomLeaderboardsClient.DevilDaggersCustomLeaderboards,
		"ddstats-rust" or "1" => CustomLeaderboardsClient.DdstatsRust,
		_ => throw new Exception("Unknown CustomLeaderboardsClient."),
	};
}
