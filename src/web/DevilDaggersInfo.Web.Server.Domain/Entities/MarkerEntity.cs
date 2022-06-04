namespace DevilDaggersInfo.Web.Server.Domain.Entities;

[Table("Markers")]
public class MarkerEntity
{
	[Key]
	[StringLength(64)]
	public string Name { get; set; } = null!;

	public long Value { get; set; }
}
