namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Players;

public class EditPlayerProfile
{
	[StringLength(2)]
	public string? CountryCode { get; set; }

	[Range(10, 20000)]
	public int? Dpi { get; set; }

	[Range(0.01f, 2)]
	public float? InGameSens { get; set; }

	[Range(85, 120)]
	public int? Fov { get; set; }

	public bool? IsRightHanded { get; set; }

	public bool? HasFlashHandEnabled { get; set; }

	[Range(1.5f, 2.2f)]
	public float? Gamma { get; set; }

	public bool? UsesLegacyAudio { get; set; }

	public bool? UsesHrtf { get; set; }

	public bool? UsesInvertY { get; set; }

	public VerticalSync VerticalSync { get; set; }

	public bool HideSettings { get; set; }

	public bool HideDonations { get; set; }

	public bool HidePastUsernames { get; set; }
}
