using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;

namespace DevilDaggersInfo.Web.Server.HostedServices;

public class PlayerNameFetchBackgroundService : AbstractBackgroundService
{
	private readonly IServiceScopeFactory _serviceScopeFactory;
	private readonly IDdLeaderboardService _leaderboardClient;

	public PlayerNameFetchBackgroundService(IServiceScopeFactory serviceScopeFactory, IDdLeaderboardService leaderboardClient, BackgroundServiceMonitor backgroundServiceMonitor, ILogger<LeaderboardHistoryBackgroundService> logger)
		: base(backgroundServiceMonitor, logger)
	{
		_serviceScopeFactory = serviceScopeFactory;
		_leaderboardClient = leaderboardClient;
	}

	protected override TimeSpan Interval => TimeSpan.FromHours(12);

	protected override async Task ExecuteTaskAsync(CancellationToken stoppingToken)
	{
		using IServiceScope scope = _serviceScopeFactory.CreateScope();
		await using ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

		IEnumerable<PlayerEntity> players = dbContext.Players.AsEnumerable();

		int attempts = 0;
		List<IDdLeaderboardService.EntryResponse>? entries = null;
		do
		{
			attempts++;
			if (attempts > 5)
				break;

			try
			{
				entries = await _leaderboardClient.GetEntriesByIds(players.Select(p => p.Id));
			}
			catch (DdLeaderboardException)
			{
				const int interval = 5;
				Logger.LogWarning("Couldn't get entries. Waiting {interval} seconds...", interval);

				await Task.Delay(TimeSpan.FromSeconds(interval), stoppingToken);
			}
		}
		while (entries == null);
		if (entries == null)
			return;

		List<(int PlayerId, string OldName, string NewName)> logs = new();
		foreach (IDdLeaderboardService.EntryResponse entry in entries)
		{
			PlayerEntity? player = players.FirstOrDefault(p => p.Id == entry.Id);
			if (player == null || player.PlayerName == entry.Username)
				continue;

			logs.Add((entry.Id, player.PlayerName, entry.Username));
			player.PlayerName = entry.Username;
		}

		if (logs.Count > 0)
			await dbContext.SaveChangesAsync(stoppingToken);
	}
}
