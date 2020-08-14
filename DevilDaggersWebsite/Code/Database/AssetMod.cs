using System;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Code.Database
{
	public class AssetMod
	{
		public IEnumerable<int> AuthorIds { get; set; }
		public AssetModTypes AssetModType { get; set; }
		public AssetModFileContents AssetModFileContents { get; set; }
		public string Name { get; set; }
		public Uri Url { get; set; }
	}
}