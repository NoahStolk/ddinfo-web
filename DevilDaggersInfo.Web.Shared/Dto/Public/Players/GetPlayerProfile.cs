namespace DevilDaggersInfo.Web.Shared.Dto.Public.Players;

public record GetPlayerProfile
{
	public string? CountryCode { get; init; }

	public int? Dpi { get; init; }

	public float? InGameSens { get; init; }

	public int? Fov { get; init; }

	public bool? IsRightHanded { get; init; }

	public bool? HasFlashHandEnabled { get; init; }

	public float? Gamma { get; init; }

	public bool? UsesLegacyAudio { get; init; }

	public bool? UsesHrtf { get; init; }

	public bool? UsesInvertY { get; set; }

	public VerticalSync VerticalSync { get; set; }

	public bool HideSettings { get; init; }

	public bool HideDonations { get; init; }

	public bool HidePastUsernames { get; init; }
}
