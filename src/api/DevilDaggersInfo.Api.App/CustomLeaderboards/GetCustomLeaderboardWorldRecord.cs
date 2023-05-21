namespace DevilDaggersInfo.Api.App.CustomLeaderboards;

public record GetCustomLeaderboardWorldRecord
{
	[Obsolete("Use WorldRecordValue instead.")]
	public double Time { get; init; }

	public required double WorldRecordValue { get; init; }

	public required CustomLeaderboardDagger? Dagger { get; init; }
}
