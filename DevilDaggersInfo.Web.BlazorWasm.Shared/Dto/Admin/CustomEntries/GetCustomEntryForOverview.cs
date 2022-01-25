namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.CustomEntries;

public class GetCustomEntryForOverview : IGetDto
{
	public int Id { get; init; }

	[Display(Name = "Spawnset")]
	public string SpawnsetName { get; init; } = null!;

	[Display(Name = "Player")]
	public string PlayerName { get; init; } = null!;

	[Format(FormatUtils.TimeFormat)]
	public double Time { get; init; }

	[Display(Name = "Gems")]
	public int GemsCollected { get; init; }

	[Display(Name = "GemsDesp")]
	public int GemsDespawned { get; init; }

	public int GemsEaten { get; init; }

	public int GemsTotal { get; init; }

	[Display(Name = "Kills")]
	public int EnemiesKilled { get; init; }

	[Display(Name = "Alive")]
	public int EnemiesAlive { get; init; }

	[Display(Name = "Fired")]
	public int DaggersFired { get; init; }

	[Display(Name = "Hit")]
	public int DaggersHit { get; init; }

	[Display(Name = "Homing")]
	public int HomingStored { get; init; }

	public int HomingEaten { get; init; }

	[Display(Name = "Death")]
	public CustomEntryDeathType DeathType { get; init; }

	[Format(FormatUtils.TimeFormat)]
	[Display(Name = "Level2")]
	public double LevelUpTime2 { get; init; }

	[Format(FormatUtils.TimeFormat)]
	[Display(Name = "Level3")]
	public double LevelUpTime3 { get; init; }

	[Format(FormatUtils.TimeFormat)]
	[Display(Name = "Level4")]
	public double LevelUpTime4 { get; init; }

	public DateTime SubmitDate { get; init; }

	[Display(Name = "Version")]
	public string ClientVersion { get; init; } = null!;
}
