using DevilDaggersWebsite.BlazorWasm.Server.Entities;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto.Admin.Players;

namespace DevilDaggersWebsite.BlazorWasm.Server.Converters.Admin
{
	public static class PlayerConverters
	{
		public static GetPlayerBase ToGetPlayerBase(this Player player) => new()
		{
			Id = player.Id,
			PlayerName = player.PlayerName,
			CountryCode = player.CountryCode,
			Dpi = player.Dpi,
			InGameSens = player.InGameSens,
			Fov = player.Fov,
			IsBanned = player.IsBanned,
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
		};

		public static GetPlayer ToGetPlayer(this Player player) => new()
		{
			Id = player.Id,
			PlayerName = player.PlayerName,
			CountryCode = player.CountryCode,
			Dpi = player.Dpi,
			InGameSens = player.InGameSens,
			Fov = player.Fov,
			IsRightHanded = player.IsRightHanded,
			HasFlashHandEnabled = player.HasFlashHandEnabled,
			Gamma = player.Gamma,
			UsesLegacyAudio = player.UsesLegacyAudio,
			IsBanned = player.IsBanned,
			BanDescription = player.BanDescription,
			BanResponsibleId = player.BanResponsibleId,
			IsBannedFromDdcl = player.IsBannedFromDdcl,
			HideSettings = player.HideSettings,
			HideDonations = player.HideDonations,
			HidePastUsernames = player.HidePastUsernames,
			AssetModIds = player.PlayerAssetMods.ConvertAll(pam => pam.AssetModId),
			TitleIds = player.PlayerTitles.ConvertAll(pt => pt.TitleId),
		};
	}
}
