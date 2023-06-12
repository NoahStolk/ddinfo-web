namespace DevilDaggersInfo.Web.Server.Domain.Services.Inversion;

public interface ICustomLeaderboardSubmissionLogger
{
	void Log(bool isValid, string message);

	IReadOnlyList<string> GetLogs(bool valid);

	void ClearLogs(bool valid);
}
