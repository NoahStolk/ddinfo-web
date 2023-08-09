using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Models.Tools;
using DevilDaggersInfo.Web.Server.Domain.Repositories;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using DevilDaggersInfo.Web.Server.Domain.Test.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace DevilDaggersInfo.Web.Server.Domain.Test.Tests.ServerDomain;

[TestClass]
public class ToolRepositoryTests
{
	private readonly ToolRepository _toolRepository;

	public ToolRepositoryTests()
	{
		MockEntities mockEntities = new();

		string toolsPath = Path.Combine("Resources", "Tools");

		IFileSystemService fileSystemService = Substitute.For<IFileSystemService>();
		fileSystemService.GetPath(DataSubDirectory.Tools).Returns(toolsPath);

		DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder = new();
		ApplicationDbContext dbContext = Substitute.For<ApplicationDbContext>(optionsBuilder.Options, Substitute.For<IHttpContextAccessor>(), Substitute.For<ILogContainerService>());
		dbContext.ToolDistributions.Returns(mockEntities.MockDbSetToolDistributions);
		_toolRepository = new(dbContext, fileSystemService, Substitute.For<ILogger<ToolRepository>>());
	}

	[TestMethod]
	public async Task GetLatestToolDistribution()
	{
		ToolDistribution? dist = await _toolRepository.GetLatestToolDistributionAsync("DevilDaggersSurvivalEditor", ToolPublishMethod.SelfContained, ToolBuildType.WindowsWpf);
		Assert.IsNotNull(dist);
		Assert.AreEqual(ToolBuildType.WindowsWpf, dist.BuildType);
		Assert.AreEqual(ToolPublishMethod.SelfContained, dist.PublishMethod);
		Assert.AreEqual("2.45.0.0", dist.VersionNumber);
	}
}
