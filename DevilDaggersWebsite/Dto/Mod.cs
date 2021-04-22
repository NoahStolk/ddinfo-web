using DevilDaggersWebsite.Enumerators;
using System;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Dto
{
	public class Mod
	{
		public Mod(string name, List<string> authors, DateTime lastUpdated, AssetModTypes assetModTypes, bool isHostedOnDdInfo, bool? containsProhibitedAssets, ModArchive? modArchive)
		{
			Name = name;
			Authors = authors;
			LastUpdated = lastUpdated;
			AssetModTypes = assetModTypes;
			IsHostedOnDdInfo = isHostedOnDdInfo;
			ContainsProhibitedAssets = containsProhibitedAssets;
			ModArchive = modArchive;
		}

		public string Name { get; }
		public List<string> Authors { get; }
		public DateTime LastUpdated { get; }
		public AssetModTypes AssetModTypes { get; }
		public bool IsHostedOnDdInfo { get; }
		public bool? ContainsProhibitedAssets { get; }
		public ModArchive? ModArchive { get; }
	}
}
