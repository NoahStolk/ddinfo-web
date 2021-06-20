using DevilDaggersDiscordBot;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;
using Core = DevilDaggersCore.Spawnsets;

namespace DevilDaggersWebsite.Caches.SpawnsetData
{
	public sealed class SpawnsetDataCache : IDynamicCache
	{
		private const string _emote = "purple_circle";

		private readonly ConcurrentDictionary<string, Core.SpawnsetData> _cache = new();

		private static readonly Lazy<SpawnsetDataCache> _lazy = new(() => new());

		private SpawnsetDataCache()
		{
		}

		public static SpawnsetDataCache Instance => _lazy.Value;

		public Core.SpawnsetData GetSpawnsetDataByFilePath(string filePath)
		{
			string name = Path.GetFileNameWithoutExtension(filePath);
			if (_cache.ContainsKey(name))
				return _cache[name];

			if (!Core.Spawnset.TryGetSpawnsetData(File.ReadAllBytes(filePath), out Core.SpawnsetData spawnsetData))
				throw new($"Failed to get spawn data from spawnset file: '{name}'.");

			_cache.TryAdd(name, spawnsetData);
			return spawnsetData;
		}

		public async Task Clear(IWebHostEnvironment env)
		{
			int cacheCount = _cache.Count;
			_cache.Clear();
			await DiscordLogger.Instance.TryLog(Channel.CacheMonitoring, env.EnvironmentName, $":{_emote}: Successfully cleared dynamic `{nameof(SpawnsetDataCache)}`. (Removed `{cacheCount}` instances.)");
		}

		public string LogState(IWebHostEnvironment env)
			=> $":{_emote}: `{nameof(SpawnsetDataCache)}` has `{_cache.Count}` instances in memory.";
	}
}
