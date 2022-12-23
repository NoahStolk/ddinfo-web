using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Web.Server.Domain.Models.Tools;

public record ToolDistribution
{
	public required string Name { get; init; }

	public required string VersionNumber { get; init; }

	public required int FileSize { get; init; }

	public required ToolPublishMethod PublishMethod { get; init; }

	public required ToolBuildType BuildType { get; init; }
}
