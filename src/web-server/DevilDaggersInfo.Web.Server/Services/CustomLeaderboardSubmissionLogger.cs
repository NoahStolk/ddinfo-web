using DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;

namespace DevilDaggersInfo.Web.Server.Services;

public class CustomLeaderboardSubmissionLogger : ICustomLeaderboardSubmissionLogger
{
	private readonly List<CustomLeaderboardHighscoreLog> _highscoreLogs = new();

	private readonly List<string> _validClLogs = new();
	private readonly List<string> _invalidClLogs = new();

	public IReadOnlyList<string> GetLogs(bool valid)
		=> valid ? _validClLogs : _invalidClLogs;

	public void ClearLogs(bool valid)
	{
		if (valid)
			_validClLogs.Clear();
		else
			_invalidClLogs.Clear();
	}

	public void Log(bool isValid, string message)
	{
		if (isValid)
			_validClLogs.Add(message);
		else
			_invalidClLogs.Add(message);
	}

	public void LogHighscore(CustomLeaderboardDagger dagger, int customLeaderboardId, string message, int rank, int totalPlayers, int time)
	{
		_highscoreLogs.Add(new()
		{
			Rank = rank,
			Dagger = dagger,
			Message = message,
			Time = time,
			TotalPlayers = totalPlayers,
			CustomLeaderboardId = customLeaderboardId,
		});
	}

	public List<CustomLeaderboardHighscoreLog> GetHighscoreLogs()
	{
		return _highscoreLogs;
	}

	public void ClearHighscoreLogs()
	{
		_highscoreLogs.Clear();
	}
}
