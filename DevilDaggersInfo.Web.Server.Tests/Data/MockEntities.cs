using DevilDaggersInfo.Web.Shared.Enums;
using MockQueryable.Moq;

namespace DevilDaggersInfo.Web.Server.Tests.Data;

public class MockEntities
{
	public MockEntities()
	{
		Spawnset.Player = TestPlayer1;
		CustomLeaderboard.Spawnset = Spawnset;
		CustomEntry.CustomLeaderboard = CustomLeaderboard;
		CustomEntry.Player = TestPlayer1;

		PropertyInfo[] properties = typeof(MockEntities).GetProperties();

		MockDbSetTools = GetEntities<ToolEntity>().AsQueryable().BuildMockDbSet();
		MockDbSetPlayers = GetEntities<PlayerEntity>().AsQueryable().BuildMockDbSet();
		MockDbSetSpawnsets = GetEntities<SpawnsetEntity>().AsQueryable().BuildMockDbSet();
		MockDbSetCustomLeaderboards = GetEntities<CustomLeaderboardEntity>().AsQueryable().BuildMockDbSet();
		MockDbSetCustomEntries = GetEntities<CustomEntryEntity>().AsQueryable().BuildMockDbSet();
		MockDbSetCustomEntryData = GetEntities<CustomEntryDataEntity>().AsQueryable().BuildMockDbSet();

		T[] GetEntities<T>() => properties
			.Where(pi => pi.PropertyType == typeof(T))
			.Select(pi => (T)pi.GetValue(this)!)
			.ToArray();
	}

	public Mock<DbSet<ToolEntity>> MockDbSetTools { get; }
	public Mock<DbSet<PlayerEntity>> MockDbSetPlayers { get; }
	public Mock<DbSet<SpawnsetEntity>> MockDbSetSpawnsets { get; }
	public Mock<DbSet<CustomLeaderboardEntity>> MockDbSetCustomLeaderboards { get; }
	public Mock<DbSet<CustomEntryEntity>> MockDbSetCustomEntries { get; }
	public Mock<DbSet<CustomEntryDataEntity>> MockDbSetCustomEntryData { get; }

	public ToolEntity Ddcl { get; } = new()
	{
		Name = "DevilDaggersCustomLeaderboards",
		RequiredVersionNumber = "1.2.0.0",
	};

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

	public SpawnsetEntity Spawnset { get; } = new()
	{
		Id = 1,
		LastUpdated = DateTime.UtcNow,
		Name = "V3",
		PlayerId = 1,
		HtmlDescription = string.Empty,
		MaxDisplayWaves = 5,
	};

	public CustomLeaderboardEntity CustomLeaderboard { get; } = new()
	{
		Id = 1,
		TimeBronze = 600000,
		TimeSilver = 1200000,
		TimeGolden = 2500000,
		TimeDevil = 5000000,
		TimeLeviathan = 10000000,
		Category = CustomLeaderboardCategory.Survival,
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
