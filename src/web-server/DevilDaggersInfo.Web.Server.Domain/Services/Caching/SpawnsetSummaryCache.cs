using DevilDaggersInfo.Core.Spawnset.Summary;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace DevilDaggersInfo.Web.Server.Domain.Services.Caching;

public class SpawnsetSummaryCache
{
	private readonly IServiceScopeFactory _serviceScopeFactory;
	private readonly ConcurrentDictionary<int, SpawnsetSummary> _cache = new();

	public SpawnsetSummaryCache(IServiceScopeFactory serviceScopeFactory)
	{
		_serviceScopeFactory = serviceScopeFactory;
	}

	public SpawnsetSummary GetSpawnsetSummaryById(int spawnsetId)
	{
		if (_cache.TryGetValue(spawnsetId, out SpawnsetSummary? summary))
			return summary;

		using IServiceScope scope = _serviceScopeFactory.CreateScope();
		using ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
		var spawnset = dbContext.Spawnsets.Select(s => new { s.Id, s.Name, s.File }).FirstOrDefault(s => s.Id == spawnsetId);
		if (spawnset == null)
			throw new($"Spawnset with ID '{spawnsetId}' not found.");

		if (!SpawnsetSummary.TryParse(spawnset.File, out SpawnsetSummary? spawnsetSummary))
			throw new($"Failed to get spawnset summary from spawnset '{spawnset.Name}'.");

		_cache.TryAdd(spawnsetId, spawnsetSummary);
		return spawnsetSummary;
	}

	public void Clear()
		=> _cache.Clear();

	public int GetCount()
		=> _cache.Count;
}
