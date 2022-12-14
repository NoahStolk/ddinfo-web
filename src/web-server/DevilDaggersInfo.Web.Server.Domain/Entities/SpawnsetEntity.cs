namespace DevilDaggersInfo.Web.Server.Domain.Entities;

[Table("SpawnsetFiles")]
public class SpawnsetEntity : IAuditable
{
	[Key]
	public int Id { get; init; }

	public int PlayerId { get; set; }

	[ForeignKey(nameof(PlayerId))]
	public PlayerEntity Player { get; set; } = null!; // TODO: Nullable.

	[StringLength(64)]
	public required string Name { get; set; }

	public int? MaxDisplayWaves { get; set; }

	[StringLength(2048)]
	public string? HtmlDescription { get; set; }

	public DateTime LastUpdated { get; init; }

	public bool IsPractice { get; set; }
}
