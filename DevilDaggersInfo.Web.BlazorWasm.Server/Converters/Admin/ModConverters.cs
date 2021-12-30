using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.Mods;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Converters.Admin;

public static class ModConverters
{
	public static GetModForOverview ToGetModForOverview(this ModEntity mod) => new()
	{
		Id = mod.Id,
		ModTypes = mod.ModTypes,
		HtmlDescription = mod.HtmlDescription?.TrimAfter(40, true),
		IsHidden = mod.IsHidden,
		LastUpdated = mod.LastUpdated,
		Name = mod.Name,
		TrailerUrl = mod.TrailerUrl?.TrimAfter(40, true),
		Url = mod.Url?.TrimAfter(40, true),
	};

	public static GetMod ToGetMod(this ModEntity mod, List<string>? binaryNames, List<string>? screenshotNames) => new()
	{
		Id = mod.Id,
		ModTypes = mod.ModTypes,
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
}
