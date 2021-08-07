using DevilDaggersWebsite.BlazorWasm.Server.Entities;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto.Public.CustomEntries;

namespace DevilDaggersWebsite.BlazorWasm.Server.Converters.Public
{
	public static class CustomEntryConverters
	{
		public static GetCustomEntryDdcl ToGetCustomEntryDdcl(this CustomEntry customEntry) => new()
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
			HomingDaggers = customEntry.HomingDaggers,
			HomingDaggersEaten = customEntry.HomingDaggersEaten,
			EnemiesKilled = customEntry.EnemiesKilled,
			LevelUpTime2 = customEntry.LevelUpTime2,
			LevelUpTime3 = customEntry.LevelUpTime3,
			LevelUpTime4 = customEntry.LevelUpTime4,
			DaggersFired = customEntry.DaggersFired,
			DaggersHit = customEntry.DaggersHit,
			SubmitDate = customEntry.SubmitDate,
			Time = customEntry.Time,
		};

		public static GetCustomEntry ToGetCustomEntry(this CustomEntry customEntry) => new()
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
			HomingDaggers = customEntry.HomingDaggers,
			HomingDaggersEaten = customEntry.HomingDaggersEaten,
			EnemiesKilled = customEntry.EnemiesKilled,
			LevelUpTime2 = customEntry.LevelUpTime2,
			LevelUpTime3 = customEntry.LevelUpTime3,
			LevelUpTime4 = customEntry.LevelUpTime4,
			DaggersFired = customEntry.DaggersFired,
			DaggersHit = customEntry.DaggersHit,
			SubmitDate = customEntry.SubmitDate,
			Time = customEntry.Time,
		};
	}
}
