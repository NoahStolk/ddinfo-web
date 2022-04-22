using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.HostedServices.DdInfoDiscordBot;

public class DiscordBotService : IHostedService
{
	private readonly IConfiguration _configuration;
	private readonly IWebHostEnvironment _environment;
	private readonly LogContainerService _logContainerService;

	public DiscordBotService(IConfiguration configuration, IWebHostEnvironment environment, LogContainerService logContainerService)
	{
		_configuration = configuration;
		_environment = environment;
		_logContainerService = logContainerService;
	}

	public async Task StartAsync(CancellationToken cancellationToken)
	{
		using DiscordClient client = new(new()
		{
			Token = _configuration["BotToken"],
			TokenType = TokenType.Bot,
		});

		await DiscordServerConstants.LoadServerChannelsAndMessages(client);

		client.MessageCreated += async (client, e) =>
		{
			string msg = e.Message.Content.ToLower();
			if (msg.Length <= 1)
				return;

			// React with an emoji when the bot gets mentioned anywhere.
			if (msg.Contains($"@!{DiscordServerConstants.BotUserId}"))
				await e.Message.CreateReactionAsync(DiscordEmoji.FromName(client, ":eye_in_speech_bubble:"));

			if (e.Channel.Id == DiscordServerConstants.TestChannelId && msg.StartsWith("."))
			{
				Action<MessageCreateEventArgs>? action = Commands.Actions.FirstOrDefault(a => msg.StartsWith(a.Key)).Value;
				if (action == null)
					await e.Channel.SendMessageAsyncSafe($"Command '{msg}' does not exist.");
				else
					action.Invoke(e);
			}
		};

		await client.ConnectAsync();

		try
		{
			await Task.Delay(-1, cancellationToken);
		}
		catch (OperationCanceledException)
		{
			if (_environment.IsDevelopment())
				return;

			DiscordChannel? logChannel = DiscordServerConstants.GetDiscordChannel(Channel.MonitoringLog, _environment);
			if (logChannel != null)
			{
				await _logContainerService.LogToLogChannel(logChannel);
				await logChannel.SendMessageAsyncSafe($"> **Application is shutting down in the `{_environment.EnvironmentName}` environment. Disconnecting from Discord...** :wave:");
			}
		}
	}

	public Task StopAsync(CancellationToken cancellationToken)
	{
		return Task.CompletedTask;
	}
}
