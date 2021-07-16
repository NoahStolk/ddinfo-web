using DevilDaggersWebsite.BlazorWasm.Shared.Dto.LeaderboardHistory;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.IO;
using System.Text;

namespace DevilDaggersWebsite.BlazorWasm.Server.Caches.LeaderboardHistory
{
	public class LeaderboardHistoryCache : IDynamicCache
	{
		private readonly ConcurrentDictionary<string, GetLeaderboardHistoryPublic> _cache = new();

		public GetLeaderboardHistoryPublic GetLeaderboardHistoryByFilePath(string filePath)
		{
			string name = Path.GetFileNameWithoutExtension(filePath);
			if (_cache.ContainsKey(name))
				return _cache[name];

			GetLeaderboardHistoryPublic lb = JsonConvert.DeserializeObject<GetLeaderboardHistoryPublic>(File.ReadAllText(filePath, Encoding.UTF8)) ?? throw new($"Corrupt leaderboard history file: {name}");
			_cache.TryAdd(name, lb);
			return lb;
		}

		public void Clear()
			=> _cache.Clear();

		public string LogState()
			=> $"`{_cache.Count}` in memory";
	}
}
