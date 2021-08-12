using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersWebsite.BlazorWasm.Shared.Dto.Admin.Players
{
	public class EditPlayer
	{
		[StringLength(2)]
		public string? CountryCode { get; set; }

		[Range(1, 20000)]
		public int? Dpi { get; set; }

		[Range(0.01f, 2)]
		public float? InGameSens { get; set; }

		[Range(0, 179)]
		public int? Fov { get; set; }

		public bool? IsRightHanded { get; set; }

		public bool? HasFlashHandEnabled { get; set; }

		[Range(0.5f, 5)]
		public float? Gamma { get; set; }

		public bool? UsesLegacyAudio { get; set; }

		public bool IsBanned { get; set; }

		[StringLength(64)]
		public string? BanDescription { get; set; }

		[Range(1, 9999999)]
		public int? BanResponsibleId { get; set; }

		public bool IsBannedFromDdcl { get; set; }

		public bool HideSettings { get; set; }

		public bool HideDonations { get; set; }

		public bool HidePastUsernames { get; set; }

		public List<int>? ModIds { get; set; }

		public List<int>? TitleIds { get; set; }
	}
}
