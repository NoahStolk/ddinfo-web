namespace DevilDaggersInfo.Web.Server.Entities;

[Table("Tools")]
public class ToolEntity
{
	[Key]
	[StringLength(64)]
	public string Name { get; init; } = string.Empty;

	[StringLength(64)]
	public string DisplayName { get; init; } = string.Empty;

	[StringLength(16)]
	public string CurrentVersionNumber { get; init; } = string.Empty;

	[StringLength(16)]
	public string RequiredVersionNumber { get; init; } = string.Empty;
}
