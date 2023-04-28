using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using MockQueryable.Moq;

namespace DevilDaggersInfo.Web.Server.Domain.Tests.Data;

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
		};

		CustomLeaderboard.Spawnset = Spawnset;
		CustomEntry.CustomLeaderboard = CustomLeaderboard;
		CustomEntry.Player = TestPlayer1;

		PropertyInfo[] properties = typeof(MockEntities).GetProperties();

		MockDbSetTools = GetEntities<ToolEntity>().AsQueryable().BuildMockDbSet();
		MockDbSetToolDistributions = GetEntities<ToolDistributionEntity>().AsQueryable().BuildMockDbSet();
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

	public Mock<DbSet<ToolEntity>> MockDbSetTools { get; }
	public Mock<DbSet<ToolDistributionEntity>> MockDbSetToolDistributions { get; }
	public Mock<DbSet<PlayerEntity>> MockDbSetPlayers { get; }
	public Mock<DbSet<SpawnsetEntity>> MockDbSetSpawnsets { get; }
	public Mock<DbSet<CustomLeaderboardEntity>> MockDbSetCustomLeaderboards { get; }
	public Mock<DbSet<CustomEntryEntity>> MockDbSetCustomEntries { get; }
	public Mock<DbSet<CustomEntryDataEntity>> MockDbSetCustomEntryData { get; }

	public ToolEntity Ddcl { get; } = new()
	{
		Name = "DevilDaggersCustomLeaderboards",
		RequiredVersionNumber = "1.2.0.0",
		DisplayName = "Devil Daggers Custom Leaderboards",
		CurrentVersionNumber = "1.2.0.0",
	};

	public ToolDistributionEntity Ddse2_WindowsWpf_Old { get; } = new()
	{
		ToolName = "DevilDaggersSurvivalEditor",
		VersionNumber = "2.0.0.0",
		BuildType = ToolBuildType.WindowsWpf,
		PublishMethod = ToolPublishMethod.SelfContained,
	};

	public ToolDistributionEntity Ddse2_WindowsWpf { get; } = new()
	{
		ToolName = "DevilDaggersSurvivalEditor",
		VersionNumber = "2.45.0.0",
		BuildType = ToolBuildType.WindowsWpf,
		PublishMethod = ToolPublishMethod.SelfContained,
	};

	public ToolDistributionEntity Ddse2_WindowsWpf_Windows7 { get; } = new()
	{
		ToolName = "DevilDaggersSurvivalEditor",
		VersionNumber = "2.45.0.0",
		BuildType = ToolBuildType.WindowsWpf,
		PublishMethod = ToolPublishMethod.Default,
	};

	public ToolDistributionEntity Ddse3_WindowsPhotino { get; } = new()
	{
		ToolName = "DevilDaggersSurvivalEditor",
		VersionNumber = "3.0.0-alpha.0",
		BuildType = (ToolBuildType)2,
		PublishMethod = ToolPublishMethod.SelfContained,
	};

	public ToolDistributionEntity Ddse3_LinuxPhotino { get; } = new()
	{
		ToolName = "DevilDaggersSurvivalEditor",
		VersionNumber = "3.0.0-alpha.0",
		BuildType = (ToolBuildType)3,
		PublishMethod = ToolPublishMethod.SelfContained,
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

	public SpawnsetEntity Spawnset { get; }

	public CustomLeaderboardEntity CustomLeaderboard { get; } = new()
	{
		Id = 1,
		Bronze = 600000,
		Silver = 1200000,
		Golden = 2500000,
		Devil = 5000000,
		Leviathan = 10000000,
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
