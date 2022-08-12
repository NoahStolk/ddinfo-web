using DevilDaggersInfo.Web.Server.Domain.Models.ModArchives;
using MainApi = DevilDaggersInfo.Api.Main.Mods;

namespace DevilDaggersInfo.Web.Server.Converters.DomainToApi.Main;

// TODO: Use domain models.
public static class ModConverters
{
	public static MainApi.GetModOverview ToGetModOverview(this ModEntity mod, ModFileSystemData modFileSystemData) => new()
	{
		Authors = mod.PlayerMods.ConvertAll(pm => pm.Player.PlayerName),
		ContainsProhibitedAssets = modFileSystemData.ModArchive?.ContainsProhibitedAssets(),
		Id = mod.Id,
		IsHosted = modFileSystemData.ModArchive != null,
		LastUpdated = mod.LastUpdated,
		ModTypes = modFileSystemData.ModArchive?.GetModTypes() ?? mod.ModTypes,
		Name = mod.Name,
	};

	public static MainApi.GetMod ToGetMod(this ModEntity mod, ModFileSystemData modFileSystemData) => new()
	{
		Authors = mod.PlayerMods.ConvertAll(pm => pm.Player.PlayerName),
		ContainsProhibitedAssets = modFileSystemData.ModArchive?.ContainsProhibitedAssets(),
		HtmlDescription = mod.HtmlDescription,
		IsHosted = modFileSystemData.ModArchive != null,
		LastUpdated = mod.LastUpdated,
		ModArchive = modFileSystemData.ModArchive == null ? null : new()
		{
			Binaries = modFileSystemData.ModArchive.Binaries.ConvertAll(b => new MainApi.GetModBinary
			{
				ModBinaryType = b.ModBinaryType,
				Name = b.Name,
				Size = b.Size,
				Assets = b.Chunks.ConvertAll(c => new MainApi.GetModAsset
				{
					Name = c.Name,
					Size = c.Size,
					Type = c.AssetType,
					IsProhibited = c.IsProhibited,
				}),
				ContainsProhibitedAssets = b.ContainsProhibitedAssets(),
				ModifiedLoudness = b.ModifiedLoudnessAssets?.ConvertAll(a => new MainApi.GetModifiedLoudness
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
		Url = mod.Url,
	};
}
