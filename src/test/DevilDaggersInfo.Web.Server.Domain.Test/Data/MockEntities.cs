using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using MockQueryable.NSubstitute;

namespace DevilDaggersInfo.Web.Server.Domain.Test.Data;

public class MockEntities
{
	public MockEntities()
	{
		byte[] v3 = File.ReadAllBytes(Path.Combine("Resources", "Spawnsets", "V3"));
		Spawnset = new()
		{
			Id = 1,
			LastUpdated = DateTime.UtcNow,
			Name = "V3",
			PlayerId = 1,
			HtmlDescription = string.Empty,
			MaxDisplayWaves = 5,
			File = v3,
			Md5Hash = MD5.HashData(v3),
			Player = TestPlayer1,
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
			IsPractice = false,
		};

		CustomLeaderboard.Spawnset = Spawnset;
		CustomEntry.CustomLeaderboard = CustomLeaderboard;
		CustomEntry.Player = TestPlayer1;

		PropertyInfo[] properties = typeof(MockEntities).GetProperties();

		MockDbSetPlayers = GetEntities<PlayerEntity>().AsQueryable().BuildMockDbSet();
		MockDbSetSpawnsets = GetEntities<SpawnsetEntity>().AsQueryable().BuildMockDbSet();
		MockDbSetCustomLeaderboards = GetEntities<CustomLeaderboardEntity>().AsQueryable().BuildMockDbSet();
		MockDbSetCustomEntries = GetEntities<CustomEntryEntity>().AsQueryable().BuildMockDbSet();
		MockDbSetCustomEntryData = GetEntities<CustomEntryDataEntity>().AsQueryable().BuildMockDbSet();

		// ! Reflection.
		T[] GetEntities<T>() => properties
			.Where(pi => pi.PropertyType == typeof(T))
			.Select(pi => (T)pi.GetValue(this)!)
			.ToArray();
	}

	public DbSet<PlayerEntity> MockDbSetPlayers { get; }
	public DbSet<SpawnsetEntity> MockDbSetSpawnsets { get; }
	public DbSet<CustomLeaderboardEntity> MockDbSetCustomLeaderboards { get; }
	public DbSet<CustomEntryEntity> MockDbSetCustomEntries { get; }
	public DbSet<CustomEntryDataEntity> MockDbSetCustomEntryData { get; }

	public PlayerEntity TestPlayer1 { get; } = new()
	{
		Id = 1,
		PlayerName = "TestPlayer1",
	};

	public PlayerEntity TestPlayer2 { get; } = new()
	{
		Id = 2,
		PlayerName = "TestPlayer2",
	};

	public SpawnsetEntity Spawnset { get; }

	public CustomLeaderboardEntity CustomLeaderboard { get; } = new()
	{
		Id = 1,
		Bronze = 600000,
		Silver = 1200000,
		Golden = 2500000,
		Devil = 5000000,
		Leviathan = 10000000,
		RankSorting = CustomLeaderboardRankSorting.TimeDesc,
		DateCreated = DateTime.UtcNow,
		DateLastPlayed = DateTime.UtcNow,
		SpawnsetId = 1,
		TotalRunsSubmitted = 666,
	};

	public CustomEntryEntity CustomEntry { get; } = new()
	{
		Id = 1,
		ClientVersion = TestConstants.DdclVersion,
		CustomLeaderboardId = 1,
		DaggersFired = 15,
		DaggersHit = 6,
		DeathType = 1,
		EnemiesAlive = 6,
		GemsCollected = 3,
		HomingStored = 0,
		EnemiesKilled = 2,
		PlayerId = 1,
		Time = 166666,
		LevelUpTime2 = 0,
		LevelUpTime3 = 0,
		LevelUpTime4 = 0,
		SubmitDate = DateTime.UtcNow,
	};
}
