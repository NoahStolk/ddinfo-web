using System.Collections.Concurrent;
using OldCore = DevilDaggersCore.Spawnsets;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Caches.SpawnsetData;

public class SpawnsetDataCache : IDynamicCache
{
	private readonly ConcurrentDictionary<string, OldCore.SpawnsetData> _cache = new();

	public OldCore.SpawnsetData GetSpawnsetDataByFilePath(string filePath)
	{
		string name = Path.GetFileNameWithoutExtension(filePath);
		if (_cache.ContainsKey(name))
			return _cache[name];

		if (!OldCore.Spawnset.TryGetSpawnsetData(File.ReadAllBytes(filePath), out OldCore.SpawnsetData spawnsetData))
			throw new($"Failed to get spawn data from spawnset file: '{name}'.");

		_cache.TryAdd(name, spawnsetData);
		return spawnsetData;
	}

	public void Clear()
		=> _cache.Clear();

	public string LogState()
		=> $"`{_cache.Count}` in memory";
}
