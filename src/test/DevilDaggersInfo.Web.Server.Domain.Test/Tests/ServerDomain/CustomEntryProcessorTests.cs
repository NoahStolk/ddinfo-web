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

[TestClass]
public class CustomEntryProcessorTests
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
		if (SpawnsetBinary.TryParse(spawnsetFileContents, out SpawnsetBinary? spawnsetBinary))
			_v3Hash = MD5.HashData(spawnsetBinary.ToBytes());
		else
			Assert.Fail("Spawnset could not be parsed.");

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

	[DataTestMethod]
	[DataRow(4, new[] { 1, 2, 3, 4 })]
	[DataRow(0, new[] { 1, 2, 3, 0 })]
	[DataRow(9, new[] { 1, 2, 3, 9 })]
	[DataRow(0, new[] { 1, 2, 3, -1 })]
	[DataRow(0, new[] { 0 })]
	[DataRow(8, new[] { 8 })]
	[DataRow(2, new[] { 3, 2 })]
	[DataRow(0, new int[] { })]
	public async Task TestHomingCount(int expected, int[] homingStored)
	{
		UploadRequest uploadRequest = CreateUploadRequest(1, 100, 4, TestConstants.DdclVersion, new UploadRequestData { HomingStored = homingStored });
		UploadResponse response = await _customEntryProcessor.ProcessUploadRequestAsync(uploadRequest);
		Assert.IsNotNull(response.Success);
		Assert.AreEqual(expected, response.Success.HomingStoredState.Value);
	}

	[TestMethod]
	public async Task ProcessUploadRequest_ExistingPlayer_ExistingEntry_NoHighscore()
	{
		UploadRequest uploadRequest = CreateUploadRequest(10, 1, 3, TestConstants.DdclVersion);
		UploadResponse response = await _customEntryProcessor.ProcessUploadRequestAsync(uploadRequest);
		Assert.IsNotNull(response.Success);

		await _dbContext.ReceivedWithAnyArgs().SaveChangesAsync();
		Assert.AreEqual(1, response.Success.SortedEntries.Count);
		Assert.AreEqual(SubmissionType.NoHighscore, response.Success.SubmissionType);
	}

	[TestMethod]
	public async Task ProcessUploadRequest_ExistingPlayer_ExistingEntry_NewHighscore()
	{
		UploadRequest uploadRequest = CreateUploadRequest(20, 1, 4, TestConstants.DdclVersion);
		UploadResponse response = await _customEntryProcessor.ProcessUploadRequestAsync(uploadRequest);
		Assert.IsNotNull(response.Success);

		await _dbContext.ReceivedWithAnyArgs().SaveChangesAsync();
		Assert.AreEqual(1, response.Success.SortedEntries.Count);
		Assert.AreEqual(SubmissionType.NewHighscore, response.Success.SubmissionType);
	}

	[TestMethod]
	public async Task ProcessUploadRequest_ExistingPlayer_NewEntry()
	{
		UploadRequest uploadRequest = CreateUploadRequest(20, 2, 5, TestConstants.DdclVersion);
		UploadResponse response = await _customEntryProcessor.ProcessUploadRequestAsync(uploadRequest);
		Assert.IsNotNull(response.Success);

		await _dbContext.CustomEntries.Received(1).AddAsync(Arg.Is<CustomEntryEntity>(ce => ce.PlayerId == 2 && ce.Time == 200000));
		await _dbContext.ReceivedWithAnyArgs().SaveChangesAsync();
		Assert.AreEqual(SubmissionType.FirstScore, response.Success.SubmissionType);
	}

	[TestMethod]
	public async Task ProcessUploadRequest_NewPlayer()
	{
		UploadRequest uploadRequest = CreateUploadRequest(30, 3, 3, TestConstants.DdclVersion);
		UploadResponse response = await _customEntryProcessor.ProcessUploadRequestAsync(uploadRequest);
		Assert.IsNotNull(response.Success);

		await _dbContext.ReceivedWithAnyArgs().SaveChangesAsync();
		await _dbContext.Players.Received(1).AddAsync(Arg.Is<PlayerEntity>(p => p.Id == 3 && p.PlayerName == "TestPlayer3"));
		await _dbContext.CustomEntries.Received(1).AddAsync(Arg.Is<CustomEntryEntity>(ce => ce.PlayerId == 3 && ce.Time == 300000));
		Assert.AreEqual(SubmissionType.FirstScore, response.Success.SubmissionType);
	}

	[DataTestMethod]
	[DataRow(0, false)]
	[DataRow(1, false)]
	[DataRow(2, false)]
	[DataRow(3, true)]
	[DataRow(4, true)]
	[DataRow(5, true)]
	[DataRow(6, false)]
	[DataRow(7, false)]
	[DataRow(8, false)]
	public async Task ProcessUploadRequest_InvalidStatus(int status, bool accepted)
	{
		UploadRequest uploadRequest = CreateUploadRequest(30, 3, status, TestConstants.DdclVersion);
		if (accepted)
			await _customEntryProcessor.ProcessUploadRequestAsync(uploadRequest);
		else
			await Assert.ThrowsExceptionAsync<CustomEntryValidationException>(async () => await _customEntryProcessor.ProcessUploadRequestAsync(uploadRequest));
	}

	[TestMethod]
	public async Task ProcessUploadRequest_Outdated()
	{
		UploadRequest uploadRequest = CreateUploadRequest(10, 1, 4, "0.0.0.0");
		CustomEntryValidationException ex = await Assert.ThrowsExceptionAsync<CustomEntryValidationException>(async () => await _customEntryProcessor.ProcessUploadRequestAsync(uploadRequest));

		await _dbContext.DidNotReceive().SaveChangesAsync();

		Assert.IsTrue(ex.Message.Contains("unsupported and outdated"));
	}

	[TestMethod]
	public async Task ProcessUploadRequest_InvalidValidation()
	{
		UploadRequest uploadRequest = CreateUploadRequest(10, 1, 4, TestConstants.DdclVersion, new UploadRequestData(), "Malformed validation");
		CustomEntryValidationException ex = await Assert.ThrowsExceptionAsync<CustomEntryValidationException>(async () => await _customEntryProcessor.ProcessUploadRequestAsync(uploadRequest));

		await _dbContext.DidNotReceive().SaveChangesAsync();

		Assert.IsTrue(ex.Message.StartsWith("Could not decrypt"));
	}
}
