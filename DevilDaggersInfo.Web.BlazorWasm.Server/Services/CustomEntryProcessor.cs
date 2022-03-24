using DevilDaggersInfo.Core.Encryption;
using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.SpawnsetHashes;
using DevilDaggersInfo.Web.BlazorWasm.Server.Converters.Public;
using DevilDaggersInfo.Web.BlazorWasm.Server.HostedServices.DdInfoDiscordBot;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomEntries;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Utils;
using DSharpPlus.Entities;
using System.Web;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Services;

public class CustomEntryProcessor
{
	private readonly ApplicationDbContext _dbContext;
	private readonly ILogger<CustomEntryProcessor> _logger;
	private readonly SpawnsetHashCache _spawnsetHashCache;
	private readonly IFileSystemService _fileSystemService;
	private readonly IWebHostEnvironment _environment;
	private readonly LogContainerService _logContainerService;

	private readonly AesBase32Wrapper _encryptionWrapper;
	private readonly Stopwatch _stopwatch;

	public CustomEntryProcessor(ApplicationDbContext dbContext, ILogger<CustomEntryProcessor> logger, SpawnsetHashCache spawnsetHashCache, IFileSystemService fileSystemService, IWebHostEnvironment environment, IConfiguration configuration, LogContainerService logContainerService)
	{
		_dbContext = dbContext;
		_logger = logger;
		_spawnsetHashCache = spawnsetHashCache;
		_fileSystemService = fileSystemService;
		_environment = environment;
		_logContainerService = logContainerService;

		IConfigurationSection section = configuration.GetRequiredSection("CustomLeaderboardSecrets");
		_encryptionWrapper = new(section["InitializationVector"], section["Password"], section["Salt"]);

		_stopwatch = Stopwatch.StartNew();
	}

	public async Task<GetUploadSuccess> ProcessUploadRequestAsync(AddUploadRequest uploadRequest)
	{
		// Check if the submission actually came from an allowed program.
		string expected = uploadRequest.CreateValidation();
		string actual;
		try
		{
			actual = _encryptionWrapper.DecodeAndDecrypt(HttpUtility.HtmlDecode(uploadRequest.Validation));
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Could not decrypt validation '{validation}'.", uploadRequest.Validation);
			throw LogAndCreateValidationException(uploadRequest, $"Could not decrypt validation '{uploadRequest.Validation}'.", null, "rotating_light");
		}

		if (actual != expected)
			throw LogAndCreateValidationException(uploadRequest, $"Invalid submission for {uploadRequest.Validation}.\nExpected: {expected}\nActual:   {actual}", null, "rotating_light");

		// Check for required client and version.
		var tool = _dbContext.Tools.Select(t => new { t.Name, t.RequiredVersionNumber }).FirstOrDefault(t => t.Name == uploadRequest.Client);
		if (tool == null)
			throw LogAndCreateValidationException(uploadRequest, $"'{uploadRequest.Client}' is not a known tool and submissions will not be accepted.");

		Version clientVersionParsed = Version.Parse(uploadRequest.ClientVersion);
		if (clientVersionParsed < Version.Parse(tool.RequiredVersionNumber))
			throw LogAndCreateValidationException(uploadRequest, $"You are using an unsupported and outdated version of {uploadRequest.Client}. Please update the program.");

		// Reject local replays as they can easily be manipulated.
		if (uploadRequest.Status == 8)
			throw LogAndCreateValidationException(uploadRequest, "Local replays cannot be validated.");

		// Reject other invalid statuses.
		if (uploadRequest.Status is not (3 or 4 or 5))
			throw LogAndCreateValidationException(uploadRequest, $"Game status {uploadRequest.Status} is not valid.", null, "rotating_light");

		// Check for existing spawnset.
		SpawnsetHashCacheData? spawnsetHashData = _spawnsetHashCache.GetSpawnset(uploadRequest.SurvivalHashMd5);
		string? spawnsetName = spawnsetHashData?.Name;
		if (string.IsNullOrEmpty(spawnsetName))
			throw LogAndCreateValidationException(uploadRequest, "This spawnset doesn't exist on DevilDaggers.info.");

		ValidateReplayBuffer(uploadRequest, spawnsetName);

		// Perform database operations from now on.

		// Add new player if necessary.
		var player = await _dbContext.Players.Select(p => new { p.Id, p.IsBannedFromDdcl }).FirstOrDefaultAsync(p => p.Id == uploadRequest.PlayerId);
		if (player == null)
		{
			await _dbContext.Players.AddAsync(new()
			{
				Id = uploadRequest.PlayerId,
				PlayerName = uploadRequest.PlayerName,
			});
		}
		else if (player.IsBannedFromDdcl)
		{
			throw LogAndCreateValidationException(uploadRequest, "Banned.", spawnsetName, "rotating_light");
		}

		// Check for existing leaderboard.
		CustomLeaderboardEntity? customLeaderboard = await _dbContext.CustomLeaderboards.Include(cl => cl.Spawnset).FirstOrDefaultAsync(cl => cl.Spawnset.Name == spawnsetName);
		if (customLeaderboard == null)
			throw LogAndCreateValidationException(uploadRequest, "This spawnset exists on DevilDaggers.info, but doesn't have a leaderboard.", spawnsetName);

		// Validate game mode.
		GameMode requiredGameMode = customLeaderboard.Category.GetRequiredGameModeForCategory();
		if (uploadRequest.GameMode != (byte)requiredGameMode)
			throw LogAndCreateValidationException(uploadRequest, $"Incorrect game mode '{(GameMode)uploadRequest.GameMode}' for category '{customLeaderboard.Category}'. Must be '{requiredGameMode}'.", spawnsetName);

		// Validate TimeAttack and Race.
		if (customLeaderboard.Category is CustomLeaderboardCategory.TimeAttack or CustomLeaderboardCategory.Race && !uploadRequest.TimeAttackOrRaceFinished)
			throw LogAndCreateValidationException(uploadRequest, $"Didn't complete the {customLeaderboard.Category} spawnset.", spawnsetName);

		// Validate Pacifist.
		if (customLeaderboard.Category == CustomLeaderboardCategory.Pacifist && uploadRequest.DaggersHit > 0)
			throw LogAndCreateValidationException(uploadRequest, $"Counted {uploadRequest.DaggersHit} dagger {(uploadRequest.DaggersHit == 1 ? "hit" : "hits")}. Can't submit score to {CustomLeaderboardCategory.Pacifist} leaderboard.", spawnsetName);

		bool isAscending = customLeaderboard.Category.IsAscending();

		// At this point, the submission is accepted.

		// Make sure HomingDaggers is not negative (happens rarely as a bug, and also for spawnsets with homing disabled which we don't want to display values for anyway).
		uploadRequest.HomingDaggers = Math.Max(0, uploadRequest.HomingDaggers);
		uploadRequest.GameData.HomingDaggers = Array.ConvertAll(uploadRequest.GameData.HomingDaggers, i => Math.Max(0, i));

		// Calculate the new rank.
		List<CustomEntryEntity> entries = await FetchEntriesFromDatabaseAsync(customLeaderboard, isAscending);
		int requestTimeAsInt = uploadRequest.GetTime();
		int rank = isAscending ? entries.Count(e => e.Time <= requestTimeAsInt) + 1 : entries.Count(e => e.Time >= requestTimeAsInt) + 1;
		int totalPlayers = entries.Count;

		CustomEntryEntity? customEntry = entries.Find(e => e.PlayerId == uploadRequest.PlayerId);
		if (customEntry == null)
			return await ProcessNewScoreAsync(uploadRequest, customLeaderboard, rank, isAscending, spawnsetName);

		// Treat identical replays as no highscore.
		if (uploadRequest.IsReplay && IsReplayTimeAlmostTheSame(requestTimeAsInt, customEntry.Time) && await IsReplayFileTheSame(customEntry.Id, uploadRequest.ReplayData))
		{
			_logger.LogWarning("Score submission replay time was modified because of identical replay (database: {originalTime} - request: {replayTime}).", FormatTimeString(customEntry.Time), FormatTimeString(requestTimeAsInt));
			return await ProcessNoHighscoreAsync(uploadRequest, customLeaderboard, entries, spawnsetName);
		}

		// User is already on the leaderboard, but did not get a better score.
		if (isAscending && customEntry.Time <= requestTimeAsInt || !isAscending && customEntry.Time >= requestTimeAsInt)
			return await ProcessNoHighscoreAsync(uploadRequest, customLeaderboard, entries, spawnsetName);

		return await ProcessHighscoreAsync(uploadRequest, customLeaderboard, entries, spawnsetName, isAscending, rank, totalPlayers, customEntry);
	}

	private void ValidateReplayBuffer(AddUploadRequest uploadRequest, string spawnsetName)
	{
		// TODO: Check might be unnecessary.
		if (uploadRequest.ReplayData == null)
			throw LogAndCreateValidationException(uploadRequest, "Replay data is required.");

		using MemoryStream ms = new(uploadRequest.ReplayData);
		using BinaryReader br = new(ms);
		br.BaseStream.Seek(50, SeekOrigin.Begin);
		int usernameLength = br.ReadInt32();
		br.BaseStream.Seek(usernameLength + 26, SeekOrigin.Current);

		// Validate replay buffer spawnset hash.
		int spawnsetLength = br.ReadInt32();
		if (spawnsetLength < 0 || spawnsetLength > SpawnsetConstants.MaxFileSize)
			throw LogAndCreateValidationException(uploadRequest, $"Invalid replay spawnset size ({spawnsetLength} / {SpawnsetConstants.MaxFileSize}).");

		byte[] replaySpawnset = br.ReadBytes(spawnsetLength);
		if (!ArrayUtils.AreEqual(MD5.HashData(replaySpawnset), uploadRequest.SurvivalHashMd5))
			throw LogAndCreateValidationException(uploadRequest, "Spawnset in replay does not match detected spawnset.", spawnsetName, "rotating_light");
	}

	private async Task<GetUploadSuccess> ProcessNewScoreAsync(AddUploadRequest uploadRequest, CustomLeaderboardEntity customLeaderboard, int rank, bool isAscending, string spawnsetName)
	{
		// Add new custom entry to this leaderboard.
		CustomEntryEntity newCustomEntry = new()
		{
			PlayerId = uploadRequest.PlayerId,
			Time = uploadRequest.GetTime(),
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
			LevelUpTime2 = uploadRequest.GetLevelUpTime2(),
			LevelUpTime3 = uploadRequest.GetLevelUpTime3(),
			LevelUpTime4 = uploadRequest.GetLevelUpTime4(),
			SubmitDate = DateTime.UtcNow,
			ClientVersion = uploadRequest.ClientVersion,
			Client = uploadRequest.Client.GetClientFromString(),
			CustomLeaderboard = customLeaderboard,
		};
		await _dbContext.CustomEntries.AddAsync(newCustomEntry);

		CustomEntryDataEntity newCustomEntryData = new() { CustomEntry = newCustomEntry };
		newCustomEntryData.Populate(uploadRequest);
		await _dbContext.CustomEntryData.AddAsync(newCustomEntryData);

		UpdateLeaderboardStatistics(customLeaderboard);

		await _dbContext.SaveChangesAsync();

		await WriteReplayFile(newCustomEntry.Id, uploadRequest.ReplayData);

		// Fetch the entries again after having modified the leaderboard.
		List<CustomEntryEntity> entries = await FetchEntriesFromDatabaseAsync(customLeaderboard, isAscending);
		int totalPlayers = entries.Count;

		await TrySendLeaderboardMessage(customLeaderboard, $"`{uploadRequest.PlayerName}` just entered the `{spawnsetName}` leaderboard!", rank, totalPlayers, newCustomEntry.Time);
		Log(uploadRequest, spawnsetName);

		List<int> replayIds = GetExistingReplayIds(customLeaderboard);
		return new()
		{
			Message = $"Welcome to the {spawnsetName} leaderboard!",
			TotalPlayers = totalPlayers,
			Leaderboard = customLeaderboard.ToGetCustomLeaderboardDdcl(),
			Category = customLeaderboard.Category,
			Entries = entries.ConvertAll(e => e.ToGetCustomEntryDdcl(replayIds.Contains(e.Id))),
			IsNewPlayerOnThisLeaderboard = true,
			IsHighscore = true,
			RankState = new(rank),
			TimeState = new(newCustomEntry.Time.ToSecondsTime()),
			EnemiesKilledState = new(newCustomEntry.EnemiesKilled),
			GemsCollectedState = new(newCustomEntry.GemsCollected),
			GemsDespawnedState = new(newCustomEntry.GemsDespawned),
			GemsEatenState = new(newCustomEntry.GemsEaten),
			DaggersHitState = new(newCustomEntry.DaggersHit),
			DaggersFiredState = new(newCustomEntry.DaggersFired),
			EnemiesAliveState = new(newCustomEntry.EnemiesAlive),
			HomingStoredState = new(newCustomEntry.HomingStored),
			HomingEatenState = new(newCustomEntry.HomingEaten),
			LevelUpTime2State = new(newCustomEntry.LevelUpTime2.ToSecondsTime()),
			LevelUpTime3State = new(newCustomEntry.LevelUpTime3.ToSecondsTime()),
			LevelUpTime4State = new(newCustomEntry.LevelUpTime4.ToSecondsTime()),
			Rank = rank,
			Time = uploadRequest.GetTime(),
			EnemiesKilled = uploadRequest.EnemiesKilled,
			GemsCollected = uploadRequest.GemsCollected,
			GemsDespawned = uploadRequest.GemsDespawned,
			GemsEaten = uploadRequest.GemsEaten,
			DaggersHit = uploadRequest.DaggersHit,
			DaggersFired = uploadRequest.DaggersFired,
			EnemiesAlive = uploadRequest.EnemiesAlive,
			HomingDaggers = uploadRequest.HomingDaggers,
			HomingDaggersEaten = uploadRequest.HomingDaggersEaten,
			LevelUpTime2 = uploadRequest.GetLevelUpTime2(),
			LevelUpTime3 = uploadRequest.GetLevelUpTime3(),
			LevelUpTime4 = uploadRequest.GetLevelUpTime4(),
		};
	}

	private async Task<GetUploadSuccess> ProcessNoHighscoreAsync(AddUploadRequest uploadRequest, CustomLeaderboardEntity customLeaderboard, List<CustomEntryEntity> entries, string spawnsetName)
	{
		if (!uploadRequest.IsReplay)
		{
			UpdateLeaderboardStatistics(customLeaderboard);
			await _dbContext.SaveChangesAsync();
		}

		Log(uploadRequest, spawnsetName);

		List<int> replayIds = GetExistingReplayIds(customLeaderboard);
		return new GetUploadSuccess
		{
			Message = $"No new highscore for {customLeaderboard.Spawnset.Name}.",
			TotalPlayers = entries.Count,
			Leaderboard = customLeaderboard.ToGetCustomLeaderboardDdcl(),
			Category = customLeaderboard.Category,
			Entries = entries.ConvertAll(e => e.ToGetCustomEntryDdcl(replayIds.Contains(e.Id))),
			IsNewPlayerOnThisLeaderboard = false,
			IsHighscore = false,
		};
	}

	private async Task<GetUploadSuccess> ProcessHighscoreAsync(
		AddUploadRequest uploadRequest,
		CustomLeaderboardEntity customLeaderboard,
		List<CustomEntryEntity> entries,
		string spawnsetName,
		bool isAscending,
		int rank,
		int totalPlayers,
		CustomEntryEntity customEntry)
	{
		// Calculate the old rank.
		int oldRank = isAscending ? entries.Count(e => e.Time < customEntry.Time) + 1 : entries.Count(e => e.Time > customEntry.Time) + 1;

		int rankDiff = oldRank - rank;
		int timeDiff = uploadRequest.GetTime() - customEntry.Time;
		int gemsCollectedDiff = uploadRequest.GemsCollected - customEntry.GemsCollected;
		int enemiesKilledDiff = uploadRequest.EnemiesKilled - customEntry.EnemiesKilled;
		int daggersFiredDiff = uploadRequest.DaggersFired - customEntry.DaggersFired;
		int daggersHitDiff = uploadRequest.DaggersHit - customEntry.DaggersHit;
		int enemiesAliveDiff = uploadRequest.EnemiesAlive - customEntry.EnemiesAlive;
		int homingStoredDiff = uploadRequest.HomingDaggers - customEntry.HomingStored;
		int homingEatenDiff = uploadRequest.HomingDaggersEaten - customEntry.HomingEaten;
		int gemsDespawnedDiff = uploadRequest.GemsDespawned - customEntry.GemsDespawned;
		int gemsEatenDiff = uploadRequest.GemsEaten - customEntry.GemsEaten;
		int gemsTotalDiff = uploadRequest.GemsTotal - customEntry.GemsTotal;
		int levelUpTime2Diff = uploadRequest.GetLevelUpTime2() - customEntry.LevelUpTime2;
		int levelUpTime3Diff = uploadRequest.GetLevelUpTime3() - customEntry.LevelUpTime3;
		int levelUpTime4Diff = uploadRequest.GetLevelUpTime4() - customEntry.LevelUpTime4;

		// Update the entry.
		customEntry.Time = uploadRequest.GetTime();
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
		customEntry.LevelUpTime2 = uploadRequest.GetLevelUpTime2();
		customEntry.LevelUpTime3 = uploadRequest.GetLevelUpTime3();
		customEntry.LevelUpTime4 = uploadRequest.GetLevelUpTime4();
		customEntry.SubmitDate = DateTime.UtcNow;
		customEntry.ClientVersion = uploadRequest.ClientVersion;
		customEntry.Client = uploadRequest.Client.GetClientFromString();

		// Update the entry data.
		CustomEntryDataEntity? customEntryData = await _dbContext.CustomEntryData.FirstOrDefaultAsync(ced => ced.CustomEntryId == customEntry.Id);
		if (customEntryData == null)
		{
			customEntryData = new() { CustomEntryId = customEntry.Id };
			customEntryData.Populate(uploadRequest);
			await _dbContext.CustomEntryData.AddAsync(customEntryData);
		}
		else
		{
			customEntryData.Populate(uploadRequest);
		}

		UpdateLeaderboardStatistics(customLeaderboard);

		await _dbContext.SaveChangesAsync();

		await WriteReplayFile(customEntry.Id, uploadRequest.ReplayData);

		// Fetch the entries again after having modified the leaderboard.
		entries = await FetchEntriesFromDatabaseAsync(customLeaderboard, isAscending);

		await TrySendLeaderboardMessage(customLeaderboard, $"`{uploadRequest.PlayerName}` just got {FormatTimeString(uploadRequest.GetTime())} seconds on the `{spawnsetName}` leaderboard, beating their previous highscore of {FormatTimeString(uploadRequest.GetTime() - timeDiff)} by {FormatTimeString(Math.Abs(timeDiff))} seconds!", rank, totalPlayers, uploadRequest.GetTime());
		Log(uploadRequest, spawnsetName);

		List<int> replayIds = GetExistingReplayIds(customLeaderboard);
		return new GetUploadSuccess
		{
			Message = $"NEW HIGHSCORE for {customLeaderboard.Spawnset.Name}!",
			TotalPlayers = totalPlayers,
			Leaderboard = customLeaderboard.ToGetCustomLeaderboardDdcl(),
			Category = customLeaderboard.Category,
			Entries = entries.ConvertAll(e => e.ToGetCustomEntryDdcl(replayIds.Contains(e.Id))),
			IsNewPlayerOnThisLeaderboard = false,
			IsHighscore = true,
			RankState = new(rank, rankDiff),
			TimeState = new(customEntry.Time.ToSecondsTime(), timeDiff.ToSecondsTime()),
			EnemiesKilledState = new(customEntry.EnemiesKilled, enemiesKilledDiff),
			GemsCollectedState = new(customEntry.GemsCollected, gemsCollectedDiff),
			GemsDespawnedState = new(customEntry.GemsDespawned, gemsDespawnedDiff),
			GemsEatenState = new(customEntry.GemsEaten, gemsEatenDiff),
			DaggersHitState = new(customEntry.DaggersHit, daggersHitDiff),
			DaggersFiredState = new(customEntry.DaggersFired, daggersFiredDiff),
			EnemiesAliveState = new(customEntry.EnemiesAlive, enemiesAliveDiff),
			HomingStoredState = new(customEntry.HomingStored, homingStoredDiff),
			HomingEatenState = new(customEntry.HomingEaten, homingEatenDiff),
			LevelUpTime2State = new(customEntry.LevelUpTime2.ToSecondsTime(), levelUpTime2Diff.ToSecondsTime()),
			LevelUpTime3State = new(customEntry.LevelUpTime3.ToSecondsTime(), levelUpTime3Diff.ToSecondsTime()),
			LevelUpTime4State = new(customEntry.LevelUpTime4.ToSecondsTime(), levelUpTime4Diff.ToSecondsTime()),
			Rank = rank,
			RankDiff = rankDiff,
			Time = uploadRequest.GetTime(),
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
			HomingDaggersDiff = homingStoredDiff,
			HomingDaggersEaten = uploadRequest.HomingDaggersEaten,
			HomingDaggersEatenDiff = homingEatenDiff,
			GemsDespawned = uploadRequest.GemsDespawned,
			GemsDespawnedDiff = gemsDespawnedDiff,
			GemsEaten = uploadRequest.GemsEaten,
			GemsEatenDiff = gemsEatenDiff,
			GemsTotal = uploadRequest.GemsTotal,
			GemsTotalDiff = gemsTotalDiff,
			LevelUpTime2 = uploadRequest.GetLevelUpTime2(),
			LevelUpTime2Diff = levelUpTime2Diff,
			LevelUpTime3 = uploadRequest.GetLevelUpTime3(),
			LevelUpTime3Diff = levelUpTime3Diff,
			LevelUpTime4 = uploadRequest.GetLevelUpTime4(),
			LevelUpTime4Diff = levelUpTime4Diff,
		};
	}

	private CustomEntryValidationException LogAndCreateValidationException(AddUploadRequest uploadRequest, string errorMessage, string? spawnsetName = null, string? errorEmoteNameOverride = null)
	{
		Log(uploadRequest, spawnsetName, errorMessage, errorEmoteNameOverride);
		return new CustomEntryValidationException(errorMessage);
	}

	private async Task WriteReplayFile(int customEntryId, byte[] replayData)
	{
		await IoFile.WriteAllBytesAsync(Path.Combine(_fileSystemService.GetPath(DataSubDirectory.CustomEntryReplays), $"{customEntryId}.ddreplay"), replayData);
	}

	private static bool IsReplayTimeAlmostTheSame(int requestTimeAsInt, int databaseTime)
	{
		if (requestTimeAsInt == databaseTime)
			return false;

		const int replayMarginErrorIn10thMillis = 1000;
		return requestTimeAsInt > databaseTime - replayMarginErrorIn10thMillis && requestTimeAsInt < databaseTime + replayMarginErrorIn10thMillis;
	}

	/// <summary>
	/// Due to a bug in the game, the final time sometimes gains a couple extra ticks if the run is a replay (more common in longer runs).
	/// We don't want these replay submissions to overwrite the real score (this spams messages and is incorrect).
	/// </summary>
	private async Task<bool> IsReplayFileTheSame(int customEntryId, byte[] newReplay)
	{
		string path = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.CustomEntryReplays), $"{customEntryId}.ddreplay");
		if (!IoFile.Exists(path))
			return false;

		byte[] originalReplay = await IoFile.ReadAllBytesAsync(path);
		if (originalReplay.Length != newReplay.Length)
			return false;

		for (int i = 0; i < originalReplay.Length; i++)
		{
			if (originalReplay[i] != newReplay[i])
				return false;
		}

		return true;
	}

	private static void UpdateLeaderboardStatistics(CustomLeaderboardEntity customLeaderboard)
	{
		customLeaderboard.DateLastPlayed = DateTime.UtcNow;
		customLeaderboard.TotalRunsSubmitted++;
	}

	private async Task<List<CustomEntryEntity>> FetchEntriesFromDatabaseAsync(CustomLeaderboardEntity? customLeaderboard, bool isAscending)
	{
		// Use tracking to update player score.
		return await _dbContext.CustomEntries
			.Include(ce => ce.Player)
			.Where(e => e.CustomLeaderboard == customLeaderboard)
			.OrderByMember(nameof(CustomEntryEntity.Time), isAscending)
			.ThenByMember(nameof(CustomEntryEntity.SubmitDate), true)
			.ToListAsync();
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

			DiscordChannel? discordChannel = DiscordServerConstants.GetDiscordChannel(Channel.CustomLeaderboards, _environment);
			if (discordChannel == null)
				return;

			await discordChannel.SendMessageAsyncSafe(null, builder.Build());
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error while attempting to send leaderboard message.");
		}
	}

	private static string FormatTimeString(int time)
		=> time.ToSecondsTime().ToString(FormatUtils.TimeFormat);

	private static string GetSpawnsetHashOrName(byte[] spawnsetHash, string? spawnsetName)
		=> string.IsNullOrEmpty(spawnsetName) ? spawnsetHash.ByteArrayToHexString() : spawnsetName;

	private void Log(AddUploadRequest uploadRequest, string? spawnsetName, string? errorMessage = null, string? errorEmoteNameOverride = null)
	{
		_stopwatch.Stop();

		string spawnsetIdentification = GetSpawnsetHashOrName(uploadRequest.SurvivalHashMd5, spawnsetName);

		string replayData = $"Replay data {uploadRequest.ReplayData.Length:N0} bytes";
		string replayString = uploadRequest.IsReplay ? " | `Replay`" : string.Empty;
		string localReplayString = uploadRequest.Status == 8 ? $" | `Local replay from {uploadRequest.ReplayPlayerId}`" : string.Empty;
		string requestInfo = $"(`{uploadRequest.ClientVersion}` | `{uploadRequest.OperatingSystem}` | `{uploadRequest.BuildMode}` | `{uploadRequest.Client}`{replayString}{localReplayString} | `{replayData}` | `Status {uploadRequest.Status}`)";

		if (!string.IsNullOrEmpty(errorMessage))
			_logContainerService.AddClLog(false, $":{errorEmoteNameOverride ?? "warning"}: `{TimeUtils.TicksToTimeString(_stopwatch.ElapsedTicks)}` Upload failed for user `{uploadRequest.PlayerName}` (`{uploadRequest.PlayerId}`) for `{spawnsetIdentification}`. {requestInfo}\n**{errorMessage}**");
		else
			_logContainerService.AddClLog(true, $":white_check_mark: `{TimeUtils.TicksToTimeString(_stopwatch.ElapsedTicks)}` `{uploadRequest.PlayerName}` just submitted a score of `{FormatTimeString(uploadRequest.GetTime())}` to `{spawnsetIdentification}`. {requestInfo}");
	}

	private List<int> GetExistingReplayIds(CustomLeaderboardEntity customLeaderboard)
	{
		return customLeaderboard.CustomEntries == null ? new() : customLeaderboard.CustomEntries
			.Where(ce => IoFile.Exists(Path.Combine(_fileSystemService.GetPath(DataSubDirectory.CustomEntryReplays), $"{ce.Id}.ddreplay")))
			.Select(ce => ce.Id)
			.ToList();
	}
}
