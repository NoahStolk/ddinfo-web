using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.SpawnsetHashes;
using DevilDaggersInfo.Web.BlazorWasm.Server.Controllers.Public;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomEntries;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Tools;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Test;

[TestClass]
public class CustomEntryTests
{
	private readonly Spawnset _spawnset;
	private readonly Mock<ApplicationDbContext> _dbContext;
	private readonly CustomEntriesController _customEntriesController;

	public CustomEntryTests()
	{
		MockEntities mockEntities = new();

		DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder = new();
		_dbContext = new Mock<ApplicationDbContext>(optionsBuilder.Options, Options.Create(new OperationalStoreOptions()))
			.SetUpDbSet(db => db.Players, mockEntities.MockDbSetPlayers)
			.SetUpDbSet(db => db.Spawnsets, mockEntities.MockDbSetSpawnsets)
			.SetUpDbSet(db => db.CustomLeaderboards, mockEntities.MockDbSetCustomLeaderboards)
			.SetUpDbSet(db => db.CustomEntries, mockEntities.MockDbSetCustomEntries)
			.SetUpDbSet(db => db.CustomEntryData, mockEntities.MockDbSetCustomEntryData);

		Mock<IWebHostEnvironment> mockEnvironment = new();
		mockEnvironment.Setup(m => m.EnvironmentName).Returns(Environments.Development);

		Mock<IToolHelper> toolHelper = new();
		toolHelper.Setup(m => m.GetToolByName(It.IsAny<string>())).Returns(new GetTool
		{
			Name = "DevilDaggersCustomLeaderboards",
			VersionNumber = new(1, 0, 0, 0),
			VersionNumberRequired = new(1, 0, 0, 0),
		});

		Mock<IFileSystemService> fileSystemService = new();
		fileSystemService.Setup(m => m.GetPath(Enums.DataSubDirectory.Spawnsets)).Returns(@"C:\Users\NOAH\source\repos\DevilDaggersInfo\DevilDaggersInfo.Web.BlazorWasm.Server\Data\Spawnsets");

		Mock<DiscordLogger> discordLogger = new(mockEnvironment.Object);
		Mock<SpawnsetHashCache> spawnsetHashCache = new(fileSystemService.Object, discordLogger.Object);

		_customEntriesController = new CustomEntriesController(_dbContext.Object, toolHelper.Object, discordLogger.Object, spawnsetHashCache.Object);

		if (!Spawnset.TryParse(File.ReadAllBytes(Path.Combine(TestConstants.DataDirectory, "Spawnsets", "V3")), out _spawnset!))
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
			SurvivalHashMd5 = GetSpawnsetHash(_spawnset),
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
			SurvivalHashMd5 = GetSpawnsetHash(_spawnset),
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
			SurvivalHashMd5 = GetSpawnsetHash(_spawnset),
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
			SurvivalHashMd5 = GetSpawnsetHash(_spawnset),
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
			SurvivalHashMd5 = GetSpawnsetHash(_spawnset),
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
			SurvivalHashMd5 = GetSpawnsetHash(_spawnset),
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
		Assert.IsTrue(problemDetails.Title == "Invalid submission.");
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

	private static byte[] GetSpawnsetHash(Spawnset spawnset)
		=> MD5.HashData(spawnset.ToBytes());
}
