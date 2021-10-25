namespace DevilDaggersInfo.Web.BlazorWasm.Server.Entities;

[Table("Markers")]
public class MarkerEntity
{
	[Key]
	[StringLength(64)]
	public string Name { get; set; } = null!;

	public long Value { get; set; }
}
