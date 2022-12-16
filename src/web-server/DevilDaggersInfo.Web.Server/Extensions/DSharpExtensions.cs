using DevilDaggersInfo.Web.Server.Domain.Extensions;
using DSharpPlus.Entities;

namespace DevilDaggersInfo.Web.Server.Extensions;

public static class DSharpExtensions
{
	public static async Task<bool> SendMessageAsyncSafe(this DiscordChannel channel, string? message, DiscordEmbed? embed = null)
	{
		if (message == null && embed == null)
			throw new InvalidOperationException("Can't send empty Discord message.");

		if (message?.Length >= 2000)
			message = $"{message[..1996]}...";

		try
		{
			if (embed == null)
				await channel.SendMessageAsync(message);
			else
				await channel.SendMessageAsync(message, embed);

			return true;
		}
		catch
		{
			return false;
		}
	}

	public static DiscordEmbedBuilder AddFieldObject(this DiscordEmbedBuilder builder, string name, object? value, bool inline = false)
	{
		string? valueString = value?.ToString()?.TrimAfter(1024);

		return builder.AddField(name, string.IsNullOrWhiteSpace(valueString) ? "null" : valueString, inline);
	}
}
