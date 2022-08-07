using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Models.ModArchives;
using DevilDaggersInfo.Web.Server.Domain.Utils;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.IO.Compression;

namespace DevilDaggersInfo.Web.Server.Domain.Services;

public class ModArchiveCache : IDynamicCache
{
	private readonly object _fileStreamLock = new();

	private readonly ConcurrentDictionary<string, ModArchiveCacheData> _cache = new();

	private readonly IFileSystemService _fileSystemService;

	public ModArchiveCache(IFileSystemService fileSystemService)
	{
		_fileSystemService = fileSystemService;
	}

	public ModArchiveCacheData GetArchiveDataByBytes(string name, byte[] bytes)
	{
		// Check memory cache.
		if (_cache.ContainsKey(name))
			return _cache[name];

		// Check file cache.
		ModArchiveCacheData? fileCache = LoadFromFileCache(name);
		if (fileCache != null)
			return fileCache;

		// Unzip zip file bytes.
		using MemoryStream ms = new(bytes);
		return CreateModArchiveCacheDataFromStream(name, ms, false); // Do not add this to the cache because it is not yet validated.
	}

	public ModArchiveCacheData GetArchiveDataByFilePath(string filePath)
	{
		// Check memory cache.
		string name = Path.GetFileNameWithoutExtension(filePath);
		if (_cache.ContainsKey(name))
			return _cache[name];

		// Check file cache.
		ModArchiveCacheData? fileCache = LoadFromFileCache(name);
		if (fileCache != null)
			return fileCache;

		// Unzip zip file.
		lock (_fileStreamLock)
		{
			using FileStream fs = new(filePath, FileMode.Open);
			return CreateModArchiveCacheDataFromStream(name, fs, true);
		}
	}

	private ModArchiveCacheData? LoadFromFileCache(string name)
	{
		string fileCachePath = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.ModArchiveCache), $"{name}.json");
		if (!File.Exists(fileCachePath))
			return null;

		ModArchiveCacheData? fileCacheArchiveData = JsonConvert.DeserializeObject<ModArchiveCacheData>(File.ReadAllText(fileCachePath));
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

			if (addToCache)
			{
				// Add to memory cache and file cache.
				_cache.TryAdd(name, archiveData);
				WriteToFileCache(name, archiveData);
			}

			return archiveData;
		}
		catch (InvalidDataException)
		{
			throw new InvalidModArchiveException("Mod archive must be a valid ZIP file.");
		}
	}

	private void WriteToFileCache(string name, ModArchiveCacheData archiveData)
	{
		string fileCacheDirectory = _fileSystemService.GetPath(DataSubDirectory.ModArchiveCache);
		Directory.CreateDirectory(fileCacheDirectory);

		File.WriteAllText(Path.Combine(fileCacheDirectory, $"{name}.json"), JsonConvert.SerializeObject(archiveData));
	}

	public void LoadEntireFileCache()
	{
		string fileCacheDirectory = _fileSystemService.GetPath(DataSubDirectory.ModArchiveCache);
		Directory.CreateDirectory(fileCacheDirectory);

		foreach (string path in Directory.GetFiles(fileCacheDirectory, "*.json"))
		{
			string name = Path.GetFileNameWithoutExtension(path);
			LoadFromFileCache(name);
		}
	}

	public void Clear()
		=> _cache.Clear();

	public int GetCount()
		=> _cache.Count;
}
