using DevilDaggersInfo.Common;
using DevilDaggersInfo.Web.Server.Domain.Commands.CustomEntries;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;

namespace DevilDaggersInfo.Web.Server.Services;

public class CustomLeaderboardSubmissionLogger : ICustomLeaderboardSubmissionLogger
{
	private readonly List<string> _validClLogs = new();
	private readonly List<string> _invalidClLogs = new();

	public IReadOnlyList<string> GetLogs(bool valid)
	{
		return valid ? _validClLogs : _invalidClLogs;
	}

	public void ClearLogs(bool valid)
	{
		if (valid)
			_validClLogs.Clear();
		else
			_invalidClLogs.Clear();
	}

	public void Log(UploadRequest uploadRequest, string spawnsetName, double elapsedMilliseconds, string? errorMessage)
	{
		string playerInfo = $"{uploadRequest.PlayerName} ({uploadRequest.PlayerId})";
		string time = uploadRequest.TimeInSeconds.ToString(StringFormats.TimeFormat);
		string requestInfo = $"{uploadRequest.Client} {uploadRequest.ClientVersion} {uploadRequest.OperatingSystem} {uploadRequest.BuildMode} | Replay size {uploadRequest.ReplayData.Length:N0} bytes | Status {uploadRequest.Status}";

		List<string> timestampEntries = new();
		(double TimeInSeconds, long Timestamp)? previous = null;
		foreach (UploadRequestTimestamp timestamp in uploadRequest.Timestamps)
		{
			DateTime dateTime = new(timestamp.Timestamp, DateTimeKind.Utc);

			if (previous.HasValue)
			{
				double timeDifference = timestamp.TimeInSeconds - previous.Value.TimeInSeconds;
				long timestampDifference = timestamp.Timestamp - previous.Value.Timestamp;
				TimeSpan timeSpanDifference = TimeSpan.FromTicks(timestampDifference);
				timestampEntries.Add($"{timestamp.TimeInSeconds.ToString(StringFormats.TimeFormat),-10} {dateTime.ToString(StringFormats.DateTimeUtcFormat),-30} {timeDifference.ToString(StringFormats.TimeFormat),-10} {timeSpanDifference,-20}");
			}
			else
			{
				timestampEntries.Add($"{timestamp.TimeInSeconds.ToString(StringFormats.TimeFormat),-10} {dateTime.ToString(StringFormats.DateTimeUtcFormat),-30}");
			}

			previous = (timestamp.TimeInSeconds, timestamp.Timestamp);
		}

		string replayMessage = $"`{elapsedMilliseconds:N0} ms | {playerInfo} {spawnsetName} {time} {requestInfo}`";
		string timestampMessage = $"""
			{replayMessage}
			```
			{string.Join('\n', timestampEntries)}
			```
			""";

		string message = uploadRequest.Status == 5 ? replayMessage : timestampMessage;
		if (errorMessage == null)
			_validClLogs.Add(message);
		else
			_invalidClLogs.Add($"{message}\n**{errorMessage}**");
	}
}
