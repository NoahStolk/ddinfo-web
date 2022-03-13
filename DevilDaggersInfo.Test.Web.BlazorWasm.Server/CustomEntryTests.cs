using DevilDaggersInfo.Core.Encryption;
using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.SpawnsetHashes;
using DevilDaggersInfo.Web.BlazorWasm.Server.Enums;
using DevilDaggersInfo.Web.BlazorWasm.Server.Exceptions;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomEntries;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace DevilDaggersInfo.Test.Web.BlazorWasm.Server;

[TestClass]
public class CustomEntryTests
{
	private readonly Mock<ApplicationDbContext> _dbContext;
	private readonly CustomEntryProcessor _customEntryProcessor;
	private readonly AesBase32Wrapper _encryptionWrapper;
	private readonly byte[] _fakeReplay;
	private readonly byte[] _v3Hash;

	public CustomEntryTests()
	{
		MockEntities mockEntities = new();

		DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder = new();
		_dbContext = new Mock<ApplicationDbContext>(optionsBuilder.Options)
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
		Mock<IWebHostEnvironment> environment = new();

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
		_customEntryProcessor = new(_dbContext.Object, customEntryProcessorLogger.Object, spawnsetHashCache.Object, fileSystemService.Object, environment.Object, configuration, new LogContainerService());

		byte[] spawnsetFileContents = File.ReadAllBytes(Path.Combine(spawnsetsPath, "V3"));
		if (SpawnsetBinary.TryParse(spawnsetFileContents, out SpawnsetBinary? spawnsetBinary))
			_v3Hash = GetSpawnsetHash(spawnsetBinary);
		else
			Assert.Fail("Spawnset could not be parsed.");

		_fakeReplay = new byte[spawnsetFileContents.Length + 88];
		Buffer.BlockCopy(spawnsetFileContents, 0, _fakeReplay, 88, spawnsetFileContents.Length);
	}

	[TestMethod]
	public async Task PostUploadRequest_ExistingPlayer_ExistingEntry_NoHighscore()
	{
		AddUploadRequest uploadRequest = new()
		{
			Time = 100000,
			PlayerId = 1,
			ClientVersion = TestConstants.DdclVersion,
			SurvivalHashMd5 = _v3Hash,
			PlayerName = "TestPlayer1",
			Client = "DevilDaggersCustomLeaderboards",
			Status = 3,
			ReplayData = _fakeReplay,
			GameData = new(),
		};
		uploadRequest.Validation = GetValidation(uploadRequest);

		GetUploadSuccess? uploadSuccess = (await _customEntryProcessor.ProcessUploadRequestAsync(uploadRequest)).Value;

		_dbContext.Verify(db => db.SaveChangesAsync(default), Times.AtLeastOnce);
		Assert.IsNotNull(uploadSuccess);
		Assert.AreEqual(1, uploadSuccess.TotalPlayers);
		Assert.IsTrue(uploadSuccess.Message.StartsWith("No new highscore"));
	}

	[TestMethod]
	public async Task PostUploadRequest_ExistingPlayer_ExistingEntry_NewHighscore()
	{
		AddUploadRequest uploadRequest = new()
		{
			Time = 200000,
			PlayerId = 1,
			ClientVersion = TestConstants.DdclVersion,
			SurvivalHashMd5 = _v3Hash,
			PlayerName = "TestPlayer1",
			Client = "DevilDaggersCustomLeaderboards",
			Status = 4,
			ReplayData = _fakeReplay,
			GameData = new(),
		};
		uploadRequest.Validation = GetValidation(uploadRequest);

		GetUploadSuccess? uploadSuccess = (await _customEntryProcessor.ProcessUploadRequestAsync(uploadRequest)).Value;

		_dbContext.Verify(db => db.SaveChangesAsync(default), Times.AtLeastOnce);
		Assert.IsNotNull(uploadSuccess);
		Assert.AreEqual(1, uploadSuccess.TotalPlayers);
		Assert.IsTrue(uploadSuccess.Message.StartsWith("NEW HIGHSCORE"));
	}

	[TestMethod]
	public async Task PostUploadRequest_ExistingPlayer_NewEntry()
	{
		AddUploadRequest uploadRequest = new()
		{
			Time = 200000,
			PlayerId = 2,
			ClientVersion = TestConstants.DdclVersion,
			SurvivalHashMd5 = _v3Hash,
			PlayerName = "TestPlayer2",
			Client = "DevilDaggersCustomLeaderboards",
			Status = 5,
			ReplayData = _fakeReplay,
			GameData = new(),
		};
		uploadRequest.Validation = GetValidation(uploadRequest);

		GetUploadSuccess? uploadSuccess = (await _customEntryProcessor.ProcessUploadRequestAsync(uploadRequest)).Value;

		_dbContext.Verify(db => db.CustomEntries.AddAsync(It.Is<CustomEntryEntity>(ce => ce.PlayerId == 2 && ce.Time == 200000), default), Times.Once);
		_dbContext.Verify(db => db.SaveChangesAsync(default), Times.AtLeastOnce);
		Assert.IsNotNull(uploadSuccess);
		Assert.IsTrue(uploadSuccess.Message.StartsWith("Welcome"));
	}

	[TestMethod]
	public async Task PostUploadRequest_NewPlayer()
	{
		AddUploadRequest uploadRequest = new()
		{
			Time = 300000,
			PlayerId = 3,
			ClientVersion = TestConstants.DdclVersion,
			SurvivalHashMd5 = _v3Hash,
			PlayerName = "TestPlayer3",
			Client = "DevilDaggersCustomLeaderboards",
			Status = 3,
			ReplayData = _fakeReplay,
			GameData = new(),
		};
		uploadRequest.Validation = GetValidation(uploadRequest);

		GetUploadSuccess? uploadSuccess = (await _customEntryProcessor.ProcessUploadRequestAsync(uploadRequest)).Value;

		_dbContext.Verify(db => db.SaveChangesAsync(default), Times.AtLeastOnce);
		_dbContext.Verify(db => db.Players.AddAsync(It.Is<PlayerEntity>(p => p.Id == 3 && p.PlayerName == "TestPlayer3"), default), Times.Once);
		_dbContext.Verify(db => db.CustomEntries.AddAsync(It.Is<CustomEntryEntity>(ce => ce.PlayerId == 3 && ce.Time == 300000), default), Times.Once);
		Assert.IsNotNull(uploadSuccess);
		Assert.IsTrue(uploadSuccess.Message.StartsWith("Welcome"));
	}

	[TestMethod]
	public async Task PostUploadRequest_Outdated()
	{
		AddUploadRequest uploadRequest = new()
		{
			Time = 100000,
			PlayerId = 1,
			ClientVersion = "0.0.0.0",
			SurvivalHashMd5 = _v3Hash,
			PlayerName = "TestPlayer1",
			Client = "DevilDaggersCustomLeaderboards",
			Status = 4,
			ReplayData = _fakeReplay,
		};
		uploadRequest.Validation = GetValidation(uploadRequest);

		CustomEntryValidationException ex = await Assert.ThrowsExceptionAsync<CustomEntryValidationException>(async () => await _customEntryProcessor.ProcessUploadRequestAsync(uploadRequest));

		_dbContext.Verify(db => db.SaveChangesAsync(default), Times.Never);

		Assert.IsTrue(ex.Message.Contains("unsupported and outdated"));
	}

	[TestMethod]
	public async Task PostUploadRequest_InvalidValidation()
	{
		AddUploadRequest uploadRequest = new()
		{
			Time = 100000,
			PlayerId = 1,
			ClientVersion = TestConstants.DdclVersion,
			SurvivalHashMd5 = _v3Hash,
			PlayerName = "TestPlayer1",
			Validation = "Malformed validation",
			Client = "DevilDaggersCustomLeaderboards",
			Status = 5,
			ReplayData = _fakeReplay,
		};

		CustomEntryValidationException ex = await Assert.ThrowsExceptionAsync<CustomEntryValidationException>(async () => await _customEntryProcessor.ProcessUploadRequestAsync(uploadRequest));

		_dbContext.Verify(db => db.SaveChangesAsync(default), Times.Never);

		Assert.IsTrue(ex.Message.StartsWith("Could not decrypt"));
	}

	private string GetValidation(AddUploadRequest uploadRequest)
	{
		string toEncrypt = string.Join(
			";",
			uploadRequest.PlayerId,
			uploadRequest.Time,
			uploadRequest.GemsCollected,
			uploadRequest.GemsDespawned,
			uploadRequest.GemsEaten,
			uploadRequest.GemsTotal,
			uploadRequest.EnemiesKilled,
			uploadRequest.DeathType,
			uploadRequest.DaggersHit,
			uploadRequest.DaggersFired,
			uploadRequest.EnemiesAlive,
			uploadRequest.HomingDaggers,
			uploadRequest.HomingDaggersEaten,
			uploadRequest.IsReplay ? 1 : 0,
			uploadRequest.SurvivalHashMd5.ByteArrayToHexString(),
			string.Join(",", new int[3] { uploadRequest.LevelUpTime2, uploadRequest.LevelUpTime3, uploadRequest.LevelUpTime4 }));
		return HttpUtility.HtmlEncode(_encryptionWrapper.EncryptAndEncode(toEncrypt));
	}

	private static byte[] GetSpawnsetHash(SpawnsetBinary spawnset)
		=> MD5.HashData(spawnset.ToBytes());
}
