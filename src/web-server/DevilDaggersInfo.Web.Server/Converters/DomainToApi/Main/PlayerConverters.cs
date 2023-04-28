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
		IsPublicDonor = player.IsPublicDonor,
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

	private static BanType ToMainApi(this Domain.Entities.Enums.BanType banType) => banType switch
	{
		Domain.Entities.Enums.BanType.NotBanned => BanType.NotBanned,
		Domain.Entities.Enums.BanType.Alt => BanType.Alt,
		Domain.Entities.Enums.BanType.Cheater => BanType.Cheater,
		Domain.Entities.Enums.BanType.Boosted => BanType.Boosted,
		Domain.Entities.Enums.BanType.IllegitimateStats => BanType.IllegitimateStats,
		Domain.Entities.Enums.BanType.BlankName => BanType.BlankName,
		_ => throw new UnreachableException(),
	};

	public static VerticalSync ToMainApi(this Domain.Entities.Enums.VerticalSync verticalSync) => verticalSync switch
	{
		Domain.Entities.Enums.VerticalSync.Unknown => VerticalSync.Unknown,
		Domain.Entities.Enums.VerticalSync.Off => VerticalSync.Off,
		Domain.Entities.Enums.VerticalSync.On => VerticalSync.On,
		Domain.Entities.Enums.VerticalSync.Adaptive => VerticalSync.Adaptive,
		_ => throw new UnreachableException(),
	};
}
