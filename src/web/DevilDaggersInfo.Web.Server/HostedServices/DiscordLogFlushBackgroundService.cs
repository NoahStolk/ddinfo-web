using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
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
	private readonly ICustomLeaderboardHighscoreLogger _customLeaderboardHighscoreLogger;
	private readonly IWebHostEnvironment _environment;
	private readonly ILogger<DiscordLogFlushBackgroundService> _logger;

	public DiscordLogFlushBackgroundService(
		ILogContainerService logContainerService,
		ICustomLeaderboardHighscoreLogger customLeaderboardHighscoreLogger,
		IWebHostEnvironment environment,
		BackgroundServiceMonitor backgroundServiceMonitor,
		ILogger<DiscordLogFlushBackgroundService> logger)
		: base(backgroundServiceMonitor, logger)
	{
		_logContainerService = logContainerService;
		_customLeaderboardHighscoreLogger = customLeaderboardHighscoreLogger;
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

		foreach (CustomLeaderboardHighscoreLog highscoreLog in _customLeaderboardHighscoreLogger.GetHighscoreLogs())
			await LogHighscore(highscoreLog);

		_customLeaderboardHighscoreLogger.ClearHighscoreLogs();
	}

	private async Task LogClLogsToChannel(bool valid, DiscordChannel channel)
	{
		const int timeoutInSeconds = 1;

		IReadOnlyList<string> logs = _customLeaderboardHighscoreLogger.GetLogs(valid);
		if (logs.Count > 0)
		{
			if (await channel.SendMessageAsyncSafe(string.Join(Environment.NewLine, logs)))
				_customLeaderboardHighscoreLogger.ClearLogs(valid);
			else
				await Task.Delay(TimeSpan.FromSeconds(timeoutInSeconds));
		}
	}

	private async Task LogHighscore(CustomLeaderboardHighscoreLog highscoreLog)
	{
		try
		{
			string thumbnailImage = highscoreLog.RankSorting switch
			{
				CustomLeaderboardRankSorting.TimeAsc or CustomLeaderboardRankSorting.TimeDesc => "stopwatch.png",
				CustomLeaderboardRankSorting.GemsCollectedAsc or CustomLeaderboardRankSorting.GemsCollectedDesc or CustomLeaderboardRankSorting.GemsDespawnedAsc or CustomLeaderboardRankSorting.GemsDespawnedDesc or CustomLeaderboardRankSorting.GemsEatenAsc or CustomLeaderboardRankSorting.GemsEatenDesc => "gem.png",
				CustomLeaderboardRankSorting.EnemiesKilledAsc or CustomLeaderboardRankSorting.EnemiesKilledDesc or CustomLeaderboardRankSorting.EnemiesAliveAsc or CustomLeaderboardRankSorting.EnemiesAliveDesc => "skull.png",
				CustomLeaderboardRankSorting.HomingStoredAsc or CustomLeaderboardRankSorting.HomingStoredDesc or CustomLeaderboardRankSorting.HomingEatenAsc or CustomLeaderboardRankSorting.HomingEatenDesc => "homing.png",
				_ => "eye2.png",
			};

			DiscordEmbedBuilder builder = new()
			{
				Title = highscoreLog.Message,
				Color = highscoreLog.Dagger.GetDiscordColor(),
				Url = $"https://devildaggers.info/custom/leaderboard/{highscoreLog.CustomLeaderboardId}",
				Thumbnail = new()
				{
					Url = $"https://devildaggers.info/images/icons/discord-bot/{thumbnailImage}",
					Height = 32,
					Width = 32,
				},
			};
			builder.AddFieldObject("Game Mode", highscoreLog.SpawnsetGameMode, false);
			builder.AddFieldObject(highscoreLog.ScoreField, highscoreLog.ScoreValue, true);
			builder.AddFieldObject("Rank", highscoreLog.RankValue, true);

			DiscordChannel? discordChannel = DiscordServerConstants.GetDiscordChannel(Channel.CustomLeaderboards, _environment);
			if (discordChannel == null)
				return;

			await discordChannel.SendMessageAsyncSafe(null, builder.Build());
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error while attempting to send leaderboard message.");
		}
	}

	private async Task LogToLogChannel(DiscordChannel logChannel) => await LogEntries(_logContainerService.LogEntries, logChannel);

	private async Task LogToAuditLogChannel(DiscordChannel auditLogChannel) => await LogEntries(_logContainerService.AuditLogEntries, auditLogChannel);

	private static async Task LogEntries(List<string> entries, DiscordChannel channel)
	{
		const int errorTimeoutInSeconds = 1;

		while (entries.Count > 0)
		{
			string entry = entries[0];
			if (await channel.SendMessageAsyncSafe(entry))
				entries.RemoveAt(0);
			else
				await Task.Delay(TimeSpan.FromSeconds(errorTimeoutInSeconds));
		}
	}
}
