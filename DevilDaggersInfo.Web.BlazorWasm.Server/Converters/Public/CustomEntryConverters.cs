using DevilDaggersInfo.Web.BlazorWasm.Server.Entities.Views;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomEntries;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Converters.Public;

public static class CustomEntryConverters
{
	public static GetCustomEntryDdcl ToGetCustomEntryDdcl(this CustomEntryDdclResult customEntry, bool hasReplay) => new()
	{
		Id = customEntry.Id,
		PlayerId = customEntry.PlayerId,
		PlayerName = customEntry.PlayerName,
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
		LevelUpTime2InSeconds = customEntry.LevelUpTime2.ToSecondsTime(),
		LevelUpTime3InSeconds = customEntry.LevelUpTime3.ToSecondsTime(),
		LevelUpTime4InSeconds = customEntry.LevelUpTime4.ToSecondsTime(),
		DaggersFired = customEntry.DaggersFired,
		DaggersHit = customEntry.DaggersHit,
		SubmitDate = customEntry.SubmitDate,
		TimeInSeconds = customEntry.Time.ToSecondsTime(),
		HasReplay = hasReplay,
	};

	public static GetCustomEntry ToGetCustomEntry(this CustomEntry customEntry, CustomLeaderboardEntity customLeaderboard, int rank)
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
			PlayerName = customEntry.PlayerName,
			CountryCode = customEntry.CountryCode,
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
			CustomLeaderboardDagger = customLeaderboard.GetDaggerFromTime(customEntry.Time),
			HasGraphs = hasGraphs,
		};
	}

	public static GetCustomEntryDdLive ToGetCustomEntryDdLive(this CustomEntry customEntry, CustomLeaderboardEntity customLeaderboard, int rank, bool hasReplay)
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
			PlayerName = customEntry.PlayerName,
			CountryCode = customEntry.CountryCode,
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
			CustomLeaderboardDagger = customLeaderboard.GetDaggerFromTime(customEntry.Time),
			HasGraphs = hasGraphs,
			HasReplay = hasReplay,
		};
	}

	public static GetCustomEntryData ToGetCustomEntryData(this CustomEntryEntity customEntry, CustomEntryDataEntity? customEntryData, HandLevel startingLevel, bool hasReplay)
	{
		return new()
		{
			CustomEntryId = customEntry.Id,
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

			GemsCollectedData = GetInt32Arr(customEntryData?.GemsCollectedData),
			EnemiesKilledData = GetInt32Arr(customEntryData?.EnemiesKilledData),
			DaggersFiredData = GetInt32Arr(customEntryData?.DaggersFiredData),
			DaggersHitData = GetInt32Arr(customEntryData?.DaggersHitData),
			EnemiesAliveData = GetInt32Arr(customEntryData?.EnemiesAliveData),
			HomingStoredData = GetInt32Arr(customEntryData?.HomingStoredData),
			HomingEatenData = GetInt32Arr(customEntryData?.HomingEatenData),
			GemsDespawnedData = GetInt32Arr(customEntryData?.GemsDespawnedData),
			GemsEatenData = GetInt32Arr(customEntryData?.GemsEatenData),
			GemsTotalData = GetInt32Arr(customEntryData?.GemsTotalData),

			Skull1sAliveData = GetUInt16Arr(customEntryData?.Skull1sAliveData),
			Skull2sAliveData = GetUInt16Arr(customEntryData?.Skull2sAliveData),
			Skull3sAliveData = GetUInt16Arr(customEntryData?.Skull3sAliveData),
			SpiderlingsAliveData = GetUInt16Arr(customEntryData?.SpiderlingsAliveData),
			Skull4sAliveData = GetUInt16Arr(customEntryData?.Skull4sAliveData),
			Squid1sAliveData = GetUInt16Arr(customEntryData?.Squid1sAliveData),
			Squid2sAliveData = GetUInt16Arr(customEntryData?.Squid2sAliveData),
			Squid3sAliveData = GetUInt16Arr(customEntryData?.Squid3sAliveData),
			CentipedesAliveData = GetUInt16Arr(customEntryData?.CentipedesAliveData),
			GigapedesAliveData = GetUInt16Arr(customEntryData?.GigapedesAliveData),
			Spider1sAliveData = GetUInt16Arr(customEntryData?.Spider1sAliveData),
			Spider2sAliveData = GetUInt16Arr(customEntryData?.Spider2sAliveData),
			LeviathansAliveData = GetUInt16Arr(customEntryData?.LeviathansAliveData),
			OrbsAliveData = GetUInt16Arr(customEntryData?.OrbsAliveData),
			ThornsAliveData = GetUInt16Arr(customEntryData?.ThornsAliveData),
			GhostpedesAliveData = GetUInt16Arr(customEntryData?.GhostpedesAliveData),
			SpiderEggsAliveData = GetUInt16Arr(customEntryData?.SpiderEggsAliveData),

			Skull1sKilledData = GetUInt16Arr(customEntryData?.Skull1sKilledData),
			Skull2sKilledData = GetUInt16Arr(customEntryData?.Skull2sKilledData),
			Skull3sKilledData = GetUInt16Arr(customEntryData?.Skull3sKilledData),
			SpiderlingsKilledData = GetUInt16Arr(customEntryData?.SpiderlingsKilledData),
			Skull4sKilledData = GetUInt16Arr(customEntryData?.Skull4sKilledData),
			Squid1sKilledData = GetUInt16Arr(customEntryData?.Squid1sKilledData),
			Squid2sKilledData = GetUInt16Arr(customEntryData?.Squid2sKilledData),
			Squid3sKilledData = GetUInt16Arr(customEntryData?.Squid3sKilledData),
			CentipedesKilledData = GetUInt16Arr(customEntryData?.CentipedesKilledData),
			GigapedesKilledData = GetUInt16Arr(customEntryData?.GigapedesKilledData),
			Spider1sKilledData = GetUInt16Arr(customEntryData?.Spider1sKilledData),
			Spider2sKilledData = GetUInt16Arr(customEntryData?.Spider2sKilledData),
			LeviathansKilledData = GetUInt16Arr(customEntryData?.LeviathansKilledData),
			OrbsKilledData = GetUInt16Arr(customEntryData?.OrbsKilledData),
			ThornsKilledData = GetUInt16Arr(customEntryData?.ThornsKilledData),
			GhostpedesKilledData = GetUInt16Arr(customEntryData?.GhostpedesKilledData),
			SpiderEggsKilledData = GetUInt16Arr(customEntryData?.SpiderEggsKilledData),

			StartingLevel = startingLevel,
			HasReplay = hasReplay,
		};

		static int[]? GetInt32Arr(byte[]? bytes)
			=> bytes == null || bytes.Length == 0 ? null : IntegerArrayCompressor.ExtractData(bytes);

		static ushort[]? GetUInt16Arr(byte[]? bytes)
			=> bytes == null || bytes.Length == 0 ? null : Array.ConvertAll(IntegerArrayCompressor.ExtractData(bytes), i => (ushort)i);
	}
}
