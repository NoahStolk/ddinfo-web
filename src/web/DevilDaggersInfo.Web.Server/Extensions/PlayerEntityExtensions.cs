namespace DevilDaggersInfo.Web.Server.Extensions;

public static class PlayerEntityExtensions
{
	public static bool HasVisibleSettings(this PlayerEntity player)
		=> !player.HideSettings && player.HasSettings();

	public static bool HasSettings(this PlayerEntity player)
	{
		return player.Dpi.HasValue ||
			player.InGameSens.HasValue ||
			player.Fov.HasValue ||
			player.IsRightHanded.HasValue ||
			player.HasFlashHandEnabled.HasValue ||
			player.Gamma.HasValue ||
			player.UsesLegacyAudio.HasValue ||
			player.UsesHrtf.HasValue ||
			player.UsesInvertY.HasValue ||
			player.VerticalSync != VerticalSync.Unknown;
	}
}
