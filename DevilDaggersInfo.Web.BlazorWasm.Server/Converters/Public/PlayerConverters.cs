using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Players;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Converters.Public;

public static class PlayerConverters
{
	public static GetPlayer ToGetPlayer(this PlayerEntity player, bool isPublicDonator, List<string> titles) => new()
	{
		BanDescription = player.BanDescription,
		CountryCode = player.CountryCode,
		Dpi = player.Dpi,
		Fov = player.Fov,
		Gamma = player.Gamma,
		HasFlashHandEnabled = player.HasFlashHandEnabled,
		Id = player.Id,
		InGameSens = player.InGameSens,
		IsBanned = player.IsBanned,
		IsPublicDonator = isPublicDonator,
		IsRightHanded = player.IsRightHanded,
		Titles = titles,
		UsesLegacyAudio = player.UsesLegacyAudio,
	};

	public static GetPlayerForSettings ToGetPlayerForSettings(this PlayerEntity player, List<int> donatorIds, List<PlayerTitleEntity> playerTitles) => new()
	{
		Id = player.Id,
		IsPublicDonator = !player.HideDonations && donatorIds.Any(d => d == player.Id),
		Titles = playerTitles.Where(pt => pt.PlayerId == player.Id).Select(pt => pt.Title.Name).ToList(),
		CountryCode = player.CountryCode,
		Dpi = player.Dpi,
		Fov = player.Fov,
		Gamma = player.Gamma,
		HasFlashHandEnabled = player.HasFlashHandEnabled,
		InGameSens = player.InGameSens,
		IsRightHanded = player.IsRightHanded,
		UsesLegacyAudio = player.UsesLegacyAudio,
	};
}
