using DevilDaggersWebsite.Core.Api;
using DevilDaggersWebsite.Core.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.Core.Tests
{
	[TestClass]
	public class CustomLeaderboardTests
	{
		[TestMethod]
		public void GetCustomLeaderboards()
		{
			DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options;

			using (ApplicationDbContext context = new ApplicationDbContext(options))
			{
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

			using (ApplicationDbContext context = new ApplicationDbContext(options))
			{
				Mock<IWebHostEnvironment> mockEnvironment = new Mock<IWebHostEnvironment>();
				mockEnvironment.Setup(m => m.EnvironmentName).Returns("Hosting:UnitTestEnvironment");

				CustomLeaderboardsController clc = new CustomLeaderboardsController(context, mockEnvironment.Object);

				List<Dto.CustomLeaderboard> customLeaderboards = clc.GetCustomLeaderboards().Value;
				Assert.AreEqual(1, customLeaderboards.Count);

				Assert.IsTrue(customLeaderboards.Any(cl => cl.Bronze == 60));
			}
		}
	}
}