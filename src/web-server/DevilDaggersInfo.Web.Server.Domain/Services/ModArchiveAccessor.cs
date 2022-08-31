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

	public ModFileSystemData GetModFileSystemData(string modName)
	{
		string modArchivePath = GetModArchivePath(modName);
		string modScreenshotsDirectory = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.ModScreenshots), modName);

		ModArchiveCacheData? modArchiveCacheData = File.Exists(modArchivePath) ? _modArchiveCache.GetArchiveDataByFilePath(modArchivePath) : null;
		List<string>? screenshotFileNames = (Directory.Exists(modScreenshotsDirectory) ? Directory.GetFiles(modScreenshotsDirectory).Select(Path.GetFileName).ToList() : null)!;

		return new(modArchiveCacheData, screenshotFileNames);
	}
}
