using DevilDaggersInfo.Core.Mod.Utils;
using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.ModArchives;
using DevilDaggersInfo.Web.BlazorWasm.Server.InternalModels;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Utils;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Services;

public class ModArchiveProcessor
{
	private readonly IFileSystemService _fileSystemService;
	private readonly ModArchiveCache _modArchiveCache;
	private readonly ModArchiveAccessor _modArchiveAccessor;

	public ModArchiveProcessor(IFileSystemService fileSystemService, ModArchiveCache modArchiveCache, ModArchiveAccessor modArchiveAccessor)
	{
		_fileSystemService = fileSystemService;
		_modArchiveCache = modArchiveCache;
		_modArchiveAccessor = modArchiveAccessor;
	}

	public async Task ProcessModBinaryUploadAsync(string modName, Dictionary<string, byte[]> binaries, List<FileSystemInformation> fileSystemInformation)
	{
		// Validate if there is enough space.
		DirectoryInfo modDirectory = new(_fileSystemService.GetPath(DataSubDirectory.Mods));
		long usedSpace = modDirectory.EnumerateFiles("*.*", SearchOption.AllDirectories).Sum(fi => fi.Length);
		if (usedSpace > ModConstants.BinaryMaxHostingSpace)
			throw new($"Cannot upload mod with binaries because the limit of {ModConstants.BinaryMaxHostingSpace:N0} bytes is exceeded.");

		// Add binaries to new zip archive.
		string zipFilePath = _modArchiveAccessor.GetModArchivePath(modName);

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

		// Read and extract the new zip file to validate it and to fill the cache if everything is OK.
		byte[] zipBytes = IoFile.ReadAllBytes(zipFilePath);

		try
		{
			List<ModBinaryCacheData> addedBinaries = _modArchiveCache.GetArchiveDataByBytes(modName, zipBytes).Binaries;
			if (addedBinaries.Count == 0)
				throw new InvalidModArchiveException("Mod archive does not contain any binaries.");

			foreach (ModBinaryCacheData binary in addedBinaries)
			{
				if (binary.Chunks.Count == 0)
					throw new InvalidModBinaryException($"Mod binary '{binary.Name}' does not contain any assets.");

				if (binary.ModBinaryType is not (ModBinaryType.Audio or ModBinaryType.Dd))
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

		fileSystemInformation.Add(new($"File {_fileSystemService.FormatPath(zipFilePath)} (`{FileSizeUtils.Format(zipBytes.Length)}`) with {(binaries.Count == 1 ? "binary" : "binaries")} {string.Join(", ", binaries.Select(kvp => $"`{BinaryFileNameUtils.SanitizeModBinaryFileName(kvp.Key, modName)}`"))} was added.", FileSystemInformationType.Add));
	}

	/// <summary>
	/// Transforms the mod archive.
	/// </summary>
	/// <returns>Whether the binary contents were changed.</returns>
	public async Task<bool> TransformBinariesInModArchiveAsync(string originalModName, string newModName, List<string> binariesToDelete, Dictionary<string, byte[]> newBinaries, List<FileSystemInformation> fileSystemInformation)
	{
		bool hasAnyBinaryContentChanges = binariesToDelete.Count > 0 || newBinaries.Count > 0;
		if (!hasAnyBinaryContentChanges && originalModName == newModName)
			return false;

		Dictionary<string, byte[]> keptBinaries = new();
		string originalArchivePath = _modArchiveAccessor.GetModArchivePath(originalModName);
		if (File.Exists(originalArchivePath))
		{
			using (ZipArchive originalArchive = ZipFile.Open(originalArchivePath, ZipArchiveMode.Read))
			{
				foreach (ZipArchiveEntry entry in originalArchive.Entries.Where(e => !binariesToDelete.Contains(e.Name)))
				{
					byte[] extractedContents = new byte[entry.Length];

					using Stream entryStream = entry.Open();
					int readBytes = StreamUtils.ForceReadAllBytes(entryStream, extractedContents, 0, extractedContents.Length);
					if (readBytes != extractedContents.Length)
						throw new InvalidOperationException($"Reading all bytes from archived mod binary did not complete. {readBytes} out of {extractedContents.Length} bytes were read.");

					keptBinaries.Add(BinaryFileNameUtils.RemoveBinaryPrefix(entry.Name, originalModName), extractedContents);
				}
			}

			string? firstCollision = keptBinaries.Keys.FirstOrDefault(keptName => newBinaries.Any(kvp => BinaryFileNameUtils.SanitizeModBinaryFileName(kvp.Key, newModName) == BinaryFileNameUtils.SanitizeModBinaryFileName(keptName, newModName)));
			if (firstCollision != null)
				throw new InvalidModArchiveException($"Cannot append binary '{firstCollision}' to mod archive because it already contains a binary with the exact same name. Either request the old binary to be deleted or rename the new binary.");
		}

		DeleteModFilesAndClearCache(originalModName, fileSystemInformation);

		Dictionary<string, byte[]> combinedBinaries = new List<Dictionary<string, byte[]>>() { keptBinaries, newBinaries }.SelectMany(dict => dict).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
		await ProcessModBinaryUploadAsync(newModName, combinedBinaries, fileSystemInformation);

		return hasAnyBinaryContentChanges;
	}

	/// <summary>
	/// Deletes the mod archive and the mod archive cache for this mod, and also clears the memory cache.
	/// <b>This method does not delete mod screenshot files</b>.
	/// </summary>
	public void DeleteModFilesAndClearCache(string modName, List<FileSystemInformation> fileSystemInformation)
	{
		// Delete archive zip file.
		string archivePath = _modArchiveAccessor.GetModArchivePath(modName);
		if (IoFile.Exists(archivePath))
		{
			IoFile.Delete(archivePath);
			fileSystemInformation.Add(new($"File {_fileSystemService.FormatPath(archivePath)} was deleted because removal was requested.", FileSystemInformationType.Delete));

			// Clear entire memory cache (can't clear individual entries).
			_modArchiveCache.Clear();
		}
		else
		{
			fileSystemInformation.Add(new($"File {_fileSystemService.FormatPath(archivePath)} was not deleted because it does not exist.", FileSystemInformationType.NotFound));
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
}
