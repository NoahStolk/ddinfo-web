namespace DevilDaggersInfo.Api.Main.Players;

public record GetPlayerProfile
{
	public required string? CountryCode { get; init; }

	public required int? Dpi { get; init; }

	public required float? InGameSens { get; init; }

	public required int? Fov { get; init; }

	public required bool? IsRightHanded { get; init; }

	public required bool? HasFlashHandEnabled { get; init; }

	public required float? Gamma { get; init; }

	public required bool? UsesLegacyAudio { get; init; }

	public required bool? UsesHrtf { get; init; }

	public required bool? UsesInvertY { get; init; }

	public required VerticalSync VerticalSync { get; init; }

	public required bool HideSettings { get; init; }

	public required bool HideDonations { get; init; }

	public required bool HidePastUsernames { get; init; }
}
