namespace DevilDaggersInfo.Web.Shared.Dto.Admin.Tools;

public record AddDistribution
{
	public string Name { get; set; } = null!;

	public ToolPublishMethod PublishMethod { get; set; }

	public ToolBuildType BuildType { get; set; }

	public string Version { get; set; } = null!;

	public byte[] ZipFileContents { get; set; } = null!;
}
