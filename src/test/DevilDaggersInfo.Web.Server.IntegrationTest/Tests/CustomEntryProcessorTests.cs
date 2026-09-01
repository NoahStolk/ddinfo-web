using DevilDaggersInfo.Core.Encryption;
using DevilDaggersInfo.Web.Server.Domain.Commands.CustomEntries;
using DevilDaggersInfo.Web.Server.Domain.Configuration;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;
using DevilDaggersInfo.Web.Server.Domain.Services;
using DevilDaggersInfo.Web.Server.IntegrationTest.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Web;

namespace DevilDaggersInfo.Web.Server.IntegrationTest.Tests;

// Submissions write replay files named after the custom entry ID, which the reset makes the same for every test case.
[NotInParallel(nameof(CustomEntryProcessorTests))]
internal sealed class CustomEntryProcessorTests
{
	private readonly AesBase32Wrapper _encryptionWrapper;
	private readonly byte[] _mockReplay;
	private readonly byte[] _v3Hash;

	public CustomEntryProcessorTests()
	{
		byte[] spawnsetFileContents = CustomLeaderboardFixtures.V3SpawnsetFile;
		if (!SpawnsetBinary.TryParse(spawnsetFileContents, out SpawnsetBinary? spawnsetBinary))
			throw new InvalidOperationException("Spawnset could not be parsed.");

		_v3Hash = MD5.HashData(spawnsetBinary.ToBytes());
		_mockReplay = BuildMockReplay(spawnsetFileContents);

		// Must match the CustomLeaderboards options the test host is configured with.
		const string secret = "0123456789abcdef";
		_encryptionWrapper = new AesBase32Wrapper(secret, secret, secret);
	}

	[ClassDataSource<MySqlFixture>(Shared = SharedType.PerTestSession)]
	public required MySqlFixture MySql { get; init; }

	private static byte[] BuildMockReplay(byte[] spawnsetFileContents)
	{
		const string name = "user";

		using MemoryStream ms = new();
		using BinaryWriter bw = new(ms);
		bw.Write("ddrpl."u8);
		bw.Seek(44, SeekOrigin.Current);
		bw.Write(name.Length);
		foreach (char c in name)
			bw.Write((byte)c);

		bw.Seek(10, SeekOrigin.Current);
		bw.Write(MD5.HashData(spawnsetFileContents));
		bw.Write(spawnsetFileContents.Length);
		bw.Write(spawnsetFileContents);

		return ms.ToArray();
	}

	/// <summary>
	/// Resets the database and seeds the spawnset, leaderboard, and the two players plus the existing entry that the
	/// submission cases are written against.
	/// </summary>
	private async Task<TestApplication> ArrangeAsync()
	{
		TestApplication app = await MySql.GetApplicationAsync(nameof(CustomEntryProcessorTests));
		await app.ResetAsync();

		await app.SeedAsync(dbContext =>
		{
			dbContext.Players.Add(EntityFixtures.Player(1, "TestPlayer1"));
			dbContext.Players.Add(EntityFixtures.Player(2, "TestPlayer2"));
			dbContext.Spawnsets.Add(CustomLeaderboardFixtures.Spawnset(playerId: 1));
		});

		await app.SeedAsync(dbContext => dbContext.CustomLeaderboards.Add(CustomLeaderboardFixtures.CustomLeaderboard(spawnsetId: 1)));
		await app.SeedAsync(dbContext => dbContext.CustomEntries.Add(CustomLeaderboardFixtures.CustomEntry(customLeaderboardId: 1, playerId: 1)));

		return app;
	}

	private static CustomEntryProcessor GetProcessor(AsyncServiceScope scope)
	{
		return scope.ServiceProvider.GetRequiredService<CustomEntryProcessor>();
	}

	private UploadRequest CreateUploadRequest(float time, int playerId, int status, string clientVersion)
	{
		return CreateUploadRequest(time, playerId, status, clientVersion, new UploadRequestData());
	}

	private UploadRequest CreateUploadRequest(float time, int playerId, int status, string clientVersion, UploadRequestData gameData, string? validation = null)
	{
		const float levelUpTime2 = 0;
		const float levelUpTime3 = 0;
		const float levelUpTime4 = 0;

		byte[] timeAsBytes = BitConverter.GetBytes(time);
		const int gemsCollected = 0;
		const int gemsDespawned = 0;
		const int gemsEaten = 0;
		const int gemsTotal = 0;
		const int enemiesAlive = 0;
		const int enemiesKilled = 0;
		const byte deathType = 0;
		const int daggersHit = 0;
		const int daggersFired = 0;
		const int homingStored = 0;
		const int homingEaten = 0;
		const bool isReplay = false;
		byte[] levelUpTime2AsBytes = BitConverter.GetBytes(levelUpTime2);
		byte[] levelUpTime3AsBytes = BitConverter.GetBytes(levelUpTime3);
		byte[] levelUpTime4AsBytes = BitConverter.GetBytes(levelUpTime4);
		const int gameMode = 0;
		const bool timeAttackOrRaceFinished = false;
		const bool prohibitedMods = false;

		string calculatedValidation = UploadRequest.CreateValidationV2(
			playerId: playerId,
			timeAsBytes: timeAsBytes,
			gemsCollected: gemsCollected,
			gemsDespawned: gemsDespawned,
			gemsEaten: gemsEaten,
			gemsTotal: gemsTotal,
			enemiesAlive: enemiesAlive,
			enemiesKilled: enemiesKilled,
			deathType: deathType,
			daggersHit: daggersHit,
			daggersFired: daggersFired,
			homingStored: homingStored,
			homingEaten: homingEaten,
			isReplay: isReplay,
			status: status,
			survivalHashMd5: _v3Hash,
			levelUpTime2AsBytes: levelUpTime2AsBytes,
			levelUpTime3AsBytes: levelUpTime3AsBytes,
			levelUpTime4AsBytes: levelUpTime4AsBytes,
			gameMode: gameMode,
			timeAttackOrRaceFinished: timeAttackOrRaceFinished,
			prohibitedMods: prohibitedMods);

		return new UploadRequest(
			survivalHashMd5: _v3Hash,
			playerId: playerId,
			playerName: $"TestPlayer{playerId}",
			replayPlayerId: 0,
			timeInSeconds: time,
			timeAsBytes: timeAsBytes,
			gemsCollected: gemsCollected,
			enemiesKilled: enemiesKilled,
			daggersFired: daggersFired,
			daggersHit: daggersHit,
			enemiesAlive: enemiesAlive,
			homingStored: homingStored,
			homingEaten: homingEaten,
			gemsDespawned: gemsDespawned,
			gemsEaten: gemsEaten,
			gemsTotal: gemsTotal,
			deathType: deathType,
			levelUpTime2InSeconds: levelUpTime2,
			levelUpTime3InSeconds: levelUpTime3,
			levelUpTime4InSeconds: levelUpTime4,
			levelUpTime2AsBytes: BitConverter.GetBytes(levelUpTime2),
			levelUpTime3AsBytes: BitConverter.GetBytes(levelUpTime3),
			levelUpTime4AsBytes: BitConverter.GetBytes(levelUpTime4),
			clientVersion: clientVersion,
			client: "ddinfo-tools",
			operatingSystem: "Windows",
			buildMode: "Release",
			validation: validation ?? HttpUtility.HtmlEncode(_encryptionWrapper.EncryptAndEncode(calculatedValidation)),
			validationVersion: 2,
			isReplay: isReplay,
			prohibitedMods: prohibitedMods,
			gameMode: gameMode,
			timeAttackOrRaceFinished: timeAttackOrRaceFinished,
			gameData: gameData,
			replayData: _mockReplay,
			status: status,
			timestamps:
			[
				new UploadRequestTimestamp
				{
					Timestamp = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks,
					TimeInSeconds = 0,
				},
				new UploadRequestTimestamp
				{
					Timestamp = new DateTime(2023, 1, 1, 0, 1, 0, DateTimeKind.Utc).Ticks,
					TimeInSeconds = 60,
				},
			]);
	}

	[Test]
	[Arguments(4, new[] { 1, 2, 3, 4 })]
	[Arguments(0, new[] { 1, 2, 3, 0 })]
	[Arguments(9, new[] { 1, 2, 3, 9 })]
	[Arguments(0, new[] { 1, 2, 3, -1 })]
	[Arguments(0, new[] { 0 })]
	[Arguments(8, new[] { 8 })]
	[Arguments(2, new[] { 3, 2 })]
	[Arguments(0, new int[] { })]
	public async Task TestHomingCount(int expected, int[] homingStored)
	{
		TestApplication app = await ArrangeAsync();
		await using AsyncServiceScope scope = app.CreateScope();

		UploadRequest uploadRequest = CreateUploadRequest(1, 100, 4, TestConstants.DdclVersion, new UploadRequestData { HomingStored = homingStored });
		UploadResponse response = await GetProcessor(scope).ProcessUploadRequestAsync(uploadRequest);

		await Assert.That(response.Success).IsNotNull();
		await Assert.That(response.Success?.HomingStoredState.Value).IsEqualTo(expected);
	}

	[Test]
	public async Task ProcessUploadRequest_ExistingPlayer_ExistingEntry_NoHighscore()
	{
		TestApplication app = await ArrangeAsync();
		await using AsyncServiceScope scope = app.CreateScope();

		UploadRequest uploadRequest = CreateUploadRequest(10, 1, 3, TestConstants.DdclVersion);
		UploadResponse response = await GetProcessor(scope).ProcessUploadRequestAsync(uploadRequest);

		await Assert.That(response.Success).IsNotNull();
		await Assert.That(response.Success?.SortedEntries.Count).IsEqualTo(1);
		await Assert.That(response.Success?.SubmissionType).IsEqualTo(SubmissionType.NoHighscore);

		// The existing entry is 166666, which the 10 second run does not beat.
		await using AsyncServiceScope assertScope = app.CreateScope();
		ApplicationDbContext dbContext = assertScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
		await Assert.That(await dbContext.CustomEntries.Where(ce => ce.PlayerId == 1).Select(ce => ce.Time).SingleAsync()).IsEqualTo(166666);
	}

	[Test]
	public async Task ProcessUploadRequest_ExistingPlayer_ExistingEntry_NewHighscore()
	{
		TestApplication app = await ArrangeAsync();
		await using AsyncServiceScope scope = app.CreateScope();

		UploadRequest uploadRequest = CreateUploadRequest(20, 1, 4, TestConstants.DdclVersion);
		UploadResponse response = await GetProcessor(scope).ProcessUploadRequestAsync(uploadRequest);

		await Assert.That(response.Success).IsNotNull();
		await Assert.That(response.Success?.SortedEntries.Count).IsEqualTo(1);
		await Assert.That(response.Success?.SubmissionType).IsEqualTo(SubmissionType.NewHighscore);

		await using AsyncServiceScope assertScope = app.CreateScope();
		ApplicationDbContext dbContext = assertScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
		await Assert.That(await dbContext.CustomEntries.Where(ce => ce.PlayerId == 1).Select(ce => ce.Time).SingleAsync()).IsEqualTo(200000);
	}

	[Test]
	public async Task ProcessUploadRequest_ExistingPlayer_NewEntry()
	{
		TestApplication app = await ArrangeAsync();
		await using AsyncServiceScope scope = app.CreateScope();

		UploadRequest uploadRequest = CreateUploadRequest(20, 2, 5, TestConstants.DdclVersion);
		UploadResponse response = await GetProcessor(scope).ProcessUploadRequestAsync(uploadRequest);

		await Assert.That(response.Success?.SubmissionType).IsEqualTo(SubmissionType.FirstScore);

		await using AsyncServiceScope assertScope = app.CreateScope();
		ApplicationDbContext dbContext = assertScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
		await Assert.That(await dbContext.CustomEntries.Where(ce => ce.PlayerId == 2).Select(ce => ce.Time).SingleAsync()).IsEqualTo(200000);
	}

	[Test]
	public async Task ProcessUploadRequest_NewPlayer()
	{
		TestApplication app = await ArrangeAsync();
		await using AsyncServiceScope scope = app.CreateScope();

		UploadRequest uploadRequest = CreateUploadRequest(30, 3, 3, TestConstants.DdclVersion);
		UploadResponse response = await GetProcessor(scope).ProcessUploadRequestAsync(uploadRequest);

		await Assert.That(response.Success?.SubmissionType).IsEqualTo(SubmissionType.FirstScore);

		// The player did not exist and must have been inserted along with the entry.
		await using AsyncServiceScope assertScope = app.CreateScope();
		ApplicationDbContext dbContext = assertScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
		await Assert.That(await dbContext.Players.Where(p => p.Id == 3).Select(p => p.PlayerName).SingleAsync()).IsEqualTo("TestPlayer3");
		await Assert.That(await dbContext.CustomEntries.Where(ce => ce.PlayerId == 3).Select(ce => ce.Time).SingleAsync()).IsEqualTo(300000);
	}

	[Test]
	[Arguments(0, false)]
	[Arguments(1, false)]
	[Arguments(2, false)]
	[Arguments(3, true)]
	[Arguments(4, true)]
	[Arguments(5, true)]
	[Arguments(6, false)]
	[Arguments(7, false)]
	[Arguments(8, false)]
	public async Task ProcessUploadRequest_InvalidStatus(int status, bool accepted)
	{
		TestApplication app = await ArrangeAsync();
		await using AsyncServiceScope scope = app.CreateScope();

		UploadRequest uploadRequest = CreateUploadRequest(30, 3, status, TestConstants.DdclVersion);
		if (accepted)
			await GetProcessor(scope).ProcessUploadRequestAsync(uploadRequest);
		else
			await Assert.That(async () => await GetProcessor(scope).ProcessUploadRequestAsync(uploadRequest)).Throws<CustomEntryValidationException>();
	}

	[Test]
	public async Task ProcessUploadRequest_Outdated()
	{
		TestApplication app = await ArrangeAsync();
		await using AsyncServiceScope scope = app.CreateScope();

		UploadRequest uploadRequest = CreateUploadRequest(10, 1, 4, "0.0.0.0");
		await Assert.That(async () => await GetProcessor(scope).ProcessUploadRequestAsync(uploadRequest))
			.Throws<CustomEntryValidationException>()
			.WithMessageContaining("unsupported and outdated");

		await AssertNothingWasWrittenAsync(app);
	}

	[Test]
	public async Task ProcessUploadRequest_InvalidValidation()
	{
		TestApplication app = await ArrangeAsync();
		await using AsyncServiceScope scope = app.CreateScope();

		UploadRequest uploadRequest = CreateUploadRequest(10, 1, 4, TestConstants.DdclVersion, new UploadRequestData(), "Malformed validation");
		CustomEntryValidationException? ex = await Assert.That(async () => await GetProcessor(scope).ProcessUploadRequestAsync(uploadRequest))
			.Throws<CustomEntryValidationException>();

		await Assert.That(ex?.Message).StartsWith("Could not decrypt");
		await AssertNothingWasWrittenAsync(app);
	}

	/// <summary>
	/// The seeded state is one entry for player 1. A rejected submission must leave exactly that behind.
	/// </summary>
	private static async Task AssertNothingWasWrittenAsync(TestApplication app)
	{
		await using AsyncServiceScope scope = app.CreateScope();
		ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

		await Assert.That(await dbContext.CustomEntries.CountAsync()).IsEqualTo(1);
		await Assert.That(await dbContext.CustomEntries.Select(ce => ce.Time).SingleAsync()).IsEqualTo(166666);
		await Assert.That(await dbContext.Players.CountAsync()).IsEqualTo(2);
	}
}
