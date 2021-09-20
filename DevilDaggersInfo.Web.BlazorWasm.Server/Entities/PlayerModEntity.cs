namespace DevilDaggersInfo.Web.BlazorWasm.Server.Entities;

[Table("PlayerAssetMods")]
public class PlayerModEntity
{
	public int PlayerId { get; set; }

	[ForeignKey(nameof(PlayerId))]
	public PlayerEntity Player { get; set; } = null!;

	[Column("ModId")]
	public int ModId { get; set; }

	[ForeignKey(nameof(ModId))]
	public ModEntity Mod { get; set; } = null!;
}
