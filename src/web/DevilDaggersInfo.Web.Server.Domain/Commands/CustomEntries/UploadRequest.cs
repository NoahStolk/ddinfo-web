using DevilDaggersInfo.Common.Extensions;
using DevilDaggersInfo.Core.CriteriaExpression;
using DevilDaggersInfo.Web.Server.Domain.Constants;
using DevilDaggersInfo.Web.Server.Domain.Utils;

namespace DevilDaggersInfo.Web.Server.Domain.Commands.CustomEntries;

public class UploadRequest
{
	public UploadRequest(
		byte[] survivalHashMd5,
		int playerId,
		string playerName,
		int replayPlayerId,
		double timeInSeconds,
		byte[] timeAsBytes,
		int gemsCollected,
		int enemiesKilled,
		int daggersFired,
		int daggersHit,
		int enemiesAlive,
		int homingStored,
		int homingEaten,
		int gemsDespawned,
		int gemsEaten,
		int gemsTotal,
		byte deathType,
		double levelUpTime2InSeconds,
		double levelUpTime3InSeconds,
		double levelUpTime4InSeconds,
		byte[] levelUpTime2AsBytes,
		byte[] levelUpTime3AsBytes,
		byte[] levelUpTime4AsBytes,
		string clientVersion,
		string client,
		string operatingSystem,
		string buildMode,
		string validation,
		int validationVersion,
		bool isReplay,
		bool prohibitedMods,
		byte gameMode,
		bool timeAttackOrRaceFinished,
		UploadRequestData gameData,
		byte[] replayData,
		int status)
	{
		ThrowUtils.ThrowIf(survivalHashMd5.Length != 16);
		ThrowUtils.ThrowIf(playerName.Length > 32);
		ThrowUtils.ThrowIf(timeAsBytes.Length != sizeof(float));
		ThrowUtils.ThrowIf(levelUpTime2AsBytes.Length != sizeof(float));
		ThrowUtils.ThrowIf(levelUpTime3AsBytes.Length != sizeof(float));
		ThrowUtils.ThrowIf(levelUpTime4AsBytes.Length != sizeof(float));
		ThrowUtils.ThrowIf(clientVersion.Length > 16);
		ThrowUtils.ThrowIf(replayData.Length > ReplayConstants.MaxFileSize);

		SurvivalHashMd5 = survivalHashMd5;
		PlayerId = playerId;
		PlayerName = playerName;
		ReplayPlayerId = replayPlayerId;
		TimeInSeconds = timeInSeconds;
		TimeAsBytes = timeAsBytes;
		GemsCollected = gemsCollected;
		EnemiesKilled = enemiesKilled;
		DaggersFired = daggersFired;
		DaggersHit = daggersHit;
		EnemiesAlive = enemiesAlive;
		HomingStored = homingStored;
		HomingEaten = homingEaten;
		GemsDespawned = gemsDespawned;
		GemsEaten = gemsEaten;
		GemsTotal = gemsTotal;
		DeathType = deathType;
		LevelUpTime2InSeconds = levelUpTime2InSeconds;
		LevelUpTime3InSeconds = levelUpTime3InSeconds;
		LevelUpTime4InSeconds = levelUpTime4InSeconds;
		LevelUpTime2AsBytes = levelUpTime2AsBytes;
		LevelUpTime3AsBytes = levelUpTime3AsBytes;
		LevelUpTime4AsBytes = levelUpTime4AsBytes;
		ClientVersion = clientVersion;
		Client = client;
		OperatingSystem = operatingSystem;
		BuildMode = buildMode;
		Validation = validation;
		ValidationVersion = validationVersion;
		IsReplay = isReplay;
		ProhibitedMods = prohibitedMods;
		GameMode = gameMode;
		TimeAttackOrRaceFinished = timeAttackOrRaceFinished;
		GameData = gameData;
		ReplayData = replayData;
		Status = status;
	}

	public byte[] SurvivalHashMd5 { get; }

	public int PlayerId { get; }

	public string PlayerName { get; }

	public int ReplayPlayerId { get; }

	public double TimeInSeconds { get; }

	public byte[] TimeAsBytes { get; }

	public int GemsCollected { get; }

	public int EnemiesKilled { get; }

	public int DaggersFired { get; }

	public int DaggersHit { get; }

	public int EnemiesAlive { get; }

	/// <summary>
	/// This value is not reliable in game memory and therefore no longer used. It is now only used for the request signature.
	/// </summary>
	public int HomingStored { get; }

	public int HomingEaten { get; }

	public int GemsDespawned { get; }

	public int GemsEaten { get; }

	public int GemsTotal { get; }

	public byte DeathType { get; }

	public double LevelUpTime2InSeconds { get; }

	public double LevelUpTime3InSeconds { get; }

	public double LevelUpTime4InSeconds { get; }

	public byte[] LevelUpTime2AsBytes { get; }

	public byte[] LevelUpTime3AsBytes { get; }

	public byte[] LevelUpTime4AsBytes { get; }

	public string ClientVersion { get; }

	public string Client { get; }

	public string OperatingSystem { get; }

	public string BuildMode { get; }

	public string Validation { get; }

	public int ValidationVersion { get; }

	public bool IsReplay { get; }

	public bool ProhibitedMods { get; }

	public byte GameMode { get; }

	public bool TimeAttackOrRaceFinished { get; }

	public UploadRequestData GameData { get; }

	public byte[] ReplayData { get; }

	public int Status { get; set; }

	public string CreateValidationV2() => CreateValidationV2(
		PlayerId,
		TimeAsBytes,
		GemsCollected,
		GemsDespawned,
		GemsEaten,
		GemsTotal,
		EnemiesAlive,
		EnemiesKilled,
		DeathType,
		DaggersHit,
		DaggersFired,
		HomingStored,
		HomingEaten,
		IsReplay,
		Status,
		SurvivalHashMd5,
		LevelUpTime2AsBytes,
		LevelUpTime3AsBytes,
		LevelUpTime4AsBytes,
		GameMode,
		TimeAttackOrRaceFinished,
		ProhibitedMods);

	/// <summary>
	/// The <see cref="UploadRequest.HomingStored"/> value is not reliable in game memory and shouldn't be used.
	/// </summary>
	public int GetFinalHomingValue()
	{
		return GameData.HomingStored.Length == 0 ? 0 : GameData.HomingStored[^1];
	}

	public TargetCollection CreateTargetCollection()
	{
		return new()
		{
			GemsCollected = GemsCollected,
			GemsDespawned = GemsDespawned,
			GemsEaten = GemsEaten,
			EnemiesKilled = EnemiesKilled,
			DaggersFired = DaggersFired,
			DaggersHit = DaggersHit,
			HomingStored = GetFinalHomingValue(),
			HomingEaten = HomingEaten,
			DeathType = DeathType,
			Time = TimeInSeconds.To10thMilliTime(),
			LevelUpTime2 = LevelUpTime2InSeconds.To10thMilliTime(),
			LevelUpTime3 = LevelUpTime3InSeconds.To10thMilliTime(),
			LevelUpTime4 = LevelUpTime4InSeconds.To10thMilliTime(),
			EnemiesAlive = EnemiesAlive,
			Skull1Kills = GetFinalEnemyStat(urd => urd.Skull1sKilled),
			Skull2Kills = GetFinalEnemyStat(urd => urd.Skull2sKilled),
			Skull3Kills = GetFinalEnemyStat(urd => urd.Skull3sKilled),
			Skull4Kills = GetFinalEnemyStat(urd => urd.Skull4sKilled),
			SpiderlingKills = GetFinalEnemyStat(urd => urd.SpiderlingsKilled),
			SpiderEggKills = GetFinalEnemyStat(urd => urd.SpiderEggsKilled),
			Squid1Kills = GetFinalEnemyStat(urd => urd.Squid1sKilled),
			Squid2Kills = GetFinalEnemyStat(urd => urd.Squid2sKilled),
			Squid3Kills = GetFinalEnemyStat(urd => urd.Squid3sKilled),
			CentipedeKills = GetFinalEnemyStat(urd => urd.CentipedesKilled),
			GigapedeKills = GetFinalEnemyStat(urd => urd.GigapedesKilled),
			GhostpedeKills = GetFinalEnemyStat(urd => urd.GhostpedesKilled),
			Spider1Kills = GetFinalEnemyStat(urd => urd.Spider1sKilled),
			Spider2Kills = GetFinalEnemyStat(urd => urd.Spider2sKilled),
			LeviathanKills = GetFinalEnemyStat(urd => urd.LeviathansKilled),
			OrbKills = GetFinalEnemyStat(urd => urd.OrbsKilled),
			ThornKills = GetFinalEnemyStat(urd => urd.ThornsKilled),
			Skull1sAlive = GetFinalEnemyStat(urd => urd.Skull1sAlive),
			Skull2sAlive = GetFinalEnemyStat(urd => urd.Skull2sAlive),
			Skull3sAlive = GetFinalEnemyStat(urd => urd.Skull3sAlive),
			Skull4sAlive = GetFinalEnemyStat(urd => urd.Skull4sAlive),
			SpiderlingsAlive = GetFinalEnemyStat(urd => urd.SpiderlingsAlive),
			SpiderEggsAlive = GetFinalEnemyStat(urd => urd.SpiderEggsAlive),
			Squid1sAlive = GetFinalEnemyStat(urd => urd.Squid1sAlive),
			Squid2sAlive = GetFinalEnemyStat(urd => urd.Squid2sAlive),
			Squid3sAlive = GetFinalEnemyStat(urd => urd.Squid3sAlive),
			CentipedesAlive = GetFinalEnemyStat(urd => urd.CentipedesAlive),
			GigapedesAlive = GetFinalEnemyStat(urd => urd.GigapedesAlive),
			GhostpedesAlive = GetFinalEnemyStat(urd => urd.GhostpedesAlive),
			Spider1sAlive = GetFinalEnemyStat(urd => urd.Spider1sAlive),
			Spider2sAlive = GetFinalEnemyStat(urd => urd.Spider2sAlive),
			LeviathansAlive = GetFinalEnemyStat(urd => urd.LeviathansAlive),
			OrbsAlive = GetFinalEnemyStat(urd => urd.OrbsAlive),
			ThornsAlive = GetFinalEnemyStat(urd => urd.ThornsAlive),
		};
	}

	private int GetFinalEnemyStat(Func<UploadRequestData, ushort[]> selector)
	{
		ushort[] arr = selector(GameData);
		return arr.Length == 0 ? 0 : arr[^1];
	}

	public static string CreateValidationV2(
		int playerId,
		byte[] timeAsBytes,
		int gemsCollected,
		int gemsDespawned,
		int gemsEaten,
		int gemsTotal,
		int enemiesAlive,
		int enemiesKilled,
		byte deathType,
		int daggersHit,
		int daggersFired,
		int homingStored,
		int homingEaten,
		bool isReplay,
		int status,
		byte[] survivalHashMd5,
		byte[] levelUpTime2AsBytes,
		byte[] levelUpTime3AsBytes,
		byte[] levelUpTime4AsBytes,
		int gameMode,
		bool timeAttackOrRaceFinished,
		bool prohibitedMods)
	{
		return string.Join(
			";",
			playerId,
			timeAsBytes.ByteArrayToHexString(),
			gemsCollected,
			gemsDespawned,
			gemsEaten,
			gemsTotal,
			enemiesAlive,
			enemiesKilled,
			deathType,
			daggersHit,
			daggersFired,
			homingStored,
			homingEaten,
			isReplay,
			status,
			survivalHashMd5.ByteArrayToHexString(),
			levelUpTime2AsBytes.ByteArrayToHexString(),
			levelUpTime3AsBytes.ByteArrayToHexString(),
			levelUpTime4AsBytes.ByteArrayToHexString(),
			gameMode,
			timeAttackOrRaceFinished,
			prohibitedMods);
	}
}
