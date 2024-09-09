using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;

namespace DevilDaggersInfo.Web.Server.Domain.Models.Players;

public record PlayerSettings
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

	public static PlayerSettings FromEntity(PlayerEntity player)
	{
		return new PlayerSettings
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
}
