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
				GemsCollected = uploadRequest.GemsCollected,
				GemsDespawned = uploadRequest.GemsDespawned,
				GemsEaten = uploadRequest.GemsEaten,
				EnemiesKilled = uploadRequest.EnemiesKilled,
				DeathType = uploadRequest.DeathType,
				DaggersHit = uploadRequest.DaggersHit,
				DaggersFired = uploadRequest.DaggersFired,
				EnemiesAlive = uploadRequest.EnemiesAlive,
				HomingDaggers = uploadRequest.HomingDaggers,
				LevelUpTime2 = uploadRequest.LevelUpTime2,
				LevelUpTime3 = uploadRequest.LevelUpTime3,
				LevelUpTime4 = uploadRequest.LevelUpTime4,
				SubmitDate = DateTime.Now,
				ClientVersion = uploadRequest.ClientVersion,
				CustomLeaderboard = customLeaderboard,
				GemsCollectedData = uploadRequest.GameStates.Select(gs => gs.GemsCollected).SelectMany(BitConverter.GetBytes).ToArray(),
				EnemiesKilledData = uploadRequest.GameStates.Select(gs => gs.EnemiesKilled).SelectMany(BitConverter.GetBytes).ToArray(),
				DaggersFiredData = uploadRequest.GameStates.Select(gs => gs.DaggersFired).SelectMany(BitConverter.GetBytes).ToArray(),
				DaggersHitData = uploadRequest.GameStates.Select(gs => gs.DaggersHit).SelectMany(BitConverter.GetBytes).ToArray(),
				EnemiesAliveData = uploadRequest.GameStates.Select(gs => gs.EnemiesAlive).SelectMany(BitConverter.GetBytes).ToArray(),
				HomingDaggersData = uploadRequest.GameStates.Select(gs => gs.HomingDaggers).SelectMany(BitConverter.GetBytes).ToArray(),
				GemsDespawnedData = uploadRequest.GameStates.Select(gs => gs.GemsDespawned).SelectMany(BitConverter.GetBytes).ToArray(),
				GemsEatenData = uploadRequest.GameStates.Select(gs => gs.GemsEaten).SelectMany(BitConverter.GetBytes).ToArray(),
				GemsTotalData = uploadRequest.GameStates.ConvertAll(gs => gs.GemsTotal).SelectMany(BitConverter.GetBytes).ToArray(),
			};
		}
	}
}
