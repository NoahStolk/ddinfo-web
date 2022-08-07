using DevilDaggersInfo.Api.Admin.CustomEntries;

namespace DevilDaggersInfo.Web.Server.Converters.DomainToApi.Admin;

public static class CustomEntryConverters
{
	public static GetCustomEntryForOverview ToGetCustomEntryForOverview(this CustomEntryEntity customEntry) => new()
	{
		Id = customEntry.Id,
		ClientVersion = customEntry.ClientVersion,
		DaggersFired = customEntry.DaggersFired,
		DaggersHit = customEntry.DaggersHit,
		DeathType = (CustomEntryDeathType)customEntry.DeathType,
		EnemiesAlive = customEntry.EnemiesAlive,
		EnemiesKilled = customEntry.EnemiesKilled,
		GemsCollected = customEntry.GemsCollected,
		GemsDespawned = customEntry.GemsDespawned,
		GemsEaten = customEntry.GemsEaten,
		GemsTotal = customEntry.GemsTotal,
		HomingStored = customEntry.HomingStored,
		HomingEaten = customEntry.HomingEaten,
		LevelUpTime2 = customEntry.LevelUpTime2.ToSecondsTime(),
		LevelUpTime3 = customEntry.LevelUpTime3.ToSecondsTime(),
		LevelUpTime4 = customEntry.LevelUpTime4.ToSecondsTime(),
		PlayerName = customEntry.Player.PlayerName,
		SpawnsetName = customEntry.CustomLeaderboard.Spawnset.Name,
		SubmitDate = customEntry.SubmitDate,
		Time = customEntry.Time.ToSecondsTime(),
	};

	public static GetCustomEntry ToGetCustomEntry(this CustomEntryEntity customEntry) => new()
	{
		Id = customEntry.Id,
		ClientVersion = customEntry.ClientVersion,
		DaggersFired = customEntry.DaggersFired,
		DaggersHit = customEntry.DaggersHit,
		DeathType = (CustomEntryDeathType)customEntry.DeathType,
		EnemiesAlive = customEntry.EnemiesAlive,
		EnemiesKilled = customEntry.EnemiesKilled,
		GemsCollected = customEntry.GemsCollected,
		GemsDespawned = customEntry.GemsDespawned,
		GemsEaten = customEntry.GemsEaten,
		GemsTotal = customEntry.GemsTotal,
		HomingStored = customEntry.HomingStored,
		HomingEaten = customEntry.HomingEaten,
		LevelUpTime2 = customEntry.LevelUpTime2.ToSecondsTime(),
		LevelUpTime3 = customEntry.LevelUpTime3.ToSecondsTime(),
		LevelUpTime4 = customEntry.LevelUpTime4.ToSecondsTime(),
		PlayerId = customEntry.PlayerId,
		SpawnsetId = customEntry.CustomLeaderboard.SpawnsetId,
		SubmitDate = customEntry.SubmitDate,
		Time = customEntry.Time.ToSecondsTime(),
	};
}
