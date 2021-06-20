using DevilDaggersDiscordBot.Extensions;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DevilDaggersDiscordBot
{
	public static class Program
	{
		public static void Main()
		{
			Async().ConfigureAwait(false).GetAwaiter().GetResult();

			static async Task Async()
			{
				string token = File.ReadAllText(".botToken");

				using DiscordClient client = new(new()
				{
					Token = token,
					TokenType = TokenType.Bot,
				});

				DiscordLogger.Instance.AuditLogMonitoringChannel = await client.GetChannelAsync(ServerConstants.AuditLogMonitoringChannelId);
				DiscordLogger.Instance.CacheMonitoringChannel = await client.GetChannelAsync(ServerConstants.CacheMonitoringChannelId);
				DiscordLogger.Instance.CustomLeaderboardMonitoringChannel = await client.GetChannelAsync(ServerConstants.CustomLeaderboardMonitoringChannelId);
				DiscordLogger.Instance.ErrorMonitoringChannel = await client.GetChannelAsync(ServerConstants.ErrorMonitoringChannelId);
				DiscordLogger.Instance.TaskMonitoringChannel = await client.GetChannelAsync(ServerConstants.TaskMonitoringChannelId);
				DiscordLogger.Instance.TestMonitoringChannel = await client.GetChannelAsync(ServerConstants.TestMonitoringChannelId);

				DiscordLogger.Instance.CustomLeaderboardsChannel = await client.GetChannelAsync(ServerConstants.CustomLeaderboardsChannelId);

				client.MessageCreated += async (client, e) =>
				{
					string msg = e.Message.Content.ToLower();
					if (msg.Length <= 1)
						return;

					// React with an emoji when the bot gets mentioned anywhere.
					if (msg.Contains($"@!{ServerConstants.BotUserId}"))
						await e.Message.CreateReactionAsync(DiscordEmoji.FromName(client, ":eye_in_speech_bubble:"));

					if (e.Channel.Id == ServerConstants.TestMonitoringChannelId && msg.StartsWith("."))
					{
						foreach (KeyValuePair<string, Action<MessageCreateEventArgs>> action in Commands.Actions)
						{
							if (msg.StartsWith(action.Key))
							{
								action.Value.Invoke(e);
								return;
							}
						}

						await e.Channel.SendMessageAsyncSafe($"Command '{msg}' does not exist.");
					}
				};

				await client.ConnectAsync();
				await Task.Delay(-1);
			}
		}
	}
}
