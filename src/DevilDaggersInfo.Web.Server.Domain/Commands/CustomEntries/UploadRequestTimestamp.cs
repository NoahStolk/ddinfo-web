namespace DevilDaggersInfo.Web.Server.Domain.Commands.CustomEntries;

public sealed record UploadRequestTimestamp
{
	public required double TimeInSeconds { get; init; }

	public required long Timestamp { get; init; }
}
