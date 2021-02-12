using DevilDaggersWebsite.Entities;

namespace DevilDaggersWebsite.Extensions
{
	public static class EntityConverterExtensions
	{
		public static Dto.CustomLeaderboard ToDto(this CustomLeaderboard customLeaderboard)
		{
			return new()
			{
				SpawnsetAuthorName = customLeaderboard.SpawnsetFile.Player.Username,
				SpawnsetName = customLeaderboard.SpawnsetFile.Name,
				Bronze = customLeaderboard.Bronze,
				Silver = customLeaderboard.Silver,
				Golden = customLeaderboard.Golden,
				Devil = customLeaderboard.Devil,
				Homing = customLeaderboard.Homing,
				DateLastPlayed = customLeaderboard.DateLastPlayed,
				DateCreated = customLeaderboard.DateCreated,
				Category = customLeaderboard.Category,
				IsAscending = customLeaderboard.IsAscending(),
			};
		}

		public static Dto.CustomEntry ToDto(this CustomEntry customEntry, string username)
		{
			return new()
			{
				PlayerId = customEntry.PlayerId,
				Username = username,
				ClientVersion = customEntry.ClientVersion,
				DeathType = customEntry.DeathType,
				EnemiesAlive = customEntry.EnemiesAlive,
				GemsCollected = customEntry.Gems,
				GemsDespawned = customEntry.GemsDespawned,
				GemsEaten = customEntry.GemsEaten,
				HomingDaggers = customEntry.Homing,
				Kills = customEntry.Kills,
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
}
