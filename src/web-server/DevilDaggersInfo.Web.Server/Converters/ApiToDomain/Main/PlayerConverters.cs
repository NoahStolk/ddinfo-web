using DevilDaggersInfo.Web.Server.Domain.Commands.Players;
using MainApi = DevilDaggersInfo.Api.Main.Players;

namespace DevilDaggersInfo.Web.Server.Converters.ApiToDomain.Main;

public static class PlayerConverters
{
	public static EditPlayerProfile ToDomain(this MainApi.EditPlayerProfile editPlayerProfile) => new()
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
}
