namespace DevilDaggersInfo.Api.App.CustomLeaderboards;

public record AddUploadRequestTimestamp
{
	public required double TimeInSeconds { get; init; }

	public required long Timestamp { get; init; }
}
