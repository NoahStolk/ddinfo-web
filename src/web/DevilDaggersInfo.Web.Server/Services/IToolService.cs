using DevilDaggersInfo.Web.Server.Entities.Enums;
using MainApi = DevilDaggersInfo.Api.Main.Tools;

namespace DevilDaggersInfo.Web.Server.Services;

public interface IToolService
{
	Task<MainApi.GetToolDistribution?> GetLatestToolDistributionAsync(string name, ToolPublishMethod publishMethod, ToolBuildType buildType);

	Task<MainApi.GetToolDistribution?> GetToolDistributionByVersionAsync(string name, ToolPublishMethod publishMethod, ToolBuildType buildType, string version);

	Task<MainApi.GetTool?> GetToolAsync(string name);

	byte[]? GetToolDistributionFile(string name, ToolPublishMethod publishMethod, ToolBuildType buildType, string version);

	Task UpdateToolDistributionStatisticsAsync(string name, ToolPublishMethod publishMethod, ToolBuildType buildType, string version);

	Task AddDistribution(string name, ToolPublishMethod publishMethod, ToolBuildType buildType, string version, byte[] zipFileContents);
}
