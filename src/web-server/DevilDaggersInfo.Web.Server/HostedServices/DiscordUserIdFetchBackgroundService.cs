using DevilDaggersInfo.Web.Server.Clients.Clubber;

namespace DevilDaggersInfo.Web.Server.HostedServices;

public class DiscordUserIdFetchBackgroundService : AbstractBackgroundService
{
	private readonly IServiceScopeFactory _serviceScopeFactory;
	private readonly ClubberClient _clubberClient;

	public DiscordUserIdFetchBackgroundService(IServiceScopeFactory serviceScopeFactory, ClubberClient clubberClient, BackgroundServiceMonitor backgroundServiceMonitor, ILogger<LeaderboardHistoryBackgroundService> logger)
		: base(backgroundServiceMonitor, logger)
	{
		_serviceScopeFactory = serviceScopeFactory;
		_clubberClient = clubberClient;
	}

	protected override TimeSpan Interval => TimeSpan.FromHours(12);

	protected override async Task ExecuteTaskAsync(CancellationToken stoppingToken)
	{
		using IServiceScope scope = _serviceScopeFactory.CreateScope();
		await using ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

		int attempts = 0;
		List<DdUser>? users = null;
		do
		{
			attempts++;
			if (attempts > 5)
				break;

			users = await _clubberClient.GetUsers();
			if (users == null)
			{
				const int interval = 5;
				Logger.LogWarning("Couldn't get users. Waiting {interval} seconds...", interval);

				await Task.Delay(TimeSpan.FromSeconds(interval), stoppingToken);
			}
		}
		while (users == null);
		if (users == null)
			return;

		List<int> ids = users.ConvertAll(u => u.LeaderboardId);
		List<PlayerEntity> players = dbContext.Players.Where(p => ids.Contains(p.Id) && p.DiscordUserId == null).ToList();

		List<(int PlayerId, ulong? OldId, ulong NewId)> logs = new();
		foreach (PlayerEntity player in players)
		{
			DdUser? user = users.Where(u => u.LeaderboardId == player.Id).MinBy(u => u.DiscordId);
			if (user == null || player.DiscordUserId == user.DiscordId)
				continue;

			logs.Add((player.Id, player.DiscordUserId, user.DiscordId));
			player.DiscordUserId = user.DiscordId;
		}

		if (logs.Count > 0)
			await dbContext.SaveChangesAsync(stoppingToken);
	}
}
