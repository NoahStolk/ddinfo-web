namespace DevilDaggersInfo.Web.BlazorWasm.Server.Caches.SpawnsetHashes;

public class SpawnsetHashCache : IDynamicCache
{
	private readonly ConcurrentBag<SpawnsetHashCacheData> _cache = new();

	private readonly IFileSystemService _fileSystemService;
	private readonly ILogger<SpawnsetHashCache> _logger;

	public SpawnsetHashCache(IFileSystemService fileSystemService, ILogger<SpawnsetHashCache> logger)
	{
		_fileSystemService = fileSystemService;
		_logger = logger;
	}

	public SpawnsetHashCacheData? GetSpawnset(byte[] hash)
	{
		SpawnsetHashCacheData? spawnsetCacheData = _cache.FirstOrDefault(scd => MatchHashes(scd.Hash, hash));
		if (spawnsetCacheData != null)
			return spawnsetCacheData;

		foreach (string spawnsetPath in Directory.GetFiles(_fileSystemService.GetPath(DataSubDirectory.Spawnsets)))
		{
			byte[] spawnsetBytes = File.ReadAllBytes(spawnsetPath);
			if (!SpawnsetBinary.TryParse(spawnsetBytes, out _))
			{
				_logger.LogError("Could not parse file at `{path}` to a spawnset. Skipping file for cache.", spawnsetPath);
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
