namespace DevilDaggersInfo.Api.App.CustomLeaderboards;

public record GetCustomLeaderboardWorldRecord
{
	public required double WorldRecordValue { get; init; }

	public required CustomLeaderboardDagger? Dagger { get; init; }
}
