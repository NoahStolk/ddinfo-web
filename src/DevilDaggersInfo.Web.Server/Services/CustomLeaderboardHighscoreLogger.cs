using DevilDaggersInfo.Core.Common;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;

namespace DevilDaggersInfo.Web.Server.Services;

public class CustomLeaderboardHighscoreLogger : ICustomLeaderboardHighscoreLogger
{
	private readonly ILogger<CustomLeaderboardHighscoreLogger> _logger;
	private readonly List<CustomLeaderboardHighscoreLog> _highscoreLogs = [];

	public CustomLeaderboardHighscoreLogger(ILogger<CustomLeaderboardHighscoreLogger> logger)
	{
		_logger = logger;
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

		CustomLeaderboardDagger? dagger = customLeaderboard.DaggerFromStat(customEntry);
		string scoreField = GetScoreFieldName(customLeaderboard.RankSorting);
		string scoreValue = GetFormattedScoreValue(customLeaderboard.RankSorting, customEntry);
		_highscoreLogs.Add(new CustomLeaderboardHighscoreLog
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

		CustomLeaderboardDagger? dagger = customLeaderboard.DaggerFromStat(customEntry);
		string scoreField = GetScoreFieldName(customLeaderboard.RankSorting);
		string scoreValue = GetFormattedScoreValue(customLeaderboard.RankSorting, customEntry);
		string message = $"`{playerName}` just got {GetScoreMessageText(customLeaderboard.RankSorting, customEntry)} on the `{spawnsetName}` leaderboard!";

		_highscoreLogs.Add(new CustomLeaderboardHighscoreLog
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
			CustomLeaderboardRankSorting.GemsCollectedAsc or CustomLeaderboardRankSorting.GemsCollectedDesc => $"{value} gems",
			CustomLeaderboardRankSorting.GemsDespawnedAsc or CustomLeaderboardRankSorting.GemsDespawnedDesc => $"{value} gems despawned",
			CustomLeaderboardRankSorting.GemsEatenAsc or CustomLeaderboardRankSorting.GemsEatenDesc => $"{value} gems eaten",
			CustomLeaderboardRankSorting.EnemiesKilledAsc or CustomLeaderboardRankSorting.EnemiesKilledDesc => $"{value} kills",
			CustomLeaderboardRankSorting.EnemiesAliveAsc or CustomLeaderboardRankSorting.EnemiesAliveDesc => $"{value} enemies alive",
			CustomLeaderboardRankSorting.HomingStoredAsc or CustomLeaderboardRankSorting.HomingStoredDesc => $"{value} homing",
			CustomLeaderboardRankSorting.HomingEatenAsc or CustomLeaderboardRankSorting.HomingEatenDesc => $"{value} homing eaten",
			_ => "?",
		};
	}

	private static string GetScoreFieldName(CustomLeaderboardRankSorting rankSorting)
	{
		return rankSorting switch
		{
			CustomLeaderboardRankSorting.TimeAsc or CustomLeaderboardRankSorting.TimeDesc => "Time",
			CustomLeaderboardRankSorting.GemsCollectedAsc or CustomLeaderboardRankSorting.GemsCollectedDesc => "Gems",
			CustomLeaderboardRankSorting.GemsDespawnedAsc or CustomLeaderboardRankSorting.GemsDespawnedDesc => "Gems Despawned",
			CustomLeaderboardRankSorting.GemsEatenAsc or CustomLeaderboardRankSorting.GemsEatenDesc => "Gems Eaten",
			CustomLeaderboardRankSorting.EnemiesKilledAsc or CustomLeaderboardRankSorting.EnemiesKilledDesc => "Kills",
			CustomLeaderboardRankSorting.EnemiesAliveAsc or CustomLeaderboardRankSorting.EnemiesAliveDesc => "Enemies Alive",
			CustomLeaderboardRankSorting.HomingStoredAsc or CustomLeaderboardRankSorting.HomingStoredDesc => "Homing",
			CustomLeaderboardRankSorting.HomingEatenAsc or CustomLeaderboardRankSorting.HomingEatenDesc => "Homing Eaten",
			_ => "?",
		};
	}

	private static string GetFormattedScoreValue(CustomLeaderboardRankSorting rankSorting, IDaggerStatCustomEntry customEntry)
	{
		return rankSorting switch
		{
			CustomLeaderboardRankSorting.TimeAsc or CustomLeaderboardRankSorting.TimeDesc => GameTime.FromGameUnits(customEntry.Time).Seconds.ToString(StringFormats.TimeFormat),
			CustomLeaderboardRankSorting.GemsCollectedAsc or CustomLeaderboardRankSorting.GemsCollectedDesc => customEntry.GemsCollected.ToString(),
			CustomLeaderboardRankSorting.GemsDespawnedAsc or CustomLeaderboardRankSorting.GemsDespawnedDesc => customEntry.GemsDespawned.ToString(),
			CustomLeaderboardRankSorting.GemsEatenAsc or CustomLeaderboardRankSorting.GemsEatenDesc => customEntry.GemsEaten.ToString(),
			CustomLeaderboardRankSorting.EnemiesKilledAsc or CustomLeaderboardRankSorting.EnemiesKilledDesc => customEntry.EnemiesKilled.ToString(),
			CustomLeaderboardRankSorting.EnemiesAliveAsc or CustomLeaderboardRankSorting.EnemiesAliveDesc => customEntry.EnemiesAlive.ToString(),
			CustomLeaderboardRankSorting.HomingStoredAsc or CustomLeaderboardRankSorting.HomingStoredDesc => customEntry.HomingStored.ToString(),
			CustomLeaderboardRankSorting.HomingEatenAsc or CustomLeaderboardRankSorting.HomingEatenDesc => customEntry.HomingEaten.ToString(),
			_ => "?",
		};
	}

	private static string GetFormattedScoreValueDifference(CustomLeaderboardRankSorting rankSorting, int valueDifference)
	{
		return rankSorting switch
		{
			CustomLeaderboardRankSorting.TimeAsc or CustomLeaderboardRankSorting.TimeDesc => GameTime.FromGameUnits(valueDifference).Seconds.ToString("+0.0000;-0.0000;+0.0000"),
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
