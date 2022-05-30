using DevilDaggersInfo.Web.Shared.Dto.Ddcl.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Server.Converters.Ddcl;

public static class CustomEntryConverters
{
	public static GetCustomEntryDdcl ToGetCustomEntryDdcl(this CustomEntryEntity customEntry, bool hasReplay) => new()
	{
		Id = customEntry.Id,
		PlayerId = customEntry.PlayerId,
		PlayerName = customEntry.Player.PlayerName,
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
}
