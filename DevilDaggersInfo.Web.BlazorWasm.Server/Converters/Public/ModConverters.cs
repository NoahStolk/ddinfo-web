using DevilDaggersInfo.Web.BlazorWasm.Server.InternalModels;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Mods;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Converters.Public;

public static class ModConverters
{
	public static GetModOverview ToGetModOverview(this ModEntity mod, ModFileSystemData? modFileSystemData) => new()
	{
		Authors = mod.PlayerMods.ConvertAll(pm => pm.Player.PlayerName),
		ContainsProhibitedAssets = modFileSystemData?.ModArchive.ContainsProhibitedAssets(),
		Id = mod.Id,
		IsHosted = modFileSystemData != null,
		LastUpdated = mod.LastUpdated,
		ModTypes = modFileSystemData?.ModArchive.GetModTypes() ?? mod.ModTypes,
		Name = mod.Name,
	};

	public static GetModDdae ToGetModDdae(this ModEntity mod, ModFileSystemData? modFileSystemData) => new()
	{
		Authors = mod.PlayerMods.ConvertAll(pm => pm.Player.PlayerName),
		ContainsProhibitedAssets = modFileSystemData?.ModArchive.ContainsProhibitedAssets(),
		HtmlDescription = mod.HtmlDescription,
		IsHosted = modFileSystemData != null,
		LastUpdated = mod.LastUpdated,
		ModArchive = modFileSystemData == null ? null : new()
		{
			Binaries = modFileSystemData.ModArchive.Binaries.ConvertAll(b => new GetModBinaryDdae
			{
				ModBinaryType = b.ModBinaryType,
				Name = b.Name,
				Size = b.Size,
			}),
			FileSize = modFileSystemData.ModArchive.FileSize,
			FileSizeExtracted = modFileSystemData.ModArchive.FileSizeExtracted,
		},
		ModTypes = modFileSystemData?.ModArchive.GetModTypes() ?? mod.ModTypes,
		Name = mod.Name,
		ScreenshotFileNames = modFileSystemData?.ScreenshotFileNames ?? new(),
		TrailerUrl = mod.TrailerUrl,
	};
}
