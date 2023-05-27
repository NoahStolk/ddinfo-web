using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Server.Domain.Services.Inversion;

public interface ICustomLeaderboardSubmissionLogger
{
	void Log(bool isValid, string message);

	IReadOnlyList<string> GetLogs(bool valid);

	void ClearLogs(bool valid);

	void LogNewScore(CustomLeaderboardEntity customLeaderboard, IDaggerStatCustomEntry customEntry, int rank, int totalPlayers, string playerName, string spawnsetName);

	void LogHighscore(CustomLeaderboardEntity customLeaderboard, IDaggerStatCustomEntry customEntry, int rank, int totalPlayers, string playerName, string spawnsetName, int valueDifference);

	List<CustomLeaderboardHighscoreLog> GetHighscoreLogs();

	void ClearHighscoreLogs();
}
