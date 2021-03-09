using DevilDaggersWebsite.Enumerators;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Dto.Admin
{
	public class AdminAssetMod
	{
		public List<int>? PlayerIds { get; init; }
		public List<AssetModTypes> AssetModTypes { get; init; } = null!;
		public List<AssetModFileContents> AssetModFileContents { get; init; } = null!;
		public string Name { get; init; } = null!;
		public string Url { get; init; } = null!;
	}
}
