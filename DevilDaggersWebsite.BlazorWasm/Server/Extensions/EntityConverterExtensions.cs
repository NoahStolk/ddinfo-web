using DevilDaggersCore.Spawnsets;
using DevilDaggersWebsite.BlazorWasm.Server.Entities;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto.CustomEntries;
using DevilDaggersWebsite.Dto.CustomLeaderboards;
using DevilDaggersWebsite.Dto.Spawnsets;

namespace DevilDaggersWebsite.BlazorWasm.Server.Extensions
{
	public static class EntityConverterExtensions
	{
		public static GetCustomLeaderboard ToDto(this CustomLeaderboard customLeaderboard)
		{
			return new()
			{
				Id = customLeaderboard.Id,
				SpawnsetAuthorName = customLeaderboard.SpawnsetFile.Player.PlayerName,
				SpawnsetName = customLeaderboard.SpawnsetFile.Name,
				TimeBronze = customLeaderboard.TimeBronze,
				TimeSilver = customLeaderboard.TimeSilver,
				TimeGolden = customLeaderboard.TimeGolden,
				TimeDevil = customLeaderboard.TimeDevil,
				TimeLeviathan = customLeaderboard.TimeLeviathan,
				DateLastPlayed = customLeaderboard.DateLastPlayed,
				DateCreated = customLeaderboard.DateCreated,
				Category = customLeaderboard.Category,
				IsAscending = customLeaderboard.Category.IsAscending(),
			};
		}

		public static GetCustomEntryPublic ToDto(this CustomEntry customEntry)
		{
			return new()
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

		public static GetSpawnsetPublic ToDto(this SpawnsetFile spawnsetFile, SpawnsetData spawnsetData, bool hasCustomLeaderboard)
		{
			return new()
			{
				MaxDisplayWaves = spawnsetFile.MaxDisplayWaves,
				HtmlDescription = spawnsetFile.HtmlDescription,
				LastUpdated = spawnsetFile.LastUpdated,
				SpawnsetData = spawnsetData,
				Name = spawnsetFile.Name,
				AuthorName = spawnsetFile.Player.PlayerName,
				HasCustomLeaderboard = hasCustomLeaderboard,
				IsPractice = spawnsetFile.IsPractice,
			};
		}
	}
}
