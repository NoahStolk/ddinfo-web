using DevilDaggersInfo.Common;
using DevilDaggersInfo.Common.Extensions;
using DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using DevilDaggersInfo.Web.Server.Extensions;
using DevilDaggersInfo.Web.Server.HostedServices.DdInfoDiscordBot;
using DevilDaggersInfo.Web.Server.Services;
using DSharpPlus.Entities;

namespace DevilDaggersInfo.Web.Server.HostedServices;

public class DiscordLogFlushBackgroundService : AbstractBackgroundService
{
	private readonly ILogContainerService _logContainerService;
	private readonly ICustomLeaderboardSubmissionLogger _customLeaderboardSubmissionLogger;
	private readonly IWebHostEnvironment _environment;
	private readonly ILogger<DiscordLogFlushBackgroundService> _logger;

	public DiscordLogFlushBackgroundService(
		ILogContainerService logContainerService,
		ICustomLeaderboardSubmissionLogger customLeaderboardSubmissionLogger,
		IWebHostEnvironment environment,
		BackgroundServiceMonitor backgroundServiceMonitor,
		ILogger<DiscordLogFlushBackgroundService> logger)
		: base(backgroundServiceMonitor, logger)
	{
		_logContainerService = logContainerService;
		_customLeaderboardSubmissionLogger = customLeaderboardSubmissionLogger;
		_environment = environment;
		_logger = logger;
	}

	protected override TimeSpan Interval => TimeSpan.FromSeconds(2);

	protected override async Task ExecuteTaskAsync(CancellationToken stoppingToken)
	{
		DiscordChannel? logChannel = DiscordServerConstants.GetDiscordChannel(Channel.MonitoringLog, _environment);
		if (logChannel != null)
			await LogToLogChannel(logChannel);

		DiscordChannel? auditLogChannel = DiscordServerConstants.GetDiscordChannel(Channel.MaintainersAuditLog, _environment);
		if (auditLogChannel != null)
			await LogToAuditLogChannel(auditLogChannel);

		DiscordChannel? validClLogChannel = DiscordServerConstants.GetDiscordChannel(Channel.MonitoringCustomLeaderboardValid, _environment);
		if (validClLogChannel != null)
			await LogClLogsToChannel(true, validClLogChannel);

		DiscordChannel? invalidClLogChannel = DiscordServerConstants.GetDiscordChannel(Channel.MonitoringCustomLeaderboardInvalid, _environment);
		if (invalidClLogChannel != null)
			await LogClLogsToChannel(false, invalidClLogChannel);

		foreach (CustomLeaderboardHighscoreLog highscoreLog in _customLeaderboardSubmissionLogger.GetHighscoreLogs())
			await LogHighscore(highscoreLog);

		_customLeaderboardSubmissionLogger.ClearHighscoreLogs();
	}

	private async Task LogClLogsToChannel(bool valid, DiscordChannel channel)
	{
		const int timeoutInSeconds = 1;

		IReadOnlyList<string> logs = _customLeaderboardSubmissionLogger.GetLogs(valid);
		if (logs.Count > 0)
		{
			if (await channel.SendMessageAsyncSafe(string.Join(Environment.NewLine, logs)))
				_customLeaderboardSubmissionLogger.ClearLogs(valid);
			else
				await Task.Delay(TimeSpan.FromSeconds(timeoutInSeconds));
		}
	}

	private async Task LogHighscore(CustomLeaderboardHighscoreLog highscoreLog)
	{
		try
		{
			DiscordEmbedBuilder builder = new()
			{
				Title = highscoreLog.Message,
				Color = highscoreLog.Dagger.GetDiscordColor(),
				Url = $"https://devildaggers.info/custom/leaderboard/{highscoreLog.CustomLeaderboardId}",
			};
			builder.AddFieldObject("Score", FormatTimeString(highscoreLog.Time.ToSecondsTime()), true);
			builder.AddFieldObject("Rank", $"{highscoreLog.Rank}/{highscoreLog.TotalPlayers}", true);

			DiscordChannel? discordChannel = DiscordServerConstants.GetDiscordChannel(Channel.CustomLeaderboards, _environment);
			if (discordChannel == null)
				return;

			await discordChannel.SendMessageAsyncSafe(null, builder.Build());
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error while attempting to send leaderboard message.");
		}

		static string FormatTimeString(double time)
			=> time.ToString(StringFormats.TimeFormat);
	}

	private async Task LogToLogChannel(DiscordChannel logChannel) => await LogEntries(_logContainerService.LogEntries, logChannel);

	private async Task LogToAuditLogChannel(DiscordChannel auditLogChannel) => await LogEntries(_logContainerService.AuditLogEntries, auditLogChannel);

	private static async Task LogEntries(List<string> entries, DiscordChannel channel)
	{
		const int timeoutInSeconds = 1;

		while (entries.Count > 0)
		{
			string entry = entries[0];
			if (await channel.SendMessageAsyncSafe(entry))
				entries.RemoveAt(0);
			else
				await Task.Delay(TimeSpan.FromSeconds(timeoutInSeconds));
		}
	}
}
