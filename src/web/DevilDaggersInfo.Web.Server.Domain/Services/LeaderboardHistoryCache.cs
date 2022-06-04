using DevilDaggersInfo.Web.Server.Domain.Models.LeaderboardHistory;
using System.Collections.Concurrent;

namespace DevilDaggersInfo.Web.Server.Domain.Services;

public class LeaderboardHistoryCache : IDynamicCache
{
	private readonly ConcurrentDictionary<string, LeaderboardHistory> _cache = new();

	public LeaderboardHistory GetLeaderboardHistoryByFilePath(string filePath)
	{
		string name = Path.GetFileNameWithoutExtension(filePath);
		if (_cache.ContainsKey(name))
			return _cache[name];

		LeaderboardHistory lb = LeaderboardHistory.CreateFromFile(File.ReadAllBytes(filePath));
		_cache.TryAdd(name, lb);
		return lb;
	}

	public void Clear()
		=> _cache.Clear();

	public int GetCount()
		=> _cache.Count;
}
