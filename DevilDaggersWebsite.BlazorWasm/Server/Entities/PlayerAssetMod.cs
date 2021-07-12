using System.ComponentModel.DataAnnotations.Schema;

namespace DevilDaggersWebsite.BlazorWasm.Server.Entities
{
	public class PlayerAssetMod
	{
		public int PlayerId { get; set; }

		[ForeignKey(nameof(PlayerId))]
		public Player Player { get; set; } = null!;

		public int AssetModId { get; set; }

		[ForeignKey(nameof(AssetModId))]
		public AssetMod AssetMod { get; set; } = null!;
	}
}
