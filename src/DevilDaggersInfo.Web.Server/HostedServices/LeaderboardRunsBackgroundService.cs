using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;

namespace DevilDaggersInfo.Web.Server.HostedServices;

public class LeaderboardRunsBackgroundService : AbstractBackgroundService
{
	private readonly IDdLeaderboardService _ddLeaderboardService;

	public LeaderboardRunsBackgroundService(BackgroundServiceMonitor backgroundServiceMonitor, ILogger<AbstractBackgroundService> logger, IDdLeaderboardService ddLeaderboardService)
		: base(backgroundServiceMonitor, logger)
	{
		_ddLeaderboardService = ddLeaderboardService;
	}

	protected override TimeSpan Interval => TimeSpan.Zero;

	protected override async Task ExecuteTaskAsync(CancellationToken stoppingToken)
	{
		// Timestamp for first run: 1641688503
		// Timestamp for first 100 runs: 1641688920
		// Idea:
		// Start with timestamp NOW.
		// After processing, save a checkpoint with values: timestampFirst, timestampLast, runIdFirst, runIdLast, bool done.
		// Use this checkpoint in all following iterations, until it reaches the end (when runIdLast is 1 OR runIdLast is lower than runIdFirst from any checkpoint, set done to true). Keep using and updating the same checkpoint with new values for timestampLast and runIdLast.
		// When a checkpoint is done, move onto the next checkpoint that is not done. If all checkpoints are done, wait 10 minutes and then create a new one starting at NOW again.
		DateTime before = DateTime.UtcNow;
		List<IDdLeaderboardService.RunResponse> runs = await _ddLeaderboardService.GetRunsByTimestamp(before, 100);
	}
}
