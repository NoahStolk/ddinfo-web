using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.HostedServices.DdInfoDiscordBot;

public class DdInfoDiscordBotService : IHostedService
{
	private readonly IConfiguration _configuration;

	public DdInfoDiscordBotService(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	public async Task StartAsync(CancellationToken cancellationToken)
	{
		using DiscordClient client = new(new()
		{
			Token = _configuration["BotToken"],
			TokenType = TokenType.Bot,
		});

		await DevilDaggersInfoServerConstants.LoadServerChannelsAndMessages(client);

		client.MessageCreated += async (client, e) =>
		{
			string msg = e.Message.Content.ToLower();
			if (msg.Length <= 1)
				return;

			// React with an emoji when the bot gets mentioned anywhere.
			if (msg.Contains($"@!{DevilDaggersInfoServerConstants.BotUserId}"))
				await e.Message.CreateReactionAsync(DiscordEmoji.FromName(client, ":eye_in_speech_bubble:"));

			if (e.Channel.Id == DevilDaggersInfoServerConstants.Channels[Channel.MonitoringTest].ChannelId && msg.StartsWith("."))
			{
				Action<MessageCreateEventArgs>? action = Commands.Actions.FirstOrDefault(a => msg.StartsWith(a.Key)).Value;
				if (action == null)
					await e.Channel.SendMessageAsyncSafe($"Command '{msg}' does not exist.");
				else
					action.Invoke(e);
			}
		};

		await client.ConnectAsync();
		await Task.Delay(-1, cancellationToken);
	}

	public Task StopAsync(CancellationToken cancellationToken)
	{
		return Task.CompletedTask;
	}
}
