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

		public AssetMod()
		{
		}

		public AssetMod(AssetModType assetModType, AssetModFileContents assetModFileContents, string name, string author, string url)
		{
			AssetModType = assetModType;
			AssetModFileContents = assetModFileContents;
			Name = name;
			Author = author;
			Url = url;
		}
	}
}