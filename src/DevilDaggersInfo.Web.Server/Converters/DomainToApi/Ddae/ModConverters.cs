using DevilDaggersInfo.Core.Mod;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using DevilDaggersInfo.Web.Server.Domain.Models.ModArchives;
using System.Diagnostics;
using DdaeApi = DevilDaggersInfo.Web.ApiSpec.Ddae.Mods;

namespace DevilDaggersInfo.Web.Server.Converters.DomainToApi.Ddae;

// TODO: Use domain models.
public static class ModConverters
{
	public static DdaeApi.GetModDdae ToDdaeApi(this ModEntity mod, ModFileSystemData modFileSystemData)
	{
		// ! Navigation property.
		return new DdaeApi.GetModDdae
		{
			Authors = mod.PlayerMods!.ConvertAll(pm => pm.Player!.PlayerName),
			ContainsProhibitedAssets = modFileSystemData.ModArchive?.ContainsProhibitedAssets(),
			HtmlDescription = mod.HtmlDescription,
			IsHostedOnDdInfo = modFileSystemData.ModArchive != null,
			LastUpdated = mod.LastUpdated,
			ModArchive = modFileSystemData.ModArchive == null ? null : new DdaeApi.GetModArchiveDdae
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
			ScreenshotFileNames = modFileSystemData.ScreenshotFileNames ?? [],
			TrailerUrl = mod.TrailerUrl,
		};
	}

	private static DdaeApi.ModBinaryTypeDdae ToDdaeApi(this ModBinaryType modBinaryType)
	{
		return modBinaryType switch
		{
			ModBinaryType.Audio => DdaeApi.ModBinaryTypeDdae.Audio,
			ModBinaryType.Dd => DdaeApi.ModBinaryTypeDdae.Dd,
			_ => throw new UnreachableException(),
		};
	}

	private static DdaeApi.ModTypesDdae ToDdaeApi(this ModTypes modTypes)
	{
		return (DdaeApi.ModTypesDdae)(int)modTypes;
	}
}
