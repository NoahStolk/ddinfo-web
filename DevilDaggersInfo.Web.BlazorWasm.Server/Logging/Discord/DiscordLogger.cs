using DevilDaggersInfo.Web.BlazorWasm.Server.HostedServices;
using DSharpPlus.Entities;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Logging.Discord;

public class DiscordLogger : ILogger
{
	private readonly string _name;
	private readonly IWebHostEnvironment _environment;
	private readonly Func<DiscordLoggerConfiguration> _getCurrentConfig;

	public DiscordLogger(string name, IWebHostEnvironment environment, Func<DiscordLoggerConfiguration> getCurrentConfig)
	{
		_name = name;
		_environment = environment;
		_getCurrentConfig = getCurrentConfig;
	}

	public IDisposable BeginScope<TState>(TState state) => default!;

	public bool IsEnabled(LogLevel logLevel) => _getCurrentConfig().LogLevels.ContainsKey(logLevel);

	public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
	{
		if (!IsEnabled(logLevel))
			return;

		DiscordLoggerConfiguration config = _getCurrentConfig();
		if (config.EventId != 0 && config.EventId != eventId.Id)
			return;

		// Do not log immediately as this is async for Discord. TODO: Refactor.
		DiscordLogFlushBackgroundService.LogEntries.Add(BuildLog(logLevel, eventId, config, state, exception));
	}

	private DiscordEmbed BuildLog<TState>(LogLevel logLevel, EventId eventId, DiscordLoggerConfiguration config, TState? state, Exception? exception)
	{
		DiscordEmbedBuilder builder = new()
		{
			Title = (state?.ToString() ?? exception?.Message ?? "No title").TrimAfter(255),
			Color = config.LogLevels.ContainsKey(logLevel) ? config.LogLevels[logLevel] : DiscordColor.White,
		};

		builder.AddFieldObject("Environment", _environment.EnvironmentName);
		builder.AddFieldObject("Logger", _name);

		if (eventId.Name != null)
			builder.AddFieldObject(eventId.Name, eventId.Id);

		if (exception != null)
		{
			builder.AddError(exception);
			foreach (DictionaryEntry? data in exception.Data)
				builder.AddFieldObject(data?.Key?.ToString() ?? "null", data?.Value?.ToString() ?? "null");
		}

		return builder.Build();
	}
}
