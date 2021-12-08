using Microsoft.Extensions.Options;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Logging.Discord;

public sealed class DiscordLoggerProvider : ILoggerProvider
{
	private readonly IDisposable _onChangeToken;
	private readonly ConcurrentDictionary<string, DiscordLogger> _loggers = new();

	private readonly IWebHostEnvironment _environment;

	private DiscordLoggerConfiguration _currentConfig;

	public DiscordLoggerProvider(IWebHostEnvironment environment, IOptionsMonitor<DiscordLoggerConfiguration> config)
	{
		_environment = environment;

		_currentConfig = config.CurrentValue;
		_onChangeToken = config.OnChange(updatedConfig => _currentConfig = updatedConfig);
	}

	public ILogger CreateLogger(string categoryName) =>
		_loggers.GetOrAdd(categoryName, name => new DiscordLogger(name, _environment, GetCurrentConfig));

	private DiscordLoggerConfiguration GetCurrentConfig() => _currentConfig;

	public void Dispose()
	{
		_loggers.Clear();
		_onChangeToken.Dispose();
	}
}
