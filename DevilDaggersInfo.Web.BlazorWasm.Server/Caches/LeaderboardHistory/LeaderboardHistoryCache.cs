namespace DevilDaggersInfo.Web.BlazorWasm.Server.Caches.LeaderboardHistory;

public class LeaderboardHistoryCache : IDynamicCache
{
	private readonly ConcurrentDictionary<string, InternalModels.Json.LeaderboardHistory> _cache = new();

	public InternalModels.Json.LeaderboardHistory GetLeaderboardHistoryByFilePath(string filePath)
	{
		string name = Path.GetFileNameWithoutExtension(filePath);
		if (_cache.ContainsKey(name))
			return _cache[name];

		InternalModels.Json.LeaderboardHistory lb = JsonConvert.DeserializeObject<InternalModels.Json.LeaderboardHistory>(File.ReadAllText(filePath, Encoding.UTF8)) ?? throw new($"Corrupt leaderboard history file: {name}");
		_cache.TryAdd(name, lb);
		return lb;
	}

	public void Clear()
		=> _cache.Clear();

	public string LogState()
		=> $"`{_cache.Count}` in memory";
}
