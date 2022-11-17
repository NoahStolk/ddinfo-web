namespace DevilDaggersInfo.Web.Server.Domain.Entities;

[Table("Tools")]
public class ToolEntity
{
	[Key]
	[StringLength(64)]
	public required string Name { get; init; }

	[StringLength(64)]
	public required string DisplayName { get; init; }

	[StringLength(16)]
	public required string CurrentVersionNumber { get; set; }

	[StringLength(16)]
	public required string RequiredVersionNumber { get; set; }
}
