using DevilDaggersInfo.Api.Main.Players;
using DevilDaggersInfo.Web.Server.Domain.Models.Players;
using System.Diagnostics;

namespace DevilDaggersInfo.Web.Server.Converters.DomainToApi.Main;

public static class PlayerConverters
{
	public static GetPlayerForLeaderboard ToGetPlayerForLeaderboard(this PlayerForLeaderboard player) => new()
	{
		BanDescription = player.BanDescription,
		BanResponsibleId = player.BanResponsibleId,
		BanType = player.BanType.ToMainApi(),
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
		VerticalSync = settings.VerticalSync.ToMainApi(),
	};

	private static BanType ToMainApi(this Types.Web.BanType banType) => banType switch
	{
		Types.Web.BanType.NotBanned => BanType.NotBanned,
		Types.Web.BanType.Alt => BanType.Alt,
		Types.Web.BanType.Cheater => BanType.Cheater,
		Types.Web.BanType.Boosted => BanType.Boosted,
		Types.Web.BanType.IllegitimateStats => BanType.IllegitimateStats,
		Types.Web.BanType.BlankName => BanType.BlankName,
		_ => throw new UnreachableException(),
	};

	public static VerticalSync ToMainApi(this Types.Web.VerticalSync verticalSync) => verticalSync switch
	{
		Types.Web.VerticalSync.Unknown => VerticalSync.Unknown,
		Types.Web.VerticalSync.Off => VerticalSync.Off,
		Types.Web.VerticalSync.On => VerticalSync.On,
		Types.Web.VerticalSync.Adaptive => VerticalSync.Adaptive,
		_ => throw new UnreachableException(),
	};
}
