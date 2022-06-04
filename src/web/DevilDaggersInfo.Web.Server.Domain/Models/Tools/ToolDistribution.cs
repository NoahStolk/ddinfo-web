using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;

namespace DevilDaggersInfo.Web.Server.Domain.Models.Tools;

public class ToolDistribution
{
	public string VersionNumber { get; set; } = string.Empty;

	public int FileSize { get; set; }

	public ToolPublishMethod PublishMethod { get; set; }

	public ToolBuildType BuildType { get; set; }
}
