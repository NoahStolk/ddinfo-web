using DevilDaggersDiscordBot.Logging;
using DevilDaggersWebsite.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
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
			if (_cache.ContainsKey(name))
				return _cache[name];

			using MemoryStream ms = new(bytes);
			ModArchiveCacheData archiveData = CreateModArchiveCacheDataFromStream(ms);

			_cache.TryAdd(name, archiveData);
			WriteToFileCache(env, name, archiveData);
			return archiveData;
		}

		public ModArchiveCacheData GetArchiveDataByFilePath(IWebHostEnvironment env, string filePath)
		{
			string name = Path.GetFileNameWithoutExtension(filePath);
			if (_cache.ContainsKey(name))
				return _cache[name];

			lock (_fileStreamLock)
			{
				using FileStream fs = new(filePath, FileMode.Open);
				ModArchiveCacheData archiveData = CreateModArchiveCacheDataFromStream(fs);

				_cache.TryAdd(name, archiveData);
				WriteToFileCache(env, name, archiveData);
				return archiveData;
			}
		}

		private static ModArchiveCacheData CreateModArchiveCacheDataFromStream(Stream stream)
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

			return archiveData;
		}

		public void LoadEntireFileCache(IWebHostEnvironment env)
		{
			string fileCacheDirectory = Path.Combine(env.WebRootPath, "mod-archive-cache");
			if (!Directory.Exists(fileCacheDirectory))
				Directory.CreateDirectory(fileCacheDirectory);

			foreach (string path in Directory.GetFiles(fileCacheDirectory, "*.json"))
			{
				ModArchiveCacheData? archiveData = JsonConvert.DeserializeObject<ModArchiveCacheData>(File.ReadAllText(path));
				if (archiveData == null)
					continue;

				string name = Path.GetFileNameWithoutExtension(path);
				_cache.TryAdd(name, archiveData);
			}
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

		public async Task Clear(IWebHostEnvironment env)
		{
			int cacheCount = _cache.Count;
			_cache.Clear();
			await DiscordLogger.Instance.TryLog(Channel.CacheMonitoring, env.EnvironmentName, $":{_emote}: Successfully cleared dynamic `{nameof(ModArchiveCache)}`. (Removed `{cacheCount}` instances.)");
		}

		public string LogState()
			=> $":{_emote}: `{nameof(ModArchiveCache)}` has `{_cache.Count}` instances in memory.";
	}
}
