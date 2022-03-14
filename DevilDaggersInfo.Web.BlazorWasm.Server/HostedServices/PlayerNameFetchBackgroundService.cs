namespace DevilDaggersInfo.Web.BlazorWasm.Server.HostedServices;

public class PlayerNameFetchBackgroundService : AbstractBackgroundService
{
	private readonly IServiceScopeFactory _serviceScopeFactory;
	private readonly AuditLogger _auditLogger;
	private readonly LeaderboardClient _leaderboardClient;

	public PlayerNameFetchBackgroundService(IServiceScopeFactory serviceScopeFactory, AuditLogger auditLogger, LeaderboardClient leaderboardClient, BackgroundServiceMonitor backgroundServiceMonitor, ILogger<LeaderboardHistoryBackgroundService> logger)
		: base(backgroundServiceMonitor, logger)
	{
		_serviceScopeFactory = serviceScopeFactory;
		_auditLogger = auditLogger;
		_leaderboardClient = leaderboardClient;
	}

	protected override TimeSpan Interval => TimeSpan.FromHours(12);

	protected override async Task ExecuteTaskAsync(CancellationToken stoppingToken)
	{
		using IServiceScope scope = _serviceScopeFactory.CreateScope();
		using ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

		IEnumerable<PlayerEntity> players = dbContext.Players.AsEnumerable();

		List<EntryResponse>? entries = null;
		do
		{
			entries = await _leaderboardClient.GetEntriesByIds(players.Select(p => p.Id));
			if (entries == null)
			{
				const int interval = 5;
				Logger.LogWarning("Couldn't get entries. Waiting {interval} seconds...", interval);

				await Task.Delay(TimeSpan.FromSeconds(interval), stoppingToken);
			}
		}
		while (entries == null);

		List<(int PlayerId, string OldName, string NewName)> logs = new();
		foreach (EntryResponse entry in entries)
		{
			PlayerEntity? player = players.FirstOrDefault(p => p.Id == entry.Id);
			if (player == null || player.PlayerName == entry.Username)
				continue;

			logs.Add((entry.Id, player.PlayerName, entry.Username));
			player.PlayerName = entry.Username;
		}

		if (logs.Count > 0)
			await dbContext.SaveChangesAsync(stoppingToken);

		const int chunk = 10;
		for (int i = 0; i < logs.Count; i += chunk)
			await _auditLogger.LogPlayerUpdates(nameof(PlayerNameFetchBackgroundService), "PlayerId", logs.Skip(i).Take(chunk).ToList());
	}
}
