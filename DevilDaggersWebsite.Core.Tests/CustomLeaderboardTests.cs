using DevilDaggersWebsite.Core.Api;
using DevilDaggersWebsite.Core.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Core.Tests
{
	[TestClass]
	public class CustomLeaderboardTests
	{
		private static readonly ApplicationDbContext _context;
		private static readonly CustomLeaderboardsController _customLeaderboardsController;

#pragma warning disable S3963 // "static" fields should be initialized inline
#pragma warning disable CA1810 // Initialize reference type static fields inline
		static CustomLeaderboardTests()
#pragma warning restore CA1810 // Initialize reference type static fields inline
		{
			DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options;
			SetUpInMemoryDatabase(options);
			_context = new ApplicationDbContext(options);

			Mock<IWebHostEnvironment> mockEnvironment = new Mock<IWebHostEnvironment>();
			mockEnvironment.Setup(m => m.EnvironmentName).Returns("Hosting:UnitTestEnvironment");

			_customLeaderboardsController = new CustomLeaderboardsController(_context, mockEnvironment.Object);
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
			Dto.UploadRequest uploadRequest = new Dto.UploadRequest
			{
				Time = 100000,
				PlayerId = 0,
				DdclClientVersion = "0.10.0.0",
			};

			Dto.UploadSuccess uploadSuccess = (await _customLeaderboardsController.ProcessUploadRequest(uploadRequest)).Value;

			Assert.AreEqual(1, uploadSuccess.TotalPlayers);

			// TODO: Find a better way to test this.
			Assert.IsTrue(uploadSuccess.Message.Contains("No new highscore", StringComparison.InvariantCulture));
		}

		private static void SetUpInMemoryDatabase(DbContextOptions<ApplicationDbContext> options)
		{
			using ApplicationDbContext context = new ApplicationDbContext(options);
			Player player = new Player
			{
				Id = 0,
				Username = "Sorath",
			};
			SpawnsetFile spawnsetFile = new SpawnsetFile
			{
				Id = 1,
				LastUpdated = DateTime.Now,
				Name = "V3",
				PlayerId = 0,
				Player = player,
			};
			CustomLeaderboardCategory customLeaderboardCategory = new CustomLeaderboardCategory
			{
				Id = 1,
				Ascending = true,
				Name = "Default",
			};
			CustomLeaderboard customLeaderboard = new CustomLeaderboard
			{
				Id = 1,
				Bronze = 60,
				Silver = 120,
				Golden = 250,
				Devil = 500,
				Homing = 1000,
				CategoryId = 1,
				DateCreated = DateTime.Now,
				DateLastPlayed = DateTime.Now,
				SpawnsetFileId = 1,
				TotalRunsSubmitted = 666,
				Category = customLeaderboardCategory,
				SpawnsetFile = spawnsetFile,
			};
			CustomEntry customEntry = new CustomEntry
			{
				Id = 1,
				ClientVersion = "Test",
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
				Player = player,
			};

			context.Players.Add(player);
			context.SpawnsetFiles.Add(spawnsetFile);
			context.CustomLeaderboardCategories.Add(customLeaderboardCategory);
			context.CustomLeaderboards.Add(customLeaderboard);
			context.CustomEntries.Add(customEntry);
			context.SaveChanges();
		}
	}
}