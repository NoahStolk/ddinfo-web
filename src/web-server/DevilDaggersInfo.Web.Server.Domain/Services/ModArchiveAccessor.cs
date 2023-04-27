using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Models.ModArchives;
using DevilDaggersInfo.Web.Server.Domain.Services.Caching;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;

namespace DevilDaggersInfo.Web.Server.Domain.Services;

public class ModArchiveAccessor
{
	private readonly IFileSystemService _fileSystemService;
	private readonly ModArchiveCache _modArchiveCache;

	public ModArchiveAccessor(IFileSystemService fileSystemService, ModArchiveCache modArchiveCache)
	{
		_fileSystemService = fileSystemService;
		_modArchiveCache = modArchiveCache;
	}

	public string GetModArchivePath(string modName) => Path.Combine(_fileSystemService.GetPath(DataSubDirectory.Mods), $"{modName}.zip");

	public async Task<ModFileSystemData> GetModFileSystemDataAsync(string modName)
	{
		string modArchivePath = GetModArchivePath(modName);
		string modScreenshotsDirectory = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.ModScreenshots), modName);

		ModArchiveCacheData? modArchiveCacheData = File.Exists(modArchivePath) ? await _modArchiveCache.GetArchiveDataByFilePathAsync(modArchivePath) : null;

		return new()
		{
			ModArchive = modArchiveCacheData,
			ScreenshotFileNames = !Directory.Exists(modScreenshotsDirectory) ? null : GetScreenshotFileNames(modScreenshotsDirectory),
		};

		List<string> GetScreenshotFileNames(string s)
		{
			// ! Path.GetFileName will never return null because modScreenshotsDirectory is not null.
			return Directory.GetFiles(s).Select(Path.GetFileName).ToList()!;
		}
	}
}
