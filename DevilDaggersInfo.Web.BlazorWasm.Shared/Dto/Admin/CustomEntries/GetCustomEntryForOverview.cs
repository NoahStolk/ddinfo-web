namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.CustomEntries;

public class GetCustomEntryForOverview : IAdminOverviewGetDto
{
	public int Id { get; init; }

	public string SpawnsetName { get; init; } = null!;

	public string PlayerName { get; init; } = null!;

	[Format(FormatUtils.TimeFormat)]
	public double Time { get; init; }

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

	[Format(FormatUtils.TimeFormat)]
	public double LevelUpTime2 { get; init; }

	[Format(FormatUtils.TimeFormat)]
	public double LevelUpTime3 { get; init; }

	[Format(FormatUtils.TimeFormat)]
	public double LevelUpTime4 { get; init; }

	public DateTime SubmitDate { get; init; }

	public string ClientVersion { get; init; } = null!;
}
