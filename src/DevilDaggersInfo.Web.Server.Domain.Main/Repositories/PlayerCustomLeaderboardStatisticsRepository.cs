using DevilDaggersInfo.Web.ApiSpec.Main.Players;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using DevilDaggersInfo.Web.Server.Domain.Main.Converters.DomainToApi;
using DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;
using DevilDaggersInfo.Web.Server.Domain.Utils;
using Microsoft.EntityFrameworkCore;

namespace DevilDaggersInfo.Web.Server.Domain.Main.Repositories;

public class PlayerCustomLeaderboardStatisticsRepository
{
	private readonly ApplicationDbContext _dbContext;

	public PlayerCustomLeaderboardStatisticsRepository(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<List<GetPlayerCustomLeaderboardStatistics>> GetCustomLeaderboardStatisticsByPlayerIdAsync(int playerId)
	{
		// ! Navigation property.
		List<CustomEntryForStats> customEntries = await _dbContext.CustomEntries
			.AsNoTracking()
			.Where(cl => cl.PlayerId == playerId)
			.Select(ce => new CustomEntryForStats
			{
				Time = ce.Time,
				GemsCollected = ce.GemsCollected,
				GemsDespawned = ce.GemsDespawned,
				GemsEaten = ce.GemsEaten,
				EnemiesKilled = ce.EnemiesKilled,
				EnemiesAlive = ce.EnemiesAlive,
				HomingStored = ce.HomingStored,
				HomingEaten = ce.HomingEaten,
				GameMode = ce.CustomLeaderboard!.Spawnset!.GameMode,
				RankSorting = ce.CustomLeaderboard.RankSorting,
				Leviathan = ce.CustomLeaderboard.Leviathan,
				Devil = ce.CustomLeaderboard.Devil,
				Golden = ce.CustomLeaderboard.Golden,
				Silver = ce.CustomLeaderboard.Silver,
				Bronze = ce.CustomLeaderboard.Bronze,
				IsFeatured = ce.CustomLeaderboard.IsFeatured,
			})
			.Where(ce => ce.IsFeatured)
			.ToListAsync();

		if (customEntries.Count == 0)
			return [];

		// ! Navigation property.
		Dictionary<(SpawnsetGameMode GameMode, CustomLeaderboardRankSorting RankSorting), int> totalCustomLeaderboards = await _dbContext.CustomLeaderboards
			.AsNoTracking()
			.Select(cl => new { cl.Spawnset!.GameMode, cl.RankSorting, cl.IsFeatured })
			.Where(cl => cl.IsFeatured)
			.GroupBy(cl => new { cl.GameMode, cl.RankSorting })
			.Select(g => new { g.Key, Count = g.Count() })
			.ToDictionaryAsync(a => (a.Key.GameMode, a.Key.RankSorting), a => a.Count);

		List<GetPlayerCustomLeaderboardStatistics> stats = [];
		foreach (SpawnsetGameMode gameMode in Enum.GetValues<SpawnsetGameMode>())
		{
			foreach (CustomLeaderboardRankSorting rankSorting in Enum.GetValues<CustomLeaderboardRankSorting>())
			{
				if (!totalCustomLeaderboards.TryGetValue((gameMode, rankSorting), out int totalCount))
					continue;

				List<CustomEntryForStats> filteredCustomEntries = customEntries.Where(ce => ce.GameMode == gameMode && ce.RankSorting == rankSorting).ToList();
				if (filteredCustomEntries.Count == 0)
					continue;

				int leviathanDaggers = 0;
				int devilDaggers = 0;
				int goldenDaggers = 0;
				int silverDaggers = 0;
				int bronzeDaggers = 0;
				int defaultDaggers = 0;
				int played = 0;
				foreach (CustomEntryForStats customEntry in filteredCustomEntries)
				{
					played++;
					switch (CustomLeaderboardUtils.GetDaggerFromStat(rankSorting, customEntry, customEntry.Leviathan, customEntry.Devil, customEntry.Golden, customEntry.Silver, customEntry.Bronze))
					{
						case CustomLeaderboardDagger.Leviathan: leviathanDaggers++; break;
						case CustomLeaderboardDagger.Devil: devilDaggers++; break;
						case CustomLeaderboardDagger.Golden: goldenDaggers++; break;
						case CustomLeaderboardDagger.Silver: silverDaggers++; break;
						case CustomLeaderboardDagger.Bronze: bronzeDaggers++; break;
						default: defaultDaggers++; break;
					}
				}

				stats.Add(new()
				{
					GameMode = gameMode.ToMainApi(),
					RankSorting = rankSorting.ToMainApi(),
					LeviathanDaggerCount = leviathanDaggers,
					DevilDaggerCount = devilDaggers,
					GoldenDaggerCount = goldenDaggers,
					SilverDaggerCount = silverDaggers,
					BronzeDaggerCount = bronzeDaggers,
					DefaultDaggerCount = defaultDaggers,
					LeaderboardsPlayedCount = played,
					TotalCount = totalCount,
				});
			}
		}

		return stats;
	}

	private sealed class CustomEntryForStats : IDaggerStatCustomEntry
	{
		public required int Time { get; init; }
		public required int GemsCollected { get; init; }
		public required int GemsDespawned { get; init; }
		public required int GemsEaten { get; init; }
		public required int EnemiesKilled { get; init; }
		public required int EnemiesAlive { get; init; }
		public required int HomingStored { get; init; }
		public required int HomingEaten { get; init; }
		public required SpawnsetGameMode GameMode { get; init; }
		public required CustomLeaderboardRankSorting RankSorting { get; init; }
		public required int Leviathan { get; init; }
		public required int Devil { get; init; }
		public required int Golden { get; init; }
		public required int Silver { get; init; }
		public required int Bronze { get; init; }
		public required bool IsFeatured { get; init; }
	}
}
