namespace DevilDaggersInfo.Web.ApiSpec.Tools.CustomLeaderboards;

public record GetCustomEntry
{
	public required int Id { get; init; }

	public required int Rank { get; init; }

	public required int PlayerId { get; init; }

	public required string PlayerName { get; init; }

	public required double TimeInSeconds { get; init; }

	public required int GemsCollected { get; init; }

	public required int? GemsDespawned { get; init; }

	public required int? GemsEaten { get; init; }

	public required int? GemsTotal { get; init; }

	public required int EnemiesAlive { get; init; }

	public required int EnemiesKilled { get; init; }

	public required int DaggersFired { get; init; }

	public required int DaggersHit { get; init; }

	public required int HomingStored { get; init; }

	public required int? HomingEaten { get; init; }

	public required byte DeathType { get; init; }

	public required double LevelUpTime2InSeconds { get; init; }

	public required double LevelUpTime3InSeconds { get; init; }

	public required double LevelUpTime4InSeconds { get; init; }

	public required DateTime SubmitDate { get; init; }

	public required bool HasReplay { get; init; }

	public required CustomLeaderboardDagger? CustomLeaderboardDagger { get; init; }
}
