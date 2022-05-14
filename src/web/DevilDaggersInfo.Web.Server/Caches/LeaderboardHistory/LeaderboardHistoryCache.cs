namespace DevilDaggersInfo.Web.Server.Caches.LeaderboardHistory;

public class LeaderboardHistoryCache : IDynamicCache
{
	private readonly ConcurrentDictionary<string, InternalModels.LeaderboardHistory> _cache = new();

	public InternalModels.LeaderboardHistory GetLeaderboardHistoryByFilePath(string filePath)
	{
		string name = Path.GetFileNameWithoutExtension(filePath);
		if (_cache.ContainsKey(name))
			return _cache[name];

		InternalModels.LeaderboardHistory lb = InternalModels.LeaderboardHistory.CreateFromFile(File.ReadAllBytes(filePath));
		_cache.TryAdd(name, lb);
		return lb;
	}

	public void Clear()
		=> _cache.Clear();

	public int GetCount()
		=> _cache.Count;
}
