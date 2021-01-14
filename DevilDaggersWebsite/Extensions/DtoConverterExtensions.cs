using DevilDaggersWebsite.Dto;
using System;
using System.Linq;

namespace DevilDaggersWebsite.Extensions
{
	public static class DtoConverterExtensions
	{
		public static Entities.CustomEntry ToCustomEntryEntity(this UploadRequest uploadRequest, Entities.CustomLeaderboard customLeaderboard)
		{
			return new Entities.CustomEntry
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
				GemsData = string.Join(",", uploadRequest.GameStates.Select(gs => gs.Gems)),
				KillsData = string.Join(",", uploadRequest.GameStates.Select(gs => gs.Kills)),
				HomingData = string.Join(",", uploadRequest.GameStates.Select(gs => gs.Homing)),
				EnemiesAliveData = string.Join(",", uploadRequest.GameStates.Select(gs => gs.EnemiesAlive)),
				DaggersFiredData = string.Join(",", uploadRequest.GameStates.Select(gs => gs.DaggersFired)),
				DaggersHitData = string.Join(",", uploadRequest.GameStates.Select(gs => gs.DaggersHit)),
			};
		}
	}
}
