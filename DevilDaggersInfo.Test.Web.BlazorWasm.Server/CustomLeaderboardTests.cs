using DevilDaggersInfo.Web.BlazorWasm.Server.Controllers.Admin;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.CustomLeaderboards;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Extensions;

namespace DevilDaggersInfo.Test.Web.BlazorWasm.Server;

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

		Mock<AuditLogger> auditLogger = new(environment.Object);

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
		Page<GetCustomLeaderboard>? customLeaderboards = _customLeaderboardsController.GetCustomLeaderboards().Value;

		_dbContext.Verify(db => db.SaveChanges(), Times.Never);
		Assert.IsNotNull(customLeaderboards);
		Assert.AreEqual(1, customLeaderboards.Results.Count);
		Assert.IsTrue(customLeaderboards.Results.Any(cl => cl.TimeBronze == 60));
	}
}
