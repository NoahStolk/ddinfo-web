using DevilDaggersInfo.Types.Core.Mods;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using DevilDaggersInfo.Web.Server.Domain.Models.ModArchives;
using DdaeApi = DevilDaggersInfo.Api.Ddae.Mods;

namespace DevilDaggersInfo.Web.Server.Converters.DomainToApi.Ddae;

// TODO: Use domain models.
public static class ModConverters
{
	// ! Navigation property.
	public static DdaeApi.GetModDdae ToDdaeApi(this ModEntity mod, ModFileSystemData modFileSystemData) => new()
	{
		Authors = mod.PlayerMods!.ConvertAll(pm => pm.Player!.PlayerName),
		ContainsProhibitedAssets = modFileSystemData.ModArchive?.ContainsProhibitedAssets(),
		HtmlDescription = mod.HtmlDescription,
		IsHostedOnDdInfo = modFileSystemData.ModArchive != null,
		LastUpdated = mod.LastUpdated,
		ModArchive = modFileSystemData.ModArchive == null ? null : new()
		{
			Binaries = modFileSystemData.ModArchive.Binaries.ConvertAll(b => new DdaeApi.GetModBinaryDdae
			{
				ModBinaryType = b.ModBinaryType.ToDdaeApi(),
				Name = b.Name,
				Size = b.Size,
			}),
			FileSize = modFileSystemData.ModArchive.FileSize,
			FileSizeExtracted = modFileSystemData.ModArchive.FileSizeExtracted,
		},
		AssetModTypes = (modFileSystemData.ModArchive?.ModTypes() ?? mod.ModTypes).ToDdaeApi(),
		Name = mod.Name,
		ScreenshotFileNames = modFileSystemData.ScreenshotFileNames ?? new(),
		TrailerUrl = mod.TrailerUrl,
	};

	private static DdaeApi.ModBinaryTypeDdae ToDdaeApi(this ModBinaryType modBinaryType) => modBinaryType switch
	{
		ModBinaryType.Audio => DdaeApi.ModBinaryTypeDdae.Audio,
		ModBinaryType.Dd => DdaeApi.ModBinaryTypeDdae.Dd,
		_ => throw new ArgumentOutOfRangeException(nameof(modBinaryType), modBinaryType, null),
	};

	private static DdaeApi.ModTypesDdae ToDdaeApi(this ModTypes modTypes) => (DdaeApi.ModTypesDdae)(int)modTypes;
}
