using DevilDaggersInfo.Common;
using DevilDaggersInfo.Common.Extensions;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
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

	public void LogHighscore(CustomLeaderboardEntity customLeaderboard, CustomEntryEntity customEntry, bool isNewScore, int rank, int totalPlayers)
	{
		CustomLeaderboardDagger dagger = customLeaderboard.DaggerFromStat(customEntry) ?? CustomLeaderboardDagger.Silver;
		string scoreField = customLeaderboard.RankSorting switch
		{
			CustomLeaderboardRankSorting.TimeAsc or CustomLeaderboardRankSorting.TimeDesc => "Time",
			CustomLeaderboardRankSorting.GemsDesc => "Gems",
			CustomLeaderboardRankSorting.KillsDesc => "Kills",
			CustomLeaderboardRankSorting.HomingDesc => "Homing",
			_ => "?",
		};

		string scoreValue = customLeaderboard.RankSorting switch
		{
			CustomLeaderboardRankSorting.TimeAsc or CustomLeaderboardRankSorting.TimeDesc => customEntry.Time.ToSecondsTime().ToString(StringFormats.TimeFormat),
			CustomLeaderboardRankSorting.GemsDesc => customEntry.GemsCollected.ToString("0"),
			CustomLeaderboardRankSorting.KillsDesc => customEntry.EnemiesKilled.ToString("0"),
			CustomLeaderboardRankSorting.HomingDesc => customEntry.HomingStored.ToString("0"),
			_ => "?",
		};

		// TODO: Pass these values as parameters.
		const string playerName = "TEMP1";
		const string spawnsetName = "TEMP2";

		// TODO: Pass the old entry as a parameter.
		const int timeDiff = 10000;

		// TODO: Use rank sorting to determine the message text.
		string message;
		if (isNewScore)
			message = $"`{playerName}` just entered the `{spawnsetName}` leaderboard!";
		else
			message = $"`{playerName}` just got {FormatTimeString(customEntry.Time.ToSecondsTime())} seconds on the `{spawnsetName}` leaderboard, beating their previous highscore of {FormatTimeString((customEntry.Time - timeDiff).ToSecondsTime())} by {FormatTimeString(Math.Abs(timeDiff.ToSecondsTime()))} seconds!";

		_highscoreLogs.Add(new()
		{
			RankValue = $"{rank}/{totalPlayers}",
			Dagger = dagger,
			Message = message,
			CustomLeaderboardId = customLeaderboard.Id,
			ScoreField = scoreField,
			ScoreValue = scoreValue,
		});

		static string FormatTimeString(double time)
			=> time.ToString(StringFormats.TimeFormat);
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
