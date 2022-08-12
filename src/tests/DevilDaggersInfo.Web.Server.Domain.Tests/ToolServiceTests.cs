using DevilDaggersInfo.Types.Web;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Models.Tools;
using DevilDaggersInfo.Web.Server.Domain.Services;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using DevilDaggersInfo.Web.Server.Domain.Tests.Data;
using DevilDaggersInfo.Web.Server.Domain.Tests.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DevilDaggersInfo.Web.Server.Domain.Tests;

[TestClass]
public class ToolServiceTests
{
	private readonly Mock<ApplicationDbContext> _dbContext;
	private readonly ToolService _toolService;

	public ToolServiceTests()
	{
		MockEntities mockEntities = new();

		string toolsPath = Path.Combine(TestUtils.ResourcePath, "Tools");

		Mock<IFileSystemService> fileSystemService = new();
		fileSystemService.Setup(m => m.GetPath(DataSubDirectory.Tools)).Returns(toolsPath);

		DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder = new();
		_dbContext = new Mock<ApplicationDbContext>(optionsBuilder.Options, new Mock<IHttpContextAccessor>().Object, new Mock<ILogContainerService>().Object).SetUpDbSet(db => db.ToolDistributions, mockEntities.MockDbSetToolDistributions);
		_toolService = new(_dbContext.Object, fileSystemService.Object, new Mock<ILogger<ToolService>>().Object);
	}

	[TestMethod]
	public async Task GetLatestToolDistribution()
	{
		ToolDistribution? dist = await _toolService.GetLatestToolDistributionAsync("DevilDaggersSurvivalEditor", ToolPublishMethod.SelfContained, ToolBuildType.WindowsWpf);
		Assert.IsNotNull(dist);
		Assert.AreEqual(ToolBuildType.WindowsWpf, dist.BuildType);
		Assert.AreEqual(ToolPublishMethod.SelfContained, dist.PublishMethod);
		Assert.AreEqual("2.45.0.0", dist.VersionNumber);
	}
}
