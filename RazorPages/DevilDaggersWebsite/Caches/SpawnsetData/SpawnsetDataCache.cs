using System.Collections.Concurrent;
using System.IO;
using Core = DevilDaggersCore.Spawnsets;

namespace DevilDaggersWebsite.Caches.SpawnsetData
{
	public class SpawnsetDataCache : IDynamicCache
	{
		private readonly ConcurrentDictionary<string, Core.SpawnsetData> _cache = new();

		public Core.SpawnsetData GetSpawnsetDataByFilePath(string filePath)
		{
			string name = Path.GetFileNameWithoutExtension(filePath);
			if (_cache.ContainsKey(name))
				return _cache[name];

			if (!Core.Spawnset.TryGetSpawnsetData(File.ReadAllBytes(filePath), out Core.SpawnsetData spawnsetData))
				throw new($"Failed to get spawn data from spawnset file: '{name}'.");

			_cache.TryAdd(name, spawnsetData);
			return spawnsetData;
		}

		public void Clear()
			=> _cache.Clear();

		public string LogState()
			=> $"`{_cache.Count}` in memory";
	}
}
