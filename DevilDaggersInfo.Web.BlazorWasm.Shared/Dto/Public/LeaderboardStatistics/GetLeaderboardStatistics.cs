namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.LeaderboardStatistics;

public class GetLeaderboardStatistics
{
	public DateTime DateTime { get; init; }
	public bool IsFetched { get; init; }
	public int TotalEntries { get; init; }

	public Dictionary<string, int> DaggersStatistics { get; init; } = null!;
	public Dictionary<string, int> DeathsStatistics { get; init; } = null!;
	public Dictionary<string, int> EnemiesStatistics { get; init; } = null!;

	public Dictionary<int, int> TimesStatistics { get; init; } = null!;
	public Dictionary<int, int> KillsStatistics { get; init; } = null!;
	public Dictionary<int, int> GemsStatistics { get; init; } = null!;
	public Dictionary<int, int> DaggersFiredStatistics { get; init; } = null!;
	public Dictionary<int, int> DaggersHitStatistics { get; init; } = null!;

	public int PlayersWithLevel1 { get; init; }
	public int PlayersWithLevel2 { get; init; }
	public int PlayersWithLevel3Or4 { get; init; }

	public GetArrayStatistics GlobalStatistics { get; init; } = null!;
}
