using DevilDaggersInfo.Web.BlazorWasm.Server;
using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.SpawnsetHashes;
using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.SpawnsetSummaries;
using DevilDaggersInfo.Web.BlazorWasm.Server.Controllers.Public;
using DevilDaggersInfo.Web.BlazorWasm.Server.Enums;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomEntries;
using Microsoft.Extensions.Logging;

namespace DevilDaggersInfo.Test.Web.BlazorWasm.Server;

[TestClass]
public class CustomEntryTests
{
#pragma warning disable S3459 // Unassigned members should be removed
	private readonly SpawnsetBinary _spawnsetBinary;
#pragma warning restore S3459 // Unassigned members should be removed

	private readonly Mock<ApplicationDbContext> _dbContext;
	private readonly CustomEntriesController _customEntriesController;

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

		Mock<IFileSystemService> fileSystemService = new();
		fileSystemService.Setup(m => m.GetPath(DataSubDirectory.Spawnsets)).Returns(@"C:\Users\NOAH\source\repos\DevilDaggersInfo\DevilDaggersInfo.Web.BlazorWasm.Server\Data\Spawnsets");

		Mock<ILogger<SpawnsetHashCache>> spawnsetHashCacheLogger = new();
		Mock<SpawnsetHashCache> spawnsetHashCache = new(fileSystemService.Object, spawnsetHashCacheLogger.Object);
		Mock<SpawnsetSummaryCache> spawnsetSummaryCache = new();

		Mock<ILogger<CustomEntriesController>> controllerLogger = new();
		_customEntriesController = new CustomEntriesController(_dbContext.Object, controllerLogger.Object, spawnsetHashCache.Object, spawnsetSummaryCache.Object, fileSystemService.Object);

		if (!SpawnsetBinary.TryParse(File.ReadAllBytes(Path.Combine(TestConstants.DataDirectory, "Spawnsets", "V3")), out _spawnsetBinary!))
			Assert.Fail("Spawnset could not be parsed.");
	}

	[TestMethod]
	public async Task PostUploadRequest_ExistingPlayer_ExistingEntry_NoHighscore()
	{
		AddUploadRequest uploadRequest = new()
		{
			Time = 100000,
			PlayerId = 1,
			ClientVersion = TestConstants.DdclVersion,
			SurvivalHashMd5 = GetSpawnsetHash(_spawnsetBinary),
			GameStates = new(),
			PlayerName = "TestPlayer1",
		};
		uploadRequest.Validation = GetValidation(uploadRequest);

		GetUploadSuccess? uploadSuccess = (await _customEntriesController.ProcessUploadRequest(uploadRequest)).Value;

		_dbContext.Verify(db => db.SaveChanges(), Times.AtLeastOnce);
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
			SurvivalHashMd5 = GetSpawnsetHash(_spawnsetBinary),
			GameStates = new(),
			PlayerName = "TestPlayer1",
		};
		uploadRequest.Validation = GetValidation(uploadRequest);

		GetUploadSuccess? uploadSuccess = (await _customEntriesController.ProcessUploadRequest(uploadRequest)).Value;

		_dbContext.Verify(db => db.SaveChanges(), Times.AtLeastOnce);
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
			SurvivalHashMd5 = GetSpawnsetHash(_spawnsetBinary),
			GameStates = new(),
			PlayerName = "TestPlayer2",
		};
		uploadRequest.Validation = GetValidation(uploadRequest);

		GetUploadSuccess? uploadSuccess = (await _customEntriesController.ProcessUploadRequest(uploadRequest)).Value;

		_dbContext.Verify(db => db.CustomEntries.Add(It.Is<CustomEntryEntity>(ce => ce.PlayerId == 2 && ce.Time == 200000)), Times.Once);
		_dbContext.Verify(db => db.SaveChanges(), Times.AtLeastOnce);
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
			SurvivalHashMd5 = GetSpawnsetHash(_spawnsetBinary),
			GameStates = new(),
			PlayerName = "TestPlayer3",
		};
		uploadRequest.Validation = GetValidation(uploadRequest);

		GetUploadSuccess? uploadSuccess = (await _customEntriesController.ProcessUploadRequest(uploadRequest)).Value;

		_dbContext.Verify(db => db.SaveChanges(), Times.AtLeastOnce);
		_dbContext.Verify(db => db.Players.Add(It.Is<PlayerEntity>(p => p.Id == 3 && p.PlayerName == "TestPlayer3")), Times.Once);
		_dbContext.Verify(db => db.CustomEntries.Add(It.Is<CustomEntryEntity>(ce => ce.PlayerId == 3 && ce.Time == 300000)), Times.Once);
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
			SurvivalHashMd5 = GetSpawnsetHash(_spawnsetBinary),
			GameStates = new(),
			PlayerName = "TestPlayer1",
		};
		uploadRequest.Validation = GetValidation(uploadRequest);

		ActionResult<GetUploadSuccess> response = await _customEntriesController.ProcessUploadRequest(uploadRequest);

		_dbContext.Verify(db => db.SaveChanges(), Times.Never);

		BadRequestObjectResult? badRequest = response.Result as BadRequestObjectResult;
		Assert.IsNotNull(badRequest);
		Assert.AreEqual(StatusCodes.Status400BadRequest, badRequest.StatusCode);

		ProblemDetails? problemDetails = badRequest.Value as ProblemDetails;
		Assert.IsNotNull(problemDetails);
		Assert.IsNotNull(problemDetails.Title);
		Assert.IsTrue(problemDetails.Title.Contains("unsupported and outdated"));
	}

	[TestMethod]
	public async Task PostUploadRequest_InvalidValidation()
	{
		AddUploadRequest uploadRequest = new()
		{
			Time = 100000,
			PlayerId = 1,
			ClientVersion = TestConstants.DdclVersion,
			SurvivalHashMd5 = GetSpawnsetHash(_spawnsetBinary),
			GameStates = new(),
			PlayerName = "TestPlayer1",
			Validation = "Malformed validation",
		};

		ActionResult<GetUploadSuccess> response = await _customEntriesController.ProcessUploadRequest(uploadRequest);

		_dbContext.Verify(db => db.SaveChanges(), Times.Never);

		BadRequestObjectResult? badRequest = response.Result as BadRequestObjectResult;
		Assert.IsNotNull(badRequest);
		Assert.AreEqual(StatusCodes.Status400BadRequest, badRequest.StatusCode);

		ProblemDetails? problemDetails = badRequest.Value as ProblemDetails;
		Assert.IsNotNull(problemDetails);
		Assert.IsNotNull(problemDetails.Title);
		Assert.IsTrue(problemDetails.Title.StartsWith("Invalid submission"));
	}

	private static string GetValidation(AddUploadRequest uploadRequest)
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
		return HttpUtility.HtmlEncode(Secrets.EncryptionWrapper.EncryptAndEncode(toEncrypt));
	}

	private static byte[] GetSpawnsetHash(SpawnsetBinary spawnset)
		=> MD5.HashData(spawnset.ToBytes());
}
