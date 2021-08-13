using DevilDaggersInfo.Web.BlazorWasm.Server.Entities;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.CustomEntries;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Enums;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Extensions;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Converters.Admin
{
	public static class CustomEntryConverters
	{
		public static GetCustomEntry ToGetCustomEntry(this CustomEntryEntity customEntry) => new()
		{
			Id = customEntry.Id,
			ClientVersion = customEntry.ClientVersion,
			DaggersFired = customEntry.DaggersFired,
			DaggersHit = customEntry.DaggersHit,
			DeathType = (DeathType)customEntry.DeathType,
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
}
