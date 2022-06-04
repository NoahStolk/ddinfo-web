using DevilDaggersInfo.Common.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using AdminApi = DevilDaggersInfo.Api.Admin.Players;

namespace DevilDaggersInfo.Web.Server.Converters.DomainToApi.Admin;

public static class PlayerConverters
{
	public static AdminApi.GetPlayerForOverview ToGetPlayerForOverview(this PlayerEntity player) => new()
	{
		Id = player.Id,
		PlayerName = player.PlayerName,
		CommonName = player.CommonName,
		DiscordUserId = player.DiscordUserId,
		CountryCode = player.CountryCode,
		Dpi = player.Dpi,
		InGameSens = player.InGameSens,
		Fov = player.Fov,
		BanType = player.BanType.ToAdminApi(),
		BanDescription = player.BanDescription,
		BanResponsibleId = player.BanResponsibleId,
		Gamma = player.Gamma,
		HasFlashHandEnabled = player.HasFlashHandEnabled,
		HideDonations = player.HideDonations,
		HidePastUsernames = player.HidePastUsernames,
		HideSettings = player.HideSettings,
		IsBannedFromDdcl = player.IsBannedFromDdcl,
		IsRightHanded = player.IsRightHanded,
		UsesLegacyAudio = player.UsesLegacyAudio,
		UsesHrtf = player.UsesHrtf,
		UsesInvertY = player.UsesInvertY,
		VerticalSync = player.VerticalSync.ToAdminApi(),
	};

	public static AdminApi.GetPlayer ToGetPlayer(this PlayerEntity player) => new()
	{
		Id = player.Id,
		CommonName = player.CommonName,
		DiscordUserId = player.DiscordUserId,
		CountryCode = player.CountryCode,
		Dpi = player.Dpi,
		InGameSens = player.InGameSens,
		Fov = player.Fov,
		IsRightHanded = player.IsRightHanded,
		HasFlashHandEnabled = player.HasFlashHandEnabled,
		Gamma = player.Gamma,
		UsesLegacyAudio = player.UsesLegacyAudio,
		UsesHrtf = player.UsesHrtf,
		UsesInvertY = player.UsesInvertY,
		VerticalSync = player.VerticalSync.ToAdminApi(),
		BanType = player.BanType.ToAdminApi(),
		BanDescription = player.BanDescription,
		BanResponsibleId = player.BanResponsibleId,
		IsBannedFromDdcl = player.IsBannedFromDdcl,
		HideSettings = player.HideSettings,
		HideDonations = player.HideDonations,
		HidePastUsernames = player.HidePastUsernames,
		ModIds = player.PlayerMods.ConvertAll(pam => pam.ModId),
	};

	public static AdminApi.BanType ToAdminApi(this BanType banType) => banType switch
	{
		BanType.NotBanned => AdminApi.BanType.NotBanned,
		BanType.Alt => AdminApi.BanType.Alt,
		BanType.Cheater => AdminApi.BanType.Cheater,
		BanType.Boosted => AdminApi.BanType.Boosted,
		BanType.IllegitimateStats => AdminApi.BanType.IllegitimateStats,
		BanType.BlankName => AdminApi.BanType.BlankName,
		_ => throw new InvalidEnumConversionException(banType),
	};

	public static AdminApi.VerticalSync ToAdminApi(this VerticalSync verticalSync) => verticalSync switch
	{
		VerticalSync.Unknown => AdminApi.VerticalSync.Unknown,
		VerticalSync.Off => AdminApi.VerticalSync.Off,
		VerticalSync.On => AdminApi.VerticalSync.On,
		VerticalSync.Adaptive => AdminApi.VerticalSync.Adaptive,
		_ => throw new InvalidEnumConversionException(verticalSync),
	};
}
