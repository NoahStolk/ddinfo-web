using DevilDaggersWebsite.BlazorWasm.Server.Clients.Leaderboard;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.IO;
using System.Text;

namespace DevilDaggersWebsite.BlazorWasm.Server.Caches.LeaderboardHistory
{
	public class LeaderboardHistoryCache : IDynamicCache
	{
		private readonly ConcurrentDictionary<string, LeaderboardResponse> _cache = new();

		public LeaderboardResponse GetLeaderboardHistoryByFilePath(string filePath)
		{
			string name = Path.GetFileNameWithoutExtension(filePath);
			if (_cache.ContainsKey(name))
				return _cache[name];

			LeaderboardResponse lb = JsonConvert.DeserializeObject<LeaderboardResponse?>(File.ReadAllText(filePath, Encoding.UTF8)) ?? throw new($"Corrupt leaderboard history file: {name}");
			_cache.TryAdd(name, lb);
			return lb;
		}

		public void Clear()
			=> _cache.Clear();

		public string LogState()
			=> $"`{_cache.Count}` in memory";
	}
}
