using DevilDaggersInfo.Web.BlazorWasm.Server.InternalModels;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Mods;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Converters.Public;

public static class ModConverters
{
	public static GetModOverview ToGetModOverview(this ModEntity mod, ModFileSystemData modFileSystemData) => new()
	{
		Authors = mod.PlayerMods.ConvertAll(pm => pm.Player.PlayerName),
		ContainsProhibitedAssets = modFileSystemData.ModArchive?.ContainsProhibitedAssets(),
		Id = mod.Id,
		IsHosted = modFileSystemData.ModArchive != null,
		LastUpdated = mod.LastUpdated,
		ModTypes = modFileSystemData.ModArchive?.GetModTypes() ?? mod.ModTypes,
		Name = mod.Name,
	};

	public static GetMod ToGetMod(this ModEntity mod, ModFileSystemData modFileSystemData) => new()
	{
		Authors = mod.PlayerMods.ConvertAll(pm => pm.Player.PlayerName),
		ContainsProhibitedAssets = modFileSystemData.ModArchive?.ContainsProhibitedAssets(),
		HtmlDescription = mod.HtmlDescription,
		IsHosted = modFileSystemData.ModArchive != null,
		LastUpdated = mod.LastUpdated,
		ModArchive = modFileSystemData.ModArchive == null ? null : new()
		{
			Binaries = modFileSystemData.ModArchive.Binaries.ConvertAll(b => new GetModBinary
			{
				ModBinaryType = b.ModBinaryType,
				Name = b.GetTrimmedName(mod.Name),
				Size = b.Size,
				Assets = b.Chunks.ConvertAll(c => new GetModAsset
				{
					Name = c.Name,
					Size = c.Size,
					Type = c.AssetType,
					IsProhibited = c.IsProhibited,
				}),
				ContainsProhibitedAssets = b.ContainsProhibitedAssets(),
				ModifiedLoudness = b.ModifiedLoudnessAssets?.ConvertAll(a => new GetModifiedLoudness
				{
					AssetName = a.Name,
					IsProhibited = a.IsProhibited,
					DefaultLoudness = a.DefaultLoudness,
					ModifiedLoudness = a.ModifiedLoudness,
				}),
			}),
			FileSize = modFileSystemData.ModArchive.FileSize,
			FileSizeExtracted = modFileSystemData.ModArchive.FileSizeExtracted,
		},
		ModTypes = modFileSystemData.ModArchive?.GetModTypes() ?? mod.ModTypes,
		Name = mod.Name,
		ScreenshotFileNames = modFileSystemData?.ScreenshotFileNames,
		TrailerUrl = mod.TrailerUrl,
	};

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
