using DevilDaggersWebsite.Enumerators;
using System;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Razor.Pagination
{
	public class Mod
	{
		public Mod(string name, List<string> authors, DateTime lastUpdated, AssetModTypes assetModTypes, bool isHostedOnDdInfo, bool? containsProhibitedAssets)
		{
			Name = name;
			Authors = authors;
			LastUpdated = lastUpdated;
			AssetModTypes = assetModTypes;
			IsHostedOnDdInfo = isHostedOnDdInfo;
			ContainsProhibitedAssets = containsProhibitedAssets;
		}

		public string Name { get; set; }
		public List<string> Authors { get; set; }
		public DateTime LastUpdated { get; set; }
		public AssetModTypes AssetModTypes { get; set; }
		public bool IsHostedOnDdInfo { get; set; }
		public bool? ContainsProhibitedAssets { get; set; }
	}
}
