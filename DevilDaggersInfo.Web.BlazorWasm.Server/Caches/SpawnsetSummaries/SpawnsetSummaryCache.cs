using DevilDaggersInfo.Core.Spawnset.Summary;
using System.Collections.Concurrent;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Caches.SpawnsetSummaries;

public class SpawnsetSummaryCache : IDynamicCache
{
	private readonly ConcurrentDictionary<string, SpawnsetSummary> _cache = new();

	public SpawnsetSummary GetSpawnsetSummaryByFilePath(string filePath)
	{
		string name = Path.GetFileNameWithoutExtension(filePath);
		if (_cache.ContainsKey(name))
			return _cache[name];

		if (!SpawnsetSummary.TryParse(File.ReadAllBytes(filePath), out SpawnsetSummary? spawnsetSummary))
			throw new($"Failed to get spawnset summary from spawnset file: '{name}'.");

		_cache.TryAdd(name, spawnsetSummary);
		return spawnsetSummary;
	}

	public void Clear()
		=> _cache.Clear();

	public string LogState()
		=> $"`{_cache.Count}` in memory";
}
