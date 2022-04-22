namespace DevilDaggersInfo.Web.Shared.Dto.Public.Players;

public record GetPlayerSettings
{
	public int? Dpi { get; init; }

	public float? InGameSens { get; init; }

	public int? Fov { get; init; }

	public bool? IsRightHanded { get; init; }

	public bool? UsesFlashHand { get; init; }

	public float? Gamma { get; init; }

	public bool? UsesLegacyAudio { get; init; }

	public bool? UsesHrtf { get; init; }

	public bool? UsesInvertY { get; set; }

	public VerticalSync VerticalSync { get; set; }

	// TODO: Json ignore?
	public float? Edpi => Dpi * InGameSens;
}
