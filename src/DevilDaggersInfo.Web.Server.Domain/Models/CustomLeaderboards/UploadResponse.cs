namespace DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

public sealed record UploadResponse
{
	public required CustomLeaderboardSummary Leaderboard { get; init; }

	public SuccessfulUploadResponse? Success { get; init; }

	public UploadCriteriaRejection? Rejection { get; init; }
}
