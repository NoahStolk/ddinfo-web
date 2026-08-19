namespace DevilDaggersInfo.Web.ApiSpec.Tools.CustomLeaderboards;

public sealed record AddUploadRequestTimestamp
{
	public required double TimeInSeconds { get; init; }

	public required long Timestamp { get; init; }
}
