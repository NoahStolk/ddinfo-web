using DevilDaggersCore.Spawnsets;
using System;
using System.Collections.Generic;
using System.IO;

namespace DevilDaggersWebsite.Caches
{
	public sealed class SpawnsetDataCache
	{
		private readonly Dictionary<string, SpawnsetData> _cache = new();

		private static readonly Lazy<SpawnsetDataCache> _lazy = new(() => new());

		private SpawnsetDataCache()
		{
		}

		public static SpawnsetDataCache Instance => _lazy.Value;

		public SpawnsetData GetSpawnsetDataByFilePath(string filePath)
		{
			string name = Path.GetFileNameWithoutExtension(filePath);
			if (_cache.ContainsKey(name))
				return _cache[name];

			if (!Spawnset.TryGetSpawnsetData(File.ReadAllBytes(filePath), out SpawnsetData spawnsetData))
				throw new($"Failed to get spawn data from spawnset file: '{name}'.");

			_cache.Add(name, spawnsetData);
			return spawnsetData;
		}

		public void Clear()
			=> _cache.Clear();
	}
}
