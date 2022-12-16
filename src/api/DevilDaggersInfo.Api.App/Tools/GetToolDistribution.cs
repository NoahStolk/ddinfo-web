using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Api.App.Tools;

public record GetToolDistribution
{
	public required string VersionNumber { get; init; } = string.Empty;

	public required int FileSize { get; set; }

	public required ToolPublishMethod PublishMethod { get; set; }

	public required ToolBuildType BuildType { get; set; }
}
