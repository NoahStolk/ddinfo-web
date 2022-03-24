namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomEntries;

public record GetCustomEntryDdcl
{
	public int Id { get; init; }

	public int PlayerId { get; init; }

	public string PlayerName { get; init; } = null!;

	[Obsolete("Use TimeInSeconds instead.")]
	public int Time { get; init; }

	public double TimeInSeconds { get; init; }

	public int GemsCollected { get; init; }

	public int EnemiesKilled { get; init; }

	public int DaggersFired { get; init; }

	public int DaggersHit { get; init; }

	public int EnemiesAlive { get; init; }

	public int HomingDaggers { get; init; }

	public int HomingDaggersEaten { get; init; }

	public int GemsDespawned { get; init; }

	public int GemsEaten { get; init; }

	public int GemsTotal { get; init; }

	public byte DeathType { get; init; }

	[Obsolete("Use LevelUpTime2InSeconds instead.")]
	public int LevelUpTime2 { get; init; }

	[Obsolete("Use LevelUpTime3InSeconds instead.")]
	public int LevelUpTime3 { get; init; }

	[Obsolete("Use LevelUpTime4InSeconds instead.")]
	public int LevelUpTime4 { get; init; }

	public double LevelUpTime2InSeconds { get; init; }

	public double LevelUpTime3InSeconds { get; init; }

	public double LevelUpTime4InSeconds { get; init; }

	public DateTime SubmitDate { get; init; }

	public string? ClientVersion { get; init; }

	public bool HasReplay { get; init; }
}
