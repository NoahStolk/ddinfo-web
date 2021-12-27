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

		LeaderboardResponse? leaderboard = null;
		List<EntryResponse> entries = new();

		for (int i = 0; i < 5;)
		{
			LeaderboardResponse? part = await LeaderboardClient.Instance.GetLeaderboard(100 * i + 1);
			if (part == null)
			{
				// Servers down, wait a few seconds.
				await Task.Delay(5000, stoppingToken);
				continue;
			}

			if (leaderboard == null)
				leaderboard = part; // The entries assigned here are unused. We use the entries list instead.

			entries.AddRange(part.Entries);

			i++;
		}

		entries = entries.OrderBy(e => e.Rank).ToList();

		LeaderboardHistory jsonModel = ConvertToJsonModel(leaderboard!, entries);

		string fileName = $"{DateTime.UtcNow:yyyyMMddHHmm}.json";
		File.WriteAllText(Path.Combine(_fileSystemService.GetPath(DataSubDirectory.LeaderboardHistory), fileName), JsonConvert.SerializeObject(jsonModel));
		Logger.LogInformation("Task execution for `{service}` succeeded. `{fileName}` with {entries} entries was created.", nameof(LeaderboardHistoryBackgroundService), fileName, entries.Count);
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

	private static LeaderboardHistory ConvertToJsonModel(LeaderboardResponse leaderboard, List<EntryResponse> entries) => new()
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
