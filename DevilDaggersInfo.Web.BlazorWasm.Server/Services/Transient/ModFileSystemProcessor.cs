using DevilDaggersInfo.Core.Mod.Utils;
using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.ModArchives;
using DevilDaggersInfo.Web.BlazorWasm.Server.InternalModels;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Utils;
using System.Linq;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Services.Transient;

public class ModFileSystemProcessor
{
	private readonly IFileSystemService _fileSystemService;
	private readonly ModArchiveCache _modArchiveCache;
	private readonly ModFileSystemAccessor _modFileSystemAccessor;

	public ModFileSystemProcessor(IFileSystemService fileSystemService, ModArchiveCache modArchiveCache, ModFileSystemAccessor modFileSystemAccessor)
	{
		_fileSystemService = fileSystemService;
		_modArchiveCache = modArchiveCache;
		_modFileSystemAccessor = modFileSystemAccessor;
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

		ValidateModArchiveOnDisk(modName, zipFilePath, zipBytes);

		fileSystemInformation.Add(new($"File {_fileSystemService.FormatPath(zipFilePath)} ({FileSizeUtils.Format(zipBytes.Length)}) with binaries {string.Join(", ", binaries.Select(kvp => $"`{kvp.Key}`"))} was added.", FileSystemInformationType.Add));
	}

	public async Task TransformBinariesInModArchiveAsync(string modName, List<string> binariesToDelete, Dictionary<string, byte[]> newBinaries, List<FileSystemInformation> fileSystemInformation)
	{
		if (binariesToDelete.Count == 0 && newBinaries.Count == 0)
			return;

		string zipFilePath = _modFileSystemAccessor.GetModArchivePath(modName);
		using ZipArchive archive = ZipFile.Open(zipFilePath, ZipArchiveMode.Read);

		Dictionary<string, byte[]> keptBinaries = new();
		foreach (ZipArchiveEntry entry in archive.Entries)
		{
			byte[] extractedContents = new byte[entry.Length];

			using Stream entryStream = entry.Open();
			int readBytes = StreamUtils.ForceReadAllBytes(entryStream, extractedContents, 0, extractedContents.Length);
			if (readBytes != extractedContents.Length)
				throw new InvalidOperationException($"Reading all bytes from archived mod binary did not complete. {readBytes} out of {extractedContents.Length} bytes were read.");

			keptBinaries.Add(entry.Name, extractedContents);
		}

		string? firstCollision = keptBinaries.Keys.FirstOrDefault(keptName => newBinaries.Any(kvp => kvp.Key == keptName));
		if (firstCollision != null)
			throw new InvalidModArchiveException($"Cannot append binary '{firstCollision}' to mod archive because it already contains a binary with the exact same name.");

		if (IoFile.Exists(zipFilePath))
			IoFile.Delete(zipFilePath);

		Dictionary<string, byte[]> combinedBinaries = new List<Dictionary<string, byte[]>>() { keptBinaries, newBinaries }.SelectMany(dict => dict).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
		await ProcessModBinaryUploadAsync(modName, combinedBinaries, fileSystemInformation);
	}

	/// <summary>
	/// Moves the mod archive, mod archive cache, and the screenshots to a new path.
	/// </summary>
	public void MoveModFilesAndClearCache(string newName, string currentName, List<FileSystemInformation> fileSystemInformation)
	{
		if (newName == currentName)
			return;

		string directory = _fileSystemService.GetPath(DataSubDirectory.Mods);
		string oldPath = Path.Combine(directory, $"{currentName}.zip");
		if (IoFile.Exists(oldPath))
		{
			string newPath = Path.Combine(directory, newName);
			IoFile.Move(oldPath, newPath);
			fileSystemInformation.Add(new($"File {_fileSystemService.FormatPath(oldPath)} was moved to {_fileSystemService.FormatPath(newPath)}.", FileSystemInformationType.Move));

			// Clear entire memory cache (can't clear individual entries).
			_modArchiveCache.Clear();
		}
		else
		{
			fileSystemInformation.Add(new($"File {_fileSystemService.FormatPath(oldPath)} was not moved because it does not exist.", FileSystemInformationType.NotFound));
		}

		string cacheDirectory = _fileSystemService.GetPath(DataSubDirectory.ModArchiveCache);
		string oldCachePath = Path.Combine(cacheDirectory, $"{currentName}.json");
		if (IoFile.Exists(oldCachePath))
		{
			string newCachePath = Path.Combine(directory, newName);
			IoFile.Move(oldCachePath, newCachePath);
			fileSystemInformation.Add(new($"File {_fileSystemService.FormatPath(oldCachePath)} was moved to {_fileSystemService.FormatPath(newCachePath)}.", FileSystemInformationType.Move));
		}
		else
		{
			fileSystemInformation.Add(new($"File {_fileSystemService.FormatPath(oldCachePath)} was not moved because it does not exist.", FileSystemInformationType.NotFound));
		}

		// Always move screenshots directory (not removed when removal is requested as screenshots are separate entities).
		string oldScreenshotsDirectory = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.ModScreenshots), currentName);
		if (Directory.Exists(oldScreenshotsDirectory))
		{
			string newScreenshotsDirectory = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.ModScreenshots), newName);
			Directory.Move(oldScreenshotsDirectory, newScreenshotsDirectory);
			fileSystemInformation.Add(new($"Directory {_fileSystemService.FormatPath(oldScreenshotsDirectory)} was moved to {_fileSystemService.FormatPath(newScreenshotsDirectory)}.", FileSystemInformationType.Move));
		}
		else
		{
			fileSystemInformation.Add(new($"Directory {_fileSystemService.FormatPath(oldScreenshotsDirectory)} was not moved because it does not exist.", FileSystemInformationType.NotFound));
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

	private void ValidateModArchiveOnDisk(string modName, string zipFilePath, byte[] zipBytes)
	{
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
	}
}
