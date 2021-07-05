using DevilDaggersCore.Extensions;
using DevilDaggersWebsite.Extensions;
using DevilDaggersWebsite.HostedServices.DdInfoDiscordBot;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Singletons
{
	public class AuditLogger
	{
		private const int _loggingMax = 60;

		private readonly DiscordLogger _discordLogger;

		public AuditLogger(DiscordLogger discordLogger)
		{
			_discordLogger = discordLogger;
		}

		public async Task LogCreate<T>(ClaimsPrincipal claimsPrincipal, string nameOfEntity, int id, T obj)
			where T : notnull
		{
			StringBuilder auditLogger = GetAuditLogger("CREATE", claimsPrincipal, nameOfEntity, id);
			auditLogger.AppendLine("```diff");

			const string propertyHeader = "Property";
			const string valueHeader = "Value";
			const int paddingL = 4;

			int maxL = propertyHeader.Length;

			Dictionary<string, string> log = GetLog(obj);
			foreach (KeyValuePair<string, string> kvp in log)
			{
				string trimmedKey = kvp.Key.TrimAfter(_loggingMax, true);
				if (trimmedKey.Length > maxL)
					maxL = trimmedKey.Length;
			}

			auditLogger.AppendFormat($"{{0,-{maxL + paddingL}}}", propertyHeader).AppendLine(valueHeader);
			auditLogger.AppendLine();
			foreach (KeyValuePair<string, string> kvp in log)
				auditLogger.AppendFormat($"{{0,-{maxL + paddingL}}}", $"+ {kvp.Key.TrimAfter(_loggingMax, true)}").AppendLine(kvp.Value.TrimAfter(_loggingMax, true));

			auditLogger.AppendLine("```");
			await _discordLogger.TryLog(Channel.MonitoringAuditLog, auditLogger.ToString());
		}

		public async Task LogEdit<T>(ClaimsPrincipal claimsPrincipal, string nameOfEntity, int id, T oldObj, T newObj)
			where T : notnull
		{
			StringBuilder auditLogger = GetAuditLogger("EDIT", claimsPrincipal, nameOfEntity, id);

			Dictionary<string, string> oldLog = GetLog(oldObj);
			Dictionary<string, string> newLog = GetLog(newObj);
			if (AreEditLogsEqual(oldLog, newLog))
			{
				auditLogger.AppendLine("`No changes.`");
				await _discordLogger.TryLog(Channel.MonitoringAuditLog, auditLogger.ToString());
				return;
			}

			auditLogger.AppendLine("```diff");

			const string propertyHeader = "Property";
			const string oldValueHeader = "Old value";
			const string newValueHeader = "New value";
			const int paddingL = 4;
			const int paddingR = 2;

			int maxL = propertyHeader.Length, maxR = oldValueHeader.Length;
			foreach (KeyValuePair<string, string> kvp in oldLog)
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
			foreach (KeyValuePair<string, string> kvp in oldLog)
			{
				string newValue = newLog[kvp.Key];
				char diff = kvp.Value == newValue ? '=' : '+';
				auditLogger.AppendFormat($"{{0,-{maxL + paddingL}}}", $"{diff} {kvp.Key}").AppendFormat($"{{0,-{maxR + paddingR}}}", kvp.Value.TrimAfter(_loggingMax, true)).AppendLine(newValue.TrimAfter(_loggingMax, true));
			}

			auditLogger.AppendLine("```");
			await _discordLogger.TryLog(Channel.MonitoringAuditLog, auditLogger.ToString());

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

		public async Task LogDelete<T>(ClaimsPrincipal claimsPrincipal, string nameOfEntity, int id, T obj)
			where T : notnull
		{
			StringBuilder auditLogger = GetAuditLogger("DELETE", claimsPrincipal, nameOfEntity, id);
			auditLogger.AppendLine("```diff");

			const string propertyHeader = "Property";
			const string valueHeader = "Value";
			const int paddingL = 4;

			int maxL = propertyHeader.Length;

			Dictionary<string, string> log = GetLog(obj);
			foreach (KeyValuePair<string, string> kvp in log)
			{
				string trimmedKey = kvp.Key.TrimAfter(_loggingMax, true);
				if (trimmedKey.Length > maxL)
					maxL = trimmedKey.Length;
			}

			auditLogger.AppendFormat($"{{0,-{maxL + paddingL}}}", propertyHeader).AppendLine(valueHeader);
			auditLogger.AppendLine();
			foreach (KeyValuePair<string, string> kvp in log)
				auditLogger.AppendFormat($"{{0,-{maxL + paddingL}}}", $"- {kvp.Key.TrimAfter(_loggingMax, true)}").AppendLine(kvp.Value.TrimAfter(_loggingMax, true));

			auditLogger.AppendLine("```");
			await _discordLogger.TryLog(Channel.MonitoringAuditLog, auditLogger.ToString());
		}

		private static StringBuilder GetAuditLogger(string action, ClaimsPrincipal claimsPrincipal, string nameOfEntity, int id)
			=> new($"`{action}` by `{claimsPrincipal.GetShortName()}` for `{nameOfEntity}` `{id}`\n");

		private static Dictionary<string, string> GetLog<T>(T obj)
			where T : notnull
		{
			Dictionary<string, string> dict = new();
			foreach (PropertyInfo pi in obj.GetType().GetProperties())
			{
				// TODO: For entities; ignore navigation properties by continuing if property has ForeignKey attribute.
				dict.Add(pi.Name, pi.GetValue(obj)?.ToString() ?? string.Empty);
			}

			return dict;
		}
	}
}
