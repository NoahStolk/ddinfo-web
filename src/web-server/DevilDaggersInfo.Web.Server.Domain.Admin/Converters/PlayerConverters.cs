using DevilDaggersInfo.Api.Admin.Players;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using System.Diagnostics;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Converters;

public static class PlayerConverters
{
	public static GetPlayerForOverview ToGetPlayerForOverview(this PlayerEntity player) => new()
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

	// ! Navigation property.
	public static GetPlayer ToGetPlayer(this PlayerEntity player) => new()
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
		ModIds = player.PlayerMods!.ConvertAll(pam => pam.ModId),
	};

	private static BanType ToAdminApi(this Types.Web.BanType banType) => banType switch
	{
		Types.Web.BanType.NotBanned => BanType.NotBanned,
		Types.Web.BanType.Alt => BanType.Alt,
		Types.Web.BanType.Cheater => BanType.Cheater,
		Types.Web.BanType.Boosted => BanType.Boosted,
		Types.Web.BanType.IllegitimateStats => BanType.IllegitimateStats,
		Types.Web.BanType.BlankName => BanType.BlankName,
		_ => throw new UnreachableException(),
	};

	private static VerticalSync ToAdminApi(this Types.Web.VerticalSync verticalSync) => verticalSync switch
	{
		Types.Web.VerticalSync.Unknown => VerticalSync.Unknown,
		Types.Web.VerticalSync.Off => VerticalSync.Off,
		Types.Web.VerticalSync.On => VerticalSync.On,
		Types.Web.VerticalSync.Adaptive => VerticalSync.Adaptive,
		_ => throw new UnreachableException(),
	};

	public static Types.Web.BanType ToDomain(this BanType banType) => banType switch
	{
		BanType.NotBanned => Types.Web.BanType.NotBanned,
		BanType.Alt => Types.Web.BanType.Alt,
		BanType.Cheater => Types.Web.BanType.Cheater,
		BanType.Boosted => Types.Web.BanType.Boosted,
		BanType.IllegitimateStats => Types.Web.BanType.IllegitimateStats,
		BanType.BlankName => Types.Web.BanType.BlankName,
		_ => throw new UnreachableException(),
	};

	public static Types.Web.VerticalSync ToDomain(this VerticalSync verticalSync) => verticalSync switch
	{
		VerticalSync.Unknown => Types.Web.VerticalSync.Unknown,
		VerticalSync.Off => Types.Web.VerticalSync.Off,
		VerticalSync.On => Types.Web.VerticalSync.On,
		VerticalSync.Adaptive => Types.Web.VerticalSync.Adaptive,
		_ => throw new UnreachableException(),
	};
}
