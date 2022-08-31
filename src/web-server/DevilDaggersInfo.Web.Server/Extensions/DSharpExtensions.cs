using DevilDaggersInfo.Web.Server.Domain.Extensions;
using DSharpPlus.Entities;

namespace DevilDaggersInfo.Web.Server.Extensions;

public static class DSharpExtensions
{
	public static void AddError(this DiscordEmbedBuilder builder, Exception exception, int level = 0)
	{
		if (level > 5)
			return;

		builder.AddField(level == 0 ? "Exception message" : $"Inner exception message {level}", exception.Message.TrimAfter(1024));

		if (exception.StackTrace != null)
			builder.AddField("Stack trace", exception.StackTrace.TrimAfter(1024));

		if (exception.InnerException != null)
			builder.AddError(exception.InnerException, ++level);
	}

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
