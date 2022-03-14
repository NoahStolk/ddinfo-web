using DevilDaggersInfo.Web.BlazorWasm.Server.InternalModels;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.HostedServices;

public class LeaderboardHistoryBackgroundService : AbstractBackgroundService
{
	private readonly IFileSystemService _fileSystemService;
	private readonly LeaderboardClient _leaderboardClient;

	public LeaderboardHistoryBackgroundService(IFileSystemService fileSystemService, LeaderboardClient leaderboardClient, BackgroundServiceMonitor backgroundServiceMonitor, ILogger<LeaderboardHistoryBackgroundService> logger)
		: base(backgroundServiceMonitor, logger)
	{
		_fileSystemService = fileSystemService;
		_leaderboardClient = leaderboardClient;
	}

	protected override TimeSpan Interval => TimeSpan.FromMinutes(1);

	protected override async Task ExecuteTaskAsync(CancellationToken stoppingToken)
	{
		// We want to retry until the file exists. We cannot just check the date, because in case the task fails, we want to try again the next minute.
		if (HistoryFileExistsForDate(DateTime.UtcNow))
			return;

		LeaderboardResponse? leaderboard = null;
		List<EntryResponse> entries = new();

		const int attempts = 5;
		const int interval = 5;
		for (int i = 0; i < attempts;)
		{
			LeaderboardResponse? part = await _leaderboardClient.GetLeaderboard(100 * i + 1);
			if (part == null)
			{
				Logger.LogWarning("Couldn't get leaderboard. Waiting {interval} seconds...", interval);

				await Task.Delay(TimeSpan.FromSeconds(interval), stoppingToken);
				continue;
			}

			if (leaderboard == null)
				leaderboard = part; // The LeaderboardResponse.Entries property here is unused. We use the entries list instead.

			entries.AddRange(part.Entries);

			i++;
		}

		if (leaderboard == null)
		{
			Logger.LogWarning("Leaderboard could not be retrieved after {attempts} attempts.", attempts);
			return;
		}

		entries = entries.OrderBy(e => e.Rank).ToList();

		LeaderboardHistory historyModel = ConvertToHistoryModel(leaderboard, entries);

		string fileName = $"{DateTime.UtcNow:yyyyMMddHHmm}.bin";
		string fullPath = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.LeaderboardHistory), fileName);
		File.WriteAllBytes(fullPath, historyModel.ToBytes());
		Logger.LogInformation("Task execution for `{service}` succeeded. `{fileName}` with {entries} entries was created.", nameof(LeaderboardHistoryBackgroundService), fullPath, entries.Count);
	}

	private bool HistoryFileExistsForDate(DateTime dateTime)
	{
		foreach (string path in Directory.GetFiles(_fileSystemService.GetPath(DataSubDirectory.LeaderboardHistory), "*.bin"))
		{
			string fileName = Path.GetFileNameWithoutExtension(path);
			if (HistoryUtils.HistoryFileNameToDateTime(fileName).Date == dateTime.Date)
				return true;
		}

		return false;
	}

	private static LeaderboardHistory ConvertToHistoryModel(LeaderboardResponse leaderboard, List<EntryResponse> entries) => new()
	{
		DaggersFiredGlobal = leaderboard.DaggersFiredGlobal,
		DaggersHitGlobal = leaderboard.DaggersHitGlobal,
		DateTime = leaderboard.DateTime,
		DeathsGlobal = leaderboard.DeathsGlobal,
		Entries = entries.ConvertAll(e => new EntryHistory
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
		GemsGlobal = leaderboard.GemsGlobal,
		KillsGlobal = leaderboard.KillsGlobal,
		Players = leaderboard.TotalPlayers,
		TimeGlobal = leaderboard.TimeGlobal,
	};
}
