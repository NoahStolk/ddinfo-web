namespace DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;

public record GetCustomLeaderboardWorldRecord
{
	public double Time { get; init; }

	public CustomLeaderboardDagger? Dagger { get; init; }
}
