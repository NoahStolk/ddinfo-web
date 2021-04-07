using DevilDaggersWebsite.Dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DevilDaggersWebsite.Caches
{
	public sealed class LeaderboardHistoryCache
	{
		private readonly Dictionary<string, Leaderboard> _cache = new();

		private static readonly Lazy<LeaderboardHistoryCache> _lazy = new(() => new());

		private LeaderboardHistoryCache()
		{
		}

		public static LeaderboardHistoryCache Instance => _lazy.Value;

		public Leaderboard GetLeaderboardHistoryByFilePath(string filePath)
		{
			string name = Path.GetFileNameWithoutExtension(filePath);
			if (_cache.ContainsKey(name))
				return _cache[name];

			Leaderboard lb = JsonConvert.DeserializeObject<Leaderboard>(File.ReadAllText(filePath, Encoding.UTF8));
			_cache.Add(name, lb);
			return lb;
		}

		public void Clear()
			=> _cache.Clear();
	}
}
