using DevilDaggersCore.Extensions;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto.Mods;
using DevilDaggersWebsite.Entities;

namespace DevilDaggersWebsite.BlazorWasm.Server.Converters
{
	public static class ModConverters
	{
		public static GetMod ToGetMod(this AssetMod mod) => new()
		{
			Id = mod.Id,
			AssetModTypes = mod.AssetModTypes,
			HtmlDescription = mod.HtmlDescription?.TrimAfter(40, true),
			IsHidden = mod.IsHidden,
			LastUpdated = mod.LastUpdated,
			Name = mod.Name,
			PlayerIds = mod.PlayerAssetMods.ConvertAll(pam => pam.PlayerId),
			TrailerUrl = mod.TrailerUrl?.TrimAfter(40, true),
			Url = mod.Url?.TrimAfter(40, true),
		};
	}
}
