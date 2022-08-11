using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;

public record GetUploadSuccess
{
	public string Message { get; init; } = string.Empty;

	public SubmissionType SubmissionType { get; init; }

	[Obsolete("Will be removed.")] // Used by DDCL 1.8.3
	public string SpawnsetName { get; init; } = string.Empty;

	[Obsolete("Will be removed.")] // Used by DDCL 1.8.3
	public CustomLeaderboardCategory Category { get; init; }

	[Obsolete("Will be removed.")] // Used by DDCL 1.8.3
	public int TotalPlayers { get; init; }

	[Obsolete("Will be removed.")] // Used by DDCL 1.8.3
	public GetCustomLeaderboardDdcl Leaderboard { get; init; } = null!;

	[Obsolete("Will be removed.")] // Used by DDCL 1.8.3
	public List<GetCustomEntryDdcl> Entries { get; init; } = new();

	[Obsolete("Will be removed.")] // Used by DDCL 1.8.3
	public bool IsNewPlayerOnThisLeaderboard { get; init; }

	[Obsolete("Will be removed.")] // Used by DDCL 1.8.3
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
