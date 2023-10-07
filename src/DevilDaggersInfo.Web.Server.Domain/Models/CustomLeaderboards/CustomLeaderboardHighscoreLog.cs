using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;

namespace DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

public record CustomLeaderboardHighscoreLog
{
	public required CustomLeaderboardDagger Dagger { get; init; }

	public required CustomLeaderboardRankSorting RankSorting { get; init; }

	public required SpawnsetGameMode SpawnsetGameMode { get; init; }

	public required int CustomLeaderboardId { get; init; }

	public required string Message { get; init; }

	public required string RankValue { get; init; }

	public required string ScoreField { get; init; }

	public required string ScoreValue { get; init; }
}
