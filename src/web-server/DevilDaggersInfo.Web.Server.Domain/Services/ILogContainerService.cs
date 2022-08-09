namespace DevilDaggersInfo.Web.Server.Domain.Services;

public interface ILogContainerService
{
	List<string> AuditLogEntries { get; }
	List<string> LogEntries { get; }

	void AddLog(string message);

	void AddAuditLog(string message);
}
