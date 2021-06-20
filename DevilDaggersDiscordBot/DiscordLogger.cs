using DevilDaggersDiscordBot.Extensions;
using DSharpPlus.Entities;
using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace DevilDaggersDiscordBot
{
	public static class DiscordLogger
	{
		public static async Task TryLogException(string title, string environmentName, Exception ex)
		{
			try
			{
				DiscordChannel? channel = ServerConstants.Channels[Channel.MonitoringError].DiscordChannel;
				if (channel == null)
					return;

				DiscordEmbedBuilder builder = new()
				{
					Title = title,
					Color = DiscordColor.Red,
				};
				builder.AddError(ex);
				foreach (DictionaryEntry? data in ex.Data)
					builder.AddFieldObject(data?.Key?.ToString() ?? "Null", data?.Value?.ToString() ?? "Null");

				await channel.SendMessageAsyncSafe(null, builder.Build());
			}
			catch (Exception logEx)
			{
				await TryLog(Channel.MonitoringError, environmentName, $"{nameof(TryLogException)} failed. {logEx.Message}");
			}
		}

		public static async Task TryLogElapsedMilliseconds(Stopwatch sw, [CallerMemberName] string methodName = "")
		{
			await TryLog(Channel.MonitoringTest, "Development", $"{methodName} took {sw.ElapsedMilliseconds} ms.");
		}

		public static async Task TryLog(Channel loggingChannel, string environmentName, string? message, DiscordEmbed? embed = null)
		{
			DiscordChannel? channel = ServerConstants.Channels[loggingChannel].DiscordChannel;
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
