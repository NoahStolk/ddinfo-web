using DevilDaggersCore.Spawnsets;
using DevilDaggersDiscordBot.Logging;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Caches
{
	public sealed class SpawnsetHashCache
	{
		private readonly List<SpawnsetCacheData> _cache = new();

		private static readonly Lazy<SpawnsetHashCache> _lazy = new(() => new());

		private SpawnsetHashCache()
		{
		}

		public static SpawnsetHashCache Instance => _lazy.Value;

		public async Task<SpawnsetCacheData?> GetSpawnset(IWebHostEnvironment env, byte[] hash)
		{
			SpawnsetCacheData? spawnsetCacheData = _cache.Find(scd => MatchHashes(scd.Hash, hash));
			if (spawnsetCacheData != null)
				return spawnsetCacheData;

			foreach (string spawnsetPath in Directory.GetFiles(Path.Combine(env.WebRootPath, "spawnsets")))
			{
				byte[] spawnsetBytes = File.ReadAllBytes(spawnsetPath);
				if (!Spawnset.TryParse(spawnsetBytes, out _))
				{
					await DiscordLogger.Instance.TryLog(Channel.ErrorMonitoring, $"Could not parse file at `{spawnsetPath}` to a spawnset. Skipping file for cache.");
					continue;
				}

				byte[] spawnsetHash = MD5.HashData(spawnsetBytes);
				string spawnsetName = Path.GetFileName(spawnsetPath);
				spawnsetCacheData = new(spawnsetName, spawnsetHash);
				_cache.Add(spawnsetCacheData);

				if (MatchHashes(spawnsetHash, hash))
				{
					await DiscordLogger.Instance.TryLog(Channel.CustomLeaderboardMonitoring, $"Successfully updated {nameof(SpawnsetHashCache)}. ({_cache.Count} instances in memory).");
					return spawnsetCacheData;
				}
			}

			await DiscordLogger.Instance.TryLog(Channel.CustomLeaderboardMonitoring, $"Successfully updated {nameof(SpawnsetHashCache)} ({_cache.Count} instances in memory).");
			return null;
		}

		public void Clear()
		{
			_cache.Clear();
		}

		private static bool MatchHashes(byte[] a, byte[] b)
		{
			if (a.Length != b.Length)
				return false;

			for (int i = 0; i < 16; i++)
			{
				if (a[i] != b[i])
					return false;
			}

			return true;
		}
	}
}
