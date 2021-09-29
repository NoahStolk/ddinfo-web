namespace DevilDaggersInfo.Web.BlazorWasm.Server.Entities;

[Table("Tools")]
public class ToolEntity
{
	[Key]
	[StringLength(64)]
	public string Name { get; set; } = null!;

	[StringLength(64)]
	public string DisplayName { get; set; } = null!;

	[StringLength(16)]
	public string CurrentVersionNumber { get; set; } = null!;

	[StringLength(16)]
	public string RequiredVersionNumber { get; set; } = null!;
}
