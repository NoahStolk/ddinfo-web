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
		foreach (PlayerEntity player in players)
		{
			DdUser? user = users.Where(u => u.LeaderboardId == player.Id).OrderBy(u => u.DiscordId).FirstOrDefault();
			if (user == null || player.DiscordUserId == user.DiscordId)
				continue;

			logs.Add((player.Id, player.DiscordUserId, user.DiscordId));
			player.DiscordUserId = user.DiscordId;
		}

		if (logs.Count > 0)
			await dbContext.SaveChangesAsync(stoppingToken);

		const int chunk = 20;
		for (int i = 0; i < logs.Count; i += chunk)
			_auditLogger.LogPlayerUpdates(nameof(DiscordUserIdFetchBackgroundService), logs.Skip(i).Take(chunk).Select(l => (l.PlayerId, l.OldId?.ToString() ?? "NULL", l.NewId.ToString())).ToList());
	}
}
