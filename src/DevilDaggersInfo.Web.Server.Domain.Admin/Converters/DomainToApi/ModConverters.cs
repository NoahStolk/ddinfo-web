using DevilDaggersInfo.Web.ApiSpec.Admin.Mods;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Extensions;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Converters.DomainToApi;

public static class ModConverters
{
	public static GetModForOverview ToAdminApi(this ModEntity mod)
	{
		return new GetModForOverview
		{
			Id = mod.Id,
			ModTypes = mod.ModTypes.ToAdminApi(),
			HtmlDescription = mod.HtmlDescription?.TrimAfter(40, true),
			IsHidden = mod.IsHidden,
			LastUpdated = mod.LastUpdated,
			Name = mod.Name,
			TrailerUrl = mod.TrailerUrl?.TrimAfter(40, true),
			Url = mod.Url.TrimAfter(40, true),
		};
	}

	public static GetMod ToAdminApi(this ModEntity mod, List<string>? binaryNames, List<string>? screenshotNames)
	{
		if (mod.PlayerMods == null)
			throw new InvalidOperationException("Player mods are not included.");

		return new GetMod
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
	}

	private static ModTypes ToAdminApi(this Entities.Enums.ModTypes modTypes)
	{
		return (ModTypes)(int)modTypes;
	}
}
