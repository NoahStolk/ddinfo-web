using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;

namespace DevilDaggersInfo.Web.Server.Domain.Entities;

[Table("AssetMods")]
public class ModEntity : IAuditable
{
	[Key]
	public int Id { get; init; }

	[StringLength(64)]
	public required string Name { get; set; }

	public bool IsHidden { get; set; }

	public DateTime LastUpdated { get; set; }

	[StringLength(64)]
	public string? TrailerUrl { get; set; }

	[StringLength(2048)]
	public string? HtmlDescription { get; set; }

	[Column("AssetModTypes")]
	public ModTypes ModTypes { get; set; }

	[StringLength(128)]
	public required string Url { get; set; }

	public List<PlayerModEntity>? PlayerMods { get; set; }
}
