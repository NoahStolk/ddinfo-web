using DevilDaggersInfo.Web.BlazorWasm.Server.Controllers.Admin;
using DevilDaggersInfo.Web.BlazorWasm.Server.Entities;
using DevilDaggersInfo.Web.BlazorWasm.Server.Extensions;
using DevilDaggersInfo.Web.BlazorWasm.Server.Singletons;
using DevilDaggersInfo.Web.BlazorWasm.Server.Singletons.AuditLog;
using DevilDaggersInfo.Web.BlazorWasm.Server.Transients;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.CustomLeaderboards;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Enums;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Test;

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
			.SetUpDbSet(db => db.Spawnsets, mockEntities.MockDbSetSpawnsets)
			.SetUpDbSet(db => db.CustomLeaderboards, mockEntities.MockDbSetCustomLeaderboards)
			.SetUpDbSet(db => db.CustomEntries, mockEntities.MockDbSetCustomEntries);

		Mock<IWebHostEnvironment> mockEnvironment = new();
		mockEnvironment.Setup(m => m.EnvironmentName).Returns(Environments.Development);

		Mock<DiscordLogger> discordLogger = new(mockEnvironment.Object);
		Mock<AuditLogger> auditLogger = new(discordLogger.Object);

		// TODO: Mock to get spawnset.
		Mock<IFileSystemService> fileSystemService = new();

		_customLeaderboardsController = new CustomLeaderboardsController(_dbContext.Object, fileSystemService.Object, auditLogger.Object);
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
