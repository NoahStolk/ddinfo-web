using DevilDaggersInfo.Web.Server.InternalModels.Mods;
using DevilDaggersInfo.Web.Shared.Dto.Ddae.Mods;

namespace DevilDaggersInfo.Web.Server.Converters.Ddae;

public static class ModConverters
{
	public static GetModDdae ToGetModDdae(this ModEntity mod, ModFileSystemData modFileSystemData) => new()
	{
		Authors = mod.PlayerMods.ConvertAll(pm => pm.Player.PlayerName),
		ContainsProhibitedAssets = modFileSystemData.ModArchive?.ContainsProhibitedAssets(),
		HtmlDescription = mod.HtmlDescription,
		IsHostedOnDdInfo = modFileSystemData.ModArchive != null,
		LastUpdated = mod.LastUpdated,
		ModArchive = modFileSystemData.ModArchive == null ? null : new()
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
		AssetModTypes = modFileSystemData.ModArchive?.GetModTypes() ?? mod.ModTypes,
		Name = mod.Name,
		ScreenshotFileNames = modFileSystemData.ScreenshotFileNames ?? new(),
		TrailerUrl = mod.TrailerUrl,
	};
}
