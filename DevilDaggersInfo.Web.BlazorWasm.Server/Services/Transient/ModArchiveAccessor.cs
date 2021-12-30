using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.ModArchives;
using DevilDaggersInfo.Web.BlazorWasm.Server.InternalModels;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Services.Transient;

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

	public bool ModArchiveExists(string modName) => IoFile.Exists(GetModArchivePath(modName));

	public ModFileSystemData GetModFileSystemData(string modName)
	{
		string modArchivePath = GetModArchivePath(modName);
		string modScreenshotsDirectory = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.ModScreenshots), modName);

		ModArchiveCacheData? modArchiveCacheData = IoFile.Exists(modArchivePath) ? _modArchiveCache.GetArchiveDataByFilePath(modArchivePath) : null;
		List<string>? screenshotFileNames = Directory.Exists(modScreenshotsDirectory) ? Directory.GetFiles(modScreenshotsDirectory).Select(p => Path.GetFileName(p)).ToList() : null;

		return new(modArchiveCacheData, screenshotFileNames);
	}
}
