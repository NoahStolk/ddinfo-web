using DevilDaggersWebsite.Enumerators;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersWebsite.Entities
{
	public class AssetMod
	{
		[Key]
		public int Id { get; set; }

		public List<PlayerAssetMod> PlayerAssetMods { get; set; } = new();
		public AssetModTypes AssetModTypes { get; set; }
		public AssetModFileContents AssetModFileContents { get; set; }
		public string Name { get; set; } = null!;
		public string Url { get; set; } = null!;
	}
}
