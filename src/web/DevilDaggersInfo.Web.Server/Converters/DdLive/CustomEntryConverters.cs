using DevilDaggersInfo.Web.Server.InternalModels.CustomEntries;
using DevilDaggersInfo.Web.Shared.Dto.Public.CustomEntries;

namespace DevilDaggersInfo.Web.Server.Converters.DdLive;

public static class CustomEntryConverters
{
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
}
