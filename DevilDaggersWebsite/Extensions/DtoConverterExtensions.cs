using DevilDaggersWebsite.Dto;
using System;
using System.Collections.Generic;
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
				Gems = uploadRequest.Gems,
				Kills = uploadRequest.Kills,
				DeathType = uploadRequest.DeathType,
				DaggersHit = uploadRequest.DaggersHit,
				DaggersFired = uploadRequest.DaggersFired,
				EnemiesAlive = uploadRequest.EnemiesAlive,
				Homing = uploadRequest.Homing,
				LevelUpTime2 = uploadRequest.LevelUpTime2,
				LevelUpTime3 = uploadRequest.LevelUpTime3,
				LevelUpTime4 = uploadRequest.LevelUpTime4,
				SubmitDate = DateTime.Now,
				ClientVersion = uploadRequest.ClientVersion,
				CustomLeaderboard = customLeaderboard,
				GemsData = string.Join(",", uploadRequest.GameStates.Select(gs => gs.Gems) ?? new List<int>()),
				KillsData = string.Join(",", uploadRequest.GameStates.Select(gs => gs.Kills) ?? new List<int>()),
				HomingData = string.Join(",", uploadRequest.GameStates.Select(gs => gs.Homing) ?? new List<int>()),
				EnemiesAliveData = string.Join(",", uploadRequest.GameStates.Select(gs => gs.EnemiesAlive) ?? new List<int>()),
				DaggersFiredData = string.Join(",", uploadRequest.GameStates.Select(gs => gs.DaggersFired) ?? new List<int>()),
				DaggersHitData = string.Join(",", uploadRequest.GameStates.Select(gs => gs.DaggersHit) ?? new List<int>()),
			};
		}
	}
}
