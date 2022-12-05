namespace DevilDaggersInfo.Api.App.CustomLeaderboards;

public record GetUploadSuccess
{
	public string Message { get; init; } = string.Empty;

	public SubmissionType SubmissionType { get; init; }

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
