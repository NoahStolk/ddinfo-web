using DevilDaggersInfo.Api.Main.Players;
using DevilDaggersInfo.Web.Server.Domain.Models.Players;

namespace DevilDaggersInfo.Web.Server.Converters.DomainToApi.Main;

public static class PlayerConverters
{
	public static GetPlayerForLeaderboard ToGetPlayerForLeaderboard(this PlayerForLeaderboard player) => new()
	{
		BanDescription = player.BanDescription,
		BanResponsibleId = player.BanResponsibleId,
		BanType = player.BanType,
		CountryCode = player.CountryCode,
		Id = player.Id,
	};

	public static GetPlayerForSettings ToGetPlayerForSettings(this PlayerForSettings player) => new()
	{
		CountryCode = player.CountryCode,
		Id = player.Id,
		Settings = player.Settings.ToGetPlayerSettings(),
	};

	public static GetPlayer ToGetPlayer(this Player player) => new()
	{
		BanDescription = player.BanDescription,
		CountryCode = player.CountryCode,
		Id = player.Id,
		IsBanned = player.IsBanned,
		IsPublicDonator = player.IsPublicDonator,
		Settings = player.Settings?.ToGetPlayerSettings(),
	};

	public static GetPlayerSettings ToGetPlayerSettings(this PlayerSettings settings) => new()
	{
		Dpi = settings.Dpi,
		Fov = settings.Fov,
		Gamma = settings.Gamma,
		InGameSens = settings.InGameSens,
		IsRightHanded = settings.IsRightHanded,
		UsesFlashHand = settings.UsesFlashHand,
		UsesLegacyAudio = settings.UsesLegacyAudio,
		UsesHrtf = settings.UsesHrtf,
		UsesInvertY = settings.UsesInvertY,
		VerticalSync = settings.VerticalSync,
	};
}
