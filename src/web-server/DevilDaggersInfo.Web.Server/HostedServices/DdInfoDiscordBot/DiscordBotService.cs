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

	public DiscordBotService(IOptions<DiscordOptions> discordBotOptions, IWebHostEnvironment environment)
	{
		_discordBotOptions = discordBotOptions;
		_environment = environment;
	}

	public async Task StartAsync(CancellationToken cancellationToken)
	{
		using DiscordClient client = new(new()
		{
			Token = _discordBotOptions.Value.BotToken,
			TokenType = TokenType.Bot,
		});

		await DiscordServerConstants.LoadServerChannelsAndMessages(client);

		client.MessageCreated += async (c, e) =>
		{
			if (e.Channel.Id == DiscordServerConstants.TestChannelId && e.Author.Id != c.CurrentUser.Id)
				await e.Channel.SendMessageAsyncSafe("Test reply.");
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
				await logChannel.SendMessageAsyncSafe($"> **Application is shutting down in the `{_environment.EnvironmentName}` environment. Disconnecting from Discord...**");
		}
	}

	public Task StopAsync(CancellationToken cancellationToken)
	{
		return Task.CompletedTask;
	}
}
