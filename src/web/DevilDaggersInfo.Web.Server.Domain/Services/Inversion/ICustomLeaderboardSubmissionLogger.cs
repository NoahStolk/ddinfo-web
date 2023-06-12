namespace DevilDaggersInfo.Web.Server.Domain.Services.Inversion;

public interface ICustomLeaderboardSubmissionLogger
{
	void Log(
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
		long elapsedMilliseconds,
		string? errorMessage);

	IReadOnlyList<string> GetLogs(bool valid);

	void ClearLogs(bool valid);
}
