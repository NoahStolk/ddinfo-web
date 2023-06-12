using DevilDaggersInfo.Common;
using DevilDaggersInfo.Web.Server.Domain.Commands.CustomEntries;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;

namespace DevilDaggersInfo.Web.Server.Services;

public class CustomLeaderboardSubmissionLogger : ICustomLeaderboardSubmissionLogger
{
	private readonly List<(string Message, string FileContents)> _validClLogs = new();
	private readonly List<(string Message, string FileContents)> _invalidClLogs = new();

	public IEnumerable<(string Message, string FileContents)> GetLogs(bool valid)
		=> valid ? _validClLogs : _invalidClLogs;

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
		string message = $"`{elapsedMilliseconds:N0} ms | {playerInfo} {spawnsetName} {time} {requestInfo}";

		List<string> timestampEntries = new();
		(double TimeInSeconds, long Timestamp)? previous = null;
		foreach (UploadRequestTimestamp timestamp in uploadRequest.Timestamps)
		{
			if (previous.HasValue)
			{
				double timeDifference = timestamp.TimeInSeconds - previous.Value.TimeInSeconds;
				long timestampDifference = timestamp.Timestamp - previous.Value.Timestamp;
				TimeSpan timeSpanDifference = TimeSpan.FromTicks(timestampDifference);
				timestampEntries.Add($"{timestamp.TimeInSeconds.ToString(StringFormats.TimeFormat),-20} {timestamp.Timestamp,-20} {timeDifference,-20} {timeSpanDifference,-20}");
			}
			else
			{
				timestampEntries.Add($"{timestamp.TimeInSeconds.ToString(StringFormats.TimeFormat),-20} {timestamp.Timestamp,-20}");
			}

			previous = (timestamp.TimeInSeconds, timestamp.Timestamp);
		}

		string timestamps = string.Join('\n', timestampEntries);

		if (errorMessage == null)
			_validClLogs.Add((message, timestamps));
		else
			_invalidClLogs.Add(($"{message}\n**{errorMessage}**", timestamps));
	}
}
