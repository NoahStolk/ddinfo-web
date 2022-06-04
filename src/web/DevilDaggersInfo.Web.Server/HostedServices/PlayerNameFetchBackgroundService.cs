using DevilDaggersInfo.Web.Server.Domain.Services;

namespace DevilDaggersInfo.Web.Server.HostedServices;

public class PlayerNameFetchBackgroundService : AbstractBackgroundService
{
	private readonly IServiceScopeFactory _serviceScopeFactory;
	private readonly IAuditLogger _auditLogger;
	private readonly LeaderboardClient _leaderboardClient;

	public PlayerNameFetchBackgroundService(IServiceScopeFactory serviceScopeFactory, IAuditLogger auditLogger, LeaderboardClient leaderboardClient, BackgroundServiceMonitor backgroundServiceMonitor, ILogger<LeaderboardHistoryBackgroundService> logger)
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

		int attempts = 0;
		List<EntryResponse>? entries = null;
		do
		{
			attempts++;
			if (attempts > 5)
				break;

			ResponseWrapper<List<EntryResponse>> wrapper = await _leaderboardClient.GetEntriesByIds(players.Select(p => p.Id));
			if (wrapper.HasError)
			{
				const int interval = 5;
				Logger.LogWarning("Couldn't get entries. Waiting {interval} seconds...", interval);

				await Task.Delay(TimeSpan.FromSeconds(interval), stoppingToken);
			}
			else
			{
				entries = wrapper.GetResponse();
			}
		}
		while (entries == null);
		if (entries == null)
			return;

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

		const int chunk = 20;
		for (int i = 0; i < logs.Count; i += chunk)
			_auditLogger.LogPlayerUpdates(nameof(PlayerNameFetchBackgroundService), logs.Skip(i).Take(chunk).ToList());
	}
}
