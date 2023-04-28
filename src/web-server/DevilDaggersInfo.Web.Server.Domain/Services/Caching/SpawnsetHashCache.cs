using DevilDaggersInfo.Common.Utils;
using DevilDaggersInfo.Core.Spawnset;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Models.Spawnsets;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Security.Cryptography;

namespace DevilDaggersInfo.Web.Server.Domain.Services.Caching;

public class SpawnsetHashCache
{
	private readonly ConcurrentBag<SpawnsetHashCacheData> _cache = new();

	private readonly ApplicationDbContext _dbContext;

	public SpawnsetHashCache(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<SpawnsetHashCacheData?> GetSpawnsetAsync(byte[] hash)
	{
		SpawnsetHashCacheData? spawnsetCacheData = _cache.FirstOrDefault(scd => ArrayUtils.AreEqual(scd.Hash, hash));
		if (spawnsetCacheData != null)
			return spawnsetCacheData;

		var spawnsetEntities = await _dbContext.Spawnsets
			.Select(s => new { s.Md5Hash, s.Name })
			.ToListAsync();

		foreach (var spawnsetEntity in spawnsetEntities)
		{
			spawnsetCacheData = new()
			{
				Hash = spawnsetEntity.Md5Hash,
				Name = spawnsetEntity.Name,
			};

			if (_cache.All(scd => scd.Name != spawnsetEntity.Name))
				_cache.Add(spawnsetCacheData);

			if (ArrayUtils.AreEqual(spawnsetEntity.Md5Hash, hash))
				return spawnsetCacheData;
		}

		return null;
	}

	public void Clear()
		=> _cache.Clear();

	public int GetCount()
		=> _cache.Count;
}
