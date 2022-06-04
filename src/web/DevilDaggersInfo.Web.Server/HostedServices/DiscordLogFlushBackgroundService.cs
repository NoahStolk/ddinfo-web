using DevilDaggersInfo.Web.Server.Domain.Services;
using DevilDaggersInfo.Web.Server.HostedServices.DdInfoDiscordBot;
using DSharpPlus.Entities;

namespace DevilDaggersInfo.Web.Server.HostedServices;

public class DiscordLogFlushBackgroundService : AbstractBackgroundService
{
	private const int _timeoutInSeconds = 1;

	private readonly LogContainerService _logContainerService;
	private readonly ICustomLeaderboardSubmissionLogger _customLeaderboardSubmissionLogger;
	private readonly IWebHostEnvironment _environment;

	public DiscordLogFlushBackgroundService(LogContainerService logContainerService, ICustomLeaderboardSubmissionLogger customLeaderboardSubmissionLogger, IWebHostEnvironment environment, BackgroundServiceMonitor backgroundServiceMonitor, ILogger<DiscordLogFlushBackgroundService> logger)
		: base(backgroundServiceMonitor, logger)
	{
		_logContainerService = logContainerService;
		_customLeaderboardSubmissionLogger = customLeaderboardSubmissionLogger;
		_environment = environment;
	}

	protected override TimeSpan Interval => TimeSpan.FromSeconds(2);

	protected override async Task ExecuteTaskAsync(CancellationToken stoppingToken)
	{
		DiscordChannel? logChannel = DiscordServerConstants.GetDiscordChannel(Channel.MonitoringLog, _environment);
		if (logChannel != null)
			await _logContainerService.LogToLogChannel(logChannel);

		DiscordChannel? auditLogChannel = DiscordServerConstants.GetDiscordChannel(Channel.MaintainersAuditLog, _environment);
		if (auditLogChannel != null)
			await _logContainerService.LogToAuditLogChannel(auditLogChannel);

		DiscordChannel? validClLogChannel = DiscordServerConstants.GetDiscordChannel(Channel.MonitoringCustomLeaderboardValid, _environment);
		if (validClLogChannel != null)
			await LogClLogsToChannel(true, validClLogChannel);

		DiscordChannel? invalidClLogChannel = DiscordServerConstants.GetDiscordChannel(Channel.MonitoringCustomLeaderboardInvalid, _environment);
		if (invalidClLogChannel != null)
			await LogClLogsToChannel(false, invalidClLogChannel);
	}

	private async Task LogClLogsToChannel(bool valid, DiscordChannel channel)
	{
		IReadOnlyList<string> logs = _customLeaderboardSubmissionLogger.GetLogs(valid);
		if (logs.Count > 0)
		{
			if (await channel.SendMessageAsyncSafe(string.Join(Environment.NewLine, logs)))
				_customLeaderboardSubmissionLogger.ClearLogs(valid);
			else
				await Task.Delay(TimeSpan.FromSeconds(_timeoutInSeconds));
		}
	}
}
