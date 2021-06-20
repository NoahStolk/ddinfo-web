using DevilDaggersWebsite.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DevilDaggersWebsite.Razor.PageModels
{
	public abstract class AbstractAdminEntityPageModel<TEntity> : PageModel
		where TEntity : class, IEntity
	{
		private const int _loggingMax = 60;

		protected AbstractAdminEntityPageModel(ApplicationDbContext dbContext)
		{
			DbContext = dbContext;

			DbSet = ((Array.Find(typeof(ApplicationDbContext).GetProperties(), pi => pi.PropertyType == typeof(DbSet<TEntity>)) ?? throw new("Could not retrieve DbSet of TEntity.")).GetValue(DbContext) as DbSet<TEntity>)!;

			EntityDisplayProperties = typeof(TEntity).GetProperties().Where(pi => pi.CanWrite && (pi.PropertyType.IsValueType || pi.PropertyType == typeof(string))).ToArray();
		}

		protected ApplicationDbContext DbContext { get; }

		public DbSet<TEntity> DbSet { get; }

		public PropertyInfo[] EntityDisplayProperties { get; }

		protected void LogCreateOrEdit(StringBuilder auditLogger, Dictionary<string, string>? oldLog, Dictionary<string, string> newLog)
		{
			if (AreLogsEqual(oldLog, newLog))
			{
				auditLogger.AppendLine("`No changes.`");
				return;
			}

			auditLogger.AppendLine("```diff");

			const string propertyHeader = "Property";
			const string oldValueHeader = "Old value";
			const string newValueHeader = "New value";
			const int paddingL = 4;
			const int paddingR = 2;

			int maxL = propertyHeader.Length, maxR = oldValueHeader.Length;
			foreach (KeyValuePair<string, string> kvp in oldLog ?? newLog)
			{
				string trimmedKey = TrimAfter(kvp.Key, _loggingMax);
				string trimmedValue = TrimAfter(kvp.Value, _loggingMax);

				if (trimmedKey.Length > maxL)
					maxL = trimmedKey.Length;
				if (trimmedValue.Length > maxR)
					maxR = trimmedValue.Length;
			}

			if (oldLog != null)
			{
				auditLogger.AppendFormat($"{{0,-{maxL + paddingL}}}", propertyHeader).AppendFormat($"{{0,-{maxR + paddingR}}}", oldValueHeader).AppendLine(newValueHeader);
				auditLogger.AppendLine();
				foreach (KeyValuePair<string, string> kvp in oldLog)
				{
					string newValue = newLog[kvp.Key];
					char diff = kvp.Value == newValue ? '=' : '+';
					auditLogger.AppendFormat($"{{0,-{maxL + paddingL}}}", $"{diff} {kvp.Key}").AppendFormat($"{{0,-{maxR + paddingR}}}", TrimAfter(kvp.Value, _loggingMax)).AppendLine(TrimAfter(newValue, _loggingMax));
				}
			}
			else
			{
				auditLogger.AppendFormat($"{{0,-{maxL + paddingL}}}", propertyHeader).AppendLine(newValueHeader);
				auditLogger.AppendLine();
				foreach (KeyValuePair<string, string> kvp in newLog)
					auditLogger.AppendFormat($"{{0,-{maxL + paddingL}}}", $"+ {TrimAfter(kvp.Key, _loggingMax)}").AppendLine(TrimAfter(kvp.Value, _loggingMax));
			}

			auditLogger.AppendLine("```");

			static bool AreLogsEqual(Dictionary<string, string>? oldLog, Dictionary<string, string> newLog)
			{
				if (oldLog != null && oldLog.Count == newLog.Count)
				{
					foreach (KeyValuePair<string, string> oldKvp in oldLog)
					{
						string newStr = newLog[oldKvp.Key];
						if (newStr != oldKvp.Value)
							return false;
					}
				}

				return true;
			}
		}

		protected void LogDelete(StringBuilder auditLogger, Dictionary<string, string> log)
		{
			auditLogger.AppendLine("```diff");

			const string propertyHeader = "Property";
			const string valueHeader = "Value";
			const int paddingL = 4;

			int maxL = propertyHeader.Length;
			foreach (KeyValuePair<string, string> kvp in log)
			{
				string trimmedKey = TrimAfter(kvp.Key, _loggingMax);
				if (trimmedKey.Length > maxL)
					maxL = trimmedKey.Length;
			}

			auditLogger.AppendFormat($"{{0,-{maxL + paddingL}}}", propertyHeader).AppendLine(valueHeader);
			auditLogger.AppendLine();
			foreach (KeyValuePair<string, string> kvp in log)
				auditLogger.AppendFormat($"{{0,-{maxL + paddingL}}}", $"- {TrimAfter(kvp.Key, _loggingMax)}").AppendLine(TrimAfter(kvp.Value, _loggingMax));

			auditLogger.AppendLine("```");
		}

		private static string TrimAfter(string str, int length)
			=> str.Length > length ? $"{str.Substring(0, length)}..." : str;
	}
}
