using DevilDaggersWebsite.Enumerators;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersWebsite.BlazorWasm.Shared.Dto.Mods
{
	public class EditMod
	{
		[StringLength(64)]
		public string Name { get; init; } = null!;

		public bool IsHidden { get; init; }

		[StringLength(64)]
		public string? TrailerUrl { get; init; }

		[StringLength(2048)]
		public string? HtmlDescription { get; init; }

		public AssetModTypes AssetModTypes { get; init; }

		[StringLength(128)]
		public string? Url { get; init; }

		public List<int>? PlayerIds { get; init; }
	}
}
