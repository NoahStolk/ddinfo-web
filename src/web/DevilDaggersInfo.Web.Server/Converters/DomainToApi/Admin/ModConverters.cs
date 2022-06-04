using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using AdminApi = DevilDaggersInfo.Api.Admin.Mods;

namespace DevilDaggersInfo.Web.Server.Converters.DomainToApi.Admin;

public static class ModConverters
{
	public static AdminApi.GetModForOverview ToGetModForOverview(this ModEntity mod) => new()
	{
		Id = mod.Id,
		ModTypes = mod.ModTypes.ToAdminApi(),
		HtmlDescription = mod.HtmlDescription?.TrimAfter(40, true),
		IsHidden = mod.IsHidden,
		LastUpdated = mod.LastUpdated,
		Name = mod.Name,
		TrailerUrl = mod.TrailerUrl?.TrimAfter(40, true),
		Url = mod.Url?.TrimAfter(40, true),
	};

	public static AdminApi.GetMod ToGetMod(this ModEntity mod, List<string>? binaryNames, List<string>? screenshotNames) => new()
	{
		Id = mod.Id,
		ModTypes = mod.ModTypes.ToAdminApi(),
		HtmlDescription = mod.HtmlDescription,
		IsHidden = mod.IsHidden,
		LastUpdated = mod.LastUpdated,
		Name = mod.Name,
		PlayerIds = mod.PlayerMods.ConvertAll(pam => pam.PlayerId),
		TrailerUrl = mod.TrailerUrl,
		Url = mod.Url,
		BinaryNames = binaryNames,
		ScreenshotNames = screenshotNames,
	};

	// TODO: Remove cast.
	private static AdminApi.ModTypes ToAdminApi(this ModTypes modTypes) => (AdminApi.ModTypes)modTypes;
}
