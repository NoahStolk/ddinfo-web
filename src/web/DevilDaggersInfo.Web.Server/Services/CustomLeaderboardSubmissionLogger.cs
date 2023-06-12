using DevilDaggersInfo.Common;
using DevilDaggersInfo.Web.Server.Domain.Commands.CustomEntries;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;

namespace DevilDaggersInfo.Web.Server.Services;

public class CustomLeaderboardSubmissionLogger : ICustomLeaderboardSubmissionLogger
{
	private readonly List<string> _validClLogs = new();
	private readonly List<string> _invalidClLogs = new();

	public IReadOnlyList<string> GetLogs(bool valid)
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
		string playerInfo = $"`{uploadRequest.PlayerName}` (`{uploadRequest.PlayerId}`)";
		string time = uploadRequest.TimeInSeconds.ToString(StringFormats.TimeFormat);
		string requestInfo = $"`{uploadRequest.Client} {uploadRequest.ClientVersion} {uploadRequest.OperatingSystem} {uploadRequest.BuildMode}` | `Replay size {uploadRequest.ReplayData.Length:N0} bytes` | `Status {uploadRequest.Status}`";
		string message = $"`{elapsedMilliseconds:N0} ms` {playerInfo} `{spawnsetName}` `{time}` {requestInfo}";

		if (errorMessage == null)
			_validClLogs.Add(message);
		else
			_invalidClLogs.Add($"{message}\n**{errorMessage}**");
	}
}
