using DevilDaggersInfo.Common;
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

	public void Log(
		string playerName,
		int playerId,
		string spawnsetName,
		double timeInSeconds,
		string clientName,
		string clientVersion,
		string operatingSystem,
		string buildMode,
		int replaySize,
		int status,
		double elapsedMilliseconds,
		string? errorMessage)
	{
		string playerInfo = $"`{playerName}` (`{playerId}`)";
		string time = timeInSeconds.ToString(StringFormats.TimeFormat);
		string requestInfo = $"`{clientName} {clientVersion} {operatingSystem} {buildMode}` | `Replay size {replaySize:N0} bytes` | `Status {status}`";
		string message = $"`{elapsedMilliseconds:N0} ms` {playerInfo} `{spawnsetName}` `{time}` {requestInfo}";

		if (errorMessage == null)
			_validClLogs.Add(message);
		else
			_invalidClLogs.Add(message);
	}
}
