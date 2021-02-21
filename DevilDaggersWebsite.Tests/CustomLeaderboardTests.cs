using DevilDaggersCore.Spawnsets;
using DevilDaggersWebsite.Api;
using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Enumerators;
using DevilDaggersWebsite.Transients;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;

namespace DevilDaggersWebsite.Tests
{
	[TestClass]
	public class CustomLeaderboardTests
	{
		private const string _ddclClientVersion = "0.14.1.0";

		private static readonly ApplicationDbContext _dbContext;
		private static readonly CustomLeaderboardsController _customLeaderboardsController;

#pragma warning disable S3963 // "static" fields should be initialized inline
		static CustomLeaderboardTests()
		{
			DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options;
			SetUpInMemoryDatabase(options);
			_dbContext = new ApplicationDbContext(options);

			Mock<IWebHostEnvironment> mockEnvironment = new Mock<IWebHostEnvironment>();
			mockEnvironment.Setup(m => m.EnvironmentName).Returns("Hosting:UnitTestEnvironment");

			Mock<ToolHelper> toolHelper = new Mock<ToolHelper>(mockEnvironment.Object);
			_customLeaderboardsController = new CustomLeaderboardsController(_dbContext, mockEnvironment.Object, toolHelper.Object);
		}
#pragma warning restore S3963 // "static" fields should be initialized inline

		[TestMethod]
		public void GetCustomLeaderboards()
		{
			List<Dto.CustomLeaderboard> customLeaderboards = _customLeaderboardsController.GetCustomLeaderboards().Value;

			Assert.AreEqual(1, customLeaderboards.Count);
			Assert.IsTrue(customLeaderboards.Any(cl => cl.TimeBronze == 60));
		}

		[TestMethod]
		public async Task PostUploadRequestExistingPlayerExistingEntryNoHighscore()
		{
			Spawnset emptySpawnset = new();

			Dto.UploadRequest uploadRequest = new()
			{
				Time = 100000,
				PlayerId = 1,
				ClientVersion = _ddclClientVersion,
				SurvivalHashMd5 = GetSpawnsetHash(emptySpawnset),
				GameStates = new(),
				PlayerName = "TestPlayer1",
			};
			uploadRequest.Validation = GetValidation(uploadRequest);

			Dto.UploadSuccess uploadSuccess = (await _customLeaderboardsController.ProcessUploadRequest(uploadRequest, new List<(string Name, Spawnset Spawnset)> { ("Empty", emptySpawnset) })).Value;

			Assert.AreEqual(1, uploadSuccess.TotalPlayers);
			Assert.IsTrue(uploadSuccess.Message.StartsWith("No new highscore", StringComparison.InvariantCulture));
		}

		[TestMethod]
		public async Task PostUploadRequestExistingPlayerExistingEntryNewHighscore()
		{
			Spawnset emptySpawnset = new();

			Dto.UploadRequest uploadRequest = new()
			{
				Time = 200000,
				PlayerId = 1,
				ClientVersion = _ddclClientVersion,
				SurvivalHashMd5 = GetSpawnsetHash(emptySpawnset),
				GameStates = new(),
				PlayerName = "TestPlayer1",
			};
			uploadRequest.Validation = GetValidation(uploadRequest);

			Dto.UploadSuccess uploadSuccess = (await _customLeaderboardsController.ProcessUploadRequest(uploadRequest, new List<(string Name, Spawnset Spawnset)> { ("Empty", emptySpawnset) })).Value;

			Assert.AreEqual(1, uploadSuccess.TotalPlayers);
			Assert.IsTrue(uploadSuccess.Message.StartsWith("NEW HIGHSCORE", StringComparison.InvariantCulture));
		}

		[TestMethod]
		public async Task PostUploadRequestExistingPlayerNewEntry()
		{
			Spawnset emptySpawnset = new();

			Dto.UploadRequest uploadRequest = new()
			{
				Time = 200000,
				PlayerId = 2,
				ClientVersion = _ddclClientVersion,
				SurvivalHashMd5 = GetSpawnsetHash(emptySpawnset),
				GameStates = new(),
				PlayerName = "TestPlayer2",
			};
			uploadRequest.Validation = GetValidation(uploadRequest);

			Dto.UploadSuccess uploadSuccess = (await _customLeaderboardsController.ProcessUploadRequest(uploadRequest, new List<(string Name, Spawnset Spawnset)> { ("Empty", emptySpawnset) })).Value;

			Assert.AreEqual(2, uploadSuccess.TotalPlayers);
			Assert.IsTrue(uploadSuccess.Message.StartsWith("Welcome", StringComparison.InvariantCulture));
		}

		[TestMethod]
		public async Task PostUploadRequestNewPlayer()
		{
			Spawnset emptySpawnset = new();

			Dto.UploadRequest uploadRequest = new()
			{
				Time = 300000,
				PlayerId = 3,
				ClientVersion = _ddclClientVersion,
				SurvivalHashMd5 = GetSpawnsetHash(emptySpawnset),
				GameStates = new(),
				PlayerName = "TestPlayer3",
			};
			uploadRequest.Validation = GetValidation(uploadRequest);

			Dto.UploadSuccess uploadSuccess = (await _customLeaderboardsController.ProcessUploadRequest(uploadRequest, new List<(string Name, Spawnset Spawnset)> { ("Empty", emptySpawnset) })).Value;

			// TODO: Clear the database before running this test.
			// Assert.AreEqual(2, uploadSuccess.TotalPlayers);
			Assert.IsTrue(uploadSuccess.Message.StartsWith("Welcome", StringComparison.InvariantCulture));
		}

		[TestMethod]
		public async Task PostUploadRequestOutdated()
		{
			Spawnset emptySpawnset = new();

			Dto.UploadRequest uploadRequest = new()
			{
				Time = 100000,
				PlayerId = 1,
				ClientVersion = "0.0.0.0",
				SurvivalHashMd5 = GetSpawnsetHash(emptySpawnset),
				GameStates = new(),
				PlayerName = "TestPlayer1",
			};
			uploadRequest.Validation = GetValidation(uploadRequest);

			ActionResult<Dto.UploadSuccess> response = await _customLeaderboardsController.ProcessUploadRequest(uploadRequest, new List<(string Name, Spawnset Spawnset)> { ("Empty", emptySpawnset) });

			Assert.IsInstanceOfType(response.Result, typeof(BadRequestObjectResult));
			BadRequestObjectResult badRequest = (BadRequestObjectResult)response.Result;
			Assert.AreEqual(StatusCodes.Status400BadRequest, badRequest.StatusCode);

			Assert.IsInstanceOfType(badRequest.Value, typeof(ProblemDetails));
			ProblemDetails problemDetails = (ProblemDetails)badRequest.Value;
			Assert.IsTrue(problemDetails.Title.Contains("unsupported and outdated", StringComparison.InvariantCulture));
		}

		[TestMethod]
		public async Task PostUploadRequestInvalidValidation()
		{
			Spawnset emptySpawnset = new();

			Dto.UploadRequest uploadRequest = new()
			{
				Time = 100000,
				PlayerId = 1,
				ClientVersion = _ddclClientVersion,
				SurvivalHashMd5 = GetSpawnsetHash(emptySpawnset),
				GameStates = new(),
				PlayerName = "TestPlayer1",
				Validation = "Malformed validation",
			};

			ActionResult<Dto.UploadSuccess> response = await _customLeaderboardsController.ProcessUploadRequest(uploadRequest, new List<(string Name, Spawnset Spawnset)> { ("Empty", emptySpawnset) });

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
				uploadRequest.EnemiesKilled,
				uploadRequest.DeathType,
				uploadRequest.DaggersHit,
				uploadRequest.DaggersFired,
				uploadRequest.EnemiesAlive,
				uploadRequest.HomingDaggers,
				string.Join(",", new[] { uploadRequest.LevelUpTime2, uploadRequest.LevelUpTime3, uploadRequest.LevelUpTime4 }));
			return HttpUtility.HtmlEncode(Secrets.EncryptionWrapper.EncryptAndEncode(toEncrypt));
		}

		private static void SetUpInMemoryDatabase(DbContextOptions<ApplicationDbContext> options)
		{
			using ApplicationDbContext dbContext = new(options);
			Player testPlayer1 = new()
			{
				Id = 1,
				PlayerName = "TestPlayer1",
			};
			Player testPlayer2 = new()
			{
				Id = 2,
				PlayerName = "TestPlayer2",
			};
			SpawnsetFile spawnsetFile = new()
			{
				Id = 1,
				LastUpdated = DateTime.Now,
				Name = "Empty",
				PlayerId = 0,
				Player = testPlayer1,
				HtmlDescription = string.Empty,
				MaxDisplayWaves = 5,
			};
			CustomLeaderboard customLeaderboard = new()
			{
				Id = 1,
				TimeBronze = 60,
				TimeSilver = 120,
				TimeGolden = 250,
				TimeDevil = 500,
				TimeLeviathan = 1000,
				Category = CustomLeaderboardCategory.Default,
				DateCreated = DateTime.Now,
				DateLastPlayed = DateTime.Now,
				SpawnsetFileId = 1,
				TotalRunsSubmitted = 666,
				SpawnsetFile = spawnsetFile,
			};
			CustomEntry customEntry = new()
			{
				Id = 1,
				ClientVersion = _ddclClientVersion,
				CustomLeaderboardId = 1,
				DaggersFired = 15,
				DaggersHit = 6,
				DeathType = 1,
				EnemiesAlive = 6,
				GemsCollected = 3,
				HomingDaggers = 0,
				EnemiesKilled = 2,
				PlayerId = 0,
				Time = 166666,
				CustomLeaderboard = customLeaderboard,
				Player = testPlayer1,
				LevelUpTime2 = 0,
				LevelUpTime3 = 0,
				LevelUpTime4 = 0,
				SubmitDate = DateTime.Now,
			};

			_dbContext.Players.Add(testPlayer1);
			_dbContext.Players.Add(testPlayer2);
			_dbContext.SpawnsetFiles.Add(spawnsetFile);
			_dbContext.CustomLeaderboards.Add(customLeaderboard);
			_dbContext.CustomEntries.Add(customEntry);
			_dbContext.SaveChanges();
		}

		private static byte[] GetSpawnsetHash(Spawnset spawnset)
		{
			if (!spawnset.TryGetBytes(out byte[] bytes))
				throw new("Could not get bytes from spawnset.");

			return MD5.HashData(bytes);
		}
	}
}
