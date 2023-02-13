namespace DevilDaggersInfo.Api.App.CustomLeaderboards;

public record GetUploadResponseRejection
{
	public required string Reason { get; init; }
}
