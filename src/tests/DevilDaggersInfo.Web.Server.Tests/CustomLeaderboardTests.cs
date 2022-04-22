using DevilDaggersInfo.Web.Server.Controllers.Admin;
using DevilDaggersInfo.Web.Server.Tests.Data;
using DevilDaggersInfo.Web.Server.Tests.Extensions;
using DevilDaggersInfo.Web.Shared.Dto;
using DevilDaggersInfo.Web.Shared.Dto.Admin.CustomLeaderboards;
using DevilDaggersInfo.Web.Shared.Enums;
using DevilDaggersInfo.Web.Shared.Extensions;

namespace DevilDaggersInfo.Web.Server.Tests;

[TestClass]
public class CustomLeaderboardTests
{
	private readonly Mock<ApplicationDbContext> _dbContext;
	private readonly CustomLeaderboardsController _customLeaderboardsController;

	public CustomLeaderboardTests()
	{
		MockEntities mockEntities = new();

		DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder = new();
		_dbContext = new Mock<ApplicationDbContext>(optionsBuilder.Options)
			.SetUpDbSet(db => db.Players, mockEntities.MockDbSetPlayers)
			.SetUpDbSet(db => db.Spawnsets, mockEntities.MockDbSetSpawnsets)
			.SetUpDbSet(db => db.CustomLeaderboards, mockEntities.MockDbSetCustomLeaderboards)
			.SetUpDbSet(db => db.CustomEntries, mockEntities.MockDbSetCustomEntries);

		Mock<IWebHostEnvironment> environment = new();
		environment.Setup(m => m.EnvironmentName).Returns(Environments.Development);

		Mock<AuditLogger> auditLogger = new(environment.Object, new LogContainerService());
		Mock<IFileSystemService> fileSystemService = new();
		CustomLeaderboardValidatorService clvs = new(_dbContext.Object, fileSystemService.Object);

		// TODO: Use repository.
		_customLeaderboardsController = new CustomLeaderboardsController(_dbContext.Object, clvs, auditLogger.Object);
	}

	[TestMethod]
	public void TestIsAscending()
	{
		Assert.IsFalse(CustomLeaderboardCategory.Survival.IsAscending());
		Assert.IsFalse(CustomLeaderboardCategory.Pacifist.IsAscending());

		Assert.IsTrue(CustomLeaderboardCategory.TimeAttack.IsAscending());
		Assert.IsTrue(CustomLeaderboardCategory.Speedrun.IsAscending());
		Assert.IsTrue(CustomLeaderboardCategory.Race.IsAscending());
	}

	[TestMethod]
	public void GetCustomLeaderboards()
	{
		Page<GetCustomLeaderboardForOverview>? customLeaderboards = _customLeaderboardsController.GetCustomLeaderboards().Value;

		_dbContext.Verify(db => db.SaveChanges(), Times.Never);
		Assert.IsNotNull(customLeaderboards);
		Assert.AreEqual(1, customLeaderboards.Results.Count);
		Assert.IsTrue(customLeaderboards.Results.Any(cl => cl.Daggers.Bronze == 60));
	}
}
