using DevilDaggersWebsite.Enumerators;
using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.Dto.Admin
{
	public class AdminAssetMod : IAdminDto
	{
		public List<int>? PlayerIds { get; init; }
		public List<AssetModTypes> AssetModTypes { get; init; } = null!;
		public List<AssetModFileContents> AssetModFileContents { get; init; } = null!;
		public string Name { get; init; } = null!;
		public string Url { get; init; } = null!;
		public bool IsHidden { get; set; }

		public Dictionary<string, string> Log()
		{
			Dictionary<string, string> dictionary = new();
			dictionary.Add(nameof(PlayerIds), PlayerIds != null ? string.Join(", ", PlayerIds) : string.Empty);
			dictionary.Add(nameof(AssetModTypes), string.Join(", ", AssetModTypes.Where(amt => amt != Enumerators.AssetModTypes.None)));
			dictionary.Add(nameof(AssetModFileContents), string.Join(", ", AssetModFileContents.Where(amfc => amfc != Enumerators.AssetModFileContents.None)));
			dictionary.Add(nameof(Name), Name);
			dictionary.Add(nameof(Url), Url);
			dictionary.Add(nameof(IsHidden), IsHidden.ToString());
			return dictionary;
		}
	}
}
