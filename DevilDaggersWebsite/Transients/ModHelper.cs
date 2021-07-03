using DevilDaggersCore.Mods;
using DevilDaggersWebsite.Caches.ModArchive;
using DevilDaggersWebsite.Dto;
using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Enumerators;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DevilDaggersWebsite.Transients
{
	public class ModHelper
	{
		private readonly IWebHostEnvironment _environment;
		private readonly ApplicationDbContext _dbContext;
		private readonly ModArchiveCache _modArchiveCache;

		public ModHelper(IWebHostEnvironment environment, ApplicationDbContext dbContext, ModArchiveCache modArchiveCache)
		{
			_environment = environment;
			_dbContext = dbContext;
			_modArchiveCache = modArchiveCache;
		}

		public List<Mod> GetMods(string? authorFilter = null, string? nameFilter = null, bool? isHostedFilter = null)
		{
			IEnumerable<AssetMod> assetModsQuery = _dbContext.AssetMods
				.AsNoTracking()
				.Include(am => am.PlayerAssetMods)
					.ThenInclude(pam => pam.Player)
				.Where(am => !am.IsHidden);

			if (!string.IsNullOrWhiteSpace(authorFilter))
				assetModsQuery = assetModsQuery.Where(am => am.PlayerAssetMods.Any(pam => pam.Player.PlayerName.Contains(authorFilter, StringComparison.InvariantCultureIgnoreCase)));
			if (!string.IsNullOrWhiteSpace(nameFilter))
				assetModsQuery = assetModsQuery.Where(am => am.Name.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase));

			List<AssetMod> assetMods = assetModsQuery.ToList();

			Dictionary<AssetMod, (bool FileExists, string? Path)> assetModsWithFileInfo = assetMods.ToDictionary(am => am, am =>
			{
				string filePath = Path.Combine(_environment.WebRootPath, "mods", $"{am.Name}.zip");
				bool fileExists = File.Exists(filePath);
				return (fileExists, fileExists ? filePath : null);
			});

			if (isHostedFilter.HasValue)
				assetModsWithFileInfo = assetModsWithFileInfo.Where(kvp => kvp.Value.FileExists == isHostedFilter.Value).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

			return assetModsWithFileInfo
				.Select(amwfi =>
				{
					bool? containsProhibitedAssets = null;
					ModArchive? modArchive = null;
					AssetModTypes assetModTypes;
					if (amwfi.Value.FileExists)
					{
						ModArchiveCacheData archiveData = _modArchiveCache.GetArchiveDataByFilePath(amwfi.Value.Path!);
						containsProhibitedAssets = archiveData.Binaries.Any(md => md.Chunks.Any(mad => mad.IsProhibited));
						modArchive = new(archiveData.FileSize, archiveData.FileSizeExtracted, archiveData.Binaries.ConvertAll(b => new ModBinary(b.Name, b.Size, b.ModBinaryType)));

						ModBinaryCacheData? ddBinary = archiveData.Binaries.Find(md => md.ModBinaryType == ModBinaryType.Dd);

						assetModTypes = AssetModTypes.None;
						if (archiveData.Binaries.Any(md => md.ModBinaryType == ModBinaryType.Audio))
							assetModTypes |= AssetModTypes.Audio;
						if (archiveData.Binaries.Any(md => md.ModBinaryType == ModBinaryType.Core) || ddBinary?.Chunks.Any(mad => mad.AssetType == AssetType.Shader) == true)
							assetModTypes |= AssetModTypes.Shader;
						if (ddBinary?.Chunks.Any(mad => mad.AssetType == AssetType.ModelBinding || mad.AssetType == AssetType.Model) == true)
							assetModTypes |= AssetModTypes.Model;
						if (ddBinary?.Chunks.Any(mad => mad.AssetType == AssetType.Texture) == true)
							assetModTypes |= AssetModTypes.Texture;
					}
					else
					{
						assetModTypes = amwfi.Key.AssetModTypes;
					}

					string modScreenshotsDirectory = Path.Combine(_environment.WebRootPath, "mod-screenshots", amwfi.Key.Name);
					List<string> screenshotFileNames;
					if (Directory.Exists(modScreenshotsDirectory))
						screenshotFileNames = Directory.GetFiles(modScreenshotsDirectory).Select(p => Path.GetFileName(p)).ToList();
					else
						screenshotFileNames = new();

					return new Mod(
						name: amwfi.Key.Name,
						htmlDescription: amwfi.Key.HtmlDescription,
						trailerUrl: amwfi.Key.TrailerUrl,
						authors: amwfi.Key.PlayerAssetMods.Select(pam => pam.Player.PlayerName).OrderBy(s => s).ToList(),
						lastUpdated: amwfi.Key.LastUpdated,
						assetModTypes: assetModTypes,
						isHostedOnDdInfo: amwfi.Value.FileExists,
						containsProhibitedAssets: containsProhibitedAssets,
						modArchive: modArchive,
						screenshotFileNames: screenshotFileNames);
				})
				.ToList();
		}
	}
}
