namespace DevilDaggersInfo.Web.Server.Domain.Entities;

[Table("Markers")]
public class MarkerEntity
{
	[Key]
	[StringLength(64)]
	public required string Name { get; set; }

	public long Value { get; set; }
}
