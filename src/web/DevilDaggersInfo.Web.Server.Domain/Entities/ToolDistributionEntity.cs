using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;

namespace DevilDaggersInfo.Web.Server.Domain.Entities;

[Table("ToolStatistics")]
public class ToolDistributionEntity
{
	[Key]
	public int Id { get; init; }

	// TODO: FK.
	[StringLength(64)]
	public string ToolName { get; set; } = string.Empty;

	[StringLength(16)]
	public string VersionNumber { get; set; } = string.Empty;

	public int DownloadCount { get; set; }

	public ToolPublishMethod PublishMethod { get; set; }

	public ToolBuildType BuildType { get; set; }
}
