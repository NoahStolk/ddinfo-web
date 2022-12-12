using DevilDaggersInfo.Web.Server.Domain.Models.LeaderboardHistory;

namespace DevilDaggersInfo.Web.Server.Domain.Services.Caching;

public interface ILeaderboardHistoryCache
{
	LeaderboardHistory GetLeaderboardHistoryByFilePath(string filePath);

	int GetCount();

	void Clear();
}
