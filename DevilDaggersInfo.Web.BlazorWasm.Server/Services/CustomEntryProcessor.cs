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

	public async Task<ActionResult<GetUploadSuccess>> ProcessUploadRequestAsync(AddUploadRequest uploadRequest)
	{
		// Check if the submission actually came from an allowed program.
		string expected = CreateValidation(uploadRequest);
		string actual = DecryptValidation(uploadRequest.Validation);
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
		if (!(uploadRequest.Status is 3 or 4 or 5))
			throw LogAndCreateValidationException(uploadRequest, $"Game status {uploadRequest.Status} is not valid.", null, "rotating_light");

		// Check for existing spawnset.
		SpawnsetHashCacheData? spawnsetHashData = _spawnsetHashCache.GetSpawnset(uploadRequest.SurvivalHashMd5);
		string? spawnsetName = spawnsetHashData?.Name;
		if (string.IsNullOrEmpty(spawnsetName))
			throw LogAndCreateValidationException(uploadRequest, "This spawnset doesn't exist on DevilDaggers.info.");

		// Validate replay buffer.
		if (uploadRequest.ReplayData == null)
			throw LogAndCreateValidationException(uploadRequest, "Replay data is required.");

		if (uploadRequest.ReplayData.Length < 88)
			throw LogAndCreateValidationException(uploadRequest, $"Invalid replay (length {uploadRequest.ReplayData.Length}).");

		// Validate replay buffer spawnset hash in case of replay.
		if (uploadRequest.IsReplay)
		{
			int replaySpawnsetLength = BitConverter.ToInt32(uploadRequest.ReplayData, 84);
			if (replaySpawnsetLength < 0 || replaySpawnsetLength > SpawnsetConstants.MaxFileSize)
				throw LogAndCreateValidationException(uploadRequest, $"Invalid replay spawnset size ({replaySpawnsetLength} / {SpawnsetConstants.MaxFileSize}).");
			if (uploadRequest.ReplayData.Length < 88 + replaySpawnsetLength)
				throw LogAndCreateValidationException(uploadRequest, $"Replay spawnset size out of bounds ({replaySpawnsetLength} / {uploadRequest.ReplayData.Length}).");

			byte[] replaySpawnset = new byte[replaySpawnsetLength];
			Buffer.BlockCopy(uploadRequest.ReplayData, 88, replaySpawnset, 0, replaySpawnsetLength);

			if (!ArrayUtils.AreEqual(MD5.HashData(replaySpawnset), uploadRequest.SurvivalHashMd5))
				throw LogAndCreateValidationException(uploadRequest, "Spawnset in replay does not match detected spawnset.", spawnsetName, "rotating_light");
		}

		// Perform database operations from now on.

		// Add the player or update the username. Also check for banned user.
		PlayerEntity? player = _dbContext.Players.FirstOrDefault(p => p.Id == uploadRequest.PlayerId);
		if (player != null)
		{
			if (player.IsBannedFromDdcl)
				throw LogAndCreateValidationException(uploadRequest, "Banned.", spawnsetName, "rotating_light");

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

		// Check for existing leaderboard.
		CustomLeaderboardEntity? customLeaderboard = _dbContext.CustomLeaderboards.Include(cl => cl.Spawnset).ThenInclude(sf => sf.Player).FirstOrDefault(cl => cl.Spawnset.Name == spawnsetName);
		if (customLeaderboard == null)
			throw LogAndCreateValidationException(uploadRequest, "This spawnset exists on DevilDaggers.info, but doesn't have a leaderboard.", spawnsetName);

		CustomLeaderboardsClient client = GetClientFromString(uploadRequest.Client);

		// Validate game mode.
		GameMode requiredGameMode = customLeaderboard.Category.GetRequiredGameModeForCategory();
		if (uploadRequest.GameMode != (byte)requiredGameMode)
			throw LogAndCreateValidationException(uploadRequest, $"Incorrect game mode '{(GameMode)uploadRequest.GameMode}' for category '{customLeaderboard.Category}'. Must be '{requiredGameMode}'.", spawnsetName);

		// Validate TimeAttack and Race.
		if (customLeaderboard.Category is CustomLeaderboardCategory.TimeAttack or CustomLeaderboardCategory.Race && !uploadRequest.TimeAttackOrRaceFinished)
			throw LogAndCreateValidationException(uploadRequest, $"Didn't complete the {customLeaderboard.Category} spawnset.", spawnsetName);

		// Validate Pacifist.
		if (customLeaderboard.Category == CustomLeaderboardCategory.Pacifist && uploadRequest.EnemiesKilled > 0)
			throw LogAndCreateValidationException(uploadRequest, $"Killed {uploadRequest.EnemiesKilled} {(uploadRequest.EnemiesKilled == 1 ? "enemy" : "enemies")}. Can't submit score to {CustomLeaderboardCategory.Pacifist} leaderboard.", spawnsetName);

		bool isAscending = customLeaderboard.Category.IsAscending();

		// At this point, the submission is accepted.

		// Make sure HomingDaggers is not negative (happens rarely as a bug, and also for spawnsets with homing disabled which we don't want to display values for anyway).
		uploadRequest.HomingDaggers = Math.Max(0, uploadRequest.HomingDaggers);
		uploadRequest.GameData.HomingDaggers = Array.ConvertAll(uploadRequest.GameData.HomingDaggers, i => Math.Max(0, i));

		// Calculate the new rank.
		List<CustomEntryEntity> entries = FetchEntriesFromDatabase(customLeaderboard, isAscending);
		int rank = isAscending ? entries.Count(e => e.Time <= uploadRequest.Time) + 1 : entries.Count(e => e.Time >= uploadRequest.Time) + 1;
		int totalPlayers = entries.Count;

		CustomEntryEntity? customEntry = entries.Find(e => e.PlayerId == uploadRequest.PlayerId);
		if (customEntry == null)
			return await ProcessNewScore(uploadRequest, customLeaderboard, rank, isAscending, spawnsetName);

		// Due to a bug in the game, we need to manually fix the request's time. The time gains a couple extra ticks if the run is a replay.
		// We don't want replays to overwrite the real score (this spams messages and is incorrect).
		// The amount of overflowing ticks varies between 0 and 3 (the longer the run the higher the amount).
		// Simply reset the time to the original when all data is the same.
		// TODO: Also apply this to ascending leaderboards.
		const int timeThreshold = 1000; // 0.1 seconds (or 6 ticks).
		const int gemThreshold = 2;
		const int killThreshold = 5;
		bool isTinyHighscore = uploadRequest.Time > customEntry.Time && uploadRequest.Time < customEntry.Time + timeThreshold;
		bool gemsAlmostTheSame = uploadRequest.GemsCollected >= customEntry.GemsCollected - gemThreshold && uploadRequest.GemsCollected <= customEntry.GemsCollected + gemThreshold;
		bool killsAlmostTheSame = uploadRequest.EnemiesKilled >= customEntry.EnemiesKilled - killThreshold && uploadRequest.EnemiesKilled <= customEntry.EnemiesKilled + killThreshold;
		bool deathTypeTheSame = uploadRequest.DeathType == customEntry.DeathType;
		if (uploadRequest.IsReplay && !isAscending && isTinyHighscore && gemsAlmostTheSame && killsAlmostTheSame && deathTypeTheSame)
			uploadRequest.Time = customEntry.Time;

		// User is already on the leaderboard, but did not get a better score.
		if (isAscending && customEntry.Time <= uploadRequest.Time || !isAscending && customEntry.Time >= uploadRequest.Time)
			return ProcessNoHighscore(uploadRequest, customLeaderboard, entries, spawnsetName);

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
		customEntry.Client = client;

		// Update the entry data.
		CustomEntryDataEntity? customEntryData = _dbContext.CustomEntryData.FirstOrDefault(ced => ced.CustomEntryId == customEntry.Id);
		if (customEntryData == null)
		{
			customEntryData = new() { CustomEntryId = customEntry.Id };
			PopulateCustomEntryData(customEntryData, uploadRequest);
			_dbContext.CustomEntryData.Add(customEntryData);
		}
		else
		{
			PopulateCustomEntryData(customEntryData, uploadRequest);
		}

		UpdateLeaderboardStatistics(customLeaderboard);

		_dbContext.SaveChanges();

		await WriteReplayFile(customEntry.Id, uploadRequest.ReplayData);

		// Fetch the entries again after having modified the leaderboard.
		entries = FetchEntriesFromDatabase(customLeaderboard, isAscending);

		await TrySendLeaderboardMessage(customLeaderboard, $"`{uploadRequest.PlayerName}` just got {FormatTimeString(uploadRequest.Time)} seconds on the `{spawnsetName}` leaderboard, beating their previous highscore of {FormatTimeString(uploadRequest.Time - timeDiff)} by {FormatTimeString(Math.Abs(timeDiff))} seconds!", rank, totalPlayers, uploadRequest.Time);
		Log(uploadRequest, spawnsetName);

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

	private static string CreateValidation(AddUploadRequest uploadRequest)
	{
		return string.Join(
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
			string.Join(",", uploadRequest.LevelUpTime2, uploadRequest.LevelUpTime3, uploadRequest.LevelUpTime4));
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

			Channel channel = _environment.IsDevelopment() ? Channel.MonitoringTest : Channel.CustomLeaderboards;
			DiscordChannel? discordChannel = DevilDaggersInfoServerConstants.Channels[channel].DiscordChannel;
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

	private void Log(AddUploadRequest uploadRequest, string? spawnsetName, string? errorMessage = null, string? errorEmoteNameOverride = null)
	{
		_stopwatch.Stop();

		string spawnsetIdentification = GetSpawnsetHashOrName(uploadRequest.SurvivalHashMd5, spawnsetName);

		string replayData = $"Replay data {uploadRequest.ReplayData.Length:N0} bytes";
		string replayString = uploadRequest.IsReplay ? " | `Replay`" : string.Empty;
		string localReplayString = uploadRequest.Status == 8 ? $" | `Local replay from {uploadRequest.ReplayPlayerId}`" : string.Empty;
		string requestInfo = $"(`{uploadRequest.ClientVersion}` | `{uploadRequest.OperatingSystem}` | `{uploadRequest.BuildMode}` | `{uploadRequest.Client}`{replayString}{localReplayString} | `{replayData}` | `Status {uploadRequest.Status}`)";

		DiscordChannel? discordChannel = DevilDaggersInfoServerConstants.Channels[Channel.MonitoringCustomLeaderboard].DiscordChannel;
		if (discordChannel == null)
			return;

		if (!string.IsNullOrEmpty(errorMessage))
			_logContainerService.AddClLog($":{errorEmoteNameOverride ?? "warning"}: `{TimeUtils.TicksToTimeString(_stopwatch.ElapsedTicks)}` Upload failed for user `{uploadRequest.PlayerName}` (`{uploadRequest.PlayerId}`) for `{spawnsetIdentification}`. {requestInfo}\n**{errorMessage}**");
		else
			_logContainerService.AddClLog($":white_check_mark: `{TimeUtils.TicksToTimeString(_stopwatch.ElapsedTicks)}` `{uploadRequest.PlayerName}` just submitted a score of `{FormatTimeString(uploadRequest.Time)}` to `{spawnsetIdentification}`. {requestInfo}");
	}

	private static void PopulateCustomEntryData(CustomEntryDataEntity ced, AddUploadRequest uploadRequest)
	{
		ced.GemsCollectedData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.GemsCollected);
		ced.EnemiesKilledData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.EnemiesKilled);
		ced.DaggersFiredData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.DaggersFired);
		ced.DaggersHitData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.DaggersHit);
		ced.EnemiesAliveData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.EnemiesAlive);
		ced.HomingStoredData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.HomingDaggers);
		ced.HomingEatenData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.HomingDaggersEaten);
		ced.GemsDespawnedData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.GemsDespawned);
		ced.GemsEatenData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.GemsEaten);
		ced.GemsTotalData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.GemsTotal);

		ced.Skull1sAliveData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.Skull1sAlive);
		ced.Skull2sAliveData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.Skull2sAlive);
		ced.Skull3sAliveData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.Skull3sAlive);
		ced.SpiderlingsAliveData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.SpiderlingsAlive);
		ced.Skull4sAliveData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.Skull4sAlive);
		ced.Squid1sAliveData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.Squid1sAlive);
		ced.Squid2sAliveData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.Squid2sAlive);
		ced.Squid3sAliveData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.Squid3sAlive);
		ced.CentipedesAliveData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.CentipedesAlive);
		ced.GigapedesAliveData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.GigapedesAlive);
		ced.Spider1sAliveData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.Spider1sAlive);
		ced.Spider2sAliveData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.Spider2sAlive);
		ced.LeviathansAliveData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.LeviathansAlive);
		ced.OrbsAliveData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.OrbsAlive);
		ced.ThornsAliveData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.ThornsAlive);
		ced.GhostpedesAliveData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.GhostpedesAlive);
		ced.SpiderEggsAliveData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.SpiderEggsAlive);

		ced.Skull1sKilledData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.Skull1sKilled);
		ced.Skull2sKilledData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.Skull2sKilled);
		ced.Skull3sKilledData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.Skull3sKilled);
		ced.SpiderlingsKilledData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.SpiderlingsKilled);
		ced.Skull4sKilledData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.Skull4sKilled);
		ced.Squid1sKilledData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.Squid1sKilled);
		ced.Squid2sKilledData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.Squid2sKilled);
		ced.Squid3sKilledData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.Squid3sKilled);
		ced.CentipedesKilledData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.CentipedesKilled);
		ced.GigapedesKilledData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.GigapedesKilled);
		ced.Spider1sKilledData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.Spider1sKilled);
		ced.Spider2sKilledData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.Spider2sKilled);
		ced.LeviathansKilledData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.LeviathansKilled);
		ced.OrbsKilledData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.OrbsKilled);
		ced.ThornsKilledData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.ThornsKilled);
		ced.GhostpedesKilledData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.GhostpedesKilled);
		ced.SpiderEggsKilledData = IntegerArrayCompressor.CompressData(uploadRequest.GameData.SpiderEggsKilled);
	}

	private static CustomLeaderboardsClient GetClientFromString(string clientString) => clientString switch
	{
		"DevilDaggersCustomLeaderboards" => CustomLeaderboardsClient.DevilDaggersCustomLeaderboards,
		"ddstats-rust" => CustomLeaderboardsClient.DdstatsRust,
		_ => throw new Exception("Unknown CustomLeaderboardsClient."),
	};

	private async Task<GetUploadSuccess> ProcessNewScore(AddUploadRequest uploadRequest, CustomLeaderboardEntity customLeaderboard, int rank, bool isAscending, string spawnsetName)
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
		PopulateCustomEntryData(newCustomEntryData, uploadRequest);
		_dbContext.CustomEntryData.Add(newCustomEntryData);

		UpdateLeaderboardStatistics(customLeaderboard);

		_dbContext.SaveChanges();

		await WriteReplayFile(newCustomEntry.Id, uploadRequest.ReplayData);

		// Fetch the entries again after having modified the leaderboard.
		List<CustomEntryEntity> entries = FetchEntriesFromDatabase(customLeaderboard, isAscending);
		int totalPlayers = entries.Count;

		await TrySendLeaderboardMessage(customLeaderboard, $"`{uploadRequest.PlayerName}` just entered the `{spawnsetName}` leaderboard!", rank, totalPlayers, uploadRequest.Time);
		Log(uploadRequest, spawnsetName);

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

	private GetUploadSuccess ProcessNoHighscore(AddUploadRequest uploadRequest, CustomLeaderboardEntity customLeaderboard, List<CustomEntryEntity> entries, string spawnsetName)
	{
		if (!uploadRequest.IsReplay)
		{
			UpdateLeaderboardStatistics(customLeaderboard);
			_dbContext.SaveChanges();
		}

		Log(uploadRequest, spawnsetName);

		return new GetUploadSuccess
		{
			Message = $"No new highscore for {customLeaderboard.Spawnset.Name}.",
			TotalPlayers = entries.Count,
			Leaderboard = customLeaderboard.ToGetCustomLeaderboardDdcl(),
			Category = customLeaderboard.Category,
			Entries = entries.ConvertAll(e => e.ToGetCustomEntryDdcl()),
			IsNewPlayerOnThisLeaderboard = false,
		};
	}
}
