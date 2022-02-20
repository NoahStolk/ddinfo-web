using DSharpPlus.Entities;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Services;

public class LogContainerService
{
	private readonly List<DiscordEmbed> _logEntries = new();

	public IReadOnlyList<DiscordEmbed> LogEntries => _logEntries;

	public void Add(DiscordEmbed embed) => _logEntries.Add(embed);

	public void RemoveFirst() => _logEntries.RemoveAt(0);
}
