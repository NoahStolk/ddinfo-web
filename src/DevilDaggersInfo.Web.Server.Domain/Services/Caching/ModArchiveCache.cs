using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Models.ModArchives;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using DevilDaggersInfo.Web.Server.Domain.Utils;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.IO.Compression;

namespace DevilDaggersInfo.Web.Server.Domain.Services.Caching;

public sealed class ModArchiveCache(IFileSystem fileSystem)
{
	private readonly ConcurrentDictionary<string, ModArchiveCacheData> _cache = new();

	public int Count => _cache.Count;

	public async Task<ModArchiveCacheData> GetArchiveDataByBytesAsync(string name, byte[] bytes)
	{
		// Check memory cache.
		if (_cache.TryGetValue(name, out ModArchiveCacheData? cachedData))
			return cachedData;

		// Check file cache.
		ModArchiveCacheData? fileCache = await LoadFromFileCacheAsync(name);
		if (fileCache != null)
			return fileCache;

		// Unzip zip file bytes.
		await using MemoryStream ms = new(bytes);
		return CreateModArchiveCacheDataFromStream(name, ms, false); // Do not add this to the cache because it is not yet validated.
	}

	public async Task<ModArchiveCacheData> GetArchiveDataByModNameAsync(string modName)
	{
		// Check memory cache.
		if (_cache.TryGetValue(modName, out ModArchiveCacheData? cachedData))
			return cachedData;

		// Check file cache.
		ModArchiveCacheData? fileCache = await LoadFromFileCacheAsync(modName);
		if (fileCache != null)
			return fileCache;

		// Unzip zip file. TODO: This should only be done manually from the admin pages.
		byte[] bytes = await fileSystem.ReadAllBytesAsync(DataSubDirectory.Mods, ModArchiveAccessor.GetModArchiveFileName(modName)) ?? throw new NotFoundException($"Mod archive for mod '{modName}' could not be found.");
		await using MemoryStream ms = new(bytes);
		ModArchiveCacheData archiveData = CreateModArchiveCacheDataFromStream(modName, ms, true);
		await WriteToFileCacheAsync(modName, archiveData);
		return archiveData;
	}

	private async Task<ModArchiveCacheData?> LoadFromFileCacheAsync(string name)
	{
		string? json = await fileSystem.ReadAllTextAsync(DataSubDirectory.ModArchiveCache, $"{name}.json");
		if (json == null)
			return null;

		ModArchiveCacheData? fileCacheArchiveData = JsonConvert.DeserializeObject<ModArchiveCacheData>(json);
		if (fileCacheArchiveData == null)
			return null;

		// Add to memory cache if present in file cache.
		_cache.TryAdd(name, fileCacheArchiveData);

		return fileCacheArchiveData;
	}

	private ModArchiveCacheData CreateModArchiveCacheDataFromStream(string name, Stream stream, bool addToCache)
	{
		try
		{
			using ZipArchive archive = new(stream);
			ModArchiveCacheData archiveData = new() { FileSize = stream.Length };
			foreach (ZipArchiveEntry entry in archive.Entries)
			{
				if (string.IsNullOrEmpty(entry.Name))
					throw new InvalidModArchiveException("Mod archive must not contain any folders.");

				byte[] extractedContents = new byte[entry.Length];

				using Stream entryStream = entry.Open();
				int readBytes = StreamUtils.ForceReadAllBytes(entryStream, extractedContents, 0, extractedContents.Length);
				if (readBytes != extractedContents.Length)
					throw new InvalidOperationException($"Reading all bytes from archived mod binary did not complete. {readBytes} out of {extractedContents.Length} bytes were read.");

				archiveData.Binaries.Add(ModBinaryCacheData.CreateFromFile(entry.Name, extractedContents));
				archiveData.FileSizeExtracted += entry.Length;
			}

			// Add to memory cache. The caller is responsible for writing the file cache.
			if (addToCache)
				_cache.TryAdd(name, archiveData);

			return archiveData;
		}
		catch (InvalidDataException)
		{
			throw new InvalidModArchiveException("Mod archive must be a valid ZIP file.");
		}
	}

	private async Task WriteToFileCacheAsync(string name, ModArchiveCacheData archiveData)
	{
		await fileSystem.WriteAllTextAsync(DataSubDirectory.ModArchiveCache, $"{name}.json", JsonConvert.SerializeObject(archiveData));
	}

	public async Task LoadEntireFileCacheAsync()
	{
		IReadOnlyList<FileEntry> files = await fileSystem.ListAsync(DataSubDirectory.ModArchiveCache);
		foreach (FileEntry file in files.Where(f => f.Name.EndsWith(".json", StringComparison.Ordinal)))
			await LoadFromFileCacheAsync(Path.GetFileNameWithoutExtension(file.Name));
	}

	public void Clear()
	{
		_cache.Clear();
	}
}
