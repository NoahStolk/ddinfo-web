using DevilDaggersInfo.Core.Mod;
using DevilDaggersInfo.Core.Mod.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Constants;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Models.ModArchives;
using DevilDaggersInfo.Web.Server.Domain.Services.Caching;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using DevilDaggersInfo.Web.Server.Domain.Utils;
using System.IO.Compression;

namespace DevilDaggersInfo.Web.Server.Domain.Services;

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

	public async Task ProcessModBinaryUploadAsync(string modName, Dictionary<BinaryName, byte[]> binaries)
	{
		// Validate if there is enough space.
		DirectoryInfo modDirectory = new(_fileSystemService.GetPath(DataSubDirectory.Mods));
		long usedSpace = modDirectory.EnumerateFiles("*.*", SearchOption.AllDirectories).Sum(fi => fi.Length);
		if (usedSpace > ModConstants.BinaryMaxHostingSpace)
			throw new Exception($"Cannot upload mod with binaries because the limit of {ModConstants.BinaryMaxHostingSpace:N0} bytes is exceeded.");

		// Add binaries to new zip archive.
		string zipFilePath = _modArchiveAccessor.GetModArchivePath(modName);

		try
		{
			using ZipArchive archive = ZipFile.Open(zipFilePath, ZipArchiveMode.Create);
			foreach (KeyValuePair<BinaryName, byte[]> binary in binaries)
			{
				await using Stream entry = archive.CreateEntry(binary.Key.ToFullName(modName), CompressionLevel.SmallestSize).Open();
				using MemoryStream ms = new(binary.Value);
				await ms.CopyToAsync(entry);
			}
		}
		catch
		{
			if (File.Exists(zipFilePath))
				File.Delete(zipFilePath);

			throw;
		}

		// Read and extract the new zip file to validate it and to fill the cache if everything is OK.
		byte[] zipBytes = await File.ReadAllBytesAsync(zipFilePath);

		try
		{
			List<ModBinaryCacheData> addedBinaries = (await _modArchiveCache.GetArchiveDataByBytesAsync(modName, zipBytes)).Binaries;
			if (addedBinaries.Count == 0)
				throw new InvalidModArchiveException("Mod archive does not contain any binaries.");

			foreach (ModBinaryCacheData binary in addedBinaries)
			{
				if (binary.TocEntries.Count == 0)
					throw new InvalidModBinaryException($"Mod binary '{binary.Name}' does not contain any assets.");

				BinaryName expectedName = BinaryName.Parse(binary.Name, modName);
				if (expectedName.BinaryType != binary.ModBinaryType)
					throw new InvalidModBinaryException($"Mod binary '{binary.Name}' does not have the correct binary type.");
			}
		}
		catch (Exception ex)
		{
			if (File.Exists(zipFilePath))
				File.Delete(zipFilePath);

			// Rethrow any exception as an invalid mod archive exception, so the middleware can handle it.
			throw new InvalidModArchiveException("Processing the mod archive failed.", ex);
		}
	}

	/// <summary>
	/// Transforms the mod archive.
	/// </summary>
	/// <param name="originalModName">The original name of the mod.</param>
	/// <param name="newModName">The new name of the mod. The original mod will be deleted and all the binaries will be recreated according to the new name.</param>
	/// <param name="binariesToDelete">The names of the binaries to delete.</param>
	/// <param name="newBinaries">The names and contents of the new binaries to add to the archive.</param>
	/// <returns>Whether the binary contents were changed.</returns>
	public async Task<bool> TransformBinariesInModArchiveAsync(string originalModName, string newModName, List<BinaryName> binariesToDelete, Dictionary<BinaryName, byte[]> newBinaries)
	{
		bool hasAnyBinaryContentChanges = binariesToDelete.Count > 0 || newBinaries.Count > 0;
		if (!hasAnyBinaryContentChanges && originalModName == newModName)
			return false;

		// Determine which binaries to keep.
		Dictionary<BinaryName, byte[]> keptBinaries = new();
		string originalArchivePath = _modArchiveAccessor.GetModArchivePath(originalModName);
		if (File.Exists(originalArchivePath))
		{
			using ZipArchive originalArchive = ZipFile.Open(originalArchivePath, ZipArchiveMode.Read);
			foreach (ZipArchiveEntry entry in originalArchive.Entries)
			{
				// Test if we need to skip (delete) this binary.
				BinaryName binaryNameFromEntry = BinaryName.Parse(entry.Name, originalModName);
				if (binariesToDelete.Contains(binaryNameFromEntry))
					continue;

				byte[] extractedContents = new byte[entry.Length];

				await using Stream entryStream = entry.Open();
				int readBytes = StreamUtils.ForceReadAllBytes(entryStream, extractedContents, 0, extractedContents.Length);
				if (readBytes != extractedContents.Length)
					throw new InvalidOperationException($"Reading all bytes from archived mod binary did not complete. {readBytes} out of {extractedContents.Length} bytes were read.");

				keptBinaries.Add(binaryNameFromEntry, extractedContents);
			}
		}

		List<BinaryName> collisions = keptBinaries.Keys.Where(newBinaries.ContainsKey).ToList();
		if (collisions.Count > 0)
			throw new InvalidModArchiveException($"Cannot append binaries {string.Join(", ", collisions.Select(s => $"'{s}'"))} to mod archive because it already contains binaries with the exact same names. Either request the old binaries to be deleted or rename the new binaries.");

		// Create a dictionary of both kept and new binaries which will be added to the transformed archive.
		Dictionary<BinaryName, byte[]> combinedBinaries = keptBinaries.Concat(newBinaries).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

		// Delete the original archive and process the new combined binaries.
		DeleteModFilesAndClearCache(originalModName);

		await ProcessModBinaryUploadAsync(newModName, combinedBinaries);

		return hasAnyBinaryContentChanges;
	}

	/// <summary>
	/// Deletes the mod archive and the mod archive cache for this mod, and also clears the memory cache.
	/// <b>This method does not delete mod screenshot files</b>.
	/// </summary>
	public void DeleteModFilesAndClearCache(string modName)
	{
		// Delete archive zip file.
		string archivePath = _modArchiveAccessor.GetModArchivePath(modName);
		if (File.Exists(archivePath))
		{
			File.Delete(archivePath);

			// Clear entire memory cache (can't clear individual entries).
			_modArchiveCache.Clear();
		}

		// Clear file cache for this mod.
		string cachePath = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.ModArchiveCache), $"{modName}.json");
		if (File.Exists(cachePath))
			File.Delete(cachePath);
	}
}
