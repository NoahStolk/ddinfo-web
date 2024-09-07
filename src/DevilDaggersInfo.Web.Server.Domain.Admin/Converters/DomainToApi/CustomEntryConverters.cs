using DevilDaggersInfo.Core.Common;
using DevilDaggersInfo.Web.ApiSpec.Admin.CustomEntries;
using DevilDaggersInfo.Web.Server.Domain.Entities;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Converters.DomainToApi;

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
			LevelUpTime2 = GameTime.FromGameUnits(customEntry.LevelUpTime2).Seconds,
			LevelUpTime3 = GameTime.FromGameUnits(customEntry.LevelUpTime3).Seconds,
			LevelUpTime4 = GameTime.FromGameUnits(customEntry.LevelUpTime4).Seconds,
			PlayerName = customEntry.Player.PlayerName,
			SpawnsetName = customEntry.CustomLeaderboard.Spawnset.Name,
			SubmitDate = customEntry.SubmitDate,
			Time = GameTime.FromGameUnits(customEntry.Time).Seconds,
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
			LevelUpTime2 = GameTime.FromGameUnits(customEntry.LevelUpTime2).Seconds,
			LevelUpTime3 = GameTime.FromGameUnits(customEntry.LevelUpTime3).Seconds,
			LevelUpTime4 = GameTime.FromGameUnits(customEntry.LevelUpTime4).Seconds,
			PlayerId = customEntry.PlayerId,
			SpawnsetId = customEntry.CustomLeaderboard.SpawnsetId,
			SubmitDate = customEntry.SubmitDate,
			Time = GameTime.FromGameUnits(customEntry.Time).Seconds,
		};
	}
}
