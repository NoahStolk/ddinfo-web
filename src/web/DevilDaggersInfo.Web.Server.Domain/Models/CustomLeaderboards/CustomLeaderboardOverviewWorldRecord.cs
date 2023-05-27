namespace DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

public record CustomLeaderboardOverviewWorldRecord : IDaggerStatCustomEntry
{
	public required double WorldRecordValue { get; init; }

	public required int Time { get; init; }

	public required int GemsCollected { get; init; }

	public required int GemsDespawned { get; init; }

	public required int GemsEaten { get; init; }

	public required int EnemiesKilled { get; init; }

	public required int EnemiesAlive { get; init; }

	public required int HomingStored { get; init; }

	public required int HomingEaten { get; init; }

	public required int PlayerId { get; init; }

	public required string PlayerName { get; init; }

	public required CustomLeaderboardDagger? Dagger { get; init; }
}
