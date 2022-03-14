using DevilDaggersInfo.Web.BlazorWasm.Server.InternalModels;
using System.Security.Claims;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Services;

public class AuditLogger
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
			string trimmedKey = kvp.Key.TrimAfter(_loggingMax, true);
			if (trimmedKey.Length > maxL)
				maxL = trimmedKey.Length;
		}

		auditLogger.AppendFormat($"{{0,-{maxL + paddingL}}}", propertyHeader).AppendLine(valueHeader);
		auditLogger.AppendLine();
		foreach (KeyValuePair<string, string> kvp in dtoLog)
			auditLogger.AppendFormat($"{{0,-{maxL + paddingL}}}", $"+ {kvp.Key.TrimAfter(_loggingMax, true)}").AppendLine(kvp.Value.TrimAfter(_loggingMax, true));

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
				string trimmedKey = kvp.Key.TrimAfter(_loggingMax, true);
				string trimmedValue = kvp.Value.TrimAfter(_loggingMax, true);

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
				auditLogger.AppendFormat($"{{0,-{maxL + paddingL}}}", $"{diff} {kvp.Key}").AppendFormat($"{{0,-{maxR + paddingR}}}", kvp.Value.TrimAfter(_loggingMax, true)).AppendLine(newValue.TrimAfter(_loggingMax, true));
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
			string trimmedKey = kvp.Key.TrimAfter(_loggingMax, true);
			if (trimmedKey.Length > maxL)
				maxL = trimmedKey.Length;
		}

		auditLogger.AppendFormat($"{{0,-{maxL + paddingL}}}", propertyHeader).AppendLine(valueHeader);
		auditLogger.AppendLine();
		foreach (KeyValuePair<string, string> kvp in entityLog)
			auditLogger.AppendFormat($"{{0,-{maxL + paddingL}}}", $"- {kvp.Key.TrimAfter(_loggingMax, true)}").AppendLine(kvp.Value.TrimAfter(_loggingMax, true));

		auditLogger.AppendLine("```");

		AddFileSystemInformation(auditLogger, fileSystemInformation);

		Log(auditLogger.ToString());
	}

	public void LogRoleAssign(UserEntity user, string roleName) => LogRoleChange("ASSIGN", user, roleName);

	public void LogRoleRevoke(UserEntity user, string roleName) => LogRoleChange("REVOKE", user, roleName);

	private void LogRoleChange(string action, UserEntity user, string roleName) => Log($"`{action} ROLE '{roleName}'` for user `{user.Name}`. Make sure to login again for this to take effect in the browser.");

	public void LogFileSystemInformation(List<FileSystemInformation>? fileSystemInformation = null)
	{
		StringBuilder auditLogger = new();
		AddFileSystemInformation(auditLogger, fileSystemInformation);
		Log(auditLogger.ToString());
	}

	public void LogPlayerUpdates(string caller, List<(int PlayerId, string OldValue, string NewValue)> logs)
	{
		StringBuilder auditLogger = new();
		if (logs.Count == 0)
		{
			auditLogger.Append(caller).AppendLine(": No players needed updating.");
		}
		else
		{
			auditLogger.Append(caller).Append(": ").Append(logs.Count).AppendLine(" players were updated.");
			auditLogger.AppendLine("```");

			const string propertyHeader = "PlayerId";
			const string oldValueHeader = "Old value";
			const string newValueHeader = "New value";
			int paddingL = Math.Max(propertyHeader.Length, logs.Max(l => l.PlayerId.ToString().Length)) + 2;
			int paddingR = Math.Max(oldValueHeader.Length, logs.Max(l => l.OldValue.Length)) + 2;

			auditLogger.AppendFormat($"{{0,-{paddingL}}}", propertyHeader).AppendFormat($"{{0,-{paddingR}}}", oldValueHeader).AppendLine(newValueHeader);
			auditLogger.AppendLine();
			foreach ((int playerId, string oldValue, string newValue) in logs)
				auditLogger.AppendFormat($"{{0,-{paddingL}}}", playerId).AppendFormat($"{{0,-{paddingR}}}", oldValue).AppendLine(newValue);

			auditLogger.AppendLine("```");
		}

		Log(auditLogger.ToString());
	}

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
		_logContainerService.Add($"[`{_environment.EnvironmentName}`]: {message}");
	}
}
