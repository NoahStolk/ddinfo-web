using DSharpPlus.Entities;

namespace DevilDaggersInfo.Web.Server.Services;

public class LogContainerService
{
	private const int _timeoutInSeconds = 1;

	private readonly List<LogEntry> _auditLogEntries = new();
	private readonly List<LogEntry> _logEntries = new();

	public void Add(DiscordEmbed embed) => _logEntries.Add(new(null, embed));

	public void Add(string message) => _logEntries.Add(new(message, null));

	public void AddAuditLog(string message) => _auditLogEntries.Add(new(message, null));

	public async Task LogToLogChannel(DiscordChannel logChannel) => await LogEntries(_logEntries, logChannel);

	public async Task LogToAuditLogChannel(DiscordChannel auditLogChannel) => await LogEntries(_auditLogEntries, auditLogChannel);

	// TODO: This responsibility should be moved to the DiscordLogFlushBackgroundService.
	private static async Task LogEntries(List<LogEntry> entries, DiscordChannel channel)
	{
		while (entries.Count > 0)
		{
			LogEntry entry = entries[0];
			if (await channel.SendMessageAsyncSafe(entry.Message, entry.Embed))
				entries.RemoveAt(0);
			else
				await Task.Delay(TimeSpan.FromSeconds(_timeoutInSeconds));
		}
	}

	private sealed record LogEntry(string? Message, DiscordEmbed? Embed);
}
