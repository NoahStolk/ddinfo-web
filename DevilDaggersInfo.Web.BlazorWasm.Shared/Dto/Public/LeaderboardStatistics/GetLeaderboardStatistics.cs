namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.LeaderboardStatistics;

public class GetLeaderboardStatistics
{
	public DateTime DateTime { get; init; }
	public bool IsFetched { get; init; }
	public int TotalEntries { get; init; }

	public Dictionary<string, int> DaggerStatistics { get; init; } = null!;
	public Dictionary<string, int> DeathStatistics { get; init; } = null!;
	public Dictionary<string, int> EnemyStatistics { get; init; } = null!;

	public Dictionary<int, int> TimeStatistics { get; init; } = null!;
	public Dictionary<int, int> KillStatistics { get; init; } = null!;
	public Dictionary<int, int> GemStatistics { get; init; } = null!;

	public int PlayersWithLevel1 { get; init; }
	public int PlayersWithLevel2 { get; init; }
	public int PlayersWithLevel3Or4 { get; init; }

	public GetArrayStatistic Time { get; init; } = null!;
	public GetArrayStatistic Kills { get; init; } = null!;
	public GetArrayStatistic Gems { get; init; } = null!;
	public GetArrayStatistic DaggersFired { get; init; } = null!;
	public GetArrayStatistic DaggersHit { get; init; } = null!;
}
