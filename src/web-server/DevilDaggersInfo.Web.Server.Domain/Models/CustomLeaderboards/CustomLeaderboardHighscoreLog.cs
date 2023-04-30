namespace DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

public record CustomLeaderboardHighscoreLog
{
	public required CustomLeaderboardDagger Dagger { get; init; }

	public required int CustomLeaderboardId { get; init; }

	public required string Message { get; init; }

	public required string RankValue { get; init; }

	public required string ScoreField { get; init; }

	public required string ScoreValue { get; init; }
}
