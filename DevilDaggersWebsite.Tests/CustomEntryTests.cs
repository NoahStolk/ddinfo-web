using DevilDaggersCore.Spawnsets;
using DevilDaggersWebsite.Api;
using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Extensions;
using DevilDaggersWebsite.Tests.Data;
using DevilDaggersWebsite.Tests.Extensions;
using DevilDaggersWebsite.Transients;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
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
			const string wwwroot = @"C:\Users\NOAH\source\repos\DevilDaggersWebsite\DevilDaggersWebsite.Razor\wwwroot";

			MockEntities mockEntities = new();

			_dbContext = new Mock<ApplicationDbContext>()
				.SetUpDbSet(db => db.Players, mockEntities.MockDbSetPlayers)
				.SetUpDbSet(db => db.SpawnsetFiles, mockEntities.MockDbSetSpawnsetFiles)
				.SetUpDbSet(db => db.CustomLeaderboards, mockEntities.MockDbSetCustomLeaderboards)
				.SetUpDbSet(db => db.CustomEntries, mockEntities.MockDbSetCustomEntries)
				.SetUpDbSet(db => db.CustomEntryData, mockEntities.MockDbSetCustomEntryData);

			Mock<IWebHostEnvironment> mockEnvironment = new();
			mockEnvironment.Setup(m => m.EnvironmentName).Returns(Environments.Development);
			mockEnvironment.Setup(m => m.WebRootPath).Returns(wwwroot);

			Mock<IToolHelper> toolHelper = new();
			toolHelper.Setup(m => m.GetToolByName(It.IsAny<string>())).Returns(new Dto.Tool
			{
				Name = "DevilDaggersCustomLeaderboards",
				VersionNumber = new(1, 0, 0, 0),
				VersionNumberRequired = new(1, 0, 0, 0),
			});

			_customEntriesController = new CustomEntriesController(_dbContext.Object, mockEnvironment.Object, toolHelper.Object);

			if (!Spawnset.TryParse(File.ReadAllBytes(Path.Combine(wwwroot, "spawnsets", "V3")), out _spawnset))
				Assert.Fail("Spawnset could not be parsed.");
		}

		[TestMethod]
		public async Task PostUploadRequest_ExistingPlayer_ExistingEntry_NoHighscore()
		{
			Dto.UploadRequest uploadRequest = new()
			{
				Time = 100000,
				PlayerId = 1,
				ClientVersion = Constants.DdclVersion,
				SurvivalHashMd5 = GetSpawnsetHash(_spawnset),
				GameStates = new(),
				PlayerName = "TestPlayer1",
			};
			uploadRequest.Validation = GetValidation(uploadRequest);

			Dto.UploadSuccess uploadSuccess = (await _customEntriesController.ProcessUploadRequest(uploadRequest)).Value;

			_dbContext.Verify(db => db.SaveChanges(), Times.AtLeastOnce);
			Assert.AreEqual(1, uploadSuccess.TotalPlayers);
			Assert.IsTrue(uploadSuccess.Message.StartsWith("No new highscore"));
		}

		[TestMethod]
		public async Task PostUploadRequest_ExistingPlayer_ExistingEntry_NewHighscore()
		{
			Dto.UploadRequest uploadRequest = new()
			{
				Time = 200000,
				PlayerId = 1,
				ClientVersion = Constants.DdclVersion,
				SurvivalHashMd5 = GetSpawnsetHash(_spawnset),
				GameStates = new(),
				PlayerName = "TestPlayer1",
			};
			uploadRequest.Validation = GetValidation(uploadRequest);

			Dto.UploadSuccess uploadSuccess = (await _customEntriesController.ProcessUploadRequest(uploadRequest)).Value;

			_dbContext.Verify(db => db.SaveChanges(), Times.AtLeastOnce);
			Assert.AreEqual(1, uploadSuccess.TotalPlayers);
			Assert.IsTrue(uploadSuccess.Message.StartsWith("NEW HIGHSCORE"));
		}

		[TestMethod]
		public async Task PostUploadRequest_ExistingPlayer_NewEntry()
		{
			Dto.UploadRequest uploadRequest = new()
			{
				Time = 200000,
				PlayerId = 2,
				ClientVersion = Constants.DdclVersion,
				SurvivalHashMd5 = GetSpawnsetHash(_spawnset),
				GameStates = new(),
				PlayerName = "TestPlayer2",
			};
			uploadRequest.Validation = GetValidation(uploadRequest);

			Dto.UploadSuccess uploadSuccess = (await _customEntriesController.ProcessUploadRequest(uploadRequest)).Value;

			_dbContext.Verify(db => db.CustomEntries.Add(It.Is<CustomEntry>(ce => ce.PlayerId == 2 && ce.Time == 200000)), Times.Once);
			_dbContext.Verify(db => db.SaveChanges(), Times.AtLeastOnce);
			Assert.IsTrue(uploadSuccess.Message.StartsWith("Welcome"));
		}

		[TestMethod]
		public async Task PostUploadRequest_NewPlayer()
		{
			Dto.UploadRequest uploadRequest = new()
			{
				Time = 300000,
				PlayerId = 3,
				ClientVersion = Constants.DdclVersion,
				SurvivalHashMd5 = GetSpawnsetHash(_spawnset),
				GameStates = new(),
				PlayerName = "TestPlayer3",
			};
			uploadRequest.Validation = GetValidation(uploadRequest);

			Dto.UploadSuccess uploadSuccess = (await _customEntriesController.ProcessUploadRequest(uploadRequest)).Value;

			_dbContext.Verify(db => db.SaveChanges(), Times.AtLeastOnce);
			_dbContext.Verify(db => db.Players.Add(It.Is<Player>(p => p.Id == 3 && p.PlayerName == "TestPlayer3")), Times.Once);
			_dbContext.Verify(db => db.CustomEntries.Add(It.Is<CustomEntry>(ce => ce.PlayerId == 3 && ce.Time == 300000)), Times.Once);
			Assert.IsTrue(uploadSuccess.Message.StartsWith("Welcome"));
		}

		[TestMethod]
		public async Task PostUploadRequest_Outdated()
		{
			Dto.UploadRequest uploadRequest = new()
			{
				Time = 100000,
				PlayerId = 1,
				ClientVersion = "0.0.0.0",
				SurvivalHashMd5 = GetSpawnsetHash(_spawnset),
				GameStates = new(),
				PlayerName = "TestPlayer1",
			};
			uploadRequest.Validation = GetValidation(uploadRequest);

			ActionResult<Dto.UploadSuccess> response = await _customEntriesController.ProcessUploadRequest(uploadRequest);

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
			Dto.UploadRequest uploadRequest = new()
			{
				Time = 100000,
				PlayerId = 1,
				ClientVersion = Constants.DdclVersion,
				SurvivalHashMd5 = GetSpawnsetHash(_spawnset),
				GameStates = new(),
				PlayerName = "TestPlayer1",
				Validation = "Malformed validation",
			};

			ActionResult<Dto.UploadSuccess> response = await _customEntriesController.ProcessUploadRequest(uploadRequest);

			_dbContext.Verify(db => db.SaveChanges(), Times.Never);

			Assert.IsInstanceOfType(response.Result, typeof(BadRequestObjectResult));
			BadRequestObjectResult badRequest = (BadRequestObjectResult)response.Result;
			Assert.AreEqual(StatusCodes.Status400BadRequest, badRequest.StatusCode);

			Assert.IsInstanceOfType(badRequest.Value, typeof(ProblemDetails));
			ProblemDetails problemDetails = (ProblemDetails)badRequest.Value;
			Assert.IsTrue(problemDetails.Title == "Invalid submission.");
		}

		private static string GetValidation(Dto.UploadRequest uploadRequest)
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
