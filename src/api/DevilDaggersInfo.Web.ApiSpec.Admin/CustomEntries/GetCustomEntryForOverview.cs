namespace DevilDaggersInfo.Api.Admin.CustomEntries;

public record GetCustomEntryForOverview : IAdminOverviewGetDto
{
	public required int Id { get; init; }

	public required string SpawnsetName { get; init; }

	public required string PlayerName { get; init; }

	public required double Time { get; init; }

	public required int GemsCollected { get; init; }

	public required int GemsDespawned { get; init; }

	public required int GemsEaten { get; init; }

	public required int GemsTotal { get; init; }

	public required int EnemiesKilled { get; init; }

	public required int EnemiesAlive { get; init; }

	public required int DaggersFired { get; init; }

	public required int DaggersHit { get; init; }

	public required int HomingStored { get; init; }

	public required int HomingEaten { get; init; }

	public required CustomEntryDeathType DeathType { get; init; }

	public required double LevelUpTime2 { get; init; }

	public required double LevelUpTime3 { get; init; }

	public required double LevelUpTime4 { get; init; }

	public required DateTime SubmitDate { get; init; }

	public required string ClientVersion { get; init; }
}
