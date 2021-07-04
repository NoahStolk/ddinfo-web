using DevilDaggersWebsite.Api;
using DevilDaggersWebsite.Dto.CustomLeaderboards;
using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Enumerators;
using DevilDaggersWebsite.Extensions;
using DevilDaggersWebsite.Tests.Data;
using DevilDaggersWebsite.Tests.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.Tests
{
	[TestClass]
	public class CustomLeaderboardTests
	{
		private readonly Mock<ApplicationDbContext> _dbContext;
		private readonly CustomLeaderboardsController _customLeaderboardsController;

		public CustomLeaderboardTests()
		{
			MockEntities mockEntities = new();

			_dbContext = new Mock<ApplicationDbContext>()
				.SetUpDbSet(db => db.Players, mockEntities.MockDbSetPlayers)
				.SetUpDbSet(db => db.SpawnsetFiles, mockEntities.MockDbSetSpawnsetFiles)
				.SetUpDbSet(db => db.CustomLeaderboards, mockEntities.MockDbSetCustomLeaderboards)
				.SetUpDbSet(db => db.CustomEntries, mockEntities.MockDbSetCustomEntries);

			_customLeaderboardsController = new CustomLeaderboardsController(_dbContext.Object);
		}

		[TestMethod]
		public void TestIsAscending()
		{
			Assert.IsFalse(CustomLeaderboardCategory.Default.IsAscending());
			Assert.IsTrue(CustomLeaderboardCategory.TimeAttack.IsAscending());
			Assert.IsTrue(CustomLeaderboardCategory.Speedrun.IsAscending());
		}

		[TestMethod]
		public void GetCustomLeaderboards()
		{
			List<GetCustomLeaderboard> customLeaderboards = _customLeaderboardsController.GetCustomLeaderboards().Value;

			_dbContext.Verify(db => db.SaveChanges(), Times.Never);
			Assert.AreEqual(1, customLeaderboards.Count);
			Assert.IsTrue(customLeaderboards.Any(cl => cl.TimeBronze == 600000));
		}
	}
}
