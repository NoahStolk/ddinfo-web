namespace DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

public record CustomLeaderboardOverviewSelectedPlayerNextDagger
{
	public required int Time { get; init; }

	public required CustomLeaderboardDagger Dagger { get; init; }
}
