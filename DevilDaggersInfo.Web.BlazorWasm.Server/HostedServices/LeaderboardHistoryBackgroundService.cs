namespace DevilDaggersInfo.Web.BlazorWasm.Server.HostedServices;

public class LeaderboardHistoryBackgroundService : AbstractBackgroundService
{
	private readonly IFileSystemService _fileSystemService;

	public LeaderboardHistoryBackgroundService(IFileSystemService fileSystemService, BackgroundServiceMonitor backgroundServiceMonitor, ILogger<LeaderboardHistoryBackgroundService> logger)
		: base(backgroundServiceMonitor, logger)
	{
		_fileSystemService = fileSystemService;
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
			File.WriteAllText(Path.Combine(_fileSystemService.GetPath(DataSubDirectory.LeaderboardHistory), fileName), JsonConvert.SerializeObject(l));
			Logger.LogInformation("Task execution for `{service}` succeeded. `{fileName}` was created.", nameof(LeaderboardHistoryBackgroundService), fileName);
		}
		else
		{
			Logger.LogError("Task execution for `{service}` failed because the Devil Daggers servers didn't return a leaderboard.", nameof(LeaderboardHistoryBackgroundService));
		}
	}

	private bool HistoryFileExistsForDate(DateTime dateTime)
	{
		foreach (string path in Directory.GetFiles(_fileSystemService.GetPath(DataSubDirectory.LeaderboardHistory), "*.json"))
		{
			string fileName = Path.GetFileNameWithoutExtension(path);
			if (HistoryUtils.HistoryJsonFileNameToDateTime(fileName).Date == dateTime.Date)
				return true;
		}

		return false;
	}
}
