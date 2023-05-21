namespace DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

public record CustomLeaderboardOverviewSelectedPlayerNextDagger
{
	public required double DaggerValue { get; init; }

	public required CustomLeaderboardDagger Dagger { get; init; }
}
