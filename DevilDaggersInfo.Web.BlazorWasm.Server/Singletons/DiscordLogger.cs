using DevilDaggersInfo.Web.BlazorWasm.Server.HostedServices.DdInfoDiscordBot;
using DSharpPlus.Entities;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Singletons;

public class DiscordLogger
{
	private readonly IWebHostEnvironment _environment;

	public DiscordLogger(IWebHostEnvironment environment)
	{
		_environment = environment;
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
