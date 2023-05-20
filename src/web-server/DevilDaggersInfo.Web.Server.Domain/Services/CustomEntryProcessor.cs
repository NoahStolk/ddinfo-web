using DevilDaggersInfo.Common;
using DevilDaggersInfo.Common.Extensions;
using DevilDaggersInfo.Core.CriteriaExpression;
using DevilDaggersInfo.Core.CriteriaExpression.Extensions;
using DevilDaggersInfo.Core.Encryption;
using DevilDaggersInfo.Core.Replay;
using DevilDaggersInfo.Core.Versioning;
using DevilDaggersInfo.Web.Server.Domain.Commands.CustomEntries;
using DevilDaggersInfo.Web.Server.Domain.Configuration;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using DevilDaggersInfo.Web.Server.Domain.Entities.Values;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Extensions;
using DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Web;

namespace DevilDaggersInfo.Web.Server.Domain.Services;

public class CustomEntryProcessor
{
	private readonly ApplicationDbContext _dbContext;
	private readonly ILogger<CustomEntryProcessor> _logger;
	private readonly IFileSystemService _fileSystemService;
	private readonly ICustomLeaderboardSubmissionLogger _submissionLogger;

	private readonly AesBase32Wrapper _encryptionWrapper;
	private readonly Stopwatch _stopwatch;

	public CustomEntryProcessor(ApplicationDbContext dbContext, ILogger<CustomEntryProcessor> logger, IFileSystemService fileSystemService, IOptions<CustomLeaderboardsOptions> customLeaderboardsOptions, ICustomLeaderboardSubmissionLogger submissionLogger)
	{
		_dbContext = dbContext;
		_logger = logger;
		_fileSystemService = fileSystemService;
		_submissionLogger = submissionLogger;

		_encryptionWrapper = new(customLeaderboardsOptions.Value.InitializationVector, customLeaderboardsOptions.Value.Password, customLeaderboardsOptions.Value.Salt);

		_stopwatch = Stopwatch.StartNew();
	}

	// Temporary hack until a proper spawnset repository is implemented.
	public bool IsUnitTest { get; init; }

	private void ValidateV2(UploadRequest uploadRequest)
	{
		string expected = uploadRequest.CreateValidationV2();
		string? actual = null;
		try
		{
			actual = _encryptionWrapper.DecodeAndDecrypt(HttpUtility.HtmlDecode(uploadRequest.Validation));
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Could not decrypt validation '{validation}'.", uploadRequest.Validation);
			LogAndThrowValidationException(uploadRequest, $"Could not decrypt validation '{uploadRequest.Validation}'.", null, "rotating_light");
		}

		if (actual != expected)
			LogAndThrowValidationException(uploadRequest, $"Invalid submission for {uploadRequest.Validation}.\n`Expected: {expected}`\n`Actual:   {actual}`", null, "rotating_light");
	}

	/// <summary>
	/// Processes an upload request.
	/// </summary>
	/// <exception cref="CustomEntryValidationException">Thrown when the request is invalid (unsupported client version, incorrect validation, invalid game status, non-existent spawnset, etc). This exception maps to HTTP 400.</exception>
	/// <returns>An <see cref="UploadResponse"/> is returned for every successful upload, as well as uploads rejected by criteria.</returns>
	public async Task<UploadResponse> ProcessUploadRequestAsync(UploadRequest uploadRequest)
	{
		// Check if the submission actually came from an allowed program.
		if (uploadRequest.ValidationVersion == 2)
			ValidateV2(uploadRequest);
		else
			LogAndThrowValidationException(uploadRequest, $"Validation version '{uploadRequest.ValidationVersion}' is not implemented.");

		// Check for required client and version.
		var tool = _dbContext.Tools.Select(t => new { t.Name, t.RequiredVersionNumber }).FirstOrDefault(t => t.Name == uploadRequest.Client);
		if (tool == null)
			LogAndThrowValidationException(uploadRequest, $"'{uploadRequest.Client}' is not a known tool and submissions will not be accepted.");

		AppVersion clientVersionParsed = AppVersion.Parse(uploadRequest.ClientVersion);
		if (clientVersionParsed < AppVersion.Parse(tool.RequiredVersionNumber))
			LogAndThrowValidationException(uploadRequest, $"You are using an unsupported and outdated version of {uploadRequest.Client}. Please update the program.");

		// Reject other invalid statuses.
		if (uploadRequest.Status is not (3 or 4 or 5))
			LogAndThrowValidationException(uploadRequest, $"Game status {uploadRequest.Status} is not accepted.", null, "rotating_light");

		if (uploadRequest.PlayerId < 1)
			LogAndThrowValidationException(uploadRequest, "Player ID is 0 or negative.", null, "rotating_light");

		// Check for existing spawnset.
		var spawnset =
			IsUnitTest
			? await _dbContext.Spawnsets.Select(s => new { s.Name, s.Md5Hash }).FirstOrDefaultAsync(s => s.Md5Hash.SequenceEqual(uploadRequest.SurvivalHashMd5))
			: await _dbContext.Spawnsets.Select(s => new { s.Name, s.Md5Hash }).FirstOrDefaultAsync(s => s.Md5Hash == uploadRequest.SurvivalHashMd5);
		if (spawnset == null)
			LogAndThrowValidationException(uploadRequest, "This spawnset doesn't exist on DevilDaggers.info.");

		ValidateReplayBuffer(uploadRequest, spawnset.Name);

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
			LogAndThrowValidationException(uploadRequest, "Banned.", spawnset.Name, "rotating_light");
		}

		// Check for existing leaderboard.
		// ! Navigation property.
		CustomLeaderboardEntity? customLeaderboard = await _dbContext.CustomLeaderboards.Include(cl => cl.Spawnset).FirstOrDefaultAsync(cl => cl.Spawnset!.Name == spawnset.Name);
		if (customLeaderboard == null)
			LogAndThrowValidationException(uploadRequest, "This spawnset exists on DevilDaggers.info, but doesn't have a leaderboard.", spawnset.Name);

		// Validate game mode.
		// ! Navigation property.
		SpawnsetGameMode spawnsetGameMode = customLeaderboard.Spawnset!.GameMode;
		if (uploadRequest.GameMode != (byte)spawnsetGameMode)
			LogAndThrowValidationException(uploadRequest, $"Incorrect game mode '{(SpawnsetGameMode)uploadRequest.GameMode}'. Must be '{spawnsetGameMode}'.", spawnset.Name);

		// Validate TimeAttack and Race.
		if (spawnsetGameMode is SpawnsetGameMode.TimeAttack or SpawnsetGameMode.Race && !uploadRequest.TimeAttackOrRaceFinished)
			LogAndThrowValidationException(uploadRequest, $"Didn't complete the {spawnsetGameMode} spawnset.", spawnset.Name);

		try
		{
			HandleAllCriteria(uploadRequest, spawnset.Name, customLeaderboard);
		}
		catch (CustomEntryCriteriaException ex)
		{
			return new()
			{
				Leaderboard = ToLeaderboardSummary(customLeaderboard),
				Rejection = new()
				{
					CriteriaName = ex.CriteriaName,
					ActualValue = ex.ActualValue,
					CriteriaOperator = ex.CriteriaOperator,
					ExpectedValue = ex.ExpectedValue,
				},
			};
		}

		// Make sure HomingDaggers is not negative (happens rarely in DD game memory for some reason). We also do this for spawnsets with homing disabled which we don't want to display values for anyway.
		uploadRequest.GameData.HomingStored = Array.ConvertAll(uploadRequest.GameData.HomingStored, i => Math.Max(0, i));

		CustomEntryEntity? customEntry = _dbContext.CustomEntries.FirstOrDefault(ce => ce.PlayerId == uploadRequest.PlayerId && ce.CustomLeaderboardId == customLeaderboard.Id);
		if (customEntry == null)
		{
			return new()
			{
				Leaderboard = ToLeaderboardSummary(customLeaderboard),
				Success = await ProcessNewScoreAsync(uploadRequest, customLeaderboard, spawnset.Name),
			};
		}

		// Treat identical replays as no highscore.
		int requestTimeAsInt = uploadRequest.TimeInSeconds.To10thMilliTime();
		if (uploadRequest.IsReplay && IsReplayTimeAlmostTheSame(requestTimeAsInt, customEntry.Time) && await IsReplayFileTheSame(customEntry.Id, uploadRequest.ReplayData))
		{
			_logger.LogInformation("Score submission replay time was modified because of identical replay (database: {originalTime} - request: {replayTime}).", FormatTimeString(customEntry.Time.ToSecondsTime()), FormatTimeString(uploadRequest.TimeInSeconds));
			return new()
			{
				Leaderboard = ToLeaderboardSummary(customLeaderboard),
				Success = await ProcessNoHighscoreAsync(uploadRequest, customLeaderboard, spawnset.Name, customEntry),
			};
		}

		bool isHighscore = customLeaderboard.RankSorting switch
		{
			CustomLeaderboardRankSorting.TimeDesc => requestTimeAsInt > customEntry.Time,
			CustomLeaderboardRankSorting.TimeAsc => requestTimeAsInt < customEntry.Time,
			_ => throw new InvalidOperationException($"Rank sorting '{customLeaderboard.RankSorting}' is not supported."),
		};

		if (isHighscore)
		{
			// User got a highscore.
			return new()
			{
				Leaderboard = ToLeaderboardSummary(customLeaderboard),
				Success = await ProcessHighscoreAsync(uploadRequest, customLeaderboard, spawnset.Name, customEntry),
			};
		}

		// User is already on the leaderboard, but did not get a better score.
		return new()
		{
			Leaderboard = ToLeaderboardSummary(customLeaderboard),
			Success = await ProcessNoHighscoreAsync(uploadRequest, customLeaderboard, spawnset.Name, customEntry),
		};
	}

	private void HandleAllCriteria(UploadRequest uploadRequest, string? spawnsetName, CustomLeaderboardEntity customLeaderboard)
	{
		TargetCollection targetCollection = new()
		{
			GemsCollected = uploadRequest.GemsCollected,
			GemsDespawned = uploadRequest.GemsDespawned,
			GemsEaten = uploadRequest.GemsEaten,
			EnemiesKilled = uploadRequest.EnemiesKilled,
			DaggersFired = uploadRequest.DaggersFired,
			DaggersHit = uploadRequest.DaggersHit,
			HomingStored = GetFinalHomingValue(uploadRequest),
			HomingEaten = uploadRequest.HomingEaten,
			DeathType = uploadRequest.DeathType,
			Time = uploadRequest.TimeInSeconds.To10thMilliTime(),
			LevelUpTime2 = uploadRequest.LevelUpTime2InSeconds.To10thMilliTime(),
			LevelUpTime3 = uploadRequest.LevelUpTime3InSeconds.To10thMilliTime(),
			LevelUpTime4 = uploadRequest.LevelUpTime4InSeconds.To10thMilliTime(),
			EnemiesAlive = uploadRequest.EnemiesAlive,
			Skull1Kills = GetFinalEnemyStat(uploadRequest, urd => urd.Skull1sKilled),
			Skull2Kills = GetFinalEnemyStat(uploadRequest, urd => urd.Skull2sKilled),
			Skull3Kills = GetFinalEnemyStat(uploadRequest, urd => urd.Skull3sKilled),
			Skull4Kills = GetFinalEnemyStat(uploadRequest, urd => urd.Skull4sKilled),
			SpiderlingKills = GetFinalEnemyStat(uploadRequest, urd => urd.SpiderlingsKilled),
			SpiderEggKills = GetFinalEnemyStat(uploadRequest, urd => urd.SpiderEggsKilled),
			Squid1Kills = GetFinalEnemyStat(uploadRequest, urd => urd.Squid1sKilled),
			Squid2Kills = GetFinalEnemyStat(uploadRequest, urd => urd.Squid2sKilled),
			Squid3Kills = GetFinalEnemyStat(uploadRequest, urd => urd.Squid3sKilled),
			CentipedeKills = GetFinalEnemyStat(uploadRequest, urd => urd.CentipedesKilled),
			GigapedeKills = GetFinalEnemyStat(uploadRequest, urd => urd.GigapedesKilled),
			GhostpedeKills = GetFinalEnemyStat(uploadRequest, urd => urd.GhostpedesKilled),
			Spider1Kills = GetFinalEnemyStat(uploadRequest, urd => urd.Spider1sKilled),
			Spider2Kills = GetFinalEnemyStat(uploadRequest, urd => urd.Spider2sKilled),
			LeviathanKills = GetFinalEnemyStat(uploadRequest, urd => urd.LeviathansKilled),
			OrbKills = GetFinalEnemyStat(uploadRequest, urd => urd.OrbsKilled),
			ThornKills = GetFinalEnemyStat(uploadRequest, urd => urd.ThornsKilled),
			Skull1sAlive = GetFinalEnemyStat(uploadRequest, urd => urd.Skull1sAlive),
			Skull2sAlive = GetFinalEnemyStat(uploadRequest, urd => urd.Skull2sAlive),
			Skull3sAlive = GetFinalEnemyStat(uploadRequest, urd => urd.Skull3sAlive),
			Skull4sAlive = GetFinalEnemyStat(uploadRequest, urd => urd.Skull4sAlive),
			SpiderlingsAlive = GetFinalEnemyStat(uploadRequest, urd => urd.SpiderlingsAlive),
			SpiderEggsAlive = GetFinalEnemyStat(uploadRequest, urd => urd.SpiderEggsAlive),
			Squid1sAlive = GetFinalEnemyStat(uploadRequest, urd => urd.Squid1sAlive),
			Squid2sAlive = GetFinalEnemyStat(uploadRequest, urd => urd.Squid2sAlive),
			Squid3sAlive = GetFinalEnemyStat(uploadRequest, urd => urd.Squid3sAlive),
			CentipedesAlive = GetFinalEnemyStat(uploadRequest, urd => urd.CentipedesAlive),
			GigapedesAlive = GetFinalEnemyStat(uploadRequest, urd => urd.GigapedesAlive),
			GhostpedesAlive = GetFinalEnemyStat(uploadRequest, urd => urd.GhostpedesAlive),
			Spider1sAlive = GetFinalEnemyStat(uploadRequest, urd => urd.Spider1sAlive),
			Spider2sAlive = GetFinalEnemyStat(uploadRequest, urd => urd.Spider2sAlive),
			LeviathansAlive = GetFinalEnemyStat(uploadRequest, urd => urd.LeviathansAlive),
			OrbsAlive = GetFinalEnemyStat(uploadRequest, urd => urd.OrbsAlive),
			ThornsAlive = GetFinalEnemyStat(uploadRequest, urd => urd.ThornsAlive),
		};

		HandleCriteria(customLeaderboard.GemsCollectedCriteria, targetCollection.GemsCollected);
		HandleCriteria(customLeaderboard.GemsDespawnedCriteria, targetCollection.GemsDespawned);
		HandleCriteria(customLeaderboard.GemsEatenCriteria, targetCollection.GemsEaten);
		HandleCriteria(customLeaderboard.EnemiesKilledCriteria, targetCollection.EnemiesKilled);
		HandleCriteria(customLeaderboard.DaggersFiredCriteria, targetCollection.DaggersFired);
		HandleCriteria(customLeaderboard.DaggersHitCriteria, targetCollection.DaggersHit);
		HandleCriteria(customLeaderboard.HomingStoredCriteria, targetCollection.HomingStored);
		HandleCriteria(customLeaderboard.HomingEatenCriteria, targetCollection.HomingEaten);
		HandleCriteria(customLeaderboard.DeathTypeCriteria, targetCollection.DeathType);
		HandleCriteria(customLeaderboard.TimeCriteria, targetCollection.Time);
		HandleCriteria(customLeaderboard.LevelUpTime2Criteria, targetCollection.LevelUpTime2);
		HandleCriteria(customLeaderboard.LevelUpTime3Criteria, targetCollection.LevelUpTime3);
		HandleCriteria(customLeaderboard.LevelUpTime4Criteria, targetCollection.LevelUpTime4);
		HandleCriteria(customLeaderboard.EnemiesAliveCriteria, targetCollection.EnemiesAlive);
		HandleCriteria(customLeaderboard.Skull1KillsCriteria, targetCollection.Skull1Kills);
		HandleCriteria(customLeaderboard.Skull2KillsCriteria, targetCollection.Skull2Kills);
		HandleCriteria(customLeaderboard.Skull3KillsCriteria, targetCollection.Skull3Kills);
		HandleCriteria(customLeaderboard.Skull4KillsCriteria, targetCollection.Skull4Kills);
		HandleCriteria(customLeaderboard.SpiderlingKillsCriteria, targetCollection.SpiderlingKills);
		HandleCriteria(customLeaderboard.SpiderEggKillsCriteria, targetCollection.SpiderEggKills);
		HandleCriteria(customLeaderboard.Squid1KillsCriteria, targetCollection.Squid1Kills);
		HandleCriteria(customLeaderboard.Squid2KillsCriteria, targetCollection.Squid2Kills);
		HandleCriteria(customLeaderboard.Squid3KillsCriteria, targetCollection.Squid3Kills);
		HandleCriteria(customLeaderboard.CentipedeKillsCriteria, targetCollection.CentipedeKills);
		HandleCriteria(customLeaderboard.GigapedeKillsCriteria, targetCollection.GigapedeKills);
		HandleCriteria(customLeaderboard.GhostpedeKillsCriteria, targetCollection.GhostpedeKills);
		HandleCriteria(customLeaderboard.Spider1KillsCriteria, targetCollection.Spider1Kills);
		HandleCriteria(customLeaderboard.Spider2KillsCriteria, targetCollection.Spider2Kills);
		HandleCriteria(customLeaderboard.LeviathanKillsCriteria, targetCollection.LeviathanKills);
		HandleCriteria(customLeaderboard.OrbKillsCriteria, targetCollection.OrbKills);
		HandleCriteria(customLeaderboard.ThornKillsCriteria, targetCollection.ThornKills);
		HandleCriteria(customLeaderboard.Skull1sAliveCriteria, targetCollection.Skull1sAlive);
		HandleCriteria(customLeaderboard.Skull2sAliveCriteria, targetCollection.Skull2sAlive);
		HandleCriteria(customLeaderboard.Skull3sAliveCriteria, targetCollection.Skull3sAlive);
		HandleCriteria(customLeaderboard.Skull4sAliveCriteria, targetCollection.Skull4sAlive);
		HandleCriteria(customLeaderboard.SpiderlingsAliveCriteria, targetCollection.SpiderlingsAlive);
		HandleCriteria(customLeaderboard.SpiderEggsAliveCriteria, targetCollection.SpiderEggsAlive);
		HandleCriteria(customLeaderboard.Squid1sAliveCriteria, targetCollection.Squid1sAlive);
		HandleCriteria(customLeaderboard.Squid2sAliveCriteria, targetCollection.Squid2sAlive);
		HandleCriteria(customLeaderboard.Squid3sAliveCriteria, targetCollection.Squid3sAlive);
		HandleCriteria(customLeaderboard.CentipedesAliveCriteria, targetCollection.CentipedesAlive);
		HandleCriteria(customLeaderboard.GigapedesAliveCriteria, targetCollection.GigapedesAlive);
		HandleCriteria(customLeaderboard.GhostpedesAliveCriteria, targetCollection.GhostpedesAlive);
		HandleCriteria(customLeaderboard.Spider1sAliveCriteria, targetCollection.Spider1sAlive);
		HandleCriteria(customLeaderboard.Spider2sAliveCriteria, targetCollection.Spider2sAlive);
		HandleCriteria(customLeaderboard.LeviathansAliveCriteria, targetCollection.LeviathansAlive);
		HandleCriteria(customLeaderboard.OrbsAliveCriteria, targetCollection.OrbsAlive);
		HandleCriteria(customLeaderboard.ThornsAliveCriteria, targetCollection.ThornsAlive);

		static int GetFinalEnemyStat(UploadRequest uploadRequest, Func<UploadRequestData, ushort[]> selector)
		{
			ushort[] arr = selector(uploadRequest.GameData);
			return arr.Length == 0 ? 0 : arr[^1];
		}

		void HandleCriteria(CustomLeaderboardCriteriaEntityValue criteria, int value, [CallerArgumentExpression("criteria")] string criteriaExpression = "")
		{
			if (criteria.Expression == null)
				return;

			if (!Expression.TryParse(criteria.Expression, out Expression? expressionParsed))
				throw new InvalidOperationException($"Could not parse criteria expression '{criteriaExpression}'.");

			int evaluatedValue = expressionParsed.Evaluate(targetCollection);
			if (IsValidForCriteria(criteria.Operator, evaluatedValue, value))
				return;

			Log(uploadRequest, spawnsetName, $"Did not meet the {criteriaExpression}. Criteria: {criteria.Operator.Display()} {evaluatedValue}. Value: {value}.");
			throw new CustomEntryCriteriaException(criteriaExpression, criteria.Operator, evaluatedValue, value);
		}

		static bool IsValidForCriteria(CustomLeaderboardCriteriaOperator op, int expectedValue, int value) => op switch
		{
			CustomLeaderboardCriteriaOperator.Equal => value == expectedValue,
			CustomLeaderboardCriteriaOperator.LessThan => value < expectedValue,
			CustomLeaderboardCriteriaOperator.GreaterThan => value > expectedValue,
			CustomLeaderboardCriteriaOperator.LessThanOrEqual => value <= expectedValue,
			CustomLeaderboardCriteriaOperator.GreaterThanOrEqual => value >= expectedValue,
			CustomLeaderboardCriteriaOperator.Modulo => value % expectedValue == 0,
			CustomLeaderboardCriteriaOperator.NotEqual => value != expectedValue,
			_ => true,
		};
	}

	private async Task<SuccessfulUploadResponse> ProcessNewScoreAsync(UploadRequest uploadRequest, CustomLeaderboardEntity customLeaderboard, string spawnsetName)
	{
		// Add new custom entry to this leaderboard.
		CustomEntryEntity newCustomEntry = new()
		{
			PlayerId = uploadRequest.PlayerId,
			Time = uploadRequest.TimeInSeconds.To10thMilliTime(),
			GemsCollected = uploadRequest.GemsCollected,
			GemsDespawned = uploadRequest.GemsDespawned,
			GemsEaten = uploadRequest.GemsEaten,
			GemsTotal = uploadRequest.GemsTotal,
			EnemiesKilled = uploadRequest.EnemiesKilled,
			DeathType = uploadRequest.DeathType,
			DaggersHit = uploadRequest.DaggersHit,
			DaggersFired = uploadRequest.DaggersFired,
			EnemiesAlive = uploadRequest.EnemiesAlive,
			HomingStored = GetFinalHomingValue(uploadRequest),
			HomingEaten = uploadRequest.HomingEaten,
			LevelUpTime2 = uploadRequest.LevelUpTime2InSeconds.To10thMilliTime(),
			LevelUpTime3 = uploadRequest.LevelUpTime3InSeconds.To10thMilliTime(),
			LevelUpTime4 = uploadRequest.LevelUpTime4InSeconds.To10thMilliTime(),
			SubmitDate = DateTime.UtcNow,
			ClientVersion = uploadRequest.ClientVersion,
			Client = uploadRequest.Client.ClientFromString(),
			CustomLeaderboard = customLeaderboard,
		};
		await _dbContext.CustomEntries.AddAsync(newCustomEntry);

		CustomEntryDataEntity newCustomEntryData = new() { CustomEntry = newCustomEntry };
		newCustomEntryData.Populate(uploadRequest.GameData);
		await _dbContext.CustomEntryData.AddAsync(newCustomEntryData);

		UpdateLeaderboardStatistics(customLeaderboard);

		await _dbContext.SaveChangesAsync();

		await WriteReplayFile(newCustomEntry.Id, uploadRequest.ReplayData);

		// Calculate rank.
		List<CustomEntryEntity> entries = await GetOrderedEntries(customLeaderboard.Id, customLeaderboard.RankSorting);
		int rank = GetRank(entries, uploadRequest.PlayerId);
		int totalPlayers = entries.Count;

		_submissionLogger.LogHighscore(
			customLeaderboard.DaggerFromStat(newCustomEntry.Time) ?? CustomLeaderboardDagger.Silver,
			customLeaderboard.Id,
			$"`{uploadRequest.PlayerName}` just entered the `{spawnsetName}` leaderboard!",
			rank,
			totalPlayers,
			newCustomEntry.Time);
		Log(uploadRequest, spawnsetName);

		List<int> replayIds = GetExistingReplayIds(entries.ConvertAll(ce => ce.Id));
		return new()
		{
			SortedEntries = entries.Select((e, i) => ToEntry(e, i + 1, customLeaderboard.DaggerFromStat(e.Time), replayIds)).ToList(),
			SubmissionType = SubmissionType.FirstScore,
			RankState = new(rank),
			TimeState = new(newCustomEntry.Time.ToSecondsTime()),
			EnemiesKilledState = new(newCustomEntry.EnemiesKilled),
			GemsCollectedState = new(newCustomEntry.GemsCollected),
			GemsDespawnedState = new(newCustomEntry.GemsDespawned),
			GemsEatenState = new(newCustomEntry.GemsEaten),
			GemsTotalState = new(newCustomEntry.GemsTotal),
			DaggersHitState = new(newCustomEntry.DaggersHit),
			DaggersFiredState = new(newCustomEntry.DaggersFired),
			EnemiesAliveState = new(newCustomEntry.EnemiesAlive),
			HomingStoredState = new(newCustomEntry.HomingStored),
			HomingEatenState = new(newCustomEntry.HomingEaten),
			LevelUpTime2State = new(newCustomEntry.LevelUpTime2.ToSecondsTime()),
			LevelUpTime3State = new(newCustomEntry.LevelUpTime3.ToSecondsTime()),
			LevelUpTime4State = new(newCustomEntry.LevelUpTime4.ToSecondsTime()),
		};
	}

	private async Task<SuccessfulUploadResponse> ProcessNoHighscoreAsync(UploadRequest uploadRequest, CustomLeaderboardEntity customLeaderboard, string spawnsetName, CustomEntryEntity currentEntry)
	{
		if (!uploadRequest.IsReplay)
		{
			UpdateLeaderboardStatistics(customLeaderboard);
			await _dbContext.SaveChangesAsync();
		}

		Log(uploadRequest, spawnsetName);

		List<CustomEntryEntity> entries = await GetOrderedEntries(customLeaderboard.Id, customLeaderboard.RankSorting);
		List<int> replayIds = GetExistingReplayIds(entries.ConvertAll(ce => ce.Id));

		int homingStored = GetFinalHomingValue(uploadRequest);
		return new()
		{
			SortedEntries = entries.Select((e, i) => ToEntry(e, i + 1, customLeaderboard.DaggerFromStat(e.Time), replayIds)).ToList(),
			SubmissionType = SubmissionType.NoHighscore,
			TimeState = new(uploadRequest.TimeInSeconds, uploadRequest.TimeInSeconds - currentEntry.Time.ToSecondsTime()),
			EnemiesKilledState = new(uploadRequest.EnemiesKilled, uploadRequest.EnemiesKilled - currentEntry.EnemiesKilled),
			GemsCollectedState = new(uploadRequest.GemsCollected, uploadRequest.GemsCollected - currentEntry.GemsCollected),
			GemsDespawnedState = new(uploadRequest.GemsDespawned, uploadRequest.GemsDespawned - currentEntry.GemsDespawned),
			GemsEatenState = new(uploadRequest.GemsEaten, uploadRequest.GemsEaten - currentEntry.GemsEaten),
			GemsTotalState = new(uploadRequest.GemsTotal, uploadRequest.GemsTotal - currentEntry.GemsTotal),
			DaggersHitState = new(uploadRequest.DaggersHit, uploadRequest.DaggersHit - currentEntry.DaggersHit),
			DaggersFiredState = new(uploadRequest.DaggersFired, uploadRequest.DaggersFired - currentEntry.DaggersFired),
			EnemiesAliveState = new(uploadRequest.EnemiesAlive, uploadRequest.EnemiesAlive - currentEntry.EnemiesAlive),
			HomingStoredState = new(homingStored, homingStored - currentEntry.HomingStored),
			HomingEatenState = new(uploadRequest.HomingEaten, uploadRequest.HomingEaten - currentEntry.HomingEaten),
			LevelUpTime2State = new(uploadRequest.LevelUpTime2InSeconds, uploadRequest.LevelUpTime2InSeconds - currentEntry.LevelUpTime2.ToSecondsTime()),
			LevelUpTime3State = new(uploadRequest.LevelUpTime3InSeconds, uploadRequest.LevelUpTime3InSeconds - currentEntry.LevelUpTime3.ToSecondsTime()),
			LevelUpTime4State = new(uploadRequest.LevelUpTime4InSeconds, uploadRequest.LevelUpTime4InSeconds - currentEntry.LevelUpTime4.ToSecondsTime()),
		};
	}

	private async Task<SuccessfulUploadResponse> ProcessHighscoreAsync(UploadRequest uploadRequest, CustomLeaderboardEntity customLeaderboard, string spawnsetName, CustomEntryEntity customEntry)
	{
		// Store old stats.
		List<CustomEntryEntity> entries = await GetOrderedEntries(customLeaderboard.Id, customLeaderboard.RankSorting);
		int oldRank = GetRank(entries, uploadRequest.PlayerId);
		int oldTime = customEntry.Time;
		int oldGemsCollected = customEntry.GemsCollected;
		int oldEnemiesKilled = customEntry.EnemiesKilled;
		int oldDaggersFired = customEntry.DaggersFired;
		int oldDaggersHit = customEntry.DaggersHit;
		int oldEnemiesAlive = customEntry.EnemiesAlive;
		int oldHomingStored = customEntry.HomingStored;
		int oldHomingEaten = customEntry.HomingEaten;
		int oldGemsDespawned = customEntry.GemsDespawned;
		int oldGemsEaten = customEntry.GemsEaten;
		int oldGemsTotal = customEntry.GemsTotal;
		int oldLevelUpTime2 = customEntry.LevelUpTime2;
		int oldLevelUpTime3 = customEntry.LevelUpTime3;
		int oldLevelUpTime4 = customEntry.LevelUpTime4;

		// Update score, chart data, stats, and replay.
		customEntry.Time = uploadRequest.TimeInSeconds.To10thMilliTime();
		customEntry.EnemiesKilled = uploadRequest.EnemiesKilled;
		customEntry.GemsCollected = uploadRequest.GemsCollected;
		customEntry.DaggersFired = uploadRequest.DaggersFired;
		customEntry.DaggersHit = uploadRequest.DaggersHit;
		customEntry.EnemiesAlive = uploadRequest.EnemiesAlive;
		customEntry.HomingStored = GetFinalHomingValue(uploadRequest);
		customEntry.HomingEaten = uploadRequest.HomingEaten;
		customEntry.GemsDespawned = uploadRequest.GemsDespawned;
		customEntry.GemsEaten = uploadRequest.GemsEaten;
		customEntry.GemsTotal = uploadRequest.GemsTotal;
		customEntry.DeathType = uploadRequest.DeathType;
		customEntry.LevelUpTime2 = uploadRequest.LevelUpTime2InSeconds.To10thMilliTime();
		customEntry.LevelUpTime3 = uploadRequest.LevelUpTime3InSeconds.To10thMilliTime();
		customEntry.LevelUpTime4 = uploadRequest.LevelUpTime4InSeconds.To10thMilliTime();
		customEntry.SubmitDate = DateTime.UtcNow;
		customEntry.ClientVersion = uploadRequest.ClientVersion;
		customEntry.Client = uploadRequest.Client.ClientFromString();

		CustomEntryDataEntity? customEntryData = await _dbContext.CustomEntryData.FirstOrDefaultAsync(ced => ced.CustomEntryId == customEntry.Id);
		if (customEntryData == null)
		{
			customEntryData = new() { CustomEntryId = customEntry.Id };
			customEntryData.Populate(uploadRequest.GameData);
			await _dbContext.CustomEntryData.AddAsync(customEntryData);
		}
		else
		{
			customEntryData.Populate(uploadRequest.GameData);
		}

		UpdateLeaderboardStatistics(customLeaderboard);

		await _dbContext.SaveChangesAsync();

		await WriteReplayFile(customEntry.Id, uploadRequest.ReplayData);

		// Calculate new rank, diffs, etc.
		entries = await GetOrderedEntries(customLeaderboard.Id, customLeaderboard.RankSorting);
		int rank = GetRank(entries, uploadRequest.PlayerId);

		int rankDiff = oldRank - rank;
		int timeDiff = customEntry.Time - oldTime;
		int gemsCollectedDiff = customEntry.GemsCollected - oldGemsCollected;
		int enemiesKilledDiff = customEntry.EnemiesKilled - oldEnemiesKilled;
		int daggersFiredDiff = customEntry.DaggersFired - oldDaggersFired;
		int daggersHitDiff = customEntry.DaggersHit - oldDaggersHit;
		int enemiesAliveDiff = customEntry.EnemiesAlive - oldEnemiesAlive;
		int homingStoredDiff = customEntry.HomingStored - oldHomingStored;
		int homingEatenDiff = customEntry.HomingEaten - oldHomingEaten;
		int gemsDespawnedDiff = customEntry.GemsDespawned - oldGemsDespawned;
		int gemsEatenDiff = customEntry.GemsEaten - oldGemsEaten;
		int gemsTotalDiff = customEntry.GemsTotal - oldGemsTotal;
		int levelUpTime2Diff = customEntry.LevelUpTime2 - oldLevelUpTime2;
		int levelUpTime3Diff = customEntry.LevelUpTime3 - oldLevelUpTime3;
		int levelUpTime4Diff = customEntry.LevelUpTime4 - oldLevelUpTime4;

		_submissionLogger.LogHighscore(
			customLeaderboard.DaggerFromStat(customEntry.Time) ?? CustomLeaderboardDagger.Silver,
			customLeaderboard.Id,
			$"`{uploadRequest.PlayerName}` just got {FormatTimeString(customEntry.Time.ToSecondsTime())} seconds on the `{spawnsetName}` leaderboard, beating their previous highscore of {FormatTimeString((customEntry.Time - timeDiff).ToSecondsTime())} by {FormatTimeString(Math.Abs(timeDiff.ToSecondsTime()))} seconds!",
			rank,
			entries.Count,
			customEntry.Time);
		Log(uploadRequest, spawnsetName);

		List<int> replayIds = GetExistingReplayIds(entries.ConvertAll(ce => ce.Id));

		return new()
		{
			SortedEntries = entries.Select((e, i) => ToEntry(e, i + 1, customLeaderboard.DaggerFromStat(e.Time), replayIds)).ToList(),
			SubmissionType = SubmissionType.NewHighscore,
			RankState = new(rank, rankDiff),
			TimeState = new(customEntry.Time.ToSecondsTime(), timeDiff.ToSecondsTime()),
			EnemiesKilledState = new(customEntry.EnemiesKilled, enemiesKilledDiff),
			GemsCollectedState = new(customEntry.GemsCollected, gemsCollectedDiff),
			GemsDespawnedState = new(customEntry.GemsDespawned, gemsDespawnedDiff),
			GemsTotalState = new(customEntry.GemsTotal, gemsTotalDiff),
			GemsEatenState = new(customEntry.GemsEaten, gemsEatenDiff),
			DaggersHitState = new(customEntry.DaggersHit, daggersHitDiff),
			DaggersFiredState = new(customEntry.DaggersFired, daggersFiredDiff),
			EnemiesAliveState = new(customEntry.EnemiesAlive, enemiesAliveDiff),
			HomingStoredState = new(customEntry.HomingStored, homingStoredDiff),
			HomingEatenState = new(customEntry.HomingEaten, homingEatenDiff),
			LevelUpTime2State = new(customEntry.LevelUpTime2.ToSecondsTime(), levelUpTime2Diff.ToSecondsTime()),
			LevelUpTime3State = new(customEntry.LevelUpTime3.ToSecondsTime(), levelUpTime3Diff.ToSecondsTime()),
			LevelUpTime4State = new(customEntry.LevelUpTime4.ToSecondsTime(), levelUpTime4Diff.ToSecondsTime()),
		};
	}

	/// <summary>
	/// The <see cref="UploadRequest.HomingStored"/> value is not reliable in game memory and shouldn't be used.
	/// </summary>
	private static int GetFinalHomingValue(UploadRequest uploadRequest)
	{
		return uploadRequest.GameData.HomingStored.Length == 0 ? 0 : uploadRequest.GameData.HomingStored[^1];
	}

	private void ValidateReplayBuffer(UploadRequest uploadRequest, string spawnsetName)
	{
		LocalReplayBinaryHeader? replayBinaryHeader = null;
		try
		{
			replayBinaryHeader = LocalReplayBinaryHeader.CreateFromByteArray(uploadRequest.ReplayData);
		}
		catch (Exception ex)
		{
			LogAndThrowValidationException(uploadRequest, $"Could not parse replay: {ex.Message}", spawnsetName, "rotating_light");
		}

		if (!replayBinaryHeader.SpawnsetMd5.SequenceEqual(uploadRequest.SurvivalHashMd5))
			LogAndThrowValidationException(uploadRequest, "Spawnset in replay does not match detected spawnset.", spawnsetName, "rotating_light");
	}

	[DoesNotReturn]
	private void LogAndThrowValidationException(UploadRequest uploadRequest, string errorMessage, string? spawnsetName = null, string? errorEmoteNameOverride = null)
	{
		Log(uploadRequest, spawnsetName, errorMessage, errorEmoteNameOverride);
		throw new CustomEntryValidationException(errorMessage);
	}

	private async Task WriteReplayFile(int customEntryId, byte[] replayData)
	{
		await File.WriteAllBytesAsync(Path.Combine(_fileSystemService.GetPath(DataSubDirectory.CustomEntryReplays), $"{customEntryId}.ddreplay"), replayData);
	}

	private static bool IsReplayTimeAlmostTheSame(int requestTimeAsInt, int databaseTime)
	{
		if (requestTimeAsInt == databaseTime)
			return false;

		const int replayMarginErrorInTenthMillis = 1000;
		return requestTimeAsInt > databaseTime - replayMarginErrorInTenthMillis && requestTimeAsInt < databaseTime + replayMarginErrorInTenthMillis;
	}

	/// <summary>
	/// Due to a bug in the game, the final time sometimes gains a couple extra ticks if the run is a replay (more common in longer runs).
	/// If a player first submitted the actual score (non-replay) and then watches the same replay again, it will send a higher score the second time.
	/// We don't want these replay submissions to overwrite the original score. To work around this, simply check if the replay buffer is exactly the same as the original.
	/// </summary>
	private async Task<bool> IsReplayFileTheSame(int customEntryId, byte[] newReplay)
	{
		string path = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.CustomEntryReplays), $"{customEntryId}.ddreplay");
		if (!File.Exists(path))
			return false;

		byte[] originalReplay = await File.ReadAllBytesAsync(path);
		return originalReplay.SequenceEqual(newReplay);
	}

	private static void UpdateLeaderboardStatistics(CustomLeaderboardEntity customLeaderboard)
	{
		customLeaderboard.DateLastPlayed = DateTime.UtcNow;
		customLeaderboard.TotalRunsSubmitted++;
	}

	private async Task<List<CustomEntryEntity>> GetOrderedEntries(int customLeaderboardId, CustomLeaderboardRankSorting rankSorting)
	{
		List<CustomEntryEntity> entries = await _dbContext.CustomEntries
			.AsNoTracking()
			.Include(ce => ce.Player)
			.Where(ce => ce.CustomLeaderboardId == customLeaderboardId)
			.ToListAsync();

		return entries.Sort(rankSorting).ToList();
	}

	private static int GetRank(List<CustomEntryEntity> orderedEntries, int playerId)
		=> orderedEntries.ConvertAll(ce => ce.PlayerId).IndexOf(playerId) + 1;

	private static string FormatTimeString(double time)
		=> time.ToString(StringFormats.TimeFormat);

	private static string GetSpawnsetHashOrName(byte[] spawnsetHash, string? spawnsetName)
		=> string.IsNullOrEmpty(spawnsetName) ? BitConverter.ToString(spawnsetHash) : spawnsetName;

	private void Log(UploadRequest uploadRequest, string? spawnsetName, string? errorMessage = null, string? errorEmoteNameOverride = null)
	{
		_stopwatch.Stop();

		string spawnsetIdentification = GetSpawnsetHashOrName(uploadRequest.SurvivalHashMd5, spawnsetName);

		string replayData = $"Replay data {uploadRequest.ReplayData.Length:N0} bytes";
		string replayString = uploadRequest.IsReplay ? " | `Replay`" : string.Empty;
		string localReplayString = uploadRequest.Status == 8 ? $" | `Local replay from {uploadRequest.ReplayPlayerId}`" : string.Empty;
		string requestInfo = $"(`{uploadRequest.ClientVersion}` | `{uploadRequest.OperatingSystem}` | `{uploadRequest.BuildMode}` | `{uploadRequest.Client}`{replayString}{localReplayString} | `{replayData}` | `Status {uploadRequest.Status}`)";
		string playerInfo = $"`{uploadRequest.PlayerName}` (`{uploadRequest.PlayerId}`)";

		if (!string.IsNullOrEmpty(errorMessage))
			_submissionLogger.Log(false, $":{errorEmoteNameOverride ?? "warning"}: `{_stopwatch.ElapsedMilliseconds:N0}` Upload failed for user {playerInfo} for `{spawnsetIdentification}`. {requestInfo}\n**{errorMessage}**");
		else
			_submissionLogger.Log(true, $":white_check_mark: `{_stopwatch.ElapsedMilliseconds:N0}` {playerInfo} just submitted a score of `{FormatTimeString(uploadRequest.TimeInSeconds)}` to `{spawnsetIdentification}`. {requestInfo}");
	}

	private List<int> GetExistingReplayIds(List<int> customEntryIds)
	{
		return customEntryIds.Where(id => File.Exists(Path.Combine(_fileSystemService.GetPath(DataSubDirectory.CustomEntryReplays), $"{id}.ddreplay"))).ToList();
	}

	private static CustomEntry ToEntry(CustomEntryEntity customEntry, int rank, CustomLeaderboardDagger? dagger, List<int> replayIds)
	{
		if (customEntry.Player == null)
			throw new InvalidOperationException("Player is not included.");

		return new()
		{
			ClientVersion = customEntry.ClientVersion,
			DaggersFired = customEntry.DaggersFired,
			DaggersHit = customEntry.DaggersHit,
			DeathType = customEntry.DeathType,
			EnemiesAlive = customEntry.EnemiesAlive,
			EnemiesKilled = customEntry.EnemiesKilled,
			GemsCollected = customEntry.GemsCollected,
			GemsDespawned = customEntry.GemsDespawned,
			GemsEaten = customEntry.GemsEaten,
			GemsTotal = customEntry.GemsTotal,
			HasReplay = replayIds.Contains(customEntry.Id),
			Id = customEntry.Id,
			HomingEaten = customEntry.HomingEaten,
			HomingStored = customEntry.HomingStored,
			LevelUpTime2 = customEntry.LevelUpTime2,
			LevelUpTime3 = customEntry.LevelUpTime3,
			LevelUpTime4 = customEntry.LevelUpTime4,
			PlayerId = customEntry.PlayerId,
			PlayerName = customEntry.Player.PlayerName,
			Rank = rank,
			SubmitDate = customEntry.SubmitDate,
			Time = customEntry.Time,
			CustomLeaderboardDagger = dagger,
			Client = default, // TODO
			CountryCode = null, // TODO
		};
	}

	private static CustomLeaderboardSummary ToLeaderboardSummary(CustomLeaderboardEntity customLeaderboard)
	{
		if (customLeaderboard.Spawnset == null)
			throw new InvalidOperationException("Spawnset is not included.");

		return new()
		{
			RankSorting = customLeaderboard.RankSorting,
			Daggers = !customLeaderboard.IsFeatured ? null : new()
			{
				Bronze = customLeaderboard.Bronze,
				Silver = customLeaderboard.Silver,
				Golden = customLeaderboard.Golden,
				Devil = customLeaderboard.Devil,
				Leviathan = customLeaderboard.Leviathan,
			},
			Id = customLeaderboard.Id,
			SpawnsetId = customLeaderboard.SpawnsetId,
			SpawnsetName = customLeaderboard.Spawnset.Name,
		};
	}

#pragma warning disable S3871, RCS1194
	private sealed class CustomEntryCriteriaException : Exception
#pragma warning restore RCS1194, S3871
	{
		public CustomEntryCriteriaException(string criteriaName, CustomLeaderboardCriteriaOperator criteriaOperator, int expectedValue, int actualValue)
		{
			CriteriaName = criteriaName;
			CriteriaOperator = criteriaOperator;
			ExpectedValue = expectedValue;
			ActualValue = actualValue;
		}

		public string CriteriaName { get; }
		public CustomLeaderboardCriteriaOperator CriteriaOperator { get; }
		public int ExpectedValue { get; }
		public int ActualValue { get; }
	}
}
