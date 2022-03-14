using DevilDaggersInfo.Web.BlazorWasm.Server.Clients.Clubber;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.HostedServices;

public class DiscordUserIdFetchBackgroundService : AbstractBackgroundService
{
	private readonly IServiceScopeFactory _serviceScopeFactory;
	private readonly AuditLogger _auditLogger;
	private readonly ClubberClient _clubberClient;

	public DiscordUserIdFetchBackgroundService(IServiceScopeFactory serviceScopeFactory, AuditLogger auditLogger, ClubberClient clubberClient, BackgroundServiceMonitor backgroundServiceMonitor, ILogger<LeaderboardHistoryBackgroundService> logger)
		: base(backgroundServiceMonitor, logger)
	{
		_serviceScopeFactory = serviceScopeFactory;
		_auditLogger = auditLogger;
		_clubberClient = clubberClient;
	}

	protected override TimeSpan Interval => TimeSpan.FromHours(12);

	protected override async Task ExecuteTaskAsync(CancellationToken stoppingToken)
	{
		using IServiceScope scope = _serviceScopeFactory.CreateScope();
		using ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

		List<DdUser>? users = null;
		do
		{
			users = await _clubberClient.GetUsers();
			if (users == null)
			{
				const int interval = 5;
				Logger.LogWarning("Couldn't get users. Waiting {interval} seconds...", interval);

				await Task.Delay(TimeSpan.FromSeconds(interval), stoppingToken);
			}
		}
		while (users == null);

		List<int> ids = users.ConvertAll(u => u.LeaderboardId);
		List<PlayerEntity> players = dbContext.Players.Where(p => ids.Contains(p.Id)).ToList();

		List<(int PlayerId, ulong? OldId, ulong NewId)> logs = new();
		foreach (DdUser user in users)
		{
			PlayerEntity? player = players.Find(p => p.Id == user.LeaderboardId);
			if (player == null || player.DiscordUserId == user.DiscordId)
				continue;

			logs.Add((player.Id, player.DiscordUserId, user.DiscordId));
			player.DiscordUserId = user.DiscordId;
		}

		if (logs.Count > 0)
			await dbContext.SaveChangesAsync(stoppingToken);

		const int chunk = 10;
		for (int i = 0; i < logs.Count; i += chunk)
			await _auditLogger.LogPlayerUpdates(nameof(DiscordUserIdFetchBackgroundService), "DiscordUserId", logs.Skip(i).Take(chunk).Select(l => (l.PlayerId, l.OldId?.ToString() ?? string.Empty, l.NewId.ToString() ?? string.Empty)).ToList());
	}
}
