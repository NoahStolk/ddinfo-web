using DevilDaggersCore.Extensions;
using DevilDaggersWebsite.BlazorWasm.Server.HostedServices.DdInfoDiscordBot;
using DevilDaggersWebsite.Extensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.BlazorWasm.Server.Singletons
{
	public class AuditLogger
	{
		private const int _loggingMax = 60;

		private readonly DiscordLogger _discordLogger;

		public AuditLogger(DiscordLogger discordLogger)
		{
			_discordLogger = discordLogger;
		}

		public async Task LogAdd<TData, TKey>(TData obj, ClaimsPrincipal claimsPrincipal, TKey id, [CallerMemberName] string endpointName = "")
			where TData : notnull
		{
			StringBuilder auditLogger = GetAuditLogger("ADD", claimsPrincipal, id, endpointName);
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

		public async Task LogEdit<TData, TKey>(TData oldObj, TData newObj, ClaimsPrincipal claimsPrincipal, TKey id, [CallerMemberName] string endpointName = "")
			where TData : notnull
		{
			StringBuilder auditLogger = GetAuditLogger("EDIT", claimsPrincipal, id, endpointName);

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

		public async Task LogDelete<TData, TKey>(TData obj, ClaimsPrincipal claimsPrincipal, TKey id, [CallerMemberName] string endpointName = "")
			where TData : notnull
		{
			StringBuilder auditLogger = GetAuditLogger("DELETE", claimsPrincipal, id, endpointName);
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

		private static StringBuilder GetAuditLogger<TKey>(string action, ClaimsPrincipal claimsPrincipal, TKey id, string endpointName)
			=> new($"`{action}` by `{claimsPrincipal.GetShortName()}` for `{GetEntityFromEndpointName(endpointName)}` `{id}`\n");

		private static string GetEntityFromEndpointName(string endpointName)
		{
			if (endpointName.StartsWith("Add"))
				return endpointName[3..];
			if (endpointName.StartsWith("Edit"))
				return endpointName[4..];
			if (endpointName.StartsWith("Delete"))
				return endpointName[6..];
			return endpointName;
		}

		private static Dictionary<string, string> GetLog<TData>(TData obj)
			where TData : notnull
		{
			Dictionary<string, string> dict = new();
			foreach (PropertyInfo pi in obj.GetType().GetProperties())
			{
				// Ignore navigation properties by continuing if property has ForeignKey attribute.
				if (pi.GetCustomAttribute<ForeignKeyAttribute>() != null)
					continue;

				// Ignore navigation properties by continuing if property is generic List.
				if (pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
					continue;

				dict.Add(pi.Name, pi.GetValue(obj)?.ToString() ?? string.Empty);
			}

			return dict;
		}
	}
}
