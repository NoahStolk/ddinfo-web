using DiscordBotDdInfo.Extensions;
using DiscordBotDdInfo.Logging;
using DSharpPlus.Entities;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace DiscordBotDdInfo
{
	public sealed class DiscordLogger
	{
		private static readonly Lazy<DiscordLogger> _lazy = new(() => new());

		private DiscordLogger()
		{
		}

		public static DiscordLogger Instance => _lazy.Value;

		internal DiscordChannel? CustomLeaderboardChannel { get; set; }
		internal DiscordChannel? ErrorChannel { get; set; }
		internal DiscordChannel? TaskChannel { get; set; }
		internal DiscordChannel? TestChannel { get; set; }

		public async Task TryLogException(string title, Exception ex)
		{
			try
			{
				if (ErrorChannel == null)
					return;

				DiscordEmbedBuilder builder = new()
				{
					Title = title,
					Color = DiscordColor.Red,
				};
				builder.AddError(ex);
				foreach (DictionaryEntry? data in ex.Data)
					builder.AddFieldObject(data?.Key?.ToString() ?? "Null", data?.Value?.ToString() ?? "Null");

				await ErrorChannel.SendMessageAsyncSafe(null, builder.Build());
			}
			catch (Exception logEx)
			{
				await TryLog(LoggingChannel.Error, $"Error report '{nameof(TryLogException)}' failed! {logEx.Message}");
			}
		}

		public async Task TryLog(LoggingChannel loggingChannel, string? message, DiscordEmbed? embed = null)
		{
			DiscordChannel? channel = loggingChannel switch
			{
				LoggingChannel.CustomLeaderboard => CustomLeaderboardChannel,
				LoggingChannel.Task => TaskChannel,
				LoggingChannel.Test => TestChannel,
				_ => ErrorChannel,
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
