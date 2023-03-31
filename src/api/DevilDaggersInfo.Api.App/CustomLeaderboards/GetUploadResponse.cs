namespace DevilDaggersInfo.Api.App.CustomLeaderboards;

public record GetUploadResponse
{
	public required int SpawnsetId { get; init; }

	public required string SpawnsetName { get; init; }

	public required int CustomLeaderboardId { get; init; }

	public GetUploadResponseHighscore? Highscore { get; init; }

	public GetUploadResponseNoHighscore? NoHighscore { get; init; }

	public GetUploadResponseFirstScore? FirstScore { get; init; }

	public GetUploadResponseCriteriaRejection? CriteriaRejection { get; init; }

	public required List<GetCustomEntry>? NewSortedEntries { get; init; }

	public required bool IsAscending { get; init; }
}
