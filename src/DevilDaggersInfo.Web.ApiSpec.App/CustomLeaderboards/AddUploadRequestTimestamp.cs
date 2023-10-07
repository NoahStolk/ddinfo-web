namespace DevilDaggersInfo.Web.ApiSpec.App.CustomLeaderboards;

public record AddUploadRequestTimestamp
{
	public required double TimeInSeconds { get; init; }

	public required long Timestamp { get; init; }
}
