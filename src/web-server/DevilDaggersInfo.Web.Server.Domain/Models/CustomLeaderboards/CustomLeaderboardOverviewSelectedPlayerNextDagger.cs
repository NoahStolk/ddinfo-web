namespace DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

public record CustomLeaderboardOverviewSelectedPlayerNextDagger
{
	[Obsolete("Use DaggerValue instead.")]
	public int Time { get; init; }

	public required double DaggerValue { get; init; }

	public required CustomLeaderboardDagger Dagger { get; init; }
}
