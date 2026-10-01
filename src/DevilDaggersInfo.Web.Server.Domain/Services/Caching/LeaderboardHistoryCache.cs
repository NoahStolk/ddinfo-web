using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Models.LeaderboardHistory;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using System.Collections.Concurrent;

namespace DevilDaggersInfo.Web.Server.Domain.Services.Caching;

public sealed class LeaderboardHistoryCache(IFileSystem fileSystem) : ILeaderboardHistoryCache
{
	private readonly ConcurrentDictionary<string, LeaderboardHistory> _cache = new();

	public async Task<LeaderboardHistory> GetLeaderboardHistoryAsync(string fileName)
	{
		if (_cache.TryGetValue(fileName, out LeaderboardHistory? value))
			return value;

		byte[] bytes = await fileSystem.ReadAllBytesAsync(DataSubDirectory.LeaderboardHistory, fileName) ?? throw new NotFoundException($"Leaderboard history file '{fileName}' could not be found.");
		LeaderboardHistory lb = LeaderboardHistory.CreateFromFile(bytes);
		_cache.TryAdd(fileName, lb);
		return lb;
	}

	public void Clear()
	{
		_cache.Clear();
	}

	public int GetCount()
	{
		return _cache.Count;
	}
}
