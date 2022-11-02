using DevilDaggersInfo.Api.Main.Players;
using DevilDaggersInfo.Types.Web;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Web.Client.StateObjects.Profile;

public class EditPlayerProfileState : IStateObject<EditPlayerProfile>
{
	[StringLength(2)]
	public string? CountryCode { get; set; }

	[Range(10, 20000, ErrorMessage = "DPI must be between 10 and 20,000.")]
	public int? Dpi { get; set; }

	[Range(0.01f, 2, ErrorMessage = "In-game sens must be between 0.01 and 2.")]
	public float? InGameSens { get; set; }

	[Range(85, 120, ErrorMessage = "FOV must be between 85 and 120.")]
	public int? Fov { get; set; }

	public bool? IsRightHanded { get; set; }

	public bool? HasFlashHandEnabled { get; set; }

	[Range(1, 3, ErrorMessage = "Gamma must be between 1 and 3.")]
	public float? Gamma { get; set; }

	public bool? UsesLegacyAudio { get; set; }

	public bool? UsesHrtf { get; set; }

	public bool? UsesInvertY { get; set; }

	public VerticalSync VerticalSync { get; set; }

	public bool HideSettings { get; set; }

	public bool HideDonations { get; set; }

	public bool HidePastUsernames { get; set; }

	public EditPlayerProfile ToModel() => new()
	{
		CountryCode = CountryCode,
		Dpi = Dpi,
		InGameSens = InGameSens,
		Fov = Fov,
		IsRightHanded = IsRightHanded,
		HasFlashHandEnabled = HasFlashHandEnabled,
		Gamma = Gamma,
		UsesLegacyAudio = UsesLegacyAudio,
		UsesHrtf = UsesHrtf,
		UsesInvertY = UsesInvertY,
		VerticalSync = VerticalSync,
		HideSettings = HideSettings,
		HideDonations = HideDonations,
		HidePastUsernames = HidePastUsernames,
	};
}
