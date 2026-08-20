using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Models.ModArchives;
using DevilDaggersInfo.Web.Server.Domain.Services.Caching;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;

namespace DevilDaggersInfo.Web.Server.Domain.Services;

public sealed class ModArchiveAccessor(IFileSystemService fileSystemService, ModArchiveCache modArchiveCache)
{
	public string GetModArchivePath(string modName)
	{
		return Path.Combine(fileSystemService.GetPath(DataSubDirectory.Mods), $"{modName}.zip");
	}

	public async Task<ModFileSystemData> GetModFileSystemDataAsync(string modName)
	{
		string modArchivePath = GetModArchivePath(modName);
		string modScreenshotsDirectory = Path.Combine(fileSystemService.GetPath(DataSubDirectory.ModScreenshots), modName);

		ModArchiveCacheData? modArchiveCacheData = File.Exists(modArchivePath) ? await modArchiveCache.GetArchiveDataByFilePathAsync(modArchivePath) : null;

		return new ModFileSystemData
		{
			ModArchive = modArchiveCacheData,
			ScreenshotFileNames = !Directory.Exists(modScreenshotsDirectory) ? null : GetScreenshotFileNames(modScreenshotsDirectory),
		};

		List<string> GetScreenshotFileNames(string directory)
		{
			// ReSharper disable once UseCollectionExpression
			// error CS8604: Possible null reference argument for parameter 'item' in 'void List<string>.Add(string item)'.
			// ! LINQ
			return Directory.GetFiles(directory).Select(Path.GetFileName).ToList()!;
		}
	}
}
