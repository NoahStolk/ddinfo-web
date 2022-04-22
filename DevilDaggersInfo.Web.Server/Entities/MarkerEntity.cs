namespace DevilDaggersInfo.Web.Server.Entities;

[Table("Markers")]
public class MarkerEntity
{
	[Key]
	[StringLength(64)]
	public string Name { get; set; } = null!;

	public long Value { get; set; }
}
