using DevilDaggersWebsite.Dto;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;

namespace DevilDaggersWebsite.Caches.LeaderboardHistory
{
	public sealed class LeaderboardHistoryCache : IDynamicCache
	{
		private const string _emote = "yellow_circle";

		private readonly ConcurrentDictionary<string, Leaderboard> _cache = new();

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

			Leaderboard lb = JsonConvert.DeserializeObject<Leaderboard?>(File.ReadAllText(filePath, Encoding.UTF8)) ?? throw new($"Corrupt leaderboard history file: {name}");
			_cache.TryAdd(name, lb);
			return lb;
		}

		public void Clear()
			=> _cache.Clear();

		public string LogState(IWebHostEnvironment env)
			=> $":{_emote}: `{_cache.Count}` in memory";
	}
}
