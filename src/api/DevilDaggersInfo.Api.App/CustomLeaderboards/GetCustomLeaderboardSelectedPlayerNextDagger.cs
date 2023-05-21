namespace DevilDaggersInfo.Api.App.CustomLeaderboards;

public record GetCustomLeaderboardSelectedPlayerNextDagger
{
	[Obsolete("Use DaggerValue instead.")]
	public double Time { get; init; }

	public required double DaggerValue { get; init; }

	public required CustomLeaderboardDagger Dagger { get; init; }
}
