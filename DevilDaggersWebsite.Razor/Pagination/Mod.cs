using DevilDaggersWebsite.Enumerators;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Razor.Pagination
{
	public class Mod
	{
		public Mod(AssetModTypes assetModTypes, string name, string url, List<string> authors)
		{
			AssetModTypes = assetModTypes;
			Name = name;
			Url = url;
			Authors = authors;
		}

		public AssetModTypes AssetModTypes { get; set; }
		public string Name { get; set; }
		public string Url { get; set; }
		public List<string> Authors { get; set; }
	}
}
