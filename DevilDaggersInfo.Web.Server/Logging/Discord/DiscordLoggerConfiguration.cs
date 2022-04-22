using DSharpPlus.Entities;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Logging.Discord;

public class DiscordLoggerConfiguration
{
	public int EventId { get; set; }

	public Dictionary<LogLevel, DiscordColor> LogLevels { get; set; } = new()
	{
		[LogLevel.Trace] = DiscordColor.Green,
		[LogLevel.Debug] = DiscordColor.HotPink,
		[LogLevel.Information] = DiscordColor.Aquamarine,
		[LogLevel.Warning] = DiscordColor.Orange,
		[LogLevel.Error] = DiscordColor.Red,
		[LogLevel.Critical] = DiscordColor.DarkRed,
		[LogLevel.None] = DiscordColor.White,
	};
}
