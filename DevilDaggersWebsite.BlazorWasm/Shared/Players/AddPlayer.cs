using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersWebsite.BlazorWasm.Shared.Players
{
	public class AddPlayer
	{
		[Range(1, 9999999)]
		public int Id { get; set; }

		[StringLength(32)]
		public string? PlayerName { get; set; }

		[StringLength(2)]
		public string? CountryCode { get; set; }

		public int? Dpi { get; set; }

		public float? InGameSens { get; set; }

		public int? Fov { get; set; }

		public bool? IsRightHanded { get; set; }

		public bool? HasFlashHandEnabled { get; set; }

		public float? Gamma { get; set; }

		public bool? UsesLegacyAudio { get; set; }

		public bool IsBanned { get; set; }

		[StringLength(64)]
		public string? BanDescription { get; set; }

		public int? BanResponsibleId { get; set; }

		public bool IsBannedFromDdcl { get; set; }

		public bool HideSettings { get; set; }

		public bool HideDonations { get; set; }

		public bool HidePastUsernames { get; set; }

		public List<int>? AssetModIds { get; set; }

		public List<int>? TitleIds { get; set; }
	}
}
