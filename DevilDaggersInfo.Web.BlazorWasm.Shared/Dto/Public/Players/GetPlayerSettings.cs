namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Players;

public class GetPlayerSettings
{
	public int? Dpi { get; init; }

	public float? InGameSens { get; init; }

	public int? Fov { get; init; }

	public bool? IsRightHanded { get; init; }

	public bool? UsesFlashHand { get; init; }

	public float? Gamma { get; init; }

	public bool? UsesLegacyAudio { get; init; }
}
