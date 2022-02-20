using Microsoft.Extensions.Options;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Logging.Discord;

public sealed class DiscordLoggerProvider : ILoggerProvider
{
	private readonly IDisposable _onChangeToken;
	private readonly ConcurrentDictionary<string, DiscordLogger> _loggers = new();

	private readonly IWebHostEnvironment _environment;
	private readonly LogContainerService _logContainerService;

	private DiscordLoggerConfiguration _currentConfig;

	public DiscordLoggerProvider(IWebHostEnvironment environment, LogContainerService logContainerService, IOptionsMonitor<DiscordLoggerConfiguration> config)
	{
		_environment = environment;
		_logContainerService = logContainerService;

		_currentConfig = config.CurrentValue;
		_onChangeToken = config.OnChange(updatedConfig => _currentConfig = updatedConfig);
	}

	public ILogger CreateLogger(string categoryName) =>
		_loggers.GetOrAdd(categoryName, name => new DiscordLogger(name, _environment, _logContainerService, GetCurrentConfig));

	private DiscordLoggerConfiguration GetCurrentConfig() => _currentConfig;

	public void Dispose()
	{
		_loggers.Clear();
		_onChangeToken.Dispose();
	}
}
