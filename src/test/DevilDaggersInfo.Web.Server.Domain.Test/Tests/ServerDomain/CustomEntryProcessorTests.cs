using DevilDaggersInfo.Core.Encryption;
using DevilDaggersInfo.Web.Server.Domain.Commands.CustomEntries;
using DevilDaggersInfo.Web.Server.Domain.Configuration;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Services;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using DevilDaggersInfo.Web.Server.Domain.Test.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace DevilDaggersInfo.Web.Server.Domain.Test.Tests.ServerDomain;

// Uploads write replay files named after the custom entry ID, which is the same for every test case.
[NotInParallel]
internal sealed class CustomEntryProcessorTests
{
	private readonly ApplicationDbContext _dbContext;
	private readonly CustomEntryProcessor _customEntryProcessor;
	private readonly AesBase32Wrapper _encryptionWrapper;
	private readonly byte[] _mockReplay;
	private readonly byte[] _v3Hash;

	public CustomEntryProcessorTests()
	{
		string spawnsetsPath = Path.Combine("Resources", "Spawnsets");
		byte[] spawnsetFileContents = File.ReadAllBytes(Path.Combine(spawnsetsPath, "V3"));
		if (!SpawnsetBinary.TryParse(spawnsetFileContents, out SpawnsetBinary? spawnsetBinary))
			throw new InvalidOperationException("Spawnset could not be parsed.");

		_v3Hash = MD5.HashData(spawnsetBinary.ToBytes());

		MockEntities mockEntities = new();

		DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder = new();
		_dbContext = Substitute.For<ApplicationDbContext>(optionsBuilder.Options, Substitute.For<IHttpContextAccessor>(), Substitute.For<ILogContainerService>());
		_dbContext.Players.Returns(mockEntities.MockDbSetPlayers);
		_dbContext.Spawnsets.Returns(mockEntities.MockDbSetSpawnsets);
		_dbContext.CustomLeaderboards.Returns(mockEntities.MockDbSetCustomLeaderboards);
		_dbContext.CustomEntries.Returns(mockEntities.MockDbSetCustomEntries);
		_dbContext.CustomEntryData.Returns(mockEntities.MockDbSetCustomEntryData);

		IFileSystemService fileSystemService = Substitute.For<IFileSystemService>();
		string replaysPath = Path.Combine("Resources", "Replays");
		fileSystemService.GetPath(DataSubDirectory.CustomEntryReplays).Returns(replaysPath);
		Directory.CreateDirectory(replaysPath);

		ILogger<CustomEntryProcessor> customEntryProcessorLogger = Substitute.For<ILogger<CustomEntryProcessor>>();

		const string secret = "0123456789abcdef";
		_encryptionWrapper = new AesBase32Wrapper(secret, secret, secret);

		CustomLeaderboardsOptions options = new()
		{
			InitializationVector = secret,
			Password = secret,
			Salt = secret,
		};

		_customEntryProcessor = new CustomEntryProcessor(_dbContext, customEntryProcessorLogger, fileSystemService, new OptionsWrapper<CustomLeaderboardsOptions>(options), Substitute.For<ICustomLeaderboardHighscoreLogger>(), Substitute.For<ICustomLeaderboardSubmissionLogger>())
		{
			IsUnitTest = true,
		};
		_mockReplay = BuildMockReplay(spawnsetFileContents);
	}

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
		UploadRequest uploadRequest = CreateUploadRequest(1, 100, 4, TestConstants.DdclVersion, new UploadRequestData { HomingStored = homingStored });
		UploadResponse response = await _customEntryProcessor.ProcessUploadRequestAsync(uploadRequest);
		await Assert.That(response.Success).IsNotNull();
		await Assert.That(response.Success?.HomingStoredState.Value).IsEqualTo(expected);
	}

	[Test]
	public async Task ProcessUploadRequest_ExistingPlayer_ExistingEntry_NoHighscore()
	{
		UploadRequest uploadRequest = CreateUploadRequest(10, 1, 3, TestConstants.DdclVersion);
		UploadResponse response = await _customEntryProcessor.ProcessUploadRequestAsync(uploadRequest);
		await Assert.That(response.Success).IsNotNull();

		await _dbContext.ReceivedWithAnyArgs().SaveChangesAsync();
		await Assert.That(response.Success?.SortedEntries.Count).IsEqualTo(1);
		await Assert.That(response.Success?.SubmissionType).IsEqualTo(SubmissionType.NoHighscore);
	}

	[Test]
	public async Task ProcessUploadRequest_ExistingPlayer_ExistingEntry_NewHighscore()
	{
		UploadRequest uploadRequest = CreateUploadRequest(20, 1, 4, TestConstants.DdclVersion);
		UploadResponse response = await _customEntryProcessor.ProcessUploadRequestAsync(uploadRequest);
		await Assert.That(response.Success).IsNotNull();

		await _dbContext.ReceivedWithAnyArgs().SaveChangesAsync();
		await Assert.That(response.Success?.SortedEntries.Count).IsEqualTo(1);
		await Assert.That(response.Success?.SubmissionType).IsEqualTo(SubmissionType.NewHighscore);
	}

	[Test]
	public async Task ProcessUploadRequest_ExistingPlayer_NewEntry()
	{
		UploadRequest uploadRequest = CreateUploadRequest(20, 2, 5, TestConstants.DdclVersion);
		UploadResponse response = await _customEntryProcessor.ProcessUploadRequestAsync(uploadRequest);
		await Assert.That(response.Success).IsNotNull();

		await _dbContext.CustomEntries.Received(1).AddAsync(Arg.Is<CustomEntryEntity>(ce => ce.PlayerId == 2 && ce.Time == 200000));
		await _dbContext.ReceivedWithAnyArgs().SaveChangesAsync();
		await Assert.That(response.Success?.SubmissionType).IsEqualTo(SubmissionType.FirstScore);
	}

	[Test]
	public async Task ProcessUploadRequest_NewPlayer()
	{
		UploadRequest uploadRequest = CreateUploadRequest(30, 3, 3, TestConstants.DdclVersion);
		UploadResponse response = await _customEntryProcessor.ProcessUploadRequestAsync(uploadRequest);
		await Assert.That(response.Success).IsNotNull();

		await _dbContext.ReceivedWithAnyArgs().SaveChangesAsync();
		await _dbContext.Players.Received(1).AddAsync(Arg.Is<PlayerEntity>(p => p.Id == 3 && p.PlayerName == "TestPlayer3"));
		await _dbContext.CustomEntries.Received(1).AddAsync(Arg.Is<CustomEntryEntity>(ce => ce.PlayerId == 3 && ce.Time == 300000));
		await Assert.That(response.Success?.SubmissionType).IsEqualTo(SubmissionType.FirstScore);
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
		UploadRequest uploadRequest = CreateUploadRequest(30, 3, status, TestConstants.DdclVersion);
		if (accepted)
			await _customEntryProcessor.ProcessUploadRequestAsync(uploadRequest);
		else
			await Assert.That(async () => await _customEntryProcessor.ProcessUploadRequestAsync(uploadRequest)).Throws<CustomEntryValidationException>();
	}

	[Test]
	public async Task ProcessUploadRequest_Outdated()
	{
		UploadRequest uploadRequest = CreateUploadRequest(10, 1, 4, "0.0.0.0");
		await Assert.That(async () => await _customEntryProcessor.ProcessUploadRequestAsync(uploadRequest))
			.Throws<CustomEntryValidationException>()
			.WithMessageContaining("unsupported and outdated");

		await _dbContext.DidNotReceive().SaveChangesAsync();
	}

	[Test]
	public async Task ProcessUploadRequest_InvalidValidation()
	{
		UploadRequest uploadRequest = CreateUploadRequest(10, 1, 4, TestConstants.DdclVersion, new UploadRequestData(), "Malformed validation");
		CustomEntryValidationException? ex = await Assert.That(async () => await _customEntryProcessor.ProcessUploadRequestAsync(uploadRequest))
			.Throws<CustomEntryValidationException>();

		await _dbContext.DidNotReceive().SaveChangesAsync();

		await Assert.That(ex?.Message).StartsWith("Could not decrypt");
	}
}
