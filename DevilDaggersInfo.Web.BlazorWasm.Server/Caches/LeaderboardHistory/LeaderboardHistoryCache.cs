using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.LeaderboardHistory;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Text;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Caches.LeaderboardHistory;

public class LeaderboardHistoryCache : IDynamicCache
{
	private readonly ConcurrentDictionary<string, GetLeaderboardHistory> _cache = new();

	public GetLeaderboardHistory GetLeaderboardHistoryByFilePath(string filePath)
	{
		string name = Path.GetFileNameWithoutExtension(filePath);
		if (_cache.ContainsKey(name))
			return _cache[name];

		GetLeaderboardHistory lb = JsonConvert.DeserializeObject<GetLeaderboardHistory>(File.ReadAllText(filePath, Encoding.UTF8)) ?? throw new($"Corrupt leaderboard history file: {name}");
		_cache.TryAdd(name, lb);
		return lb;
	}

	public void Clear()
		=> _cache.Clear();

	public string LogState()
		=> $"`{_cache.Count}` in memory";
}
