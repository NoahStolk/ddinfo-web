using System;
using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.Code.Users
{
	public class AssetMod
	{
		public AssetModType AssetModType { get; set; }
		public string Name { get; set; }
		public string Author { get; set; }
		public string Url { get; set; }

		public AssetMod(AssetModType assetModType, string name, string author, string url)
		{
			AssetModType = assetModType;
			Name = name;
			Author = author;
			Url = url;
		}

		public string DisplayType()
		{
			List<int> flags = new List<int>();
			int type = (int)AssetModType;
			for (int i = (int)Math.Pow(2, typeof(AssetModType).GetEnumValues().Length - 1); i >= 1; i /= 2)
				if ((type & i) != 0)
					flags.Add(i);
			return string.Join(", ", flags.Select(f => ((AssetModType)f).ToString()));
		}
	}
}