using DevilDaggersInfo.Web.Server.Domain.Commands.CustomEntries;
using ToolsApi = DevilDaggersInfo.Web.ApiSpec.Tools.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Server.Converters.ApiToDomain.Tools;

public static class CustomLeaderboardConverters
{
	public static UploadRequest ToDomain(this ToolsApi.AddUploadRequest uploadRequest)
	{
		UploadRequestData gameData = new()
		{
			GemsCollected = uploadRequest.GameData.GemsCollected.ToArray(),
			EnemiesKilled = uploadRequest.GameData.EnemiesKilled.ToArray(),
			DaggersFired = uploadRequest.GameData.DaggersFired.ToArray(),
			DaggersHit = uploadRequest.GameData.DaggersHit.ToArray(),
			EnemiesAlive = uploadRequest.GameData.EnemiesAlive.ToArray(),
			HomingStored = uploadRequest.GameData.HomingStored.ToArray(),
			HomingEaten = uploadRequest.GameData.HomingEaten.ToArray(),
			GemsDespawned = uploadRequest.GameData.GemsDespawned.ToArray(),
			GemsEaten = uploadRequest.GameData.GemsEaten.ToArray(),
			GemsTotal = uploadRequest.GameData.GemsTotal.ToArray(),
			Skull1sAlive = uploadRequest.GameData.Skull1sAlive.ToArray(),
			Skull2sAlive = uploadRequest.GameData.Skull2sAlive.ToArray(),
			Skull3sAlive = uploadRequest.GameData.Skull3sAlive.ToArray(),
			SpiderlingsAlive = uploadRequest.GameData.SpiderlingsAlive.ToArray(),
			Skull4sAlive = uploadRequest.GameData.Skull4sAlive.ToArray(),
			Squid1sAlive = uploadRequest.GameData.Squid1sAlive.ToArray(),
			Squid2sAlive = uploadRequest.GameData.Squid2sAlive.ToArray(),
			Squid3sAlive = uploadRequest.GameData.Squid3sAlive.ToArray(),
			CentipedesAlive = uploadRequest.GameData.CentipedesAlive.ToArray(),
			GigapedesAlive = uploadRequest.GameData.GigapedesAlive.ToArray(),
			Spider1sAlive = uploadRequest.GameData.Spider1sAlive.ToArray(),
			Spider2sAlive = uploadRequest.GameData.Spider2sAlive.ToArray(),
			LeviathansAlive = uploadRequest.GameData.LeviathansAlive.ToArray(),
			OrbsAlive = uploadRequest.GameData.OrbsAlive.ToArray(),
			ThornsAlive = uploadRequest.GameData.ThornsAlive.ToArray(),
			GhostpedesAlive = uploadRequest.GameData.GhostpedesAlive.ToArray(),
			SpiderEggsAlive = uploadRequest.GameData.SpiderEggsAlive.ToArray(),
			Skull1sKilled = uploadRequest.GameData.Skull1sKilled.ToArray(),
			Skull2sKilled = uploadRequest.GameData.Skull2sKilled.ToArray(),
			Skull3sKilled = uploadRequest.GameData.Skull3sKilled.ToArray(),
			SpiderlingsKilled = uploadRequest.GameData.SpiderlingsKilled.ToArray(),
			Skull4sKilled = uploadRequest.GameData.Skull4sKilled.ToArray(),
			Squid1sKilled = uploadRequest.GameData.Squid1sKilled.ToArray(),
			Squid2sKilled = uploadRequest.GameData.Squid2sKilled.ToArray(),
			Squid3sKilled = uploadRequest.GameData.Squid3sKilled.ToArray(),
			CentipedesKilled = uploadRequest.GameData.CentipedesKilled.ToArray(),
			GigapedesKilled = uploadRequest.GameData.GigapedesKilled.ToArray(),
			Spider1sKilled = uploadRequest.GameData.Spider1sKilled.ToArray(),
			Spider2sKilled = uploadRequest.GameData.Spider2sKilled.ToArray(),
			LeviathansKilled = uploadRequest.GameData.LeviathansKilled.ToArray(),
			OrbsKilled = uploadRequest.GameData.OrbsKilled.ToArray(),
			ThornsKilled = uploadRequest.GameData.ThornsKilled.ToArray(),
			GhostpedesKilled = uploadRequest.GameData.GhostpedesKilled.ToArray(),
			SpiderEggsKilled = uploadRequest.GameData.SpiderEggsKilled.ToArray(),
		};

		List<UploadRequestTimestamp> timestamps = uploadRequest.Timestamps?.ConvertAll(t => new UploadRequestTimestamp
		{
			Timestamp = t.Timestamp,
			TimeInSeconds = t.TimeInSeconds,
		}) ?? [];

		return new(
			survivalHashMd5: uploadRequest.SurvivalHashMd5,
			playerId: uploadRequest.PlayerId,
			playerName: uploadRequest.PlayerName,
			replayPlayerId: uploadRequest.ReplayPlayerId,
			timeInSeconds: uploadRequest.TimeInSeconds,
			timeAsBytes: uploadRequest.TimeAsBytes,
			gemsCollected: uploadRequest.GemsCollected,
			enemiesKilled: uploadRequest.EnemiesKilled,
			daggersFired: uploadRequest.DaggersFired,
			daggersHit: uploadRequest.DaggersHit,
			enemiesAlive: uploadRequest.EnemiesAlive,
			homingStored: uploadRequest.HomingStored,
			homingEaten: uploadRequest.HomingEaten,
			gemsDespawned: uploadRequest.GemsDespawned,
			gemsEaten: uploadRequest.GemsEaten,
			gemsTotal: uploadRequest.GemsTotal,
			deathType: uploadRequest.DeathType,
			levelUpTime2InSeconds: uploadRequest.LevelUpTime2InSeconds,
			levelUpTime3InSeconds: uploadRequest.LevelUpTime3InSeconds,
			levelUpTime4InSeconds: uploadRequest.LevelUpTime4InSeconds,
			levelUpTime2AsBytes: uploadRequest.LevelUpTime2AsBytes,
			levelUpTime3AsBytes: uploadRequest.LevelUpTime3AsBytes,
			levelUpTime4AsBytes: uploadRequest.LevelUpTime4AsBytes,
			clientVersion: uploadRequest.ClientVersion,
			client: uploadRequest.Client,
			operatingSystem: uploadRequest.OperatingSystem,
			buildMode: uploadRequest.BuildMode,
			validation: uploadRequest.Validation,
			validationVersion: uploadRequest.ValidationVersion,
			isReplay: uploadRequest.IsReplay,
			prohibitedMods: uploadRequest.ProhibitedMods,
			gameMode: uploadRequest.GameMode,
			timeAttackOrRaceFinished: uploadRequest.TimeAttackOrRaceFinished,
			gameData: gameData,
			replayData: uploadRequest.ReplayData,
			status: uploadRequest.Status,
			timestamps: timestamps);
	}
}
