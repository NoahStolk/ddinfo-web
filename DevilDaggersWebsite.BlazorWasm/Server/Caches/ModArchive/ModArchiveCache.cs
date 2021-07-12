using DevilDaggersWebsite.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.IO;
using System.IO.Compression;

namespace DevilDaggersWebsite.BlazorWasm.Server.Caches.ModArchive
{
	public class ModArchiveCache : IDynamicCache
	{
		private readonly object _fileStreamLock = new();

		private readonly ConcurrentDictionary<string, ModArchiveCacheData> _cache = new();

		private readonly IWebHostEnvironment _environment;

		public ModArchiveCache(IWebHostEnvironment environment)
		{
			_environment = environment;
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
			string fileCachePath = Path.Combine(_environment.WebRootPath, "mod-archive-cache", $"{name}.json");
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
			using ZipArchive archive = new(stream);
			ModArchiveCacheData archiveData = new() { FileSize = stream.Length };
			foreach (ZipArchiveEntry entry in archive.Entries)
			{
				if (string.IsNullOrEmpty(entry.Name))
					throw new InvalidModArchiveException("Mod archive must not contain any folders.");

				byte[] extractedContents = new byte[entry.Length];

				using Stream entryStream = entry.Open();
				entryStream.Read(extractedContents, 0, extractedContents.Length);

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

		private void WriteToFileCache(string name, ModArchiveCacheData archiveData)
		{
			try
			{
				string fileCacheDirectory = Path.Combine(_environment.WebRootPath, "mod-archive-cache");
				if (!Directory.Exists(fileCacheDirectory))
					Directory.CreateDirectory(fileCacheDirectory);

				File.WriteAllText(Path.Combine(fileCacheDirectory, $"{name}.json"), JsonConvert.SerializeObject(archiveData));
			}
			catch
			{
				// Ignore exceptions.
			}
		}

		public void LoadEntireFileCache()
		{
			string fileCacheDirectory = Path.Combine(_environment.WebRootPath, "mod-archive-cache");
			if (!Directory.Exists(fileCacheDirectory))
				Directory.CreateDirectory(fileCacheDirectory);

			foreach (string path in Directory.GetFiles(fileCacheDirectory, "*.json"))
			{
				string name = Path.GetFileNameWithoutExtension(path);
				LoadFromFileCache(name);
			}
		}

		public void Clear()
			=> _cache.Clear();

		public string LogState()
		{
			int fileCaches = Directory.GetFiles(Path.Combine(_environment.WebRootPath, "mod-archive-cache")).Length;

			return $"`{_cache.Count}` in memory\n`{fileCaches}` in file system";
		}
	}
}
