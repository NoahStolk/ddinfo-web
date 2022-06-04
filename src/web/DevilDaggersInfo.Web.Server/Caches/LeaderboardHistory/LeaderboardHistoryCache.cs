using DevilDaggersInfo.Web.Server.Domain.Services;

namespace DevilDaggersInfo.Web.Server.Caches.LeaderboardHistory;

public class LeaderboardHistoryCache : IDynamicCache
{
	private readonly ConcurrentDictionary<string, InternalModels.LeaderboardHistory.LeaderboardHistory> _cache = new();

	public InternalModels.LeaderboardHistory.LeaderboardHistory GetLeaderboardHistoryByFilePath(string filePath)
	{
		string name = Path.GetFileNameWithoutExtension(filePath);
		if (_cache.ContainsKey(name))
			return _cache[name];

		InternalModels.LeaderboardHistory.LeaderboardHistory lb = InternalModels.LeaderboardHistory.LeaderboardHistory.CreateFromFile(File.ReadAllBytes(filePath));
		_cache.TryAdd(name, lb);
		return lb;
	}

	public void Clear()
		=> _cache.Clear();

	public int GetCount()
		=> _cache.Count;
}
