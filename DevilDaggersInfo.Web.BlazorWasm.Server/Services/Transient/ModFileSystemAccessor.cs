using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.ModArchives;
using DevilDaggersInfo.Web.BlazorWasm.Server.InternalModels;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Services.Transient;

public class ModFileSystemAccessor
{
	private readonly IFileSystemService _fileSystemService;
	private readonly ModArchiveCache _modArchiveCache;

	public ModFileSystemAccessor(IFileSystemService fileSystemService, ModArchiveCache modArchiveCache)
	{
		_fileSystemService = fileSystemService;
		_modArchiveCache = modArchiveCache;
	}

	public string GetModArchivePath(string modName) => Path.Combine(_fileSystemService.GetPath(DataSubDirectory.Mods), $"{modName}.zip");

	public bool ModArchiveExists(string modName) => IoFile.Exists(GetModArchivePath(modName));

	public ModFileSystemData? GetModFileSystemData(string modName)
	{
		string path = GetModArchivePath(modName);
		if (!IoFile.Exists(path))
			return null;

		ModArchiveCacheData cachedArchiveData = _modArchiveCache.GetArchiveDataByFilePath(path);
		List<string> screenshotFileNames = new();
		string modScreenshotsDirectory = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.ModScreenshots), modName);
		if (Directory.Exists(modScreenshotsDirectory))
			screenshotFileNames = Directory.GetFiles(modScreenshotsDirectory).Select(p => Path.GetFileName(p)).ToList();

		return new(cachedArchiveData, screenshotFileNames);
	}
}
