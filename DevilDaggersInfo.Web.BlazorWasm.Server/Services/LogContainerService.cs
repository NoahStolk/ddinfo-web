using DSharpPlus.Entities;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Services;

public class LogContainerService
{
	private readonly List<DiscordEmbed> _logEntries = new();

	public IReadOnlyList<DiscordEmbed> LogEntries => _logEntries;

	public void Add(DiscordEmbed embed) => _logEntries.Add(embed);

	public async Task LogToChannel(DiscordChannel channel)
	{
		while (LogEntries.Count > 0)
		{
			DiscordEmbed embed = LogEntries[0];
			await channel.SendMessageAsyncSafe(null, embed);
			_logEntries.RemoveAt(0);
		}
	}
}
