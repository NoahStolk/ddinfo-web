using DSharpPlus.Entities;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Services;

public class LogContainerService
{
	private readonly List<LogEntry> _logEntries = new();
	private readonly List<string> _clLogs = new();

	public void Add(DiscordEmbed embed) => _logEntries.Add(new(null, embed));

	public void Add(string message) => _logEntries.Add(new(message, null));

	public void AddClLog(string message) => _clLogs.Add(message);

	public async Task LogToChannel(DiscordChannel channel)
	{
		while (_logEntries.Count > 0)
		{
			LogEntry entry = _logEntries[0];
			await channel.SendMessageAsyncSafe(entry.Message, entry.Embed);
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

	private sealed record LogEntry(string? Message, DiscordEmbed? Embed);
}
