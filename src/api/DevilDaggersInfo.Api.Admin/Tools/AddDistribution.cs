using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Api.Admin.Tools;

public record AddDistribution
{
	public required string Name { get; set; }

	public required ToolPublishMethod PublishMethod { get; set; }

	public required ToolBuildType BuildType { get; set; }

	public required string Version { get; set; }

	public required byte[] ZipFileContents { get; set; }

	public required bool UpdateVersion { get; set; }

	public required bool UpdateRequiredVersion { get; set; }
}
