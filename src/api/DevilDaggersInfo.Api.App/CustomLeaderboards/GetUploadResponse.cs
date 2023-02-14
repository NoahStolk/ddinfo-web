namespace DevilDaggersInfo.Api.App.CustomLeaderboards;

public record GetUploadResponse
{
	public GetUploadResponseHighscore? Highscore { get; init; }

	public GetUploadResponseNoHighscore? NoHighscore { get; init; }

	public GetUploadResponseFirstScore? FirstScore { get; init; }

	public GetUploadResponseCriteriaRejection? CriteriaRejection { get; init; }

	// TODO: Make required when DDCL 1.8.3 is removed.
	public List<GetCustomEntry>? NewSortedEntries { get; init; }

	// TODO: Make required when DDCL 1.8.3 is removed.
	public bool IsAscending { get; init; }
}
