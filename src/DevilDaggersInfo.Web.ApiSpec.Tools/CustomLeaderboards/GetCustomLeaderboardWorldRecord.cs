namespace DevilDaggersInfo.Web.ApiSpec.Tools.CustomLeaderboards;

public sealed record GetCustomLeaderboardWorldRecord
{
	public required double WorldRecordValue { get; init; }

	public required CustomLeaderboardDagger? Dagger { get; init; }
}
