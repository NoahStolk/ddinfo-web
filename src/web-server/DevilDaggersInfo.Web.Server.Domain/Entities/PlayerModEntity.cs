namespace DevilDaggersInfo.Web.Server.Domain.Entities;

[Table("PlayerAssetMods")]
public class PlayerModEntity
{
	public int PlayerId { get; set; }

	[ForeignKey(nameof(PlayerId))]
	public PlayerEntity? Player { get; set; }

	[Column("AssetModId")]
	public int ModId { get; set; }

	[ForeignKey(nameof(ModId))]
	public ModEntity? Mod { get; set; }
}
