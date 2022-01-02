using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomEntries;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Converters.Public;

public static class CustomEntryConverters
{
	public static GetCustomEntryDdcl ToGetCustomEntryDdcl(this CustomEntryEntity customEntry) => new()
	{
		PlayerId = customEntry.PlayerId,
		PlayerName = customEntry.Player.PlayerName,
		ClientVersion = customEntry.ClientVersion,
		DeathType = customEntry.DeathType,
		EnemiesAlive = customEntry.EnemiesAlive,
		GemsCollected = customEntry.GemsCollected,
		GemsDespawned = customEntry.GemsDespawned,
		GemsEaten = customEntry.GemsEaten,
		GemsTotal = customEntry.GemsTotal,
		HomingDaggers = customEntry.HomingStored,
		HomingDaggersEaten = customEntry.HomingEaten,
		EnemiesKilled = customEntry.EnemiesKilled,
		LevelUpTime2 = customEntry.LevelUpTime2,
		LevelUpTime3 = customEntry.LevelUpTime3,
		LevelUpTime4 = customEntry.LevelUpTime4,
		DaggersFired = customEntry.DaggersFired,
		DaggersHit = customEntry.DaggersHit,
		SubmitDate = customEntry.SubmitDate,
		Time = customEntry.Time,
	};

	public static GetCustomEntry ToGetCustomEntry(this CustomEntryEntity customEntry, int rank, bool hasReplay)
	{
		if (!Version.TryParse(customEntry.ClientVersion, out Version? version))
			version = new(0, 0, 0, 0);

		bool isDdcl = customEntry.Client == CustomLeaderboardsClient.DevilDaggersCustomLeaderboards;
		bool hasHomingEatenValue = !isDdcl || version >= FeatureConstants.DdclHomingEaten;
		bool hasV3_1Values = !isDdcl || version >= FeatureConstants.DdclV3_1;
		bool hasGraphs = !isDdcl || version >= FeatureConstants.DdclGraphs;

		return new()
		{
			Id = customEntry.Id,
			Rank = rank,
			PlayerId = customEntry.PlayerId,
			PlayerName = customEntry.Player.PlayerName,
			CountryCode = customEntry.Player.CountryCode,
			Client = customEntry.Client,
			ClientVersion = customEntry.ClientVersion,
			DeathType = customEntry.DeathType,
			EnemiesAlive = customEntry.EnemiesAlive,
			GemsCollected = customEntry.GemsCollected,
			GemsDespawned = hasV3_1Values ? customEntry.GemsDespawned : null,
			GemsEaten = hasV3_1Values ? customEntry.GemsEaten : null,
			HomingStored = customEntry.HomingStored,
			HomingEaten = hasHomingEatenValue ? customEntry.HomingEaten : null,
			EnemiesKilled = customEntry.EnemiesKilled,
			LevelUpTime2 = customEntry.LevelUpTime2.ToSecondsTime(),
			LevelUpTime3 = customEntry.LevelUpTime3.ToSecondsTime(),
			LevelUpTime4 = customEntry.LevelUpTime4.ToSecondsTime(),
			DaggersFired = customEntry.DaggersFired,
			DaggersHit = customEntry.DaggersHit,
			SubmitDate = customEntry.SubmitDate,
			Time = customEntry.Time.ToSecondsTime(),
			CustomLeaderboardDagger = customEntry.CustomLeaderboard.GetDaggerFromTime(customEntry.Time),
			HasReplay = hasReplay,
			HasGraphs = hasGraphs,
		};
	}

	public static GetCustomEntryData ToGetCustomEntryData(this CustomEntryEntity customEntry, CustomEntryDataEntity? customEntryData, HandLevel startingLevel)
	{
		return new()
		{
			PlayerId = customEntry.PlayerId,
			PlayerName = customEntry.Player.PlayerName,
			SpawnsetName = customEntry.CustomLeaderboard.Spawnset.Name,
			ClientVersion = customEntry.ClientVersion,
			DeathType = customEntry.DeathType,
			EnemiesAlive = customEntry.EnemiesAlive,
			GemsCollected = customEntry.GemsCollected,
			GemsDespawned = customEntry.GemsDespawned,
			GemsEaten = customEntry.GemsEaten,
			GemsTotal = customEntry.GemsTotal,
			HomingStored = customEntry.HomingStored,
			HomingEaten = customEntry.HomingEaten,
			EnemiesKilled = customEntry.EnemiesKilled,
			LevelUpTime2 = customEntry.LevelUpTime2.ToSecondsTime(),
			LevelUpTime3 = customEntry.LevelUpTime3.ToSecondsTime(),
			LevelUpTime4 = customEntry.LevelUpTime4.ToSecondsTime(),
			DaggersFired = customEntry.DaggersFired,
			DaggersHit = customEntry.DaggersHit,
			SubmitDate = customEntry.SubmitDate,
			Time = customEntry.Time.ToSecondsTime(),
			CustomLeaderboardDagger = customEntry.CustomLeaderboard.GetDaggerFromTime(customEntry.Time),

			GemsCollectedData = GetIntArr(customEntryData?.GemsCollectedData),
			EnemiesKilledData = GetIntArr(customEntryData?.EnemiesKilledData),
			DaggersFiredData = GetIntArr(customEntryData?.DaggersFiredData),
			DaggersHitData = GetIntArr(customEntryData?.DaggersHitData),
			EnemiesAliveData = GetIntArr(customEntryData?.EnemiesAliveData),
			HomingStoredData = GetIntArr(customEntryData?.HomingStoredData),
			HomingEatenData = GetIntArr(customEntryData?.HomingEatenData),
			GemsDespawnedData = GetIntArr(customEntryData?.GemsDespawnedData),
			GemsEatenData = GetIntArr(customEntryData?.GemsEatenData),
			GemsTotalData = GetIntArr(customEntryData?.GemsTotalData),

			Skull1sAliveData = GetUshortArr(customEntryData?.Skull1sAliveData),
			Skull2sAliveData = GetUshortArr(customEntryData?.Skull2sAliveData),
			Skull3sAliveData = GetUshortArr(customEntryData?.Skull3sAliveData),
			SpiderlingsAliveData = GetUshortArr(customEntryData?.SpiderlingsAliveData),
			Skull4sAliveData = GetUshortArr(customEntryData?.Skull4sAliveData),
			Squid1sAliveData = GetUshortArr(customEntryData?.Squid1sAliveData),
			Squid2sAliveData = GetUshortArr(customEntryData?.Squid2sAliveData),
			Squid3sAliveData = GetUshortArr(customEntryData?.Squid3sAliveData),
			CentipedesAliveData = GetUshortArr(customEntryData?.CentipedesAliveData),
			GigapedesAliveData = GetUshortArr(customEntryData?.GigapedesAliveData),
			Spider1sAliveData = GetUshortArr(customEntryData?.Spider1sAliveData),
			Spider2sAliveData = GetUshortArr(customEntryData?.Spider2sAliveData),
			LeviathansAliveData = GetUshortArr(customEntryData?.LeviathansAliveData),
			OrbsAliveData = GetUshortArr(customEntryData?.OrbsAliveData),
			ThornsAliveData = GetUshortArr(customEntryData?.ThornsAliveData),
			GhostpedesAliveData = GetUshortArr(customEntryData?.GhostpedesAliveData),
			SpiderEggsAliveData = GetUshortArr(customEntryData?.SpiderEggsAliveData),

			Skull1sKilledData = GetUshortArr(customEntryData?.Skull1sKilledData),
			Skull2sKilledData = GetUshortArr(customEntryData?.Skull2sKilledData),
			Skull3sKilledData = GetUshortArr(customEntryData?.Skull3sKilledData),
			SpiderlingsKilledData = GetUshortArr(customEntryData?.SpiderlingsKilledData),
			Skull4sKilledData = GetUshortArr(customEntryData?.Skull4sKilledData),
			Squid1sKilledData = GetUshortArr(customEntryData?.Squid1sKilledData),
			Squid2sKilledData = GetUshortArr(customEntryData?.Squid2sKilledData),
			Squid3sKilledData = GetUshortArr(customEntryData?.Squid3sKilledData),
			CentipedesKilledData = GetUshortArr(customEntryData?.CentipedesKilledData),
			GigapedesKilledData = GetUshortArr(customEntryData?.GigapedesKilledData),
			Spider1sKilledData = GetUshortArr(customEntryData?.Spider1sKilledData),
			Spider2sKilledData = GetUshortArr(customEntryData?.Spider2sKilledData),
			LeviathansKilledData = GetUshortArr(customEntryData?.LeviathansKilledData),
			OrbsKilledData = GetUshortArr(customEntryData?.OrbsKilledData),
			ThornsKilledData = GetUshortArr(customEntryData?.ThornsKilledData),
			GhostpedesKilledData = GetUshortArr(customEntryData?.GhostpedesKilledData),
			SpiderEggsKilledData = GetUshortArr(customEntryData?.SpiderEggsKilledData),

			StartingLevel = startingLevel,
		};

		static int[]? GetIntArr(byte[]? bytes)
			=> bytes == null || bytes.Length == 0 ? null : IntegerArrayCompressor.ExtractData(bytes);

		static ushort[]? GetUshortArr(byte[]? bytes)
			=> bytes == null || bytes.Length == 0 ? null : IntegerArrayCompressor.ExtractData(bytes).Select(d => (ushort)d).ToArray();
	}
}
