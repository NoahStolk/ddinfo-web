namespace DevilDaggersInfo.Web.Shared.Dto.Public.Tools;

public record GetToolDistribution
{
	public string VersionNumber { get; set; } = string.Empty;

	public int FileSize { get; set; }

	public ToolPublishMethod PublishMethod { get; set; }

	public ToolBuildType BuildType { get; set; }
}
