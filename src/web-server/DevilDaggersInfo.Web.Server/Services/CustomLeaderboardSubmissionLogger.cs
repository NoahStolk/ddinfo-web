using DevilDaggersInfo.Common;
using DevilDaggersInfo.Common.Extensions;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using DevilDaggersInfo.Web.Server.Domain.Main.Converters.DomainToApi;
using DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;

namespace DevilDaggersInfo.Web.Server.Services;

public class CustomLeaderboardSubmissionLogger : ICustomLeaderboardSubmissionLogger
{
	private readonly ILogger<CustomLeaderboardSubmissionLogger> _logger;
	private readonly List<CustomLeaderboardHighscoreLog> _highscoreLogs = new();

	private readonly List<string> _validClLogs = new();
	private readonly List<string> _invalidClLogs = new();

	public CustomLeaderboardSubmissionLogger(ILogger<CustomLeaderboardSubmissionLogger> logger)
	{
		_logger = logger;
	}

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

	public void LogNewScore(
		CustomLeaderboardEntity customLeaderboard,
		IDaggerStatCustomEntry customEntry,
		int rank,
		int totalPlayers,
		string playerName,
		string spawnsetName)
	{
		if (customLeaderboard.Spawnset == null)
			_logger.LogError("Spawnset is not included in the custom leaderboard. Defaulting to Survival.");

		CustomLeaderboardDagger dagger = customLeaderboard.DaggerFromStat(customEntry) ?? CustomLeaderboardDagger.Silver;
		string scoreField = GetScoreFieldName(customLeaderboard.RankSorting);
		string scoreValue = GetFormattedScoreValue(customLeaderboard.RankSorting, customEntry);
		_highscoreLogs.Add(new()
		{
			RankValue = $"{rank}/{totalPlayers}",
			Dagger = dagger,
			Message = $"`{playerName}` just entered the `{spawnsetName}` leaderboard!",
			CustomLeaderboardId = customLeaderboard.Id,
			ScoreField = scoreField,
			ScoreValue = scoreValue,
			RankSorting = customLeaderboard.RankSorting,
			SpawnsetGameMode = customLeaderboard.Spawnset?.GameMode ?? SpawnsetGameMode.Survival,
		});
	}

	public void LogHighscore(
		CustomLeaderboardEntity customLeaderboard,
		IDaggerStatCustomEntry customEntry,
		int rank,
		int totalPlayers,
		string playerName,
		string spawnsetName,
		int valueDifference)
	{
		if (customLeaderboard.Spawnset == null)
			_logger.LogError("Spawnset is not included in the custom leaderboard. Defaulting to Survival.");

		CustomLeaderboardDagger dagger = customLeaderboard.DaggerFromStat(customEntry) ?? CustomLeaderboardDagger.Silver;
		string scoreField = GetScoreFieldName(customLeaderboard.RankSorting);
		string scoreValue = GetFormattedScoreValue(customLeaderboard.RankSorting, customEntry);
		string message = $"`{playerName}` just got {GetScoreMessageText(customLeaderboard.RankSorting, customEntry)} on the `{spawnsetName}` leaderboard!";

		_highscoreLogs.Add(new()
		{
			RankValue = $"{rank}/{totalPlayers}",
			Dagger = dagger,
			Message = message,
			CustomLeaderboardId = customLeaderboard.Id,
			ScoreField = scoreField,
			ScoreValue = $"{scoreValue} ({GetFormattedScoreValueDifference(customLeaderboard.RankSorting, valueDifference)})",
			RankSorting = customLeaderboard.RankSorting,
			SpawnsetGameMode = customLeaderboard.Spawnset?.GameMode ?? SpawnsetGameMode.Survival,
		});
	}

	private static string GetScoreMessageText(CustomLeaderboardRankSorting rankSorting, IDaggerStatCustomEntry customEntry)
	{
		string value = GetFormattedScoreValue(rankSorting, customEntry);
		return rankSorting switch
		{
			CustomLeaderboardRankSorting.TimeAsc or CustomLeaderboardRankSorting.TimeDesc => $"{value} seconds",
			CustomLeaderboardRankSorting.GemsDesc => $"{value} gems",
			CustomLeaderboardRankSorting.KillsDesc => $"{value} kills",
			CustomLeaderboardRankSorting.HomingDesc => $"{value} homing",
			_ => "?",
		};
	}

	private static string GetScoreFieldName(CustomLeaderboardRankSorting rankSorting)
	{
		return rankSorting switch
		{
			CustomLeaderboardRankSorting.TimeAsc or CustomLeaderboardRankSorting.TimeDesc => "Time",
			CustomLeaderboardRankSorting.GemsDesc => "Gems",
			CustomLeaderboardRankSorting.KillsDesc => "Kills",
			CustomLeaderboardRankSorting.HomingDesc => "Homing",
			_ => "?",
		};
	}

	private static string GetFormattedScoreValue(CustomLeaderboardRankSorting rankSorting, IDaggerStatCustomEntry customEntry)
	{
		return rankSorting switch
		{
			CustomLeaderboardRankSorting.TimeAsc or CustomLeaderboardRankSorting.TimeDesc => customEntry.Time.ToSecondsTime().ToString(StringFormats.TimeFormat),
			CustomLeaderboardRankSorting.GemsDesc => customEntry.GemsCollected.ToString(),
			CustomLeaderboardRankSorting.KillsDesc => customEntry.EnemiesKilled.ToString(),
			CustomLeaderboardRankSorting.HomingDesc => customEntry.HomingStored.ToString(),
			_ => "?",
		};
	}

	private static string GetFormattedScoreValueDifference(CustomLeaderboardRankSorting rankSorting, int valueDifference)
	{
		return rankSorting switch
		{
			CustomLeaderboardRankSorting.TimeAsc or CustomLeaderboardRankSorting.TimeDesc => valueDifference.ToSecondsTime().ToString("+0.0000;-0.0000;+0.0000"),
			_ => valueDifference.ToString("+0;-0;+0"),
		};
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
