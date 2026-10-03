using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Models.ModArchives;
using DevilDaggersInfo.Web.Server.Domain.Services.Caching;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;

namespace DevilDaggersInfo.Web.Server.Domain.Services;

public sealed class ModArchiveAccessor(IFileSystem fileSystem, ModArchiveCache modArchiveCache)
{
	/// <summary>
	/// Returns the name of the mod archive file within <see cref="DataSubDirectory.Mods"/>.
	/// </summary>
	public static string GetModArchiveFileName(string modName)
	{
		return $"{modName}.zip";
	}

	/// <summary>
	/// Returns the prefix of the mod's screenshot file names within <see cref="DataSubDirectory.ModScreenshots"/>.
	/// </summary>
	public static string GetModScreenshotsPrefix(string modName)
	{
		return $"{modName}/";
	}

	public async Task<ModFileSystemData> GetModFileSystemDataAsync(string modName)
	{
		ModArchiveCacheData? modArchiveCacheData = await fileSystem.ExistsAsync(DataSubDirectory.Mods, GetModArchiveFileName(modName)) ? await modArchiveCache.GetArchiveDataByModNameAsync(modName) : null;

		string screenshotsPrefix = GetModScreenshotsPrefix(modName);
		IReadOnlyList<FileEntry> screenshots = await fileSystem.ListAsync(DataSubDirectory.ModScreenshots, screenshotsPrefix);

		return new ModFileSystemData
		{
			ModArchive = modArchiveCacheData,
			ScreenshotFileNames = screenshots.Count == 0 ? null : [.. screenshots.Select(s => s.Name[screenshotsPrefix.Length..])],
		};
	}
}
