namespace DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;

[Obsolete("Use GetCustomEntry instead.")]
public record GetCustomEntryDdcl
{
	public int Id { get; init; }

	public int PlayerId { get; init; }

	public required string PlayerName { get; init; }

	public double TimeInSeconds { get; init; }

	public int GemsCollected { get; init; }

	public int EnemiesKilled { get; init; }

	public int DaggersFired { get; init; }

	public int DaggersHit { get; init; }

	public int EnemiesAlive { get; init; }

	public int HomingStored { get; init; }

	public int HomingEaten { get; init; }

	public int GemsDespawned { get; init; }

	public int GemsEaten { get; init; }

	public int GemsTotal { get; init; }

	public byte DeathType { get; init; }

	public double LevelUpTime2InSeconds { get; init; }

	public double LevelUpTime3InSeconds { get; init; }

	public double LevelUpTime4InSeconds { get; init; }

	public DateTime SubmitDate { get; init; }

	public string? ClientVersion { get; init; }

	public bool HasReplay { get; init; }
}
