namespace DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

public record UploadResponse
{
	public SuccessfulUploadResponse? Success { get; init; }

	public UploadCriteriaRejection? Rejection { get; init; }
}
