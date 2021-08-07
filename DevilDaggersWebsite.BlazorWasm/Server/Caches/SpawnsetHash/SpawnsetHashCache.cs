using DevilDaggersCore.Spawnsets;
using DevilDaggersWebsite.BlazorWasm.Server.Enumerators;
using DevilDaggersWebsite.BlazorWasm.Server.HostedServices.DdInfoDiscordBot;
using DevilDaggersWebsite.BlazorWasm.Server.Singletons;
using DevilDaggersWebsite.BlazorWasm.Server.Transients;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.BlazorWasm.Server.Caches.SpawnsetHash
{
	public class SpawnsetHashCache : IDynamicCache
	{
		private readonly ConcurrentBag<SpawnsetHashCacheData> _cache = new();

		private readonly IFileSystemService _fileSystemService;
		private readonly DiscordLogger _discordLogger;

		public SpawnsetHashCache(IFileSystemService fileSystemService, DiscordLogger discordLogger)
		{
			_fileSystemService = fileSystemService;
			_discordLogger = discordLogger;
		}

		public async Task<SpawnsetHashCacheData?> GetSpawnset(byte[] hash)
		{
			SpawnsetHashCacheData? spawnsetCacheData = _cache.FirstOrDefault(scd => MatchHashes(scd.Hash, hash));
			if (spawnsetCacheData != null)
				return spawnsetCacheData;

			foreach (string spawnsetPath in Directory.GetFiles(_fileSystemService.GetPath(DataSubDirectory.Spawnsets)))
			{
				byte[] spawnsetBytes = File.ReadAllBytes(spawnsetPath);
				if (!Spawnset.TryParse(spawnsetBytes, out _))
				{
					await _discordLogger.TryLog(Channel.MonitoringError, $":x: Could not parse file at `{spawnsetPath}` to a spawnset. Skipping file for cache.");
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
