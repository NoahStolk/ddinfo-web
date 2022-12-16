using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Web.Server.Domain.Models.Tools;

public class ToolDistribution
{
	public string Name { get; init; } = string.Empty;

	public string VersionNumber { get; init; } = string.Empty;

	public int FileSize { get; init; }

	public ToolPublishMethod PublishMethod { get; init; }

	public ToolBuildType BuildType { get; init; }
}
