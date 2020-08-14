namespace DevilDaggersWebsite.Code.Database
{
	public class PlayerAssetMod
	{
		public int PlayerId { get; set; }
		public Player Player { get; set; }

		public int AssetModId { get; set; }
		public AssetMod AssetMod { get; set; }
	}
}