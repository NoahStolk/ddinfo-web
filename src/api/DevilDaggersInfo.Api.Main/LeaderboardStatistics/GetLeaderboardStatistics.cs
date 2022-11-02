namespace DevilDaggersInfo.Api.Main.LeaderboardStatistics;

public record GetLeaderboardStatistics
{
	public DateTime DateTime { get; init; }

	public bool IsFetched { get; init; }

	public int TotalEntries { get; init; }

	public required Dictionary<string, int> DaggersStatistics { get; init; }

	public required Dictionary<string, int> DeathsStatistics { get; init; }

	public required Dictionary<string, int> EnemiesStatistics { get; init; }

	public required Dictionary<int, int> TimesStatistics { get; init; }

	public required Dictionary<int, int> KillsStatistics { get; init; }

	public required Dictionary<int, int> GemsStatistics { get; init; }

	public required Dictionary<int, int> DaggersFiredStatistics { get; init; }

	public required Dictionary<int, int> DaggersHitStatistics { get; init; }

	public int PlayersWithLevel1 { get; init; }

	public int PlayersWithLevel2 { get; init; }

	public int PlayersWithLevel3Or4 { get; init; }

	public required GetArrayStatistics GlobalStatistics { get; init; }
}
