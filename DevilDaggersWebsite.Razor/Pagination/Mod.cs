using DevilDaggersWebsite.Enumerators;
using System;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Razor.Pagination
{
	public class Mod
	{
		public Mod(AssetModTypes assetModTypes, AssetModFileContents assetModFileContents, string name, Uri url, List<string> authors)
		{
			AssetModTypes = assetModTypes;
			AssetModFileContents = assetModFileContents;
			Name = name;
			Url = url;
			Authors = authors;
		}

		public AssetModTypes AssetModTypes { get; set; }
		public AssetModFileContents AssetModFileContents { get; set; }
		public string Name { get; set; }
		public Uri Url { get; set; }
		public List<string> Authors { get; set; }
	}
}
