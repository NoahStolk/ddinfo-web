namespace DevilDaggersInfo.Web.ApiSpec.Main.Tools;

public record GetToolDistribution
{
	public required string VersionNumber { get; init; }

	public required int FileSize { get; init; }

	public required ToolPublishMethod PublishMethod { get; init; }

	public required ToolBuildType BuildType { get; init; }
}
