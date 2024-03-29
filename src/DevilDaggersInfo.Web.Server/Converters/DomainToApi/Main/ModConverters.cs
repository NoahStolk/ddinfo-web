using DevilDaggersInfo.Core.Asset;
using DevilDaggersInfo.Core.Mod;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using DevilDaggersInfo.Web.Server.Domain.Models.ModArchives;
using System.Diagnostics;
using MainApi = DevilDaggersInfo.Web.ApiSpec.Main.Mods;

namespace DevilDaggersInfo.Web.Server.Converters.DomainToApi.Main;

// TODO: Use domain models.
public static class ModConverters
{
	public static MainApi.GetModOverview ToMainApiOverview(this ModEntity mod, ModFileSystemData modFileSystemData)
	{
		if (mod.PlayerMods == null)
			throw new InvalidOperationException("Player mods are not included.");

		// ! Navigation property.
		return new()
		{
			Authors = mod.PlayerMods.ConvertAll(pm => pm.Player!.PlayerName),
			ContainsProhibitedAssets = modFileSystemData.ModArchive?.ContainsProhibitedAssets(),
			Id = mod.Id,
			IsHosted = modFileSystemData.ModArchive != null,
			LastUpdated = mod.LastUpdated,
			ModTypes = (modFileSystemData.ModArchive?.ModTypes() ?? mod.ModTypes).ToMainApi(),
			Name = mod.Name,
		};
	}

	public static MainApi.GetMod ToMainApi(this ModEntity mod, ModFileSystemData modFileSystemData)
	{
		if (mod.PlayerMods == null)
			throw new InvalidOperationException("Player mods are not included.");

		// ! Navigation property.
		return new()
		{
			Authors = mod.PlayerMods.ConvertAll(pm => pm.Player!.PlayerName),
			ContainsProhibitedAssets = modFileSystemData.ModArchive?.ContainsProhibitedAssets(),
			HtmlDescription = mod.HtmlDescription,
			IsHosted = modFileSystemData.ModArchive != null,
			LastUpdated = mod.LastUpdated,
			ModArchive = modFileSystemData.ModArchive == null ? null : new()
			{
				Binaries = modFileSystemData.ModArchive.Binaries.ConvertAll(b => new MainApi.GetModBinary
				{
					ModBinaryType = b.ModBinaryType.ToMainApi(),
					Name = b.Name,
					Size = b.Size,
					Assets = b.TocEntries.ConvertAll(c => new MainApi.GetModAsset
					{
						Name = c.Name, Size = c.Size, Type = c.AssetType.ToMainApi(), IsProhibited = c.IsProhibited,
					}),
					ContainsProhibitedAssets = b.ContainsProhibitedAssets(),
					ModifiedLoudness = b.ModifiedLoudnessAssets?.ConvertAll(a => new MainApi.GetModifiedLoudness
					{
						AssetName = a.Name, IsProhibited = a.IsProhibited, DefaultLoudness = a.DefaultLoudness, ModifiedLoudness = a.ModifiedLoudness,
					}),
				}),
				FileSize = modFileSystemData.ModArchive.FileSize,
				FileSizeExtracted = modFileSystemData.ModArchive.FileSizeExtracted,
			},
			ModTypes = (modFileSystemData.ModArchive?.ModTypes() ?? mod.ModTypes).ToMainApi(),
			Name = mod.Name,
			ScreenshotFileNames = modFileSystemData.ScreenshotFileNames,
			TrailerUrl = mod.TrailerUrl,
			Url = mod.Url,
		};
	}

	private static MainApi.AssetType ToMainApi(this AssetType assetType)
	{
		return assetType switch
		{
			AssetType.Mesh => MainApi.AssetType.Mesh,
			AssetType.Texture => MainApi.AssetType.Texture,
			AssetType.Shader => MainApi.AssetType.Shader,
			AssetType.Audio => MainApi.AssetType.Audio,
			AssetType.ObjectBinding => MainApi.AssetType.ObjectBinding,
			_ => throw new UnreachableException(),
		};
	}

	private static MainApi.ModBinaryType ToMainApi(this ModBinaryType modBinaryType)
	{
		return modBinaryType switch
		{
			ModBinaryType.Audio => MainApi.ModBinaryType.Audio,
			ModBinaryType.Dd => MainApi.ModBinaryType.Dd,
			_ => throw new UnreachableException(),
		};
	}

	private static MainApi.ModTypes ToMainApi(this ModTypes modTypes)
	{
		return (MainApi.ModTypes)(int)modTypes;
	}
}
