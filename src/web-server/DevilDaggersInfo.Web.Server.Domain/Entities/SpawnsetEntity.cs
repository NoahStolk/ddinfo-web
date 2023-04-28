namespace DevilDaggersInfo.Web.Server.Domain.Entities;

[Table("SpawnsetFiles")]
public class SpawnsetEntity : IAuditable
{
	[Key]
	public int Id { get; init; }

	public int PlayerId { get; set; }

	[ForeignKey(nameof(PlayerId))]
	public PlayerEntity? Player { get; set; }

	[StringLength(64)]
	public required string Name { get; set; }

	public int? MaxDisplayWaves { get; set; }

	[StringLength(2048)]
	public string? HtmlDescription { get; set; }

	public DateTime LastUpdated { get; init; }

	public bool IsPractice { get; set; }

	[MaxLength(70 * 1024)]
	public required byte[] File { get; init; }

	[MaxLength(16)]
	public required byte[] Md5Hash { get; init; }
}
