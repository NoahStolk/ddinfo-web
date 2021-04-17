using DevilDaggersCore.Spawnsets;
using DevilDaggersDiscordBot.Logging;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Caches.SpawnsetHash
{
	public sealed class SpawnsetHashCache
	{
		private readonly ConcurrentBag<SpawnsetHashCacheData> _cache = new();

		private static readonly Lazy<SpawnsetHashCache> _lazy = new(() => new());

		private SpawnsetHashCache()
		{
		}

		public static SpawnsetHashCache Instance => _lazy.Value;

		public async Task<SpawnsetHashCacheData?> GetSpawnset(IWebHostEnvironment env, byte[] hash)
		{
			SpawnsetHashCacheData? spawnsetCacheData = _cache.FirstOrDefault(scd => MatchHashes(scd.Hash, hash));
			if (spawnsetCacheData != null)
				return spawnsetCacheData;

			int cacheCount = _cache.Count;
			foreach (string spawnsetPath in Directory.GetFiles(Path.Combine(env.WebRootPath, "spawnsets")))
			{
				byte[] spawnsetBytes = File.ReadAllBytes(spawnsetPath);
				if (!Spawnset.TryParse(spawnsetBytes, out _))
				{
					await DiscordLogger.Instance.TryLog(Channel.ErrorMonitoring, env.EnvironmentName, $"Could not parse file at `{spawnsetPath}` to a spawnset. Skipping file for cache.");
					continue;
				}

				byte[] spawnsetHash = MD5.HashData(spawnsetBytes);
				string spawnsetName = Path.GetFileName(spawnsetPath);
				spawnsetCacheData = new(spawnsetName, spawnsetHash);

				if (!_cache.Any(scd => scd.Name == spawnsetName))
					_cache.Add(spawnsetCacheData);

				if (MatchHashes(spawnsetHash, hash))
				{
					await LogCacheInfo();
					return spawnsetCacheData;
				}
			}

			await LogCacheInfo();
			return null;

			async Task LogCacheInfo()
			{
				if (_cache.Count > cacheCount)
					await DiscordLogger.Instance.TryLog(Channel.CacheMonitoring, env.EnvironmentName, $"Successfully updated `{nameof(SpawnsetHashCache)}`. (`{_cache.Count}` (`+{_cache.Count - cacheCount}`) instances in memory.)");
			}
		}

		public async Task Clear(IWebHostEnvironment env)
		{
			int cacheCount = _cache.Count;
			_cache.Clear();
			await DiscordLogger.Instance.TryLog(Channel.CacheMonitoring, env.EnvironmentName, $"Successfully cleared `{nameof(SpawnsetHashCache)}`. (Removed `{cacheCount}` instances.)");
		}

		private static bool MatchHashes(byte[] a, byte[] b)
		{
			if (a.Length != b.Length)
				return false;

			for (int i = 0; i < a.Length; i++)
			{
				if (a[i] != b[i])
					return false;
			}

			return true;
		}
	}
}
