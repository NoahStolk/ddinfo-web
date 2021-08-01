using DevilDaggersCore.Extensions;
using DevilDaggersWebsite.BlazorWasm.Server.Entities;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto.Admin.Mods;

namespace DevilDaggersWebsite.BlazorWasm.Server.Converters.Admin
{
	public static class ModConverters
	{
		public static GetModForOverview ToGetModForOverview(this AssetMod mod) => new()
		{
			Id = mod.Id,
			AssetModTypes = mod.AssetModTypes,
			HtmlDescription = mod.HtmlDescription?.TrimAfter(40, true),
			IsHidden = mod.IsHidden,
			LastUpdated = mod.LastUpdated,
			Name = mod.Name,
			TrailerUrl = mod.TrailerUrl?.TrimAfter(40, true),
			Url = mod.Url?.TrimAfter(40, true),
		};
	}
}
