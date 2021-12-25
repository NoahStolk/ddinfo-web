using DevilDaggersInfo.Core.Mod.Utils;
using DevilDaggersInfo.Web.BlazorWasm.Client.Pages.Custom.Mods;
using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.ModArchives;
using DevilDaggersInfo.Web.BlazorWasm.Server.InternalModels;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.Mods;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Utils;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Services.Transient;

public class ModFileSystemProcessor
{
	private readonly IFileSystemService _fileSystemService;
	private readonly ModArchiveCache _modArchiveCache;

	public ModFileSystemProcessor(IFileSystemService fileSystemService, ModArchiveCache modArchiveCache)
	{
		_fileSystemService = fileSystemService;
		_modArchiveCache = modArchiveCache;
	}

	public async Task ProcessModBinaryUploadAsync(string modName, Dictionary<string, byte[]> binaries, List<FileSystemInformation> fileSystemInformation)
	{
		string modsDirectory = _fileSystemService.GetPath(DataSubDirectory.Mods);
		DirectoryInfo di = new(modsDirectory);
		long usedSpace = di.EnumerateFiles("*.*", SearchOption.AllDirectories).Sum(fi => fi.Length);
		if (usedSpace > ModConstants.BinaryMaxHostingSpace)
			throw new($"Cannot upload mod with binaries because the limit of {ModConstants.BinaryMaxHostingSpace:N0} bytes is exceeded.");

		string zipFilePath = Path.Combine(modsDirectory, $"{modName}.zip");

		try
		{
			using ZipArchive archive = ZipFile.Open(zipFilePath, ZipArchiveMode.Create);
			foreach (KeyValuePair<string, byte[]> binary in binaries)
			{
				string binaryName = BinaryFileNameUtils.SanitizeModBinaryFileName(binary.Key, modName);
				using Stream entry = archive.CreateEntry(binaryName, CompressionLevel.SmallestSize).Open();
				using MemoryStream ms = new(binary.Value);
				await ms.CopyToAsync(entry);
			}
		}
		catch
		{
			if (IoFile.Exists(zipFilePath))
				IoFile.Delete(zipFilePath);

			throw;
		}

		// We read and extract the .zip file we just created to validate it and to fill the cache if everything is OK.
		byte[] zipBytes = IoFile.ReadAllBytes(zipFilePath);

		try
		{
			List<ModBinaryCacheData> archive = _modArchiveCache.GetArchiveDataByBytes(modName, zipBytes).Binaries;
			if (archive.Count == 0)
				throw new InvalidModArchiveException("Mod archive does not contain any binaries.");

			foreach (ModBinaryCacheData binary in archive)
			{
				if (binary.Chunks.Count == 0)
					throw new InvalidModBinaryException($"Mod binary '{binary.Name}' does not contain any assets.");

				if (!(binary.ModBinaryType is ModBinaryType.Audio or ModBinaryType.Dd))
					throw new InvalidModBinaryException($"Mod binary '{binary.Name}' is a '{binary.ModBinaryType}' mod which is not allowed.");

				string expectedPrefix = BinaryFileNameUtils.GetBinaryPrefix(binary.ModBinaryType, modName);

				if (!binary.Name.StartsWith(expectedPrefix))
					throw new InvalidModBinaryException($"Name of mod binary '{binary.Name}' must start with '{expectedPrefix}'.");

				if (binary.Name.Length == expectedPrefix.Length)
					throw new InvalidModBinaryException($"Name of mod binary '{binary.Name}' must not be equal to '{expectedPrefix}'.");
			}
		}
		catch (InvalidModArchiveException)
		{
			if (IoFile.Exists(zipFilePath))
				IoFile.Delete(zipFilePath);

			throw;
		}
		catch (InvalidModBinaryException)
		{
			if (IoFile.Exists(zipFilePath))
				IoFile.Delete(zipFilePath);

			throw;
		}

		fileSystemInformation.Add(new($"File {_fileSystemService.FormatPath(zipFilePath)} ({FileSizeUtils.Format(zipBytes.Length)}) with binaries {string.Join(", ", binaries.Select(kvp => $"`{kvp.Key}`"))} was added.", FileSystemInformationType.Add));
	}

	public void ProcessModScreenshotUpload(string modName, Dictionary<string, byte[]> screenshots, List<FileSystemInformation> fileSystemInformation)
	{
		string modScreenshotsDirectory = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.ModScreenshots), modName);
		Directory.CreateDirectory(modScreenshotsDirectory);
		int i = 0;
		foreach (KeyValuePair<string, byte[]> kvp in screenshots.OrderBy(kvp => kvp.Key))
		{
			if (!PngFileUtils.HasValidPngHeader(kvp.Value))
			{
				fileSystemInformation.Add(new($"File {kvp.Key} was skipped because it is not a valid PNG file.", FileSystemInformationType.Skip));
				continue;
			}

			string path = Path.Combine(modScreenshotsDirectory, $"{i:00}.png");
			IoFile.WriteAllBytes(path, kvp.Value);
			fileSystemInformation.Add(new($"File {_fileSystemService.FormatPath(path)} was added.", FileSystemInformationType.Add));

			i++;
		}
	}

	/// <summary>
	/// Deletes the mod archive and the mod archive cache for this mod, and also clears the memory cache.
	/// <b>This method does not delete mod screenshot files</b>.
	/// </summary>
	public void DeleteModFilesAndClearCache(string modName, List<FileSystemInformation> fileSystemInformation)
	{
		// Delete file.
		string path = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.Mods), $"{modName}.zip");
		if (IoFile.Exists(path))
		{
			IoFile.Delete(path);
			fileSystemInformation.Add(new($"File {_fileSystemService.FormatPath(path)} was deleted because removal was requested.", FileSystemInformationType.Delete));

			// Clear entire memory cache (can't clear individual entries).
			_modArchiveCache.Clear();
		}
		else
		{
			fileSystemInformation.Add(new($"File {_fileSystemService.FormatPath(path)} was not deleted because it does not exist.", FileSystemInformationType.NotFound));
		}

		// Clear file cache for this mod.
		string cachePath = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.ModArchiveCache), $"{modName}.json");
		if (IoFile.Exists(cachePath))
		{
			IoFile.Delete(cachePath);
			fileSystemInformation.Add(new($"File {_fileSystemService.FormatPath(cachePath)} was deleted because removal was requested.", FileSystemInformationType.Delete));
		}
		else
		{
			fileSystemInformation.Add(new($"File {_fileSystemService.FormatPath(cachePath)} was not deleted because it does not exist.", FileSystemInformationType.NotFound));
		}
	}

	public void DeleteScreenshotsDirectory(string modName, List<FileSystemInformation> fileSystemInformation)
	{
		string screenshotsDirectory = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.ModScreenshots), modName);
		if (Directory.Exists(screenshotsDirectory))
		{
			Directory.Delete(screenshotsDirectory, true);
			fileSystemInformation.Add(new($"Directory {_fileSystemService.FormatPath(screenshotsDirectory)} was deleted because removal was requested.", FileSystemInformationType.Delete));
		}
		else
		{
			fileSystemInformation.Add(new($"Directory {_fileSystemService.FormatPath(screenshotsDirectory)} was not deleted because it does not exist.", FileSystemInformationType.NotFound));
		}
	}
}
