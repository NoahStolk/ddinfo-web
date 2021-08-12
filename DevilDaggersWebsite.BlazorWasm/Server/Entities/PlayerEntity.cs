using DevilDaggersWebsite.BlazorWasm.Shared;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DevilDaggersWebsite.BlazorWasm.Server.Entities
{
	[Table("Players")]
	public class PlayerEntity : IEntity
	{
		[Key]
		public int Id { get; set; }

		[StringLength(32)]
		public string PlayerName { get; set; } = null!;

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

		public List<PlayerModEntity> PlayerAssetMods { get; set; } = new();

		public List<PlayerTitleEntity> PlayerTitles { get; set; } = new();

		public float? Edpi => Dpi * InGameSens;

		public string EdpiString => Edpi.HasValue ? Edpi.Value.ToString(FormatUtils.InGameSensFormat) : string.Empty;
		public string DpiString => Dpi.HasValue ? Dpi.Value.ToString() : string.Empty;
		public string InGameSensString => InGameSens.HasValue ? InGameSens.Value.ToString(FormatUtils.InGameSensFormat) : string.Empty;
		public string GammaString => Gamma.HasValue ? Gamma.Value.ToString(FormatUtils.GammaFormat) : string.Empty;
		public string RightHandedString => IsRightHanded.HasValue ? IsRightHanded.Value ? "Right" : "Left" : string.Empty;
		public string FlashEnabledString => HasFlashHandEnabled.HasValue ? HasFlashHandEnabled.Value ? "On" : "Off" : string.Empty;
		public string UsesLegacyAudioString => UsesLegacyAudio.HasValue ? UsesLegacyAudio.Value ? "On" : "Off" : string.Empty;

		public override string ToString()
			=> $"{PlayerName} ({Id})";

		public bool IsPublicDonator(IEnumerable<DonationEntity> donations)
			=> !HideDonations && donations.Any(d => d.PlayerId == Id && !d.IsRefunded && d.ConvertedEuroCentsReceived > 0);

		public bool HasSettings()
			=> Dpi.HasValue || InGameSens.HasValue || Fov.HasValue || IsRightHanded.HasValue || HasFlashHandEnabled.HasValue || Gamma.HasValue || UsesLegacyAudio.HasValue;
	}
}
