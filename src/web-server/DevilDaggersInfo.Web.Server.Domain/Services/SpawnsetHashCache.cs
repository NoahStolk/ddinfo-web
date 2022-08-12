using DevilDaggersInfo.Common.Utils;
using DevilDaggersInfo.Core.Spawnset;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Models.Spawnsets;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Security.Cryptography;

namespace DevilDaggersInfo.Web.Server.Domain.Services;

public class SpawnsetHashCache
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
		SpawnsetHashCacheData? spawnsetCacheData = _cache.FirstOrDefault(scd => ArrayUtils.AreEqual(scd.Hash, hash));
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

			if (ArrayUtils.AreEqual(spawnsetHash, hash))
				return spawnsetCacheData;
		}

		return null;
	}

	public void Clear()
		=> _cache.Clear();

	public int GetCount()
		=> _cache.Count;
}
