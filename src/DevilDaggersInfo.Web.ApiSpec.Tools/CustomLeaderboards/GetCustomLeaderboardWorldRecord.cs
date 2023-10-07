namespace DevilDaggersInfo.Web.ApiSpec.Tools.CustomLeaderboards;

public record GetCustomLeaderboardWorldRecord
{
	public required double WorldRecordValue { get; init; }

	public required CustomLeaderboardDagger? Dagger { get; init; }
}
