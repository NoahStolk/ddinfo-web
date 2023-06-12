using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Server.Domain.Services.Inversion;

public interface ICustomLeaderboardHighscoreLogger
{
	void LogNewScore(CustomLeaderboardEntity customLeaderboard, IDaggerStatCustomEntry customEntry, int rank, int totalPlayers, string playerName, string spawnsetName);

	void LogHighscore(CustomLeaderboardEntity customLeaderboard, IDaggerStatCustomEntry customEntry, int rank, int totalPlayers, string playerName, string spawnsetName, int valueDifference);

	List<CustomLeaderboardHighscoreLog> GetHighscoreLogs();

	void ClearHighscoreLogs();
}
