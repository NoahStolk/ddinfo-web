using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Api.Main.Players;

public record EditPlayerProfile
{
	[StringLength(2)]
	public required string? CountryCode { get; init; }

	[Range(10, 20000, ErrorMessage = "DPI must be between 10 and 20,000.")]
	public required int? Dpi { get; init; }

	[Range(0.01f, 2, ErrorMessage = "In-game sens must be between 0.01 and 2.")]
	public required float? InGameSens { get; init; }

	[Range(85, 120, ErrorMessage = "FOV must be between 85 and 120.")]
	public required int? Fov { get; init; }

	public required bool? IsRightHanded { get; init; }

	public required bool? HasFlashHandEnabled { get; init; }

	[Range(1, 3, ErrorMessage = "Gamma must be between 1 and 3.")]
	public required float? Gamma { get; init; }

	public required bool? UsesLegacyAudio { get; init; }

	public required bool? UsesHrtf { get; init; }

	public required bool? UsesInvertY { get; init; }

	public required VerticalSync VerticalSync { get; init; }

	public required bool HideSettings { get; init; }

	public required bool HideDonations { get; init; }

	public required bool HidePastUsernames { get; init; }
}
