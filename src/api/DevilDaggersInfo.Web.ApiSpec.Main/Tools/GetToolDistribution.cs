namespace DevilDaggersInfo.Api.Main.Tools;

public record GetToolDistribution
{
	public required string VersionNumber { get; init; }

	public required int FileSize { get; init; }

	public required ToolPublishMethod PublishMethod { get; set; }

	public required ToolBuildType BuildType { get; set; }
}
