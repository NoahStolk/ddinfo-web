using DiscordBotDdInfo.Logging;
using DSharpPlus;
using System.IO;
using System.Threading.Tasks;

namespace DiscordBotDdInfo
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
				DiscordLogger.Instance.CustomLeaderboardsChannel = await client.GetChannelAsync(ServerConstants.CustomLeaderboardsChannelId);
				DiscordLogger.Instance.CustomLeaderboardMonitoringChannel = await client.GetChannelAsync(ServerConstants.CustomLeaderboardMonitoringChannelId);
				DiscordLogger.Instance.ErrorMonitoringChannel = await client.GetChannelAsync(ServerConstants.ErrorMonitoringChannelId);
				DiscordLogger.Instance.TaskMonitoringChannel = await client.GetChannelAsync(ServerConstants.TaskMonitoringChannelId);
				DiscordLogger.Instance.TestMonitoringChannel = await client.GetChannelAsync(ServerConstants.TestMonitoringChannelId);

				client.MessageCreated += async (client, e) => await CommandHandler.Instance.MessageReceived(client, e);

				await client.ConnectAsync();
				await Task.Delay(-1);
			}
		}
	}
}
