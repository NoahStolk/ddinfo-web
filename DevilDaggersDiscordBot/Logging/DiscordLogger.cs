using DevilDaggersDiscordBot.Extensions;
using DSharpPlus.Entities;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace DevilDaggersDiscordBot.Logging
{
	public sealed class DiscordLogger
	{
		private static readonly Lazy<DiscordLogger> _lazy = new(() => new());

		private DiscordLogger()
		{
		}

		public static DiscordLogger Instance => _lazy.Value;

		internal DiscordChannel? CustomLeaderboardsChannel { get; set; }
		internal DiscordChannel? CustomLeaderboardMonitoringChannel { get; set; }
		internal DiscordChannel? ErrorMonitoringChannel { get; set; }
		internal DiscordChannel? TaskMonitoringChannel { get; set; }
		internal DiscordChannel? TestMonitoringChannel { get; set; }

		public async Task TryLogException(string title, Exception ex)
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
				await TryLog(Channel.ErrorMonitoring, $"Error report '{nameof(TryLogException)}' failed! {logEx.Message}");
			}
		}

		public async Task TryLog(Channel loggingChannel, string? message, DiscordEmbed? embed = null)
		{
			DiscordChannel? channel = loggingChannel switch
			{
				Channel.CustomLeaderboards => CustomLeaderboardsChannel,
				Channel.CustomLeaderboardMonitoring => CustomLeaderboardMonitoringChannel,
				Channel.TaskMonitoring => TaskMonitoringChannel,
				Channel.TestMonitoring => TestMonitoringChannel,
				_ => ErrorMonitoringChannel,
			};

			try
			{
				if (channel != null)
					await channel.SendMessageAsyncSafe(message, embed);
			}
			catch
			{
				// Ignore exceptions that occurred while attempting to log.
			}
		}
	}
}
