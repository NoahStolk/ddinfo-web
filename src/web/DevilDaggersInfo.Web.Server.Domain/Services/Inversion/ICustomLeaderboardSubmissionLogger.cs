using DevilDaggersInfo.Web.Server.Domain.Commands.CustomEntries;

namespace DevilDaggersInfo.Web.Server.Domain.Services.Inversion;

public interface ICustomLeaderboardSubmissionLogger
{
	void Log(UploadRequest uploadRequest, string spawnsetName, double elapsedMilliseconds, string? errorMessage);

	IEnumerable<string> GetLogs(bool valid);

	void ClearLogs(bool valid);
}
