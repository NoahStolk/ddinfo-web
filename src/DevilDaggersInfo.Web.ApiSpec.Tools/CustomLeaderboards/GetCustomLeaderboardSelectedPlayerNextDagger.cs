namespace DevilDaggersInfo.Web.ApiSpec.Tools.CustomLeaderboards;

public record GetCustomLeaderboardSelectedPlayerNextDagger
{
	public required double DaggerValue { get; init; }

	public required CustomLeaderboardDagger Dagger { get; init; }
}
