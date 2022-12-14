namespace DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

public class UploadResponse
{
	public required string Message { get; init; }

	public SubmissionType SubmissionType { get; init; }

	public required CustomLeaderboardSummary Leaderboard { get; init; }

	public required List<CustomEntry> SortedEntries { get; init; }

	public UploadResponseScoreState<int> RankState { get; init; }

	public UploadResponseScoreState<double> TimeState { get; init; }

	public UploadResponseScoreState<int> GemsCollectedState { get; init; }

	public UploadResponseScoreState<int> EnemiesKilledState { get; init; }

	public UploadResponseScoreState<int> DaggersFiredState { get; init; }

	public UploadResponseScoreState<int> DaggersHitState { get; init; }

	public UploadResponseScoreState<int> EnemiesAliveState { get; init; }

	public UploadResponseScoreState<int> HomingStoredState { get; init; }

	public UploadResponseScoreState<int> HomingEatenState { get; init; }

	public UploadResponseScoreState<int> GemsDespawnedState { get; init; }

	public UploadResponseScoreState<int> GemsEatenState { get; init; }

	public UploadResponseScoreState<int> GemsTotalState { get; init; }

	public UploadResponseScoreState<double> LevelUpTime2State { get; init; }

	public UploadResponseScoreState<double> LevelUpTime3State { get; init; }

	public UploadResponseScoreState<double> LevelUpTime4State { get; init; }
}
