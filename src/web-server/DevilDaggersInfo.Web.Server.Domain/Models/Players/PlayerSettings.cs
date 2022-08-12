using DevilDaggersInfo.Types.Web;
using DevilDaggersInfo.Web.Server.Domain.Entities;

namespace DevilDaggersInfo.Web.Server.Domain.Models.Players;

public record PlayerSettings
{
	public int? Dpi { get; init; }

	public float? InGameSens { get; init; }

	public int? Fov { get; init; }

	public bool? IsRightHanded { get; init; }

	public bool? UsesFlashHand { get; init; }

	public float? Gamma { get; init; }

	public bool? UsesLegacyAudio { get; init; }

	public bool? UsesHrtf { get; init; }

	public bool? UsesInvertY { get; init; }

	public VerticalSync VerticalSync { get; init; }

	public static PlayerSettings FromEntity(PlayerEntity player) => new()
	{
		Dpi = player.Dpi,
		Fov = player.Fov,
		Gamma = player.Gamma,
		UsesFlashHand = player.HasFlashHandEnabled,
		InGameSens = player.InGameSens,
		IsRightHanded = player.IsRightHanded,
		UsesLegacyAudio = player.UsesLegacyAudio,
		UsesHrtf = player.UsesHrtf,
		UsesInvertY = player.UsesInvertY,
		VerticalSync = player.VerticalSync,
	};
}
