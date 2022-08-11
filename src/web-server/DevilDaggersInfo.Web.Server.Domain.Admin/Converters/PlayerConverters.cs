using DevilDaggersInfo.Api.Admin.Players;
using DevilDaggersInfo.Web.Server.Domain.Entities;

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
		BanType = player.BanType,
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
		VerticalSync = player.VerticalSync,
	};

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
		VerticalSync = player.VerticalSync,
		BanType = player.BanType,
		BanDescription = player.BanDescription,
		BanResponsibleId = player.BanResponsibleId,
		IsBannedFromDdcl = player.IsBannedFromDdcl,
		HideSettings = player.HideSettings,
		HideDonations = player.HideDonations,
		HidePastUsernames = player.HidePastUsernames,
		ModIds = player.PlayerMods.ConvertAll(pam => pam.ModId),
	};
}
