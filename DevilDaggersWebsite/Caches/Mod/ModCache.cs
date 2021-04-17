using DevilDaggersDiscordBot.Logging;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Caches.Mod
{
	public sealed class ModCache
	{
		private readonly ConcurrentDictionary<string, List<Dto.ModData>> _cache = new();

		private static readonly Lazy<ModCache> _lazy = new(() => new());

		private ModCache()
		{
		}

		public static ModCache Instance => _lazy.Value;

		public List<Dto.ModData> GetModDataByFilePath(string filePath)
		{
			string name = Path.GetFileNameWithoutExtension(filePath);
			if (_cache.ContainsKey(name))
				return _cache[name];

			List<Dto.ModData> modData = new();
			using FileStream fs = new(filePath, FileMode.Open);
			using ZipArchive archive = new(fs);
			foreach (ZipArchiveEntry entry in archive.Entries)
			{
				byte[] extractedContents = new byte[entry.Length];

				using Stream stream = entry.Open();
				stream.Read(extractedContents, 0, extractedContents.Length);

				modData.Add(Dto.ModData.CreateFromFile(entry.Name, extractedContents));
			}

			_cache.TryAdd(name, modData);
			return modData;
		}

		public async Task Clear(IWebHostEnvironment env)
		{
			int cacheCount = _cache.Count;
			_cache.Clear();
			await DiscordLogger.Instance.TryLog(Channel.CacheMonitoring, env.EnvironmentName, $"Successfully cleared `{nameof(ModCache)}`. (Removed `{cacheCount}` instances.)");
		}
	}
}
