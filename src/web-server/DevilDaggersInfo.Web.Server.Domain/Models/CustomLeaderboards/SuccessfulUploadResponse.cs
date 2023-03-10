namespace DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

public class SuccessfulUploadResponse
{
	// TODO: Remove when DDCL 1.8.3 is removed.
	[Obsolete("Use SubmissionType instead.")]
	public required string Message { get; init; }

	public required SubmissionType SubmissionType { get; init; }

	public required List<CustomEntry> SortedEntries { get; init; }

	public UploadResponseScoreState<int> RankState { get; init; }

	public required UploadResponseScoreState<double> TimeState { get; init; }

	public required UploadResponseScoreState<int> GemsCollectedState { get; init; }

	public required UploadResponseScoreState<int> EnemiesKilledState { get; init; }

	public required UploadResponseScoreState<int> DaggersFiredState { get; init; }

	public required UploadResponseScoreState<int> DaggersHitState { get; init; }

	public required UploadResponseScoreState<int> EnemiesAliveState { get; init; }

	public required UploadResponseScoreState<int> HomingStoredState { get; init; }

	public required UploadResponseScoreState<int> HomingEatenState { get; init; }

	public required UploadResponseScoreState<int> GemsDespawnedState { get; init; }

	public required UploadResponseScoreState<int> GemsEatenState { get; init; }

	public required UploadResponseScoreState<int> GemsTotalState { get; init; }

	public required UploadResponseScoreState<double> LevelUpTime2State { get; init; }

	public required UploadResponseScoreState<double> LevelUpTime3State { get; init; }

	public required UploadResponseScoreState<double> LevelUpTime4State { get; init; }
}
