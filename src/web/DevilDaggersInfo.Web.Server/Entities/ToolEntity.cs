namespace DevilDaggersInfo.Web.Server.Entities;

[Table("Tools")]
public class ToolEntity
{
	[Key]
	[StringLength(64)]
	public string Name { get; set; } = string.Empty;

	[StringLength(64)]
	public string DisplayName { get; set; } = string.Empty;

	[StringLength(16)]
	public string CurrentVersionNumber { get; set; } = string.Empty;

	[StringLength(16)]
	public string RequiredVersionNumber { get; set; } = string.Empty;
}
