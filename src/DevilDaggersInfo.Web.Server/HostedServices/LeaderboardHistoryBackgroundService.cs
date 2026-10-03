using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Models.LeaderboardHistory;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using DevilDaggersInfo.Web.Server.Services;
using DevilDaggersInfo.Web.Server.Utils;

namespace DevilDaggersInfo.Web.Server.HostedServices;

internal sealed class LeaderboardHistoryBackgroundService(
	IFileSystemService fileSystemService,
	IDdLeaderboardService leaderboardClient,
	BackgroundServiceMonitor backgroundServiceMonitor,
	ILogger<LeaderboardHistoryBackgroundService> logger)
	: AbstractBackgroundService(backgroundServiceMonitor, logger)
{
	private const int _leaderboardPageCount = 5;
	private const int _playersPerPage = 100;

	protected override TimeSpan Interval => TimeSpan.FromMinutes(1);

	protected override async Task ExecuteTaskAsync(CancellationToken stoppingToken)
	{
		// We want to retry until the file exists. We cannot just check the date, because in case the task fails, we want to try again the next minute.
		if (HistoryFileExistsForDate(DateTime.UtcNow))
			return;

		IDdLeaderboardService.LeaderboardResponse? leaderboard = null;
		List<IDdLeaderboardService.EntryResponse> entries = [];

		for (int i = 0; i < _leaderboardPageCount; i++)
		{
			IDdLeaderboardService.LeaderboardResponse? response = await GetLeaderboardPage(_playersPerPage * i + 1, stoppingToken);
			if (response == null)
			{
				// Give up and let the next run start over, so an outage doesn't keep this task running (and logging) indefinitely.
				Logger.LogInformation("Couldn't get DD leaderboard (page {Page} of {Total}). Trying again in {Interval}.", i + 1, _leaderboardPageCount, Interval);
				return;
			}

			leaderboard ??= response; // The LeaderboardResponse.Entries property here is unused. We use the entries local instead.

			entries.AddRange(response.Entries);
		}

		if (entries.Count != _leaderboardPageCount * _playersPerPage)
			Logger.LogWarning("Leaderboard entries count ({Count}) does not match expected count ({ExpectedCount}). Duplicates and ranks below the expected count will be removed.", entries.Count, _leaderboardPageCount * _playersPerPage);

		entries = [.. entries.DistinctBy(e => e.Rank).Where(e => e.Rank <= _leaderboardPageCount * _playersPerPage).OrderBy(e => e.Rank)];

		if (entries.Count != _leaderboardPageCount * _playersPerPage)
			Logger.LogWarning("Leaderboard entries count ({Count}) does not match expected count ({ExpectedCount}). Some ranks appear to be missing.", entries.Count, _leaderboardPageCount * _playersPerPage);

		// ! Every page was fetched, so the first response is set.
		LeaderboardHistory historyModel = ConvertToHistoryModel(leaderboard!, entries);

		string fileName = $"{DateTime.UtcNow:yyyyMMddHHmm}.bin";
		string fullPath = Path.Combine(fileSystemService.GetPath(DataSubDirectory.LeaderboardHistory), fileName);
		await IoFile.WriteAllBytesAsync(fullPath, historyModel.ToBytes(), stoppingToken);
	}

	private async Task<IDdLeaderboardService.LeaderboardResponse?> GetLeaderboardPage(int rankStart, CancellationToken stoppingToken)
	{
		const int maxAttempts = 3;
		for (int attempt = 1; attempt <= maxAttempts; attempt++)
		{
			try
			{
				return await leaderboardClient.GetLeaderboard(rankStart, _playersPerPage);
			}
			catch (DdLeaderboardException) when (attempt < maxAttempts)
			{
				// Back off 5 seconds, then 10 seconds.
				await Task.Delay(TimeSpan.FromSeconds(5 << (attempt - 1)), stoppingToken);
			}
			catch (DdLeaderboardException)
			{
				// The leaderboard service already logged the failure.
			}
		}

		return null;
	}

	private bool HistoryFileExistsForDate(DateTime dateTime)
	{
		foreach (string path in Directory.GetFiles(fileSystemService.GetPath(DataSubDirectory.LeaderboardHistory), "*.bin"))
		{
			string fileName = Path.GetFileNameWithoutExtension(path);
			if (HistoryUtils.HistoryFileNameToDateTime(fileName).Date == dateTime.Date)
				return true;
		}

		return false;
	}

	private static LeaderboardHistory ConvertToHistoryModel(IDdLeaderboardService.LeaderboardResponse leaderboard, List<IDdLeaderboardService.EntryResponse> entries)
	{
		return new LeaderboardHistory
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
}
