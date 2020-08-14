using System;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Code.Database
{
	public class AssetMod
	{
		public int Id { get; set; }

		public List<PlayerAssetMod> PlayerAssetMods { get; set; }
		public AssetModTypes AssetModTypes { get; set; }
		public AssetModFileContents AssetModFileContents { get; set; }
		public string Name { get; set; }
		public Uri Url { get; set; }
	}
}