using DevilDaggersWebsite.Enumerators;
using System;
using System.Collections.Generic;

namespace DevilDaggersWebsite.BlazorWasm.Shared.Dto.Mods
{
	public class GetModPublic
	{
		public string Name { get; init; } = null!;

		public string? HtmlDescription { get; init; }

		public string? TrailerUrl { get; init; }

		public List<string> Authors { get; init; } = null!;

		public DateTime LastUpdated { get; init; }

		public AssetModTypes AssetModTypes { get; init; }

		public bool IsHostedOnDdInfo { get; init; }

		public bool? ContainsProhibitedAssets { get; init; }

		public ModArchivePublic? ModArchive { get; init; }

		public List<string> ScreenshotFileNames { get; init; } = null!;
	}
}
