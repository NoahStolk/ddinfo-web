using DevilDaggersDiscordBot.Logging;
using DevilDaggersWebsite.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Caches.ModArchive
{
	public sealed class ModArchiveCache : IDynamicCache
	{
		private const string _emote = "green_circle";

		private readonly object _fileStreamLock = new();

		private readonly ConcurrentDictionary<string, ModArchiveCacheData> _cache = new();

		private static readonly Lazy<ModArchiveCache> _lazy = new(() => new());

		private ModArchiveCache()
		{
		}

		public static ModArchiveCache Instance => _lazy.Value;

		public ModArchiveCacheData GetArchiveDataByBytes(IWebHostEnvironment env, string name, byte[] bytes)
		{
			// Check memory cache.
			if (_cache.ContainsKey(name))
				return _cache[name];

			// Check file cache.
			ModArchiveCacheData? fileCache = LoadFromFileCache(env, name);
			if (fileCache != null)
				return fileCache;

			// Unzip zip file bytes.
			using MemoryStream ms = new(bytes);
			return CreateModArchiveCacheDataFromStream(env, name, ms);
		}

		public ModArchiveCacheData GetArchiveDataByFilePath(IWebHostEnvironment env, string filePath)
		{
			// Check memory cache.
			string name = Path.GetFileNameWithoutExtension(filePath);
			if (_cache.ContainsKey(name))
				return _cache[name];

			// Check file cache.
			ModArchiveCacheData? fileCache = LoadFromFileCache(env, name);
			if (fileCache != null)
				return fileCache;

			// Unzip zip file.
			lock (_fileStreamLock)
			{
				using FileStream fs = new(filePath, FileMode.Open);
				return CreateModArchiveCacheDataFromStream(env, name, fs);
			}
		}

		private ModArchiveCacheData? LoadFromFileCache(IWebHostEnvironment env, string name)
		{
			string fileCachePath = Path.Combine(env.WebRootPath, "mod-archive-cache", $"{name}.json");
			if (!File.Exists(fileCachePath))
				return null;

			ModArchiveCacheData? fileCacheArchiveData = JsonConvert.DeserializeObject<ModArchiveCacheData>(File.ReadAllText(fileCachePath));
			if (fileCacheArchiveData == null)
				return null;

			// Add to memory cache if present in file cache.
			_cache.TryAdd(name, fileCacheArchiveData);

			return fileCacheArchiveData;
		}

		private ModArchiveCacheData CreateModArchiveCacheDataFromStream(IWebHostEnvironment env, string name, Stream stream)
		{
			using ZipArchive archive = new(stream);
			ModArchiveCacheData archiveData = new() { FileSize = stream.Length };
			foreach (ZipArchiveEntry entry in archive.Entries)
			{
				if (string.IsNullOrEmpty(entry.Name))
					throw new InvalidModBinaryException("Zip archive must not contain any folders.");

				byte[] extractedContents = new byte[entry.Length];

				using Stream entryStream = entry.Open();
				entryStream.Read(extractedContents, 0, extractedContents.Length);

				archiveData.Binaries.Add(ModBinaryCacheData.CreateFromFile(entry.Name, extractedContents));
				archiveData.FileSizeExtracted += entry.Length;
			}

			// Add to memory cache and file cache.
			_cache.TryAdd(name, archiveData);
			WriteToFileCache(env, name, archiveData);

			return archiveData;
		}

		private static void WriteToFileCache(IWebHostEnvironment env, string name, ModArchiveCacheData archiveData)
		{
			try
			{
				string fileCacheDirectory = Path.Combine(env.WebRootPath, "mod-archive-cache");
				if (!Directory.Exists(fileCacheDirectory))
					Directory.CreateDirectory(fileCacheDirectory);

				File.WriteAllText(Path.Combine(fileCacheDirectory, $"{name}.json"), JsonConvert.SerializeObject(archiveData));
			}
			catch
			{
				// Ignore exceptions.
			}
		}

		public void LoadEntireFileCache(IWebHostEnvironment env)
		{
			string fileCacheDirectory = Path.Combine(env.WebRootPath, "mod-archive-cache");
			if (!Directory.Exists(fileCacheDirectory))
				Directory.CreateDirectory(fileCacheDirectory);

			foreach (string path in Directory.GetFiles(fileCacheDirectory, "*.json"))
			{
				string name = Path.GetFileNameWithoutExtension(path);
				LoadFromFileCache(env, name);
			}
		}

		public List<(string ArchiveName, string BinaryName)> GetExistingBinaryNames()
		{
			List<(string ArchiveName, string BinaryName)> binaryNames = new();
			foreach (KeyValuePair<string, ModArchiveCacheData> c in _cache)
			{
				foreach (ModBinaryCacheData b in c.Value.Binaries)
					binaryNames.Add((c.Key, b.Name));
			}

			return binaryNames;
		}

		public async Task Clear(IWebHostEnvironment env)
		{
			int cacheCount = _cache.Count;
			_cache.Clear();
			await DiscordLogger.Instance.TryLog(Channel.CacheMonitoring, env.EnvironmentName, $":{_emote}: Successfully cleared dynamic `{nameof(ModArchiveCache)}`. (Removed `{cacheCount}` instances. File cache is not affected.)");
		}

		public string LogState(IWebHostEnvironment env)
		{
			int fileCaches = Directory.GetFiles(Path.Combine(env.WebRootPath, "mod-archive-cache")).Length;

			return $":{_emote}: `{nameof(ModArchiveCache)}` has `{_cache.Count}` instances in memory and `{fileCaches}` instances in file system.";
		}
	}
}
