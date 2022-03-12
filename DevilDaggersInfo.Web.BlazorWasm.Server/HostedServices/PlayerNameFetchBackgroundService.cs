namespace DevilDaggersInfo.Web.BlazorWasm.Server.HostedServices;

public class PlayerNameFetchBackgroundService : AbstractBackgroundService
{
	private readonly IServiceScopeFactory _serviceScopeFactory;
	private readonly AuditLogger _auditLogger;

	public PlayerNameFetchBackgroundService(BackgroundServiceMonitor backgroundServiceMonitor, ILogger<LeaderboardHistoryBackgroundService> logger, IServiceScopeFactory serviceScopeFactory, AuditLogger auditLogger)
		: base(backgroundServiceMonitor, logger)
	{
		_serviceScopeFactory = serviceScopeFactory;
		_auditLogger = auditLogger;
	}

	protected override TimeSpan Interval => TimeSpan.FromDays(1);

	protected override async Task ExecuteTaskAsync(CancellationToken stoppingToken)
	{
		using IServiceScope scope = _serviceScopeFactory.CreateScope();
		using ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

		IEnumerable<PlayerEntity> players = dbContext.Players.AsEnumerable();

		List<EntryResponse>? entries = null;
		do
		{
			try
			{
				entries = await LeaderboardClient.Instance.GetEntriesByIds(players.Select(p => p.Id));
			}
			catch (Exception ex)
			{
				const int interval = 10;
				Logger.LogWarning(ex, "Couldn't get entries. Waiting {interval} seconds...", interval);

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

		await _auditLogger.LogPlayerRenames(logs);
	}
}
