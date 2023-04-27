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

	private static BanType ToAdminApi(this Entities.Enums.BanType banType) => banType switch
	{
		Entities.Enums.BanType.NotBanned => BanType.NotBanned,
		Entities.Enums.BanType.Alt => BanType.Alt,
		Entities.Enums.BanType.Cheater => BanType.Cheater,
		Entities.Enums.BanType.Boosted => BanType.Boosted,
		Entities.Enums.BanType.IllegitimateStats => BanType.IllegitimateStats,
		Entities.Enums.BanType.BlankName => BanType.BlankName,
		_ => throw new UnreachableException(),
	};

	private static VerticalSync ToAdminApi(this Entities.Enums.VerticalSync verticalSync) => verticalSync switch
	{
		Entities.Enums.VerticalSync.Unknown => VerticalSync.Unknown,
		Entities.Enums.VerticalSync.Off => VerticalSync.Off,
		Entities.Enums.VerticalSync.On => VerticalSync.On,
		Entities.Enums.VerticalSync.Adaptive => VerticalSync.Adaptive,
		_ => throw new UnreachableException(),
	};

	public static Entities.Enums.BanType ToDomain(this BanType banType) => banType switch
	{
		BanType.NotBanned => Entities.Enums.BanType.NotBanned,
		BanType.Alt => Entities.Enums.BanType.Alt,
		BanType.Cheater => Entities.Enums.BanType.Cheater,
		BanType.Boosted => Entities.Enums.BanType.Boosted,
		BanType.IllegitimateStats => Entities.Enums.BanType.IllegitimateStats,
		BanType.BlankName => Entities.Enums.BanType.BlankName,
		_ => throw new UnreachableException(),
	};

	public static Entities.Enums.VerticalSync ToDomain(this VerticalSync verticalSync) => verticalSync switch
	{
		VerticalSync.Unknown => Entities.Enums.VerticalSync.Unknown,
		VerticalSync.Off => Entities.Enums.VerticalSync.Off,
		VerticalSync.On => Entities.Enums.VerticalSync.On,
		VerticalSync.Adaptive => Entities.Enums.VerticalSync.Adaptive,
		_ => throw new UnreachableException(),
	};
}
