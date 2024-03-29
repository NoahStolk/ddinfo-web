using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;

namespace DevilDaggersInfo.Web.Server.Services;

public class LogContainerService : ILogContainerService
{
	public List<string> AuditLogEntries { get; } = [];
	public List<string> LogEntries { get; } = [];

	public void AddLog(string message)
	{
		LogEntries.Add(message);
	}

	public void AddAuditLog(string message)
	{
		AuditLogEntries.Add(message);
	}
}
