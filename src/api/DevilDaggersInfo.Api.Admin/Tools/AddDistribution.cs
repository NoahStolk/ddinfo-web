using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Api.Admin.Tools;

public record AddDistribution
{
	public string Name { get; set; } = null!;

	public ToolPublishMethod PublishMethod { get; set; }

	public ToolBuildType BuildType { get; set; }

	public string Version { get; set; } = null!;

	public byte[] ZipFileContents { get; set; } = null!;

	public bool UpdateVersion { get; set; }

	public bool UpdateRequiredVersion { get; set; }
}
