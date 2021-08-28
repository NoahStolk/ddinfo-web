namespace DevilDaggersInfo.Web.BlazorWasm.Server.Entities;

[Table("ToolStatistics")]
public class ToolStatisticEntity : IEntity
{
	[Key]
	public int Id { get; init; }

	[StringLength(64)]
	public string ToolName { get; set; } = null!;

	[StringLength(16)]
	public string VersionNumber { get; set; } = null!;

	public int DownloadCount { get; set; }
}
