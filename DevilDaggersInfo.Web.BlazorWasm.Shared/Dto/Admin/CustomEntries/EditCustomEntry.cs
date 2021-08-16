namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.CustomEntries;

public class EditCustomEntry
{
	public int CustomLeaderboardId { get; init; }

	public int PlayerId { get; init; }

	public int Time { get; init; }

	public int GemsCollected { get; init; }

	public int GemsDespawned { get; init; }

	public int GemsEaten { get; init; }

	public int GemsTotal { get; init; }

	public int EnemiesKilled { get; init; }

	public int EnemiesAlive { get; init; }

	public int DaggersFired { get; init; }

	public int DaggersHit { get; init; }

	public int HomingStored { get; init; }

	public int HomingEaten { get; init; }

	public CustomEntryDeathType DeathType { get; init; }

	public int LevelUpTime2 { get; init; }

	public int LevelUpTime3 { get; init; }

	public int LevelUpTime4 { get; init; }

	public DateTime SubmitDate { get; init; }

	[StringLength(16)]
	public string ClientVersion { get; init; } = null!;
}
