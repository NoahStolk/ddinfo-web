namespace DevilDaggersInfo.Web.ApiSpec.Main.LeaderboardStatistics;

public record GetLeaderboardStatistics
{
	public required DateTime DateTime { get; init; }

	public required bool IsFetched { get; init; }

	public required int TotalEntries { get; init; }

	public required Dictionary<string, int> DaggersStatistics { get; init; }

	public required Dictionary<string, int> DeathsStatistics { get; init; }

	public required Dictionary<string, int> EnemiesStatistics { get; init; }

	public required Dictionary<int, int> TimesStatistics { get; init; }

	public required Dictionary<int, int> KillsStatistics { get; init; }

	public required Dictionary<int, int> GemsStatistics { get; init; }

	public required Dictionary<int, int> DaggersFiredStatistics { get; init; }

	public required Dictionary<int, int> DaggersHitStatistics { get; init; }

	public required int PlayersWithLevel1 { get; init; }

	public required int PlayersWithLevel2 { get; init; }

	public required int PlayersWithLevel3Or4 { get; init; }

	public required GetArrayStatistics GlobalStatistics { get; init; }
}
