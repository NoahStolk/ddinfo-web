using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersWebsite.Dto.Players
{
	public class AddPlayer
	{
		[Range(1, 9999999)]
		public int Id { get; init; }

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

		public bool IsBanned { get; init; }

		[StringLength(64)]
		public string? BanDescription { get; init; }

		public int? BanResponsibleId { get; init; }

		public bool IsBannedFromDdcl { get; init; }

		public bool HideSettings { get; init; }

		public bool HideDonations { get; init; }

		public bool HidePastUsernames { get; init; }

		public List<int>? AssetModIds { get; init; }

		public List<int>? TitleIds { get; init; }
	}
}
