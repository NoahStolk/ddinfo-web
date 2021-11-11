using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.ModArchives;
using DevilDaggersInfo.Web.BlazorWasm.Server.InternalModels;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Repositories;

public class ModRepository
{
	private readonly IFileSystemService _fileSystemService;
	private readonly ModArchiveCache _modArchiveCache;

	public ModRepository(IFileSystemService fileSystemService, ModArchiveCache modArchiveCache)
	{
		_fileSystemService = fileSystemService;
		_modArchiveCache = modArchiveCache;
	}

	public ModFileSystemData? GetModFileSystemData(ModEntity modEntity)
	{
		string path = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.Mods), $"{modEntity.Name}.zip");
		bool fileExists = IoFile.Exists(path);
		if (!fileExists)
			return null;

		ModArchiveCacheData cachedArchiveData = _modArchiveCache.GetArchiveDataByFilePath(path);
		List<string> screenshotFileNames = new();
		string modScreenshotsDirectory = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.ModScreenshots), modEntity.Name);
		if (Directory.Exists(modScreenshotsDirectory))
			screenshotFileNames = Directory.GetFiles(modScreenshotsDirectory).Select(p => Path.GetFileName(p)).ToList();

		return new(cachedArchiveData, screenshotFileNames);
	}
}
