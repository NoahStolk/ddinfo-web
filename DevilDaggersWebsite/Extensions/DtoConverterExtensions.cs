using DevilDaggersWebsite.Dto;
using System;
using System.Linq;

namespace DevilDaggersWebsite.Extensions
{
	public static class DtoConverterExtensions
	{
		public static Entities.CustomEntry ToCustomEntryEntity(this UploadRequest uploadRequest, Entities.CustomLeaderboard customLeaderboard)
		{
			return new()
			{
				PlayerId = uploadRequest.PlayerId,
				Time = uploadRequest.Time,
				Gems = uploadRequest.GemsCollected,
				GemsDespawned = uploadRequest.GemsDespawned,
				GemsEaten = uploadRequest.GemsEaten,
				Kills = uploadRequest.Kills,
				DeathType = uploadRequest.DeathType,
				DaggersHit = uploadRequest.DaggersHit,
				DaggersFired = uploadRequest.DaggersFired,
				EnemiesAlive = uploadRequest.EnemiesAlive,
				Homing = uploadRequest.HomingDaggers,
				LevelUpTime2 = uploadRequest.LevelUpTime2,
				LevelUpTime3 = uploadRequest.LevelUpTime3,
				LevelUpTime4 = uploadRequest.LevelUpTime4,
				SubmitDate = DateTime.Now,
				ClientVersion = uploadRequest.ClientVersion,
				CustomLeaderboard = customLeaderboard,
				GemsData = string.Join(",", uploadRequest.GameStates.Select(gs => gs.GemsCollected)),
				GemsDespawnedData = string.Join(",", uploadRequest.GameStates.Select(gs => gs.GemsDespawned)),
				GemsEatenData = string.Join(",", uploadRequest.GameStates.Select(gs => gs.GemsEaten)),
				KillsData = string.Join(",", uploadRequest.GameStates.Select(gs => gs.Kills)),
				HomingData = string.Join(",", uploadRequest.GameStates.Select(gs => gs.HomingDaggers)),
				EnemiesAliveData = string.Join(",", uploadRequest.GameStates.Select(gs => gs.EnemiesAlive)),
				DaggersFiredData = string.Join(",", uploadRequest.GameStates.Select(gs => gs.DaggersFired)),
				DaggersHitData = string.Join(",", uploadRequest.GameStates.Select(gs => gs.DaggersHit)),
			};
		}
	}
}
