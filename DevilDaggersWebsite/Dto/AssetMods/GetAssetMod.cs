using DevilDaggersWebsite.Enumerators;
using System;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Dto.AssetMods
{
	public class GetAssetMod
	{
		public int Id { get; init; }

		public string Name { get; init; } = null!;

		public bool IsHidden { get; init; }

		public DateTime LastUpdated { get; init; }

		public string? TrailerUrl { get; init; }

		public string? HtmlDescription { get; init; }

		public List<AssetModTypes>? AssetModTypes { get; init; }

		public string? Url { get; init; }

		public List<int>? PlayerIds { get; init; }
	}
}
