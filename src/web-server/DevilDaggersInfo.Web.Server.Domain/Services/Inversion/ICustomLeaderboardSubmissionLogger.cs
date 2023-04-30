using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Server.Domain.Services.Inversion;

public interface ICustomLeaderboardSubmissionLogger
{
	void Log(bool isValid, string message);

	IReadOnlyList<string> GetLogs(bool valid);

	void ClearLogs(bool valid);

	void LogHighscore(CustomLeaderboardEntity customLeaderboard, CustomEntryEntity customEntry, bool isNewScore, int rank, int totalPlayers);

	List<CustomLeaderboardHighscoreLog> GetHighscoreLogs();

	void ClearHighscoreLogs();
}
