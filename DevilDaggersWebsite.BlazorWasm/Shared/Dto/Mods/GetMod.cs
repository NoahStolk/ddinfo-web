using DevilDaggersWebsite.BlazorWasm.Shared.Enums;
using System;
using System.Collections.Generic;

namespace DevilDaggersWebsite.BlazorWasm.Shared.Dto.Mods
{
	public class GetMod : IGetDto<int>
	{
		public int Id { get; init; }

		public string Name { get; init; } = null!;

		public bool IsHidden { get; init; }

		public DateTime LastUpdated { get; init; }

		public string? TrailerUrl { get; init; }

		public string? HtmlDescription { get; init; }

		public AssetModTypes AssetModTypes { get; init; }

		public string? Url { get; init; }

		public List<int>? PlayerIds { get; init; }
	}
}
