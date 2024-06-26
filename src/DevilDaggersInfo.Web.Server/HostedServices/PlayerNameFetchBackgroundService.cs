using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using DevilDaggersInfo.Web.Server.Services;
using Microsoft.EntityFrameworkCore;

namespace DevilDaggersInfo.Web.Server.HostedServices;

public class PlayerNameFetchBackgroundService : AbstractBackgroundService
{
	private readonly IServiceScopeFactory _serviceScopeFactory;
	private readonly IDdLeaderboardService _leaderboardClient;

	public PlayerNameFetchBackgroundService(IServiceScopeFactory serviceScopeFactory, IDdLeaderboardService leaderboardClient, BackgroundServiceMonitor backgroundServiceMonitor, ILogger<PlayerNameFetchBackgroundService> logger)
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

		List<int> playerIds = await dbContext.Players.Select(p => p.Id).ToListAsync(stoppingToken);

		int attempts = 0;
		List<IDdLeaderboardService.EntryResponse>? entries = null;
		do
		{
			attempts++;
			if (attempts > 5)
				break;

			try
			{
				entries = await _leaderboardClient.GetEntriesByIds(playerIds);
			}
			catch (DdLeaderboardException ex)
			{
				const int interval = 5;
				Logger.LogWarning(ex, "Couldn't get entries from DD leaderboards. Waiting {Interval} seconds...", interval);

				await Task.Delay(TimeSpan.FromSeconds(interval), stoppingToken);
			}
		}
		while (entries == null);
		if (entries == null)
			return;

		List<(int PlayerId, string OldName, string NewName)> logs = [];
		foreach (IDdLeaderboardService.EntryResponse entry in entries)
		{
			PlayerEntity? player = await dbContext.Players.FirstOrDefaultAsync(p => p.Id == entry.Id, stoppingToken);
			if (player == null || player.PlayerName == entry.Username)
				continue;

			logs.Add((entry.Id, player.PlayerName, entry.Username));
			player.PlayerName = entry.Username;
		}

		if (logs.Count > 0)
			await dbContext.SaveChangesAsync(stoppingToken);
	}
}
