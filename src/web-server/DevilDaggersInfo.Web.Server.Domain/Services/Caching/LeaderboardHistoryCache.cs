using DevilDaggersInfo.Web.Server.Domain.Models.LeaderboardHistory;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using System.Collections.Concurrent;

namespace DevilDaggersInfo.Web.Server.Domain.Services.Caching;

public class LeaderboardHistoryCache : ILeaderboardHistoryCache
{
	private readonly ConcurrentDictionary<string, LeaderboardHistory> _cache = new();

	private readonly IFileSystemService _fileSystemService;

	public LeaderboardHistoryCache(IFileSystemService fileSystemService)
	{
		_fileSystemService = fileSystemService;
	}

	public LeaderboardHistory GetLeaderboardHistoryByFilePath(string filePath)
	{
		string name = Path.GetFileNameWithoutExtension(filePath);
		if (_cache.TryGetValue(name, out LeaderboardHistory? value))
			return value;

		LeaderboardHistory lb = LeaderboardHistory.CreateFromFile(_fileSystemService.ReadAllBytes(filePath));
		_cache.TryAdd(name, lb);
		return lb;
	}

	public void Clear()
		=> _cache.Clear();

	public int GetCount()
		=> _cache.Count;
}
