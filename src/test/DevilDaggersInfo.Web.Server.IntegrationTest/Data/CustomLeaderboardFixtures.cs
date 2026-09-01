namespace DevilDaggersInfo.Web.Server.IntegrationTest.Data;

/// <summary>
/// The spawnset, leaderboard and entry that the custom entry submission tests are written against. These are seeded as
/// real rows rather than mocked, so the identifiers below are the ones the database will actually assign.
/// </summary>
internal static class CustomLeaderboardFixtures
{
	internal static byte[] V3SpawnsetFile { get; } = File.ReadAllBytes(Path.Combine("Resources", "Spawnsets", "V3"));

	internal static SpawnsetEntity Spawnset(int playerId)
	{
		return new SpawnsetEntity
		{
			Id = 1,
			LastUpdated = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
			Name = "V3",
			PlayerId = playerId,
			HtmlDescription = string.Empty,
			MaxDisplayWaves = 5,
			File = V3SpawnsetFile,
			Md5Hash = MD5.HashData(V3SpawnsetFile),
			GameMode = SpawnsetGameMode.Survival,
			AdditionalGems = 0,
			HandLevel = SpawnsetHandLevel.Level1,
			LoopLength = 56,
			LoopSpawnCount = 17,
			SpawnVersion = 4,
			WorldVersion = 9,
			TimerStart = 0,
			EffectiveHandLevel = SpawnsetHandLevel.Level1,
			EffectiveHandMesh = SpawnsetHandLevel.Level1,
			PreLoopLength = 451,
			PreLoopSpawnCount = 90,
			EffectiveGemsOrHoming = 0,
		};
	}

	internal static CustomLeaderboardEntity CustomLeaderboard(int spawnsetId)
	{
		return new CustomLeaderboardEntity
		{
			Id = 1,
			Bronze = 600000,
			Silver = 1200000,
			Golden = 2500000,
			Devil = 5000000,
			Leviathan = 10000000,
			RankSorting = CustomLeaderboardRankSorting.TimeDesc,
			DateCreated = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
			DateLastPlayed = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
			SpawnsetId = spawnsetId,
			TotalRunsSubmitted = 666,
		};
	}

	internal static CustomEntryEntity CustomEntry(int customLeaderboardId, int playerId)
	{
		return new CustomEntryEntity
		{
			Id = 1,
			ClientVersion = TestConstants.DdclVersion,
			CustomLeaderboardId = customLeaderboardId,
			DaggersFired = 15,
			DaggersHit = 6,
			DeathType = 1,
			EnemiesAlive = 6,
			GemsCollected = 3,
			HomingStored = 0,
			EnemiesKilled = 2,
			PlayerId = playerId,
			Time = 166666,
			LevelUpTime2 = 0,
			LevelUpTime3 = 0,
			LevelUpTime4 = 0,
			SubmitDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
		};
	}
}
