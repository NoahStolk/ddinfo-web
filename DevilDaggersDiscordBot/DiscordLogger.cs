using DevilDaggersDiscordBot.Extensions;
using DSharpPlus.Entities;
using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace DevilDaggersDiscordBot
{
	public sealed class DiscordLogger
	{
		private static readonly Lazy<DiscordLogger> _lazy = new(() => new());

		private DiscordLogger()
		{
		}

		public static DiscordLogger Instance => _lazy.Value;

		internal DiscordChannel? AuditLogMonitoringChannel { get; set; }
		internal DiscordChannel? CacheMonitoringChannel { get; set; }
		internal DiscordChannel? CustomLeaderboardMonitoringChannel { get; set; }
		internal DiscordChannel? ErrorMonitoringChannel { get; set; }
		internal DiscordChannel? TaskMonitoringChannel { get; set; }
		internal DiscordChannel? TestMonitoringChannel { get; set; }

		internal DiscordChannel? CustomLeaderboardsChannel { get; set; }

		public async Task TryLogException(string title, string environmentName, Exception ex)
		{
			try
			{
				if (ErrorMonitoringChannel == null)
					return;

				DiscordEmbedBuilder builder = new()
				{
					Title = title,
					Color = DiscordColor.Red,
				};
				builder.AddError(ex);
				foreach (DictionaryEntry? data in ex.Data)
					builder.AddFieldObject(data?.Key?.ToString() ?? "Null", data?.Value?.ToString() ?? "Null");

				await ErrorMonitoringChannel.SendMessageAsyncSafe(null, builder.Build());
			}
			catch (Exception logEx)
			{
				await TryLog(Channel.ErrorMonitoring, environmentName, $"Error report '{nameof(TryLogException)}' failed! {logEx.Message}");
			}
		}

		public async Task LogElapsedMilliseconds(Stopwatch sw, [CallerMemberName] string methodName = "")
		{
			await TryLog(Channel.TestMonitoring, "Development", $"{methodName} took {sw.ElapsedMilliseconds} ms.");
		}

		public async Task TryLog(Channel loggingChannel, string environmentName, string? message, DiscordEmbed? embed = null)
		{
			DiscordChannel? channel = loggingChannel switch
			{
				Channel.AuditLogMonitoring => AuditLogMonitoringChannel,
				Channel.CacheMonitoring => CacheMonitoringChannel,
				Channel.CustomLeaderboardMonitoring => CustomLeaderboardMonitoringChannel,
				Channel.TaskMonitoring => TaskMonitoringChannel,
				Channel.TestMonitoring => TestMonitoringChannel,

				Channel.CustomLeaderboards => CustomLeaderboardsChannel,

				_ => ErrorMonitoringChannel,
			};

			if (channel == null)
				return;

			try
			{
				string? composedMessage = embed == null ? $"[`{environmentName}`]: {message}" : null;
				await channel.SendMessageAsyncSafe(composedMessage, embed);
			}
			catch
			{
				// Ignore exceptions that occurred while attempting to log.
			}
		}
	}
}
