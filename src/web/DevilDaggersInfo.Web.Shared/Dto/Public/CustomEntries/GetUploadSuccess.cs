using DevilDaggersInfo.Web.Shared.Dto.Public.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Shared.Dto.Public.CustomEntries;

public record GetUploadSuccess
{
	public string Message { get; init; } = string.Empty;

	public int TotalPlayers { get; init; }

	public GetCustomLeaderboardDdcl Leaderboard { get; init; } = null!;

	public CustomLeaderboardCategory Category { get; init; }

	public List<GetCustomEntryDdcl> Entries { get; init; } = new();

	public bool IsNewPlayerOnThisLeaderboard { get; init; }

	public bool IsHighscore { get; init; }

	public GetScoreState<int> RankState { get; init; }

	public GetScoreState<double> TimeState { get; init; }

	public GetScoreState<int> GemsCollectedState { get; init; }

	public GetScoreState<int> EnemiesKilledState { get; init; }

	public GetScoreState<int> DaggersFiredState { get; init; }

	public GetScoreState<int> DaggersHitState { get; init; }

	public GetScoreState<int> EnemiesAliveState { get; init; }

	public GetScoreState<int> HomingStoredState { get; init; }

	public GetScoreState<int> HomingEatenState { get; init; }

	public GetScoreState<int> GemsDespawnedState { get; init; }

	public GetScoreState<int> GemsEatenState { get; init; }

	public GetScoreState<int> GemsTotalState { get; init; }

	public GetScoreState<double> LevelUpTime2State { get; init; }

	public GetScoreState<double> LevelUpTime3State { get; init; }

	public GetScoreState<double> LevelUpTime4State { get; init; }
}
