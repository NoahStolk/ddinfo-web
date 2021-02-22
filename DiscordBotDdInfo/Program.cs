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
				DiscordLogger.Instance.CustomLeaderboardChannel = await client.GetChannelAsync(ServerConstants.CustomLeaderboardChannelId);
				DiscordLogger.Instance.ErrorChannel = await client.GetChannelAsync(ServerConstants.ErrorChannelId);
				DiscordLogger.Instance.TaskChannel = await client.GetChannelAsync(ServerConstants.TaskChannelId);
				DiscordLogger.Instance.TestChannel = await client.GetChannelAsync(ServerConstants.TestChannelId);

				client.MessageCreated += async (client, e) => await CommandHandler.Instance.MessageReceived(client, e);

				await client.ConnectAsync();
				await Task.Delay(-1);
			}
		}
	}
}
