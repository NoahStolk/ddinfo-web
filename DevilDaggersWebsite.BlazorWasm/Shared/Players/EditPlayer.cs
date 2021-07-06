using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersWebsite.BlazorWasm.Shared.Players
{
	public class EditPlayer
	{
		[StringLength(32)]
		public string? PlayerName { get; init; }

		[StringLength(2)]
		public string? CountryCode { get; init; }

		public int? Dpi { get; init; }

		public float? InGameSens { get; init; }

		public int? Fov { get; init; }

		public bool? IsRightHanded { get; init; }

		public bool? HasFlashHandEnabled { get; init; }

		public float? Gamma { get; init; }

		public bool? UsesLegacyAudio { get; init; }

		public bool HideSettings { get; init; }

		public bool HideDonations { get; init; }

		public bool HidePastUsernames { get; init; }

		public List<int>? AssetModIds { get; init; }

		public List<int>? TitleIds { get; init; }
	}
}
