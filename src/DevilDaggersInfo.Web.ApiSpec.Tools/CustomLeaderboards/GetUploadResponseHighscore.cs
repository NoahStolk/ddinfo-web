namespace DevilDaggersInfo.Web.ApiSpec.Tools.CustomLeaderboards;

public record GetUploadResponseHighscore
{
	public required GetScoreState<int> RankState { get; init; }

	public required GetScoreState<double> TimeState { get; init; }

	public required GetScoreState<int> GemsCollectedState { get; init; }

	public required GetScoreState<int> EnemiesKilledState { get; init; }

	public required GetScoreState<int> DaggersFiredState { get; init; }

	public required GetScoreState<int> DaggersHitState { get; init; }

	public required GetScoreState<int> EnemiesAliveState { get; init; }

	public required GetScoreState<int> HomingStoredState { get; init; }

	public required GetScoreState<int> HomingEatenState { get; init; }

	public required GetScoreState<int> GemsDespawnedState { get; init; }

	public required GetScoreState<int> GemsEatenState { get; init; }

	public required GetScoreState<int> GemsTotalState { get; init; }

	public required GetScoreState<double> LevelUpTime2State { get; init; }

	public required GetScoreState<double> LevelUpTime3State { get; init; }

	public required GetScoreState<double> LevelUpTime4State { get; init; }
}
