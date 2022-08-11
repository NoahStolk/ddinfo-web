using DevilDaggersInfo.Types.Web;
using DevilDaggersInfo.Web.Server.Domain.Models.Players;
using MainApi = DevilDaggersInfo.Api.Main.Players;

namespace DevilDaggersInfo.Web.Server.Converters.DomainToApi.Main;

public static class PlayerConverters
{
	public static MainApi.GetPlayerProfile ToMainApi(this PlayerProfile editPlayerProfile) => new()
	{
		CountryCode = editPlayerProfile.CountryCode,
		Dpi = editPlayerProfile.Dpi,
		Fov = editPlayerProfile.Fov,
		Gamma = editPlayerProfile.Gamma,
		HasFlashHandEnabled = editPlayerProfile.HasFlashHandEnabled,
		HideDonations = editPlayerProfile.HideDonations,
		HidePastUsernames = editPlayerProfile.HidePastUsernames,
		HideSettings = editPlayerProfile.HideSettings,
		InGameSens = editPlayerProfile.InGameSens,
		IsRightHanded = editPlayerProfile.IsRightHanded,
		UsesHrtf = editPlayerProfile.UsesHrtf,
		UsesInvertY = editPlayerProfile.UsesInvertY,
		UsesLegacyAudio = editPlayerProfile.UsesLegacyAudio,
		VerticalSync = editPlayerProfile.VerticalSync,
	};

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
			VerticalSync = player.VerticalSync,
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
			VerticalSync = player.VerticalSync,
		},
	};
}
