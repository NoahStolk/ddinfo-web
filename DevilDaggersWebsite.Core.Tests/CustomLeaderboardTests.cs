using DevilDaggersCore.Spawnsets;
using DevilDaggersWebsite.Core.Api;
using DevilDaggersWebsite.Core.Entities;
using DevilDaggersWebsite.Core.Enumerators;
using DevilDaggersWebsite.Core.Transients;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DevilDaggersWebsite.Core.Tests
{
	[TestClass]
	public class CustomLeaderboardTests
	{
		private const string _ddclClientVersion = "0.10.4.0";

		private static readonly ApplicationDbContext _context;
		private static readonly CustomLeaderboardsController _customLeaderboardsController;

#pragma warning disable S3963 // "static" fields should be initialized inline
		static CustomLeaderboardTests()
		{
			DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options;
			SetUpInMemoryDatabase(options);
			_context = new ApplicationDbContext(options);

			Mock<IWebHostEnvironment> mockEnvironment = new Mock<IWebHostEnvironment>();
			mockEnvironment.Setup(m => m.EnvironmentName).Returns("Hosting:UnitTestEnvironment");

			Mock<ToolHelper> mockToolHelper = new Mock<ToolHelper>(mockEnvironment.Object);
			_customLeaderboardsController = new CustomLeaderboardsController(_context, mockEnvironment.Object, mockToolHelper.Object);
		}
#pragma warning restore S3963 // "static" fields should be initialized inline

		[TestMethod]
		public void GetCustomLeaderboards()
		{
			List<Dto.CustomLeaderboard> customLeaderboards = _customLeaderboardsController.GetCustomLeaderboards().Value;

			Assert.AreEqual(1, customLeaderboards.Count);
			Assert.IsTrue(customLeaderboards.Any(cl => cl.Bronze == 60));
		}

		[TestMethod]
		public async Task PostUploadRequestExistingPlayerExistingEntryNoHighscore()
		{
			Spawnset emptySpawnset = new Spawnset();

			Dto.UploadRequest uploadRequest = new Dto.UploadRequest
			{
				Time = 100000,
				PlayerId = 1,
				ClientVersion = _ddclClientVersion,
				SpawnsetHash = emptySpawnset.GetHashString(),
				GameStates = new List<Dto.GameState>(),
				Username = "TestPlayer1",
			};
			uploadRequest.Validation = GetValidation(uploadRequest);

			Dto.UploadSuccess uploadSuccess = (await _customLeaderboardsController.ProcessUploadRequest(uploadRequest, new List<(string name, Spawnset spawnset)> { ("Empty", emptySpawnset) })).Value;

			Assert.AreEqual(1, uploadSuccess.TotalPlayers);
			Assert.IsTrue(uploadSuccess.Message.StartsWith("No new highscore", StringComparison.InvariantCulture));
		}

		[TestMethod]
		public async Task PostUploadRequestExistingPlayerExistingEntryNewHighscore()
		{
			Spawnset emptySpawnset = new Spawnset();

			Dto.UploadRequest uploadRequest = new Dto.UploadRequest
			{
				Time = 200000,
				PlayerId = 1,
				ClientVersion = _ddclClientVersion,
				SpawnsetHash = emptySpawnset.GetHashString(),
				GameStates = new List<Dto.GameState>(),
				Username = "TestPlayer1",
			};
			uploadRequest.Validation = GetValidation(uploadRequest);

			Dto.UploadSuccess uploadSuccess = (await _customLeaderboardsController.ProcessUploadRequest(uploadRequest, new List<(string name, Spawnset spawnset)> { ("Empty", emptySpawnset) })).Value;

			Assert.AreEqual(1, uploadSuccess.TotalPlayers);
			Assert.IsTrue(uploadSuccess.Message.StartsWith("NEW HIGHSCORE", StringComparison.InvariantCulture));
		}

		[TestMethod]
		public async Task PostUploadRequestExistingPlayerNewEntry()
		{
			Spawnset emptySpawnset = new Spawnset();

			Dto.UploadRequest uploadRequest = new Dto.UploadRequest
			{
				Time = 200000,
				PlayerId = 2,
				ClientVersion = _ddclClientVersion,
				SpawnsetHash = emptySpawnset.GetHashString(),
				GameStates = new List<Dto.GameState>(),
				Username = "TestPlayer2",
			};
			uploadRequest.Validation = GetValidation(uploadRequest);

			Dto.UploadSuccess uploadSuccess = (await _customLeaderboardsController.ProcessUploadRequest(uploadRequest, new List<(string name, Spawnset spawnset)> { ("Empty", emptySpawnset) })).Value;

			Assert.AreEqual(2, uploadSuccess.TotalPlayers);
			Assert.IsTrue(uploadSuccess.Message.StartsWith("Welcome", StringComparison.InvariantCulture));
		}

		[TestMethod]
		public async Task PostUploadRequestNewPlayer()
		{
			Spawnset emptySpawnset = new Spawnset();

			Dto.UploadRequest uploadRequest = new Dto.UploadRequest
			{
				Time = 300000,
				PlayerId = 3,
				ClientVersion = _ddclClientVersion,
				SpawnsetHash = emptySpawnset.GetHashString(),
				GameStates = new List<Dto.GameState>(),
				Username = "TestPlayer3",
			};
			uploadRequest.Validation = GetValidation(uploadRequest);

			Dto.UploadSuccess uploadSuccess = (await _customLeaderboardsController.ProcessUploadRequest(uploadRequest, new List<(string name, Spawnset spawnset)> { ("Empty", emptySpawnset) })).Value;

			// TODO: Clear the database before running this test.
			// Assert.AreEqual(2, uploadSuccess.TotalPlayers);
			Assert.IsTrue(uploadSuccess.Message.StartsWith("Welcome", StringComparison.InvariantCulture));
		}

		[TestMethod]
		public async Task PostUploadRequestOutdated()
		{
			Spawnset emptySpawnset = new Spawnset();

			Dto.UploadRequest uploadRequest = new Dto.UploadRequest
			{
				Time = 100000,
				PlayerId = 1,
				ClientVersion = "0.0.0.0",
				SpawnsetHash = emptySpawnset.GetHashString(),
				GameStates = new List<Dto.GameState>(),
				Username = "TestPlayer1",
			};
			uploadRequest.Validation = GetValidation(uploadRequest);

			ActionResult<Dto.UploadSuccess> response = await _customLeaderboardsController.ProcessUploadRequest(uploadRequest, new List<(string name, Spawnset spawnset)> { ("Empty", emptySpawnset) });

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
			Spawnset emptySpawnset = new Spawnset();

			Dto.UploadRequest uploadRequest = new Dto.UploadRequest
			{
				Time = 100000,
				PlayerId = 1,
				ClientVersion = _ddclClientVersion,
				SpawnsetHash = emptySpawnset.GetHashString(),
				GameStates = new List<Dto.GameState>(),
				Username = "TestPlayer1",
				Validation = "Malformed validation",
			};

			ActionResult<Dto.UploadSuccess> response = await _customLeaderboardsController.ProcessUploadRequest(uploadRequest, new List<(string name, Spawnset spawnset)> { ("Empty", emptySpawnset) });

			Assert.IsInstanceOfType(response.Result, typeof(BadRequestObjectResult));
			BadRequestObjectResult badRequest = (BadRequestObjectResult)response.Result;
			Assert.AreEqual(StatusCodes.Status400BadRequest, badRequest.StatusCode);

			Assert.IsInstanceOfType(badRequest.Value, typeof(ProblemDetails));
			ProblemDetails problemDetails = (ProblemDetails)badRequest.Value;
			Assert.IsTrue(problemDetails.Title == "Invalid submission.");
		}

		private static string GetValidation(Dto.UploadRequest uploadRequest)
		{
			string toEncrypt = string.Join(";", uploadRequest.PlayerId, uploadRequest.Time, uploadRequest.Gems, uploadRequest.Kills, uploadRequest.DeathType, uploadRequest.DaggersHit, uploadRequest.DaggersFired, uploadRequest.EnemiesAlive, uploadRequest.Homing, string.Join(",", new[] { uploadRequest.LevelUpTime2, uploadRequest.LevelUpTime3, uploadRequest.LevelUpTime4 }));
			return HttpUtility.HtmlEncode(Secrets.EncryptionWrapper.EncryptAndEncode(toEncrypt));
		}

		private static void SetUpInMemoryDatabase(DbContextOptions<ApplicationDbContext> options)
		{
			using ApplicationDbContext context = new ApplicationDbContext(options);
			Player testPlayer1 = new Player
			{
				Id = 1,
				Username = "TestPlayer1",
			};
			Player testPlayer2 = new Player
			{
				Id = 2,
				Username = "TestPlayer2",
			};
			SpawnsetFile spawnsetFile = new SpawnsetFile
			{
				Id = 1,
				LastUpdated = DateTime.Now,
				Name = "Empty",
				PlayerId = 0,
				Player = testPlayer1,
				HtmlDescription = string.Empty,
				MaxDisplayWaves = 5,
			};
			CustomLeaderboard customLeaderboard = new CustomLeaderboard
			{
				Id = 1,
				Bronze = 60,
				Silver = 120,
				Golden = 250,
				Devil = 500,
				Homing = 1000,
				Category = CustomLeaderboardCategory.Default,
				DateCreated = DateTime.Now,
				DateLastPlayed = DateTime.Now,
				SpawnsetFileId = 1,
				TotalRunsSubmitted = 666,
				SpawnsetFile = spawnsetFile,
			};
			CustomEntry customEntry = new CustomEntry
			{
				Id = 1,
				ClientVersion = _ddclClientVersion,
				CustomLeaderboardId = 1,
				DaggersFired = 15,
				DaggersHit = 6,
				DeathType = 1,
				EnemiesAlive = 6,
				Gems = 3,
				Homing = 0,
				Kills = 2,
				PlayerId = 0,
				Time = 166666,
				CustomLeaderboard = customLeaderboard,
				Player = testPlayer1,
				DaggersFiredData = "0",
				DaggersHitData = "0",
				EnemiesAliveData = "0",
				GemsData = "0",
				KillsData = "0",
				HomingData = "0",
				LevelUpTime2 = 0,
				LevelUpTime3 = 0,
				LevelUpTime4 = 0,
				SubmitDate = DateTime.Now,
			};

			context.Players.Add(testPlayer1);
			context.Players.Add(testPlayer2);
			context.SpawnsetFiles.Add(spawnsetFile);
			context.CustomLeaderboards.Add(customLeaderboard);
			context.CustomEntries.Add(customEntry);
			context.SaveChanges();
		}
	}
}