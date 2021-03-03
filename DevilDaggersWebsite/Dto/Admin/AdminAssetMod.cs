using DevilDaggersWebsite.Enumerators;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Dto.Admin
{
	public class AdminAssetMod
	{
		public List<int>? PlayerIds { get; init; }
		public List<AssetModTypes> AssetModTypes { get; init; }
		public List<AssetModFileContents> AssetModFileContents { get; init; }
		public string Name { get; init; }
		public string Url { get; init; }
	}
}
