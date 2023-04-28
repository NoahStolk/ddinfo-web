using DevilDaggersInfo.Api.Admin.CustomEntries;
using DevilDaggersInfo.Common.Extensions;
using DevilDaggersInfo.Web.Server.Domain.Entities;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Converters;

public static class CustomEntryConverters
{
	public static GetCustomEntryForOverview ToAdminApiOverview(this CustomEntryEntity customEntry)
	{
		if (customEntry.Player == null)
			throw new InvalidOperationException("Player is not included.");

		if (customEntry.CustomLeaderboard == null)
			throw new InvalidOperationException("Custom leaderboard is not included.");

		if (customEntry.CustomLeaderboard.Spawnset == null)
			throw new InvalidOperationException("Custom leaderboard spawnset is not included.");

		return new()
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
	}

	public static GetCustomEntry ToAdminApi(this CustomEntryEntity customEntry)
	{
		if (customEntry.CustomLeaderboard == null)
			throw new InvalidOperationException("Custom leaderboard is not included.");

		return new()
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
}
