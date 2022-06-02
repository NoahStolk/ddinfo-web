using DevilDaggersInfo.Common.Exceptions;
using DevilDaggersInfo.Web.Server.Entities.Enums;
using MainApi = DevilDaggersInfo.Api.Main.Players;

namespace DevilDaggersInfo.Web.Server.Converters.DomainToApi.Main;

public static class PlayerConverters
{
	public static MainApi.GetPlayer ToGetPlayer(this PlayerEntity player, bool isPublicDonator) => new()
	{
		BanDescription = player.BanDescription,
		CountryCode = player.CountryCode,
		Id = player.Id,
		IsBanned = player.BanType != BanType.NotBanned,
		IsPublicDonator = isPublicDonator,
		Settings = !player.HasVisibleSettings() ? null : new()
		{
			Dpi = player.Dpi,
			Fov = player.Fov,
			Gamma = player.Gamma,
			InGameSens = player.InGameSens,
			IsRightHanded = player.IsRightHanded,
			UsesFlashHand = player.HasFlashHandEnabled,
			UsesLegacyAudio = player.UsesLegacyAudio,
			UsesHrtf = player.UsesHrtf,
			UsesInvertY = player.UsesInvertY,
			VerticalSync = player.VerticalSync.ToMainApi(),
		},
	};

	public static MainApi.GetPlayerForSettings ToGetPlayerForSettings(this PlayerEntity player) => new()
	{
		CountryCode = player.CountryCode,
		Id = player.Id,
		Settings = new()
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
			VerticalSync = player.VerticalSync.ToMainApi(),
		},
	};

	private static MainApi.VerticalSync ToMainApi(this VerticalSync verticalSync) => verticalSync switch
	{
		VerticalSync.Unknown => MainApi.VerticalSync.Unknown,
		VerticalSync.Off => MainApi.VerticalSync.Off,
		VerticalSync.On => MainApi.VerticalSync.On,
		VerticalSync.Adaptive => MainApi.VerticalSync.Adaptive,
		_ => throw new InvalidEnumConversionException(verticalSync),
	};
}
