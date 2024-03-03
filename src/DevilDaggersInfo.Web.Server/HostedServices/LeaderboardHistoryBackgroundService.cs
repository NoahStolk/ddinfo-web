using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Models.LeaderboardHistory;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using DevilDaggersInfo.Web.Server.Services;
using DevilDaggersInfo.Web.Server.Utils;

namespace DevilDaggersInfo.Web.Server.HostedServices;

public class LeaderboardHistoryBackgroundService : AbstractBackgroundService
{
	private readonly IFileSystemService _fileSystemService;
	private readonly IDdLeaderboardService _leaderboardClient;

	public LeaderboardHistoryBackgroundService(IFileSystemService fileSystemService, IDdLeaderboardService leaderboardClient, BackgroundServiceMonitor backgroundServiceMonitor, ILogger<LeaderboardHistoryBackgroundService> logger)
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

		IDdLeaderboardService.LeaderboardResponse? leaderboard = null;
		List<IDdLeaderboardService.EntryResponse> entries = new();

		const int leaderboardPageCount = 5;
		const int playerPerPage = 100;
		for (int i = 0; i < leaderboardPageCount;)
		{
			IDdLeaderboardService.LeaderboardResponse response;
			try
			{
				response = await _leaderboardClient.GetLeaderboard(playerPerPage * i + 1, 100);
			}
			catch (DdLeaderboardException)
			{
				const int interval = 5;
				Logger.LogWarning("Couldn't get DD leaderboard (page {Page} of {Total}). Waiting {Interval} seconds...", i, leaderboardPageCount, interval);

				await Task.Delay(TimeSpan.FromSeconds(interval), stoppingToken);
				continue; // Continue without increasing i, so the request is retried.
			}

			leaderboard ??= response; // The LeaderboardResponse.Entries property here is unused. We use the entries local instead.

			entries.AddRange(response.Entries);

			i++;
		}

		if (entries.Count != leaderboardPageCount * playerPerPage)
			Logger.LogWarning("Leaderboard entries count ({Count}) does not match expected count ({ExpectedCount}). Duplicates and ranks below the expected count will be removed.", entries.Count, leaderboardPageCount * playerPerPage);

		entries = entries.DistinctBy(e => e.Rank).Where(e => e.Rank <= leaderboardPageCount * playerPerPage).OrderBy(e => e.Rank).ToList();

		if (entries.Count != leaderboardPageCount * playerPerPage)
			Logger.LogWarning("Leaderboard entries count ({Count}) does not match expected count ({ExpectedCount}). Some ranks appear to be missing.", entries.Count, leaderboardPageCount * playerPerPage);

		// ! Loop is endless.
		LeaderboardHistory historyModel = ConvertToHistoryModel(leaderboard!, entries);

		string fileName = $"{DateTime.UtcNow:yyyyMMddHHmm}.bin";
		string fullPath = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.LeaderboardHistory), fileName);
		await IoFile.WriteAllBytesAsync(fullPath, historyModel.ToBytes(), stoppingToken);
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

	private static LeaderboardHistory ConvertToHistoryModel(IDdLeaderboardService.LeaderboardResponse leaderboard, List<IDdLeaderboardService.EntryResponse> entries) => new()
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
