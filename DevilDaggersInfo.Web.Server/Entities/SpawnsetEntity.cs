namespace DevilDaggersInfo.Web.BlazorWasm.Server.Entities;

[Table("SpawnsetFiles")]
public class SpawnsetEntity
{
	[Key]
	public int Id { get; init; }

	public int PlayerId { get; set; }

	[ForeignKey(nameof(PlayerId))]
	public PlayerEntity Player { get; set; } = null!;

	[StringLength(64)]
	public string Name { get; set; } = null!;

	public int? MaxDisplayWaves { get; set; }

	[StringLength(2048)]
	public string? HtmlDescription { get; set; }

	public DateTime LastUpdated { get; init; }

	public bool IsPractice { get; set; }
}
