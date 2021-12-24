using DevilDaggersInfo.Web.BlazorWasm.Server.InternalModels.Json;

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
			LeaderboardHistory jsonModel = new()
			{
				DaggersFiredGlobal = l.DaggersFiredGlobal,
				DaggersHitGlobal = l.DaggersHitGlobal,
				DateTime = l.DateTime,
				DeathsGlobal = l.DeathsGlobal,
				Entries = l.Entries.ConvertAll(e => new EntryHistory
				{
					DaggersFired = e.DaggersFired,
					DaggersFiredTotal = e.DaggersFiredTotal,
					DaggersHit = e.DaggersHit,
					DaggersHitTotal = e.DaggersHitTotal,
					DeathsTotal = e.DeathsTotal,
					DeathType = (byte)e.DeathType,
					Gems = e.Gems,
					GemsTotal = e.GemsTotal,
					Id = e.Id,
					Kills = e.Kills,
					KillsTotal = e.KillsTotal,
					Rank = e.Rank,
					Time = e.Time,
					TimeTotal = e.TimeTotal,
					Username = e.Username,
				}),
				GemsGlobal = l.GemsGlobal,
				KillsGlobal = l.KillsGlobal,
				Players = l.TotalPlayers,
				TimeGlobal = l.TimeGlobal,
			};

			string fileName = $"{DateTime.UtcNow:yyyyMMddHHmm}.json";
			File.WriteAllText(Path.Combine(_fileSystemService.GetPath(DataSubDirectory.LeaderboardHistory), fileName), JsonConvert.SerializeObject(jsonModel));
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
