using DevilDaggersWebsite.BlazorWasm.Server.Controllers.Admin;
using DevilDaggersWebsite.BlazorWasm.Server.Entities;
using DevilDaggersWebsite.BlazorWasm.Server.Extensions;
using DevilDaggersWebsite.BlazorWasm.Server.Singletons;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto.Admin.CustomLeaderboards;
using DevilDaggersWebsite.BlazorWasm.Shared.Enums;
using DevilDaggersWebsite.Tests.Data;
using DevilDaggersWebsite.Tests.Extensions;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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

			DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder = new();
			_dbContext = new Mock<ApplicationDbContext>(optionsBuilder.Options, Options.Create(new OperationalStoreOptions()))
				.SetUpDbSet(db => db.Players, mockEntities.MockDbSetPlayers)
				.SetUpDbSet(db => db.SpawnsetFiles, mockEntities.MockDbSetSpawnsetFiles)
				.SetUpDbSet(db => db.CustomLeaderboards, mockEntities.MockDbSetCustomLeaderboards)
				.SetUpDbSet(db => db.CustomEntries, mockEntities.MockDbSetCustomEntries);

			Mock<IWebHostEnvironment> mockEnvironment = new();
			mockEnvironment.Setup(m => m.EnvironmentName).Returns(Environments.Development);

			Mock<DiscordLogger> discordLogger = new(mockEnvironment.Object);
			Mock<AuditLogger> auditLogger = new(discordLogger.Object);

			_customLeaderboardsController = new CustomLeaderboardsController(_dbContext.Object, auditLogger.Object);
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
			Page<GetCustomLeaderboard> customLeaderboards = _customLeaderboardsController.GetCustomLeaderboards().Value;

			_dbContext.Verify(db => db.SaveChanges(), Times.Never);
			Assert.AreEqual(1, customLeaderboards.Results.Count);
			Assert.IsTrue(customLeaderboards.Results.Any(cl => cl.TimeBronze == 60));
		}
	}
}
