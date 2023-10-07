namespace DevilDaggersInfo.Api.Admin.Tools;

public record AddDistribution
{
	public required string Name { get; init; }

	public required ToolPublishMethod PublishMethod { get; init; }

	public required ToolBuildType BuildType { get; init; }

	public required string Version { get; init; }

	public required byte[] ZipFileContents { get; init; }

	public required bool UpdateVersion { get; init; }

	public required bool UpdateRequiredVersion { get; init; }
}
