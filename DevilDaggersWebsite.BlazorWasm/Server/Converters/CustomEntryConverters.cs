using DevilDaggersWebsite.BlazorWasm.Server.Entities;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto.CustomEntries;

namespace DevilDaggersWebsite.BlazorWasm.Server.Converters
{
	public static class CustomEntryConverters
	{
		public static GetCustomEntry ToGetCustomEntry(this CustomEntry customEntry) => new()
		{
			Id = customEntry.Id,
			ClientVersion = customEntry.ClientVersion,
			DaggersFired = customEntry.DaggersFired,
			DaggersHit = customEntry.DaggersHit,
			DeathType = customEntry.DeathType,
			EnemiesAlive = customEntry.EnemiesAlive,
			EnemiesKilled = customEntry.EnemiesKilled,
			GemsCollected = customEntry.GemsCollected,
			GemsDespawned = customEntry.GemsDespawned,
			GemsEaten = customEntry.GemsEaten,
			GemsTotal = customEntry.GemsTotal,
			HomingDaggers = customEntry.HomingDaggers,
			HomingDaggersEaten = customEntry.HomingDaggersEaten,
			LevelUpTime2 = customEntry.LevelUpTime2 / 10000f,
			LevelUpTime3 = customEntry.LevelUpTime3 / 10000f,
			LevelUpTime4 = customEntry.LevelUpTime4 / 10000f,
			PlayerName = customEntry.Player.PlayerName,
			SpawnsetName = customEntry.CustomLeaderboard.SpawnsetFile.Name,
			SubmitDate = customEntry.SubmitDate,
			Time = customEntry.Time / 10000f,
		};

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

		public static GetCustomEntryPublic ToGetCustomEntryPublic(this CustomEntry customEntry) => new()
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
