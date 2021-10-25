using DevilDaggersWebsite.Enumerators;
using System;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Dto
{
	public class Mod
	{
		public Mod(string name, string? htmlDescription, string? trailerUrl, List<string> authors, DateTime lastUpdated, AssetModTypes assetModTypes, bool isHostedOnDdInfo, bool? containsProhibitedAssets, ModArchive? modArchive, List<string> screenshotFileNames)
		{
			Name = name;
			HtmlDescription = htmlDescription;
			TrailerUrl = trailerUrl;
			Authors = authors;
			LastUpdated = lastUpdated;
			AssetModTypes = assetModTypes;
			IsHostedOnDdInfo = isHostedOnDdInfo;
			ContainsProhibitedAssets = containsProhibitedAssets;
			ModArchive = modArchive;
			ScreenshotFileNames = screenshotFileNames;
		}

		public string Name { get; }
		public string? HtmlDescription { get; }
		public string? TrailerUrl { get; }
		public List<string> Authors { get; }
		public DateTime LastUpdated { get; }
		public AssetModTypes AssetModTypes { get; }
		public bool IsHostedOnDdInfo { get; }
		public bool? ContainsProhibitedAssets { get; }
		public ModArchive? ModArchive { get; }
		public List<string> ScreenshotFileNames { get; }
	}
}
