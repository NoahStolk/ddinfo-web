using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;

namespace DevilDaggersInfo.Web.Server.Domain.Entities;

[Table("ToolStatistics")]
public class ToolDistributionEntity
{
	[Key]
	public int Id { get; init; }

	// TODO: FK.
	[StringLength(64)]
	public required string ToolName { get; set; }

	[StringLength(16)]
	public required string VersionNumber { get; set; }

	public int DownloadCount { get; set; }

	public ToolPublishMethod PublishMethod { get; set; }

	public ToolBuildType BuildType { get; set; }
}
