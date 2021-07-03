using DevilDaggersCore.Spawnsets;
using DevilDaggersWebsite.HostedServices.DdInfoDiscordBot;
using DevilDaggersWebsite.Singletons;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Caches.SpawnsetHash
{
	public class SpawnsetHashCache : IDynamicCache
	{
		private readonly ConcurrentBag<SpawnsetHashCacheData> _cache = new();

		private readonly DiscordLogger _discordLogger;

		public SpawnsetHashCache(DiscordLogger discordLogger)
		{
			_discordLogger = discordLogger;
		}

		public async Task<SpawnsetHashCacheData?> GetSpawnset(IWebHostEnvironment env, byte[] hash)
		{
			SpawnsetHashCacheData? spawnsetCacheData = _cache.FirstOrDefault(scd => MatchHashes(scd.Hash, hash));
			if (spawnsetCacheData != null)
				return spawnsetCacheData;

			foreach (string spawnsetPath in Directory.GetFiles(Path.Combine(env.WebRootPath, "spawnsets")))
			{
				byte[] spawnsetBytes = File.ReadAllBytes(spawnsetPath);
				if (!Spawnset.TryParse(spawnsetBytes, out _))
				{
					await _discordLogger.TryLog(Channel.MonitoringError, env.EnvironmentName, $":x: Could not parse file at `{spawnsetPath}` to a spawnset. Skipping file for cache.");
					continue;
				}

				byte[] spawnsetHash = MD5.HashData(spawnsetBytes);
				string spawnsetName = Path.GetFileName(spawnsetPath);
				spawnsetCacheData = new(spawnsetName, spawnsetHash);

				if (!_cache.Any(scd => scd.Name == spawnsetName))
					_cache.Add(spawnsetCacheData);

				if (MatchHashes(spawnsetHash, hash))
					return spawnsetCacheData;
			}

			return null;

			static bool MatchHashes(byte[] a, byte[] b)
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

		public void Clear()
			=> _cache.Clear();

		public string LogState()
			=> $"`{_cache.Count}` in memory";
	}
}
