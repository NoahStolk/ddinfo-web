using DevilDaggersCore.Extensions;
using DSharpPlus.Entities;
using System;
using System.Threading.Tasks;

namespace DevilDaggersDiscordBot.Extensions
{
	public static class DSharpExtensions
	{
		public static void AddError(this DiscordEmbedBuilder builder, Exception exception, int level = 0)
		{
			if (level > 5)
				return;

			builder.AddField(level == 0 ? "Exception message" : $"Inner exception message {level}", exception.Message.TrimAfter(1024));
			if (exception.InnerException != null)
				builder.AddError(exception.InnerException, ++level);
		}

		public static async Task SendMessageAsyncSafe(this DiscordChannel channel, string? message, DiscordEmbed? embed = null)
		{
			if (message?.Length >= 2000)
				message = $"{message.Substring(0, 1996)}...";

			await channel.SendMessageAsync(message, embed);
		}

		public static DiscordEmbedBuilder AddFieldObject(this DiscordEmbedBuilder builder, string name, object? value, bool inline = false)
		{
			string? valueString = value?.ToString()?.TrimAfter(1024);

			return builder.AddField(name, string.IsNullOrWhiteSpace(valueString) ? "null" : valueString, inline);
		}
	}
}
