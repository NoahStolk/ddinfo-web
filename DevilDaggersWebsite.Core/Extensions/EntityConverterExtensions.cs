using DevilDaggersWebsite.Core.Entities;

namespace DevilDaggersWebsite.Core.Extensions
{
	public static class EntityConverterExtensions
	{
		public static Dto.CustomLeaderboard ToDto(this CustomLeaderboard customLeaderboard)
		{
			return new Dto.CustomLeaderboard
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
			};
		}

		public static Dto.CustomEntry ToDto(this CustomEntry customEntry, string username)
		{
			return new Dto.CustomEntry
			{
				PlayerId = customEntry.PlayerId,
				Username = username,
				ClientVersion = customEntry.ClientVersion,
				DeathType = customEntry.DeathType,
				EnemiesAlive = customEntry.EnemiesAlive,
				Gems = customEntry.Gems,
				Homing = customEntry.Homing,
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