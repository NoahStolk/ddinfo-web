using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;

namespace DevilDaggersInfo.Web.Server.Domain.Entities;

[Table("AssetMods")]
public class ModEntity
{
	[Key]
	public int Id { get; init; }

	[StringLength(64)]
	public string Name { get; set; } = null!;

	public bool IsHidden { get; set; }

	public DateTime LastUpdated { get; set; }

	[StringLength(64)]
	public string? TrailerUrl { get; set; }

	[StringLength(2048)]
	public string? HtmlDescription { get; set; }

	[Column("AssetModTypes")]
	public ModTypes ModTypes { get; set; }

	[StringLength(128)]
	public string Url { get; set; } = null!;

	public List<PlayerModEntity> PlayerMods { get; set; } = new();
}
