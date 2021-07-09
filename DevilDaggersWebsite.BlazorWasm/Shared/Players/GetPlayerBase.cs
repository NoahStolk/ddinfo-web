using System.ComponentModel.DataAnnotations;

namespace DevilDaggersWebsite.BlazorWasm.Shared.Players
{
	public class GetPlayerBase : IGetDto<int>
	{
		public int Id { get; init; }

		[Display(Name = "Name")]
		public string? PlayerName { get; init; }

		[Display(Name = "Country")]
		public string? CountryCode { get; init; }

		public int? Dpi { get; init; }

		[Display(Name = "Sens")]
		public float? InGameSens { get; init; }

		public int? Fov { get; init; }

		[Display(Name = "RightHand")]
		public bool? IsRightHanded { get; init; }

		[Display(Name = "FlashHand")]
		public bool? HasFlashHandEnabled { get; init; }

		public float? Gamma { get; init; }

		[Display(Name = "LegacyAudio")]
		public bool? UsesLegacyAudio { get; init; }

		[Display(Name = "Banned")]
		public bool IsBanned { get; init; }

		public string? BanDescription { get; init; }

		public int? BanResponsibleId { get; init; }

		[Display(Name = "BannedDdcl")]
		public bool IsBannedFromDdcl { get; init; }

		public bool HideSettings { get; init; }

		public bool HideDonations { get; init; }

		public bool HidePastUsernames { get; init; }
	}
}
