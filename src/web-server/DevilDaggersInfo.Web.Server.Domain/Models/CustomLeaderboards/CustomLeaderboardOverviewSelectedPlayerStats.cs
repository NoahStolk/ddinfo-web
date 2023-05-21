namespace DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

public record CustomLeaderboardOverviewSelectedPlayerStats
{
	public required int Rank { get; init; }

	public required double HighscoreValue { get; init; }

	public required CustomLeaderboardDagger? Dagger { get; init; }

	public required CustomLeaderboardOverviewSelectedPlayerNextDagger? NextDagger { get; init; }
}
