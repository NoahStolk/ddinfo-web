namespace DevilDaggersWebsite.Code.Users
{
	public class AssetMod : AbstractUserData
	{
		public override string FileName => "mods";

		public AssetModType AssetModType { get; set; }
		public AssetModFileContents AssetModFileContents { get; set; }
		public string Name { get; set; }
		public string Author { get; set; }
		public string Url { get; set; }
	}
}