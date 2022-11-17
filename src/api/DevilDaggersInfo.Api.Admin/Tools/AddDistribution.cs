using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Api.Admin.Tools;

public record AddDistribution
{
	public required string Name { get; set; }

	public ToolPublishMethod PublishMethod { get; set; }

	public ToolBuildType BuildType { get; set; }

	public required string Version { get; set; }

	public required byte[] ZipFileContents { get; set; }

	public bool UpdateVersion { get; set; }

	public bool UpdateRequiredVersion { get; set; }
}
