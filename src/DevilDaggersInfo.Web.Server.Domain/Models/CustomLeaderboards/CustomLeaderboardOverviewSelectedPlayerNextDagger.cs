namespace DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

public sealed record CustomLeaderboardOverviewSelectedPlayerNextDagger
{
	public required double DaggerValue { get; init; }

	public required CustomLeaderboardDagger Dagger { get; init; }
}
