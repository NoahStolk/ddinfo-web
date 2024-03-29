using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Services.Caching;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using System.Diagnostics;
using System.Text;

namespace DevilDaggersInfo.Web.Server.HostedServices;

public class StartupCacheHostedService : IHostedService
{
	private readonly IWebHostEnvironment _env;
	private readonly IFileSystemService _fileSystemService;
	private readonly ILogContainerService _logContainerService;
	private readonly LeaderboardStatisticsCache _leaderboardStatisticsCache;
	private readonly ILeaderboardHistoryCache _leaderboardHistoryCache;
	private readonly ModArchiveCache _modArchiveCache;

	public StartupCacheHostedService(
		IWebHostEnvironment env,
		IFileSystemService fileSystemService,
		ILogContainerService logContainerService,
		LeaderboardStatisticsCache leaderboardStatisticsCache,
		ILeaderboardHistoryCache leaderboardHistoryCache,
		ModArchiveCache modArchiveCache)
	{
		_env = env;
		_fileSystemService = fileSystemService;
		_logContainerService = logContainerService;
		_leaderboardStatisticsCache = leaderboardStatisticsCache;
		_leaderboardHistoryCache = leaderboardHistoryCache;
		_modArchiveCache = modArchiveCache;
	}

	public async Task StartAsync(CancellationToken cancellationToken)
	{
		Stopwatch sw = Stopwatch.StartNew();
		StringBuilder sb = new();

		// Initiate static caches.
		_leaderboardStatisticsCache.Initiate();

		sb.Append("- `LeaderboardStatisticsCache` initiation done at ").Append(sw.ElapsedMilliseconds.ToString("N0")).AppendLine(" ms");

		// Initiate dynamic caches.

		// SpawnsetSummaryCache does not need to be initiated as it is fast enough.

		// LeaderboardHistoryCache will be initiated here.
		foreach (string historyFilePath in _fileSystemService.TryGetFiles(DataSubDirectory.LeaderboardHistory).Where(p => p.EndsWith(".bin")))
			_leaderboardHistoryCache.GetLeaderboardHistoryByFilePath(historyFilePath);

		sb.Append("- `LeaderboardHistoryCache` initiation done at ").Append(sw.ElapsedMilliseconds.ToString("N0")).AppendLine(" ms");

		/* The ModArchiveCache is initially very slow because it requires unzipping huge mod archive zip files.
		 * The idea to fix this; when adding data (based on a mod archive) to the ConcurrentBag, write this data to a JSON file as well, so it is not lost when the site shuts down.
		 * The cache then needs to be initiated here, by reading all the JSON files and populating the ConcurrentBag on start up.*/
		await _modArchiveCache.LoadEntireFileCacheAsync();

		sb.Append("- `ModArchiveCache` initiation done at ").Append(sw.ElapsedMilliseconds.ToString("N0")).AppendLine(" ms");

		if (!_env.IsDevelopment())
			_logContainerService.AddLog($"{DateTime.UtcNow:HH:mm:ss.fff}: Initiating caches...\n{sb}");

		sw.Stop();
	}

	public Task StopAsync(CancellationToken cancellationToken)
	{
		return Task.CompletedTask;
	}
}
