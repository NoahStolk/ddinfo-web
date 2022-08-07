using DevilDaggersInfo.Web.Core.Claims;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Services;
using System.Security.Claims;

namespace DevilDaggersInfo.Web.Server.Services;

public class AuditLogger : IAuditLogger
{
	private const int _loggingMax = 60;

	private readonly IWebHostEnvironment _environment;
	private readonly LogContainerService _logContainerService;

	public AuditLogger(IWebHostEnvironment environment, LogContainerService logContainerService)
	{
		_environment = environment;
		_logContainerService = logContainerService;
	}

	public void LogAdd(Dictionary<string, string> dtoLog, ClaimsPrincipal claimsPrincipal, int id, List<FileSystemInformation>? fileSystemInformation = null, [CallerMemberName] string endpointName = "")
	{
		StringBuilder auditLogger = GetAuditLogger(claimsPrincipal, id, endpointName);
		auditLogger.AppendLine("```diff");

		const string propertyHeader = "Property";
		const string valueHeader = "Value";
		const int paddingL = 4;

		int maxL = propertyHeader.Length;

		foreach (KeyValuePair<string, string> kvp in dtoLog)
		{
			string trimmedKey = SanitizeLog(kvp.Key);
			if (trimmedKey.Length > maxL)
				maxL = trimmedKey.Length;
		}

		auditLogger.AppendFormat($"{{0,-{maxL + paddingL}}}", propertyHeader).AppendLine(valueHeader);
		auditLogger.AppendLine();
		foreach (KeyValuePair<string, string> kvp in dtoLog)
			auditLogger.AppendFormat($"{{0,-{maxL + paddingL}}}", $"+ {SanitizeLog(kvp.Key)}").AppendLine(SanitizeLog(kvp.Value));

		auditLogger.AppendLine("```");

		AddFileSystemInformation(auditLogger, fileSystemInformation);

		Log(auditLogger.ToString());
	}

	public void LogEdit(Dictionary<string, string> oldDtoLog, Dictionary<string, string> dtoLog, ClaimsPrincipal claimsPrincipal, int id, List<FileSystemInformation>? fileSystemInformation = null, [CallerMemberName] string endpointName = "")
	{
		StringBuilder auditLogger = GetAuditLogger(claimsPrincipal, id, endpointName);

		if (AreEditLogsEqual(oldDtoLog, dtoLog))
		{
			auditLogger.AppendLine("`No changes.`");
		}
		else
		{
			auditLogger.AppendLine("```diff");

			const string propertyHeader = "Property";
			const string oldValueHeader = "Old value";
			const string newValueHeader = "New value";
			const int paddingL = 4;
			const int paddingR = 2;

			int maxL = propertyHeader.Length, maxR = oldValueHeader.Length;
			foreach (KeyValuePair<string, string> kvp in oldDtoLog)
			{
				string trimmedKey = SanitizeLog(kvp.Key);
				string trimmedValue = SanitizeLog(kvp.Value);

				if (trimmedKey.Length > maxL)
					maxL = trimmedKey.Length;
				if (trimmedValue.Length > maxR)
					maxR = trimmedValue.Length;
			}

			auditLogger.AppendFormat($"{{0,-{maxL + paddingL}}}", propertyHeader).AppendFormat($"{{0,-{maxR + paddingR}}}", oldValueHeader).AppendLine(newValueHeader);
			auditLogger.AppendLine();
			foreach (KeyValuePair<string, string> kvp in oldDtoLog)
			{
				string newValue = dtoLog[kvp.Key];
				char diff = kvp.Value == newValue ? '=' : '+';
				auditLogger.AppendFormat($"{{0,-{maxL + paddingL}}}", $"{diff} {kvp.Key}").AppendFormat($"{{0,-{maxR + paddingR}}}", SanitizeLog(kvp.Value)).AppendLine(SanitizeLog(newValue));
			}

			auditLogger.AppendLine("```");
		}

		AddFileSystemInformation(auditLogger, fileSystemInformation);

		Log(auditLogger.ToString());

		static bool AreEditLogsEqual(Dictionary<string, string> oldLog, Dictionary<string, string> newLog)
		{
			if (oldLog.Count != newLog.Count)
				throw new("Logs are not equal in length which should not be possible.");

			foreach (KeyValuePair<string, string> kvp in oldLog)
			{
				if (!newLog.ContainsKey(kvp.Key))
					throw new($"New log does not contain property '{kvp.Key}'.");

				if (newLog[kvp.Key] != kvp.Value)
					return false;
			}

			return true;
		}
	}

	public void LogDelete(Dictionary<string, string> entityLog, ClaimsPrincipal claimsPrincipal, int id, List<FileSystemInformation>? fileSystemInformation = null, [CallerMemberName] string endpointName = "")
	{
		StringBuilder auditLogger = GetAuditLogger(claimsPrincipal, id, endpointName);
		auditLogger.AppendLine("```diff");

		const string propertyHeader = "Property";
		const string valueHeader = "Value";
		const int paddingL = 4;

		int maxL = propertyHeader.Length;

		foreach (KeyValuePair<string, string> kvp in entityLog)
		{
			string trimmedKey = SanitizeLog(kvp.Key);
			if (trimmedKey.Length > maxL)
				maxL = trimmedKey.Length;
		}

		auditLogger.AppendFormat($"{{0,-{maxL + paddingL}}}", propertyHeader).AppendLine(valueHeader);
		auditLogger.AppendLine();
		foreach (KeyValuePair<string, string> kvp in entityLog)
			auditLogger.AppendFormat($"{{0,-{maxL + paddingL}}}", $"- {SanitizeLog(kvp.Key)}").AppendLine(SanitizeLog(kvp.Value));

		auditLogger.AppendLine("```");

		AddFileSystemInformation(auditLogger, fileSystemInformation);

		Log(auditLogger.ToString());
	}

	public void LogRoleAssign(string userName, string roleName) => LogRoleChange("ASSIGN", userName, roleName);

	public void LogRoleRevoke(string userName, string roleName) => LogRoleChange("REVOKE", userName, roleName);

	private void LogRoleChange(string action, string userName, string roleName) => Log($"`{action} ROLE '{roleName}'` for user `{userName}`. Make sure to login again for this to take effect in the browser.");

	private static StringBuilder GetAuditLogger(ClaimsPrincipal claimsPrincipal, int id, string endpointName) => new($"`{endpointName}` by `{claimsPrincipal.GetName() ?? "?"}` for ID `{id}`\n");

	private static void AddFileSystemInformation(StringBuilder auditLogger, List<FileSystemInformation>? auditLogFileSystemInformation)
	{
		if (auditLogFileSystemInformation == null || auditLogFileSystemInformation.Count == 0)
			return;

		auditLogger.AppendLine("*File system information:*");
		foreach (FileSystemInformation alfsi in auditLogFileSystemInformation)
		{
			string emote = alfsi.Type switch
			{
				FileSystemInformationType.Delete => "wastebasket",
				FileSystemInformationType.Move => "arrow_right",
				FileSystemInformationType.NotFound => "information_source",
				FileSystemInformationType.NotFoundUnexpected or FileSystemInformationType.Skip => "warning",
				FileSystemInformationType.Add => "new",
				FileSystemInformationType.Update => "white_check_mark",
				_ => "black_circle",
			};
			auditLogger.Append(':').Append(emote).Append(": ").AppendLine(alfsi.Message);
		}
	}

	private void Log(string? message)
	{
		_logContainerService.AddAuditLog($"[`{_environment.EnvironmentName}`]: {message}");
	}

	private static string SanitizeLog(string log)
	{
		return log
			.Replace("\r", string.Empty)
			.Replace("\n", string.Empty)
			.Replace("\t", string.Empty)
			.TrimAfter(_loggingMax, true);
	}
}
