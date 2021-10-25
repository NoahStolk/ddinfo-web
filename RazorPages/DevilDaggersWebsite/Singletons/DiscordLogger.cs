using DevilDaggersWebsite.Extensions;
using DevilDaggersWebsite.HostedServices.DdInfoDiscordBot;
using DSharpPlus.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Singletons
{
	public class DiscordLogger
	{
		private readonly IWebHostEnvironment _environment;

		public DiscordLogger(IWebHostEnvironment environment)
		{
			_environment = environment;
		}

		public async Task TryLogException(string title, Exception ex)
		{
			try
			{
				DiscordChannel? channel = DevilDaggersInfoServerConstants.Channels[Channel.MonitoringError].DiscordChannel;
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
				await TryLog(Channel.MonitoringError, $"{nameof(TryLogException)} failed. {logEx.Message}");
			}
		}

		public async Task TryLogElapsedMilliseconds(Stopwatch sw, [CallerMemberName] string methodName = "")
		{
			await TryLog(Channel.MonitoringTest, $"{methodName} took {sw.ElapsedMilliseconds} ms.");
		}

		public async Task TryLog(Channel loggingChannel, string? message, DiscordEmbed? embed = null, bool includeEnvironmentName = true)
		{
			if (_environment.IsDevelopment())
				loggingChannel = Channel.MonitoringTest;

			DiscordChannel? channel = DevilDaggersInfoServerConstants.Channels[loggingChannel].DiscordChannel;
			if (channel == null)
				return;

			try
			{
				string? composedMessage = embed == null ? includeEnvironmentName ? $"[`{_environment.EnvironmentName}`]: {message}" : message : null;
				await channel.SendMessageAsyncSafe(composedMessage, embed);
			}
			catch
			{
				// Ignore exceptions that occurred while attempting to log.
			}
		}

		public static async Task TryEditMessage(DiscordMessage message, DiscordEmbed embed)
		{
			try
			{
				await message.ModifyAsync(":eye_in_speech_bubble:");
				await message.ModifyAsync(embed);
			}
			catch
			{
				// Ignore exceptions that occurred while attempting to edit message.
			}
		}
	}
}
