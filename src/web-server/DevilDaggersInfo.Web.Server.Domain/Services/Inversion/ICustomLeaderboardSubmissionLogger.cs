using DevilDaggersInfo.Types.Web;
using DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Server.Domain.Services.Inversion;

public interface ICustomLeaderboardSubmissionLogger
{
	void Log(bool isValid, string message);

	IReadOnlyList<string> GetLogs(bool valid);

	void ClearLogs(bool valid);

	void LogHighscore(CustomLeaderboardDagger dagger, int customLeaderboardId, string message, int rank, int totalPlayers, int time);

	List<CustomLeaderboardHighscoreLog> GetHighscoreLogs();

	void ClearHighscoreLogs();
}
