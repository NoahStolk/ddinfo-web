namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Players;

public class EditPlayerProfile
{
	[StringLength(2)]
	public string? CountryCode { get; init; }

	[Range(10, 20000)]
	public int? Dpi { get; init; }

	[Range(0.01f, 2)]
	public float? InGameSens { get; init; }

	[Range(85, 120)]
	public int? Fov { get; init; }

	public bool? IsRightHanded { get; init; }

	public bool? HasFlashHandEnabled { get; init; }

	[Range(1.5f, 2.2f)]
	public float? Gamma { get; init; }

	public bool? UsesLegacyAudio { get; init; }

	public bool? UsesHrtf { get; init; }

	public bool? UsesInvertY { get; set; }

	public VerticalSync VerticalSync { get; set; }

	public bool HideSettings { get; init; }

	public bool HideDonations { get; init; }

	public bool HidePastUsernames { get; init; }
}
