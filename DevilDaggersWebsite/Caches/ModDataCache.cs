using DevilDaggersWebsite.Dto;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace DevilDaggersWebsite.Caches
{
	public sealed class ModDataCache
	{
		private readonly ConcurrentDictionary<string, List<ModData>> _cache = new();

		private static readonly Lazy<ModDataCache> _lazy = new(() => new());

		private ModDataCache()
		{
		}

		public static ModDataCache Instance => _lazy.Value;

		public List<ModData> GetModDataByFilePath(string filePath)
		{
			string name = Path.GetFileNameWithoutExtension(filePath);
			if (_cache.ContainsKey(name))
				return _cache[name];

			List<ModData> modData = new();
			using FileStream fs = new(filePath, FileMode.Open);
			using ZipArchive archive = new(fs);
			foreach (ZipArchiveEntry entry in archive.Entries)
			{
				byte[] extractedContents = new byte[entry.Length];

				using Stream stream = entry.Open();
				stream.Read(extractedContents, 0, extractedContents.Length);

				modData.Add(ModData.CreateFromFile(entry.Name, extractedContents));
			}

			_cache.TryAdd(name, modData);
			return modData;
		}

		public void Clear()
			=> _cache.Clear();
	}
}
