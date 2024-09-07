using DevilDaggersInfo.Web.Server.Domain.Configuration;
using DevilDaggersInfo.Web.Server.Extensions;
using DSharpPlus;
using DSharpPlus.Entities;
using Microsoft.Extensions.Options;

namespace DevilDaggersInfo.Web.Server.HostedServices.DdInfoDiscordBot;

public class DiscordBotService : IHostedService
{
	private readonly IOptions<DiscordOptions> _discordBotOptions;
	private readonly IWebHostEnvironment _environment;

	private DiscordClient? _client;

	public DiscordBotService(IOptions<DiscordOptions> discordBotOptions, IWebHostEnvironment environment)
	{
		_discordBotOptions = discordBotOptions;
		_environment = environment;
	}

	public async Task StartAsync(CancellationToken cancellationToken)
	{
		_client = new DiscordClient(new DiscordConfiguration
		{
			Token = _discordBotOptions.Value.BotToken,
			TokenType = TokenType.Bot,
		});

		await DiscordServerConstants.LoadServerChannels(_client);

		_client.MessageCreated += async (c, e) =>
		{
			if (e.Channel.Id == DiscordServerConstants.TestChannelId && e.Author.Id != c.CurrentUser.Id)
				await e.Channel.SendMessageAsyncSafe("Test reply.");
		};

		await _client.ConnectAsync();
	}

	public async Task StopAsync(CancellationToken cancellationToken)
	{
		if (!_environment.IsDevelopment())
		{
			DiscordChannel? logChannel = DiscordServerConstants.GetDiscordChannel(Channel.MonitoringLog, _environment);
			if (logChannel != null)
				await logChannel.SendMessageAsyncSafe($"> **Application is shutting down in the `{_environment.EnvironmentName}` environment. Disconnecting from Discord...**");
		}

		if (_client != null)
		{
			await _client.DisconnectAsync();
			_client.Dispose();
		}
	}
}
