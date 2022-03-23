using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomLeaderboards;

namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomEntries;

public class GetUploadSuccess
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

	[Obsolete("Use RankState instead.")]
	public int Rank { get; init; }

	[Obsolete("Use RankState instead.")]
	public int RankDiff { get; init; }

	[Obsolete("Use TimeState instead.")]
	public int Time { get; init; }

	[Obsolete("Use TimeState instead.")]
	public int TimeDiff { get; init; }

	[Obsolete("Use GemsCollectedState instead.")]
	public int GemsCollected { get; init; }

	[Obsolete("Use GemsCollectedState instead.")]
	public int GemsCollectedDiff { get; init; }

	[Obsolete("Use EnemiesKilledState instead.")]
	public int EnemiesKilled { get; init; }

	[Obsolete("Use EnemiesKilledState instead.")]
	public int EnemiesKilledDiff { get; init; }

	[Obsolete("Use DaggersFiredState instead.")]
	public int DaggersFired { get; init; }

	[Obsolete("Use DaggersFiredState instead.")]
	public int DaggersFiredDiff { get; init; }

	[Obsolete("Use DaggersHitState instead.")]
	public int DaggersHit { get; init; }

	[Obsolete("Use DaggersHitState instead.")]
	public int DaggersHitDiff { get; init; }

	[Obsolete("Use EnemiesAliveState instead.")]
	public int EnemiesAlive { get; init; }

	[Obsolete("Use EnemiesAliveState instead.")]
	public int EnemiesAliveDiff { get; init; }

	[Obsolete("Use HomingDaggersStoredState instead.")]
	public int HomingDaggers { get; init; }

	[Obsolete("Use HomingDaggersStoredState instead.")]
	public int HomingDaggersDiff { get; init; }

	[Obsolete("Use HomingDaggersEatenState instead.")]
	public int HomingDaggersEaten { get; init; }

	[Obsolete("Use HomingDaggersEatenState instead.")]
	public int HomingDaggersEatenDiff { get; init; }

	[Obsolete("Use GemsDespawnedState instead.")]
	public int GemsDespawned { get; init; }

	[Obsolete("Use GemsDespawnedState instead.")]
	public int GemsDespawnedDiff { get; init; }

	[Obsolete("Use GemsEatenState instead.")]
	public int GemsEaten { get; init; }

	[Obsolete("Use GemsEatenState instead.")]
	public int GemsEatenDiff { get; init; }

	[Obsolete("Use GemsTotalState instead.")]
	public int GemsTotal { get; init; }

	[Obsolete("Use GemsTotalState instead.")]
	public int GemsTotalDiff { get; init; }

	[Obsolete("Use LevelUpTime2State instead.")]
	public int LevelUpTime2 { get; init; }

	[Obsolete("Use LevelUpTime2State instead.")]
	public int LevelUpTime2Diff { get; init; }

	[Obsolete("Use LevelUpTime3State instead.")]
	public int LevelUpTime3 { get; init; }

	[Obsolete("Use LevelUpTime3State instead.")]
	public int LevelUpTime3Diff { get; init; }

	[Obsolete("Use LevelUpTime4State instead.")]
	public int LevelUpTime4 { get; init; }

	[Obsolete("Use LevelUpTime4State instead.")]
	public int LevelUpTime4Diff { get; init; }
}
