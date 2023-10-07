namespace DevilDaggersInfo.Web.ApiSpec.Main.Players;

public record GetPlayerSettings
{
	public required int? Dpi { get; init; }

	public required float? InGameSens { get; init; }

	public required int? Fov { get; init; }

	public required bool? IsRightHanded { get; init; }

	public required bool? UsesFlashHand { get; init; }

	public required float? Gamma { get; init; }

	public required bool? UsesLegacyAudio { get; init; }

	public required bool? UsesHrtf { get; init; }

	public required bool? UsesInvertY { get; init; }

	public required VerticalSync VerticalSync { get; init; }

	// TODO: Json ignore?
	public float? Edpi => Dpi * InGameSens;
}
