using DevilDaggersInfo.Web.BlazorWasm.Server.HostedServices.DdInfoDiscordBot;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.HostedServices;

public class LeaderboardHistoryBackgroundService : AbstractBackgroundService
{
	private readonly IWebHostEnvironment _environment;

	public LeaderboardHistoryBackgroundService(IWebHostEnvironment environment, BackgroundServiceMonitor backgroundServiceMonitor, DiscordLogger discordLogger)
		: base(backgroundServiceMonitor, discordLogger)
	{
		_environment = environment;
	}

	protected override TimeSpan Interval => TimeSpan.FromMinutes(1);

	protected override async Task ExecuteTaskAsync(CancellationToken stoppingToken)
	{
		// We want to retry until the file exists. We cannot just check the date, because in case the task fails, we want to try again the next minute.
		if (HistoryFileExistsForDate(DateTime.UtcNow))
			return;

		LeaderboardResponse? l = await LeaderboardClient.Instance.GetLeaderboard(1);
		if (l != null)
		{
			string fileName = $"{DateTime.UtcNow:yyyyMMddHHmm}.json";
			File.WriteAllText(Path.Combine(_environment.WebRootPath, "leaderboard-history", fileName), JsonConvert.SerializeObject(l));
			await DiscordLogger.TryLog(Channel.MonitoringTask, $":white_check_mark: Task execution for `{nameof(LeaderboardHistoryBackgroundService)}` succeeded. `{fileName}` was created.");
		}
		else
		{
			await DiscordLogger.TryLog(Channel.MonitoringTask, $":x: Task execution for `{nameof(LeaderboardHistoryBackgroundService)}` failed because the Devil Daggers servers didn't return a leaderboard.");
		}
	}

	private bool HistoryFileExistsForDate(DateTime dateTime)
	{
		foreach (string path in Directory.GetFiles(Path.Combine(_environment.WebRootPath, "leaderboard-history"), "*.json"))
		{
			string fileName = Path.GetFileNameWithoutExtension(path);
			if (HistoryUtils.HistoryJsonFileNameToDateTime(fileName).Date == dateTime.Date)
				return true;
		}

		return false;
	}
}
