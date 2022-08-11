using DevilDaggersInfo.Types.Web;
using DevilDaggersInfo.Web.Server.Domain.Models.Tools;

namespace DevilDaggersInfo.Web.Server.Domain.Services;

public interface IToolService
{
	Task<ToolDistribution?> GetLatestToolDistributionAsync(string name, ToolPublishMethod publishMethod, ToolBuildType buildType);

	Task<ToolDistribution?> GetToolDistributionByVersionAsync(string name, ToolPublishMethod publishMethod, ToolBuildType buildType, string version);

	Task<Tool?> GetToolAsync(string name);

	Task<List<ToolDistribution>> GetLatestToolDistributionsAsync(OperatingSystemType operatingSystem);

	byte[]? GetToolDistributionFile(string name, ToolPublishMethod publishMethod, ToolBuildType buildType, string version);

	Task UpdateToolDistributionStatisticsAsync(string name, ToolPublishMethod publishMethod, ToolBuildType buildType, string version);

	Task AddDistribution(string name, ToolPublishMethod publishMethod, ToolBuildType buildType, string version, byte[] zipFileContents);
}
