using DSharpPlus.Entities;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Services;

public class LogContainerService
{
	private readonly List<LogEntry> _logEntries = new();
	private readonly List<string> _validClLogs = new();
	private readonly List<string> _invalidClLogs = new();

	public void Add(DiscordEmbed embed) => _logEntries.Add(new(null, embed));

	public void Add(string message) => _logEntries.Add(new(message, null));

	public void AddClLog(bool valid, string message)
	{
		if (valid)
			_validClLogs.Add(message);
		else
			_invalidClLogs.Add(message);
	}

	public async Task LogToChannel(DiscordChannel channel)
	{
		while (_logEntries.Count > 0)
		{
			LogEntry entry = _logEntries[0];
			await channel.SendMessageAsyncSafe(entry.Message, entry.Embed);
			_logEntries.RemoveAt(0);
		}
	}

	public async Task LogClLogsToChannel(bool valid, DiscordChannel channel)
	{
		List<string> logs = valid ? _validClLogs : _invalidClLogs;
		if (logs.Count > 0)
		{
			await channel.SendMessageAsyncSafe(string.Join(Environment.NewLine, logs));
			logs.Clear();
		}
	}

	private sealed record LogEntry(string? Message, DiscordEmbed? Embed);
}
