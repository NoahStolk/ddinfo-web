using Microsoft.Extensions.Options;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Logging.Discord;

public sealed class DiscordLoggerProvider : ILoggerProvider
{
	private readonly IDisposable _onChangeToken;
	private readonly ConcurrentDictionary<string, DiscordLogger> _loggers = new();

	private DiscordLoggerConfiguration _currentConfig;

	public DiscordLoggerProvider(IOptionsMonitor<DiscordLoggerConfiguration> config)
	{
		_currentConfig = config.CurrentValue;
		_onChangeToken = config.OnChange(updatedConfig => _currentConfig = updatedConfig);
	}

	public ILogger CreateLogger(string categoryName) =>
		_loggers.GetOrAdd(categoryName, name => new DiscordLogger(name, GetCurrentConfig));

	private DiscordLoggerConfiguration GetCurrentConfig() => _currentConfig;

	public void Dispose()
	{
		_loggers.Clear();
		_onChangeToken.Dispose();
	}
}
