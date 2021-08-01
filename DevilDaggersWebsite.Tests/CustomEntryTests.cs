using DevilDaggersCore.Spawnsets;
using DevilDaggersWebsite.BlazorWasm.Server;
using DevilDaggersWebsite.BlazorWasm.Server.Caches.SpawnsetHash;
using DevilDaggersWebsite.BlazorWasm.Server.Controllers.Public;
using DevilDaggersWebsite.BlazorWasm.Server.Entities;
using DevilDaggersWebsite.BlazorWasm.Server.Extensions;
using DevilDaggersWebsite.BlazorWasm.Server.Singletons;
using DevilDaggersWebsite.BlazorWasm.Server.Transients;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto.Public.CustomEntries;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto.Public.Tools;
using DevilDaggersWebsite.Tests.Data;
using DevilDaggersWebsite.Tests.Extensions;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;

namespace DevilDaggersWebsite.Tests
{
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
				.SetUpDbSet(db => db.SpawnsetFiles, mockEntities.MockDbSetSpawnsetFiles)
				.SetUpDbSet(db => db.CustomLeaderboards, mockEntities.MockDbSetCustomLeaderboards)
				.SetUpDbSet(db => db.CustomEntries, mockEntities.MockDbSetCustomEntries)
				.SetUpDbSet(db => db.CustomEntryData, mockEntities.MockDbSetCustomEntryData);

			Mock<IWebHostEnvironment> mockEnvironment = new();
			mockEnvironment.Setup(m => m.EnvironmentName).Returns(Environments.Development);
			mockEnvironment.Setup(m => m.WebRootPath).Returns(TestConstants.WebRoot);

			Mock<IToolHelper> toolHelper = new();
			toolHelper.Setup(m => m.GetToolByName(It.IsAny<string>())).Returns(new GetTool
			{
				Name = "DevilDaggersCustomLeaderboards",
				VersionNumber = new(1, 0, 0, 0),
				VersionNumberRequired = new(1, 0, 0, 0),
			});

			Mock<DiscordLogger> discordLogger = new(mockEnvironment.Object);
			Mock<SpawnsetHashCache> spawnsetHashCache = new(discordLogger.Object, mockEnvironment.Object);

			_customEntriesController = new CustomEntriesController(_dbContext.Object, toolHelper.Object, discordLogger.Object, spawnsetHashCache.Object);

			if (!Spawnset.TryParse(File.ReadAllBytes(Path.Combine(TestConstants.WebRoot, "spawnsets", "V3")), out _spawnset))
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

			GetUploadSuccess uploadSuccess = (await _customEntriesController.ProcessUploadRequest(uploadRequest)).Value;

			_dbContext.Verify(db => db.SaveChanges(), Times.AtLeastOnce);
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

			GetUploadSuccess uploadSuccess = (await _customEntriesController.ProcessUploadRequest(uploadRequest)).Value;

			_dbContext.Verify(db => db.SaveChanges(), Times.AtLeastOnce);
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

			GetUploadSuccess uploadSuccess = (await _customEntriesController.ProcessUploadRequest(uploadRequest)).Value;

			_dbContext.Verify(db => db.CustomEntries.Add(It.Is<CustomEntry>(ce => ce.PlayerId == 2 && ce.Time == 200000)), Times.Once);
			_dbContext.Verify(db => db.SaveChanges(), Times.AtLeastOnce);
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

			GetUploadSuccess uploadSuccess = (await _customEntriesController.ProcessUploadRequest(uploadRequest)).Value;

			_dbContext.Verify(db => db.SaveChanges(), Times.AtLeastOnce);
			_dbContext.Verify(db => db.Players.Add(It.Is<Player>(p => p.Id == 3 && p.PlayerName == "TestPlayer3")), Times.Once);
			_dbContext.Verify(db => db.CustomEntries.Add(It.Is<CustomEntry>(ce => ce.PlayerId == 3 && ce.Time == 300000)), Times.Once);
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

			Assert.IsInstanceOfType(response.Result, typeof(BadRequestObjectResult));
			BadRequestObjectResult badRequest = (BadRequestObjectResult)response.Result;
			Assert.AreEqual(StatusCodes.Status400BadRequest, badRequest.StatusCode);

			Assert.IsInstanceOfType(badRequest.Value, typeof(ProblemDetails));
			ProblemDetails problemDetails = (ProblemDetails)badRequest.Value;
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

			Assert.IsInstanceOfType(response.Result, typeof(BadRequestObjectResult));
			BadRequestObjectResult badRequest = (BadRequestObjectResult)response.Result;
			Assert.AreEqual(StatusCodes.Status400BadRequest, badRequest.StatusCode);

			Assert.IsInstanceOfType(badRequest.Value, typeof(ProblemDetails));
			ProblemDetails problemDetails = (ProblemDetails)badRequest.Value;
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
		{
			if (!spawnset.TryGetBytes(out byte[] bytes))
				throw new("Could not get bytes from spawnset.");

			return MD5.HashData(bytes);
		}
	}
}
