using DevilDaggersInfo.Core.Encryption;
using DevilDaggersInfo.Web.Server.Domain.Commands.CustomEntries;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Services;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using DevilDaggersInfo.Web.Server.Domain.Tests.Data;
using DevilDaggersInfo.Web.Server.Domain.Tests.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace DevilDaggersInfo.Web.Server.Domain.Tests;

[TestClass]
public class CustomEntryProcessorTests
{
	private readonly Mock<ApplicationDbContext> _dbContext;
	private readonly CustomEntryProcessor _customEntryProcessor;
	private readonly AesBase32Wrapper _encryptionWrapper;
	private readonly byte[] _mockReplay;
	private readonly byte[] _v3Hash;

	public CustomEntryProcessorTests()
	{
		MockEntities mockEntities = new();

		DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder = new();
		_dbContext = new Mock<ApplicationDbContext>(optionsBuilder.Options, new Mock<IHttpContextAccessor>().Object, new Mock<ILogContainerService>().Object)
			.SetUpDbSet(db => db.Tools, mockEntities.MockDbSetTools)
			.SetUpDbSet(db => db.Players, mockEntities.MockDbSetPlayers)
			.SetUpDbSet(db => db.Spawnsets, mockEntities.MockDbSetSpawnsets)
			.SetUpDbSet(db => db.CustomLeaderboards, mockEntities.MockDbSetCustomLeaderboards)
			.SetUpDbSet(db => db.CustomEntries, mockEntities.MockDbSetCustomEntries)
			.SetUpDbSet(db => db.CustomEntryData, mockEntities.MockDbSetCustomEntryData);

		string spawnsetsPath = Path.Combine(TestUtils.ResourcePath, "Spawnsets");
		string replaysPath = Path.Combine(TestUtils.ResourcePath, "Replays");

		Mock<IFileSystemService> fileSystemService = new();
		fileSystemService.Setup(m => m.GetPath(DataSubDirectory.Spawnsets)).Returns(spawnsetsPath);
		fileSystemService.Setup(m => m.GetPath(DataSubDirectory.CustomEntryReplays)).Returns(replaysPath);

		Directory.CreateDirectory(replaysPath);

		Mock<ILogger<SpawnsetHashCache>> spawnsetHashCacheLogger = new();
		Mock<SpawnsetHashCache> spawnsetHashCache = new(fileSystemService.Object, spawnsetHashCacheLogger.Object);
		Mock<ILogger<CustomEntryProcessor>> customEntryProcessorLogger = new();

		const string secret = "secretsecretsecr";
		Dictionary<string, object> appSettings = new()
		{
			["CustomLeaderboardSecrets"] = new Dictionary<string, string>
			{
				["InitializationVector"] = secret,
				["Password"] = secret,
				["Salt"] = secret,
			},
		};
		ConfigurationBuilder builder = new();
		builder.AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(appSettings))));
		IConfigurationRoot configuration = builder.Build();

		_encryptionWrapper = new(secret, secret, secret);
		_customEntryProcessor = new(_dbContext.Object, customEntryProcessorLogger.Object, spawnsetHashCache.Object, fileSystemService.Object, configuration, new Mock<ICustomLeaderboardSubmissionLogger>().Object);

		byte[] spawnsetFileContents = File.ReadAllBytes(Path.Combine(spawnsetsPath, "V3"));
		if (SpawnsetBinary.TryParse(spawnsetFileContents, out SpawnsetBinary? spawnsetBinary))
			_v3Hash = MD5.HashData(spawnsetBinary.ToBytes());
		else
			Assert.Fail("Spawnset could not be parsed.");

		_mockReplay = BuildMockReplay(spawnsetFileContents);
	}

	private static byte[] BuildMockReplay(byte[] spawnsetFileContents)
	{
		const string name = "user";

		using MemoryStream ms = new();
		using BinaryWriter bw = new(ms);
		bw.Write(Encoding.UTF8.GetBytes("ddrpl."));
		bw.Seek(44, SeekOrigin.Current);
		bw.Write(name.Length);
		for (int i = 0; i < name.Length; i++)
			bw.Write((byte)name[i]);

		bw.Seek(10, SeekOrigin.Current);
		bw.Write(MD5.HashData(spawnsetFileContents));
		bw.Write(spawnsetFileContents.Length);
		bw.Write(spawnsetFileContents);

		return ms.ToArray();
	}

	private UploadRequest CreateUploadRequest(float time, int playerId, int status, string clientVersion) => CreateUploadRequest(time, playerId, status, clientVersion, new());

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

		return new(
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
			client: "DevilDaggersCustomLeaderboards",
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
			status: status);
	}

	[DataTestMethod]
	[DataRow(4, new int[] { 1, 2, 3, 4 })]
	[DataRow(0, new int[] { 1, 2, 3, 0 })]
	[DataRow(9, new int[] { 1, 2, 3, 9 })]
	[DataRow(0, new int[] { 1, 2, 3, -1 })]
	[DataRow(0, new int[] { 0 })]
	[DataRow(8, new int[] { 8 })]
	[DataRow(2, new int[] { 3, 2 })]
	[DataRow(0, new int[] { })]
	public async Task TestHomingCount(int expected, int[] homingStored)
	{
		UploadRequest uploadRequest = CreateUploadRequest(1, 100, 4, TestConstants.DdclVersion, new() { HomingStored = homingStored });
		UploadResponse uploadSuccess = await _customEntryProcessor.ProcessUploadRequestAsync(uploadRequest);
		Assert.AreEqual(expected, uploadSuccess.HomingStoredState.Value);
	}

	[TestMethod]
	public async Task ProcessUploadRequest_ExistingPlayer_ExistingEntry_NoHighscore()
	{
		UploadRequest uploadRequest = CreateUploadRequest(10, 1, 3, TestConstants.DdclVersion);
		UploadResponse uploadSuccess = await _customEntryProcessor.ProcessUploadRequestAsync(uploadRequest);

		_dbContext.Verify(db => db.SaveChangesAsync(default), Times.AtLeastOnce);
		Assert.AreEqual(1, uploadSuccess.SortedEntries.Count);
		Assert.IsTrue(uploadSuccess.Message.StartsWith("No new highscore"));
	}

	[TestMethod]
	public async Task ProcessUploadRequest_ExistingPlayer_ExistingEntry_NewHighscore()
	{
		UploadRequest uploadRequest = CreateUploadRequest(20, 1, 4, TestConstants.DdclVersion);
		UploadResponse uploadSuccess = await _customEntryProcessor.ProcessUploadRequestAsync(uploadRequest);

		_dbContext.Verify(db => db.SaveChangesAsync(default), Times.AtLeastOnce);
		Assert.AreEqual(1, uploadSuccess.SortedEntries.Count);
		Assert.IsTrue(uploadSuccess.Message.StartsWith("NEW HIGHSCORE"));
	}

	[TestMethod]
	public async Task ProcessUploadRequest_ExistingPlayer_NewEntry()
	{
		UploadRequest uploadRequest = CreateUploadRequest(20, 2, 5, TestConstants.DdclVersion);
		UploadResponse uploadSuccess = await _customEntryProcessor.ProcessUploadRequestAsync(uploadRequest);

		_dbContext.Verify(db => db.CustomEntries.AddAsync(It.Is<CustomEntryEntity>(ce => ce.PlayerId == 2 && ce.Time == 200000), default), Times.Once);
		_dbContext.Verify(db => db.SaveChangesAsync(default), Times.AtLeastOnce);
		Assert.IsTrue(uploadSuccess.Message.StartsWith("Welcome"));
	}

	[TestMethod]
	public async Task ProcessUploadRequest_NewPlayer()
	{
		UploadRequest uploadRequest = CreateUploadRequest(30, 3, 3, TestConstants.DdclVersion);
		UploadResponse uploadSuccess = await _customEntryProcessor.ProcessUploadRequestAsync(uploadRequest);

		_dbContext.Verify(db => db.SaveChangesAsync(default), Times.AtLeastOnce);
		_dbContext.Verify(db => db.Players.AddAsync(It.Is<PlayerEntity>(p => p.Id == 3 && p.PlayerName == "TestPlayer3"), default), Times.Once);
		_dbContext.Verify(db => db.CustomEntries.AddAsync(It.Is<CustomEntryEntity>(ce => ce.PlayerId == 3 && ce.Time == 300000), default), Times.Once);
		Assert.IsTrue(uploadSuccess.Message.StartsWith("Welcome"));
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

		_dbContext.Verify(db => db.SaveChangesAsync(default), Times.Never);

		Assert.IsTrue(ex.Message.Contains("unsupported and outdated"));
	}

	[TestMethod]
	public async Task ProcessUploadRequest_InvalidValidation()
	{
		UploadRequest uploadRequest = CreateUploadRequest(10, 1, 4, TestConstants.DdclVersion, new(), "Malformed validation");
		CustomEntryValidationException ex = await Assert.ThrowsExceptionAsync<CustomEntryValidationException>(async () => await _customEntryProcessor.ProcessUploadRequestAsync(uploadRequest));

		_dbContext.Verify(db => db.SaveChangesAsync(default), Times.Never);

		Assert.IsTrue(ex.Message.StartsWith("Could not decrypt"));
	}
}
