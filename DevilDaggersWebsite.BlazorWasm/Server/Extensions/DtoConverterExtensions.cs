using DevilDaggersWebsite.BlazorWasm.Shared.Dto.Public.CustomEntries;
using System;

namespace DevilDaggersWebsite.BlazorWasm.Server.Extensions
{
	public static class DtoConverterExtensions
	{
		public static Entities.CustomEntry ToCustomEntryEntity(this AddUploadRequest uploadRequest, Entities.CustomLeaderboard customLeaderboard)
		{
			return new()
			{
				PlayerId = uploadRequest.PlayerId,
				Time = uploadRequest.Time,
				GemsCollected = uploadRequest.GemsCollected,
				GemsDespawned = uploadRequest.GemsDespawned,
				GemsEaten = uploadRequest.GemsEaten,
				GemsTotal = uploadRequest.GemsTotal,
				EnemiesKilled = uploadRequest.EnemiesKilled,
				DeathType = uploadRequest.DeathType,
				DaggersHit = uploadRequest.DaggersHit,
				DaggersFired = uploadRequest.DaggersFired,
				EnemiesAlive = uploadRequest.EnemiesAlive,
				HomingDaggers = uploadRequest.HomingDaggers,
				HomingDaggersEaten = uploadRequest.HomingDaggersEaten,
				LevelUpTime2 = uploadRequest.LevelUpTime2,
				LevelUpTime3 = uploadRequest.LevelUpTime3,
				LevelUpTime4 = uploadRequest.LevelUpTime4,
				SubmitDate = DateTime.UtcNow,
				ClientVersion = uploadRequest.ClientVersion,
				CustomLeaderboard = customLeaderboard,
			};
		}
	}
}
