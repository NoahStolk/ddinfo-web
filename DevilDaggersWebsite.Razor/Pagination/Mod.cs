using DevilDaggersWebsite.Enumerators;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Razor.Pagination
{
	public class Mod
	{
		public Mod(AssetModTypes assetModTypes, string name, List<string> authors, bool isHostedOnDdInfo, bool? containsAnyProhibitedAssets)
		{
			AssetModTypes = assetModTypes;
			Name = name;
			Authors = authors;
			IsHostedOnDdInfo = isHostedOnDdInfo;
			ContainsAnyProhibitedAssets = containsAnyProhibitedAssets;
		}

		public AssetModTypes AssetModTypes { get; set; }
		public string Name { get; set; }
		public List<string> Authors { get; set; }
		public bool IsHostedOnDdInfo { get; set; }
		public bool? ContainsAnyProhibitedAssets { get; set; }
	}
}
