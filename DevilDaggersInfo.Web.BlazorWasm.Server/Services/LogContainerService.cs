using DSharpPlus.Entities;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Services;

public class LogContainerService
{
	private readonly List<DiscordEmbed> _logEntries = new();
	private readonly List<string> _clLogs = new();

	public void Add(DiscordEmbed embed) => _logEntries.Add(embed);

	public void AddClLog(string message) => _clLogs.Add(message);

	public async Task LogToChannel(DiscordChannel channel)
	{
		while (_logEntries.Count > 0)
		{
			DiscordEmbed embed = _logEntries[0];
			await channel.SendMessageAsyncSafe(null, embed);
			_logEntries.RemoveAt(0);
		}
	}

	public async Task LogClLogsToChannel(DiscordChannel channel)
	{
		if (_clLogs.Count > 0)
		{
			await channel.SendMessageAsyncSafe(string.Join(Environment.NewLine, _clLogs));
			_clLogs.Clear();
		}
	}
}
