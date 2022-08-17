using DevilDaggersInfo.Common.Extensions;
using DevilDaggersInfo.Core.Versioning;
using DevilDaggersInfo.Types.Web;
using DevilDaggersInfo.Web.Server.Domain.Constants;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Entities.Values;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Extensions;
using DevilDaggersInfo.Web.Server.Domain.Models;
using DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;
using DevilDaggersInfo.Web.Server.Domain.Models.Spawnsets;
using DevilDaggersInfo.Web.Server.Domain.Services.Caching;
using DevilDaggersInfo.Web.Server.Domain.Utils;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace DevilDaggersInfo.Web.Server.Domain.Repositories;

public class CustomLeaderboardRepository
{
	private readonly ApplicationDbContext _dbContext;
	private readonly CustomEntryRepository _customEntryRepository;
	private readonly SpawnsetHashCache _spawnsetHashCache;

	public CustomLeaderboardRepository(ApplicationDbContext dbContext, CustomEntryRepository customEntryRepository, SpawnsetHashCache spawnsetHashCache)
	{
		_dbContext = dbContext;
		_customEntryRepository = customEntryRepository;
		_spawnsetHashCache = spawnsetHashCache;
	}

	public async Task<Page<CustomLeaderboardOverview>> GetCustomLeaderboardOverviewsAsync(
		CustomLeaderboardCategory? category,
		string? spawnsetFilter,
		string? authorFilter,
		int pageIndex,
		int pageSize,
		CustomLeaderboardSorting? sortBy,
		bool ascending,
		int? selectedPlayerId,
		bool onlyFeatured)
	{
		// Build query.
		IQueryable<CustomLeaderboardEntity> customLeaderboardsQuery = _dbContext.CustomLeaderboards
			.AsNoTracking()
			.Include(cl => cl.Spawnset)
				.ThenInclude(sf => sf.Player);

		if (category.HasValue)
			customLeaderboardsQuery = customLeaderboardsQuery.Where(cl => category == cl.Category);

		if (onlyFeatured)
			customLeaderboardsQuery = customLeaderboardsQuery.Where(cl => cl.IsFeatured);

		// Casing is ignored by default because of IQueryable.
		if (!string.IsNullOrWhiteSpace(spawnsetFilter))
			customLeaderboardsQuery = customLeaderboardsQuery.Where(cl => cl.Spawnset.Name.Contains(spawnsetFilter));

		if (!string.IsNullOrWhiteSpace(authorFilter))
			customLeaderboardsQuery = customLeaderboardsQuery.Where(cl => cl.Spawnset.Player.PlayerName.Contains(authorFilter));

		// Execute query.
		List<CustomLeaderboardEntity> customLeaderboards = await customLeaderboardsQuery.ToListAsync();

		// Query custom entries for additional data.
		List<int> customLeaderboardIds = customLeaderboards.ConvertAll(cl => cl.Id);
		List<CustomEntrySummary> customEntries = await _dbContext.CustomEntries
			.AsNoTracking()
			.Include(ce => ce.Player)
			.Select(ce => new CustomEntrySummary { CustomLeaderboardId = ce.CustomLeaderboardId, PlayerId = ce.PlayerId, PlayerName = ce.Player.PlayerName, Time = ce.Time, SubmitDate = ce.SubmitDate })
			.Where(ce => customLeaderboardIds.Contains(ce.CustomLeaderboardId))
			.ToListAsync();

		List<CustomLeaderboardData> customLeaderboardData = new();
		foreach (CustomLeaderboardEntity cl in customLeaderboards)
		{
			List<CustomEntrySummary> sortedCustomEntries = customEntries.Where(ce => ce.CustomLeaderboardId == cl.Id).Sort(cl.Category).ToList();

			CustomEntrySummary? worldRecord = sortedCustomEntries.Count == 0 ? null : sortedCustomEntries[0];
			CustomLeaderboardOverviewWorldRecord? worldRecordModel = worldRecord == null ? null : new()
			{
				Time = worldRecord.Time,
				PlayerId = worldRecord.PlayerId,
				PlayerName = worldRecord.PlayerName,
				Dagger = cl.GetDaggerFromTime(worldRecord.Time),
			};

			CustomEntrySummary? selectedEntry = sortedCustomEntries.Find(ce => ce.PlayerId == selectedPlayerId);
			CustomLeaderboardOverviewSelectedPlayerStats? selectedPlayerStatsModel = GetSelectedStats(cl, sortedCustomEntries, selectedEntry);
			customLeaderboardData.Add(new(cl, worldRecordModel, selectedPlayerStatsModel, sortedCustomEntries.Count));
		}

		customLeaderboardData = (sortBy switch
		{
			CustomLeaderboardSorting.AuthorName => customLeaderboardData.OrderBy(cl => cl.CustomLeaderboard.Spawnset.Player.PlayerName.ToLower(), ascending),
			CustomLeaderboardSorting.DateLastPlayed => customLeaderboardData.OrderBy(cl => cl.CustomLeaderboard.DateLastPlayed ?? cl.CustomLeaderboard.DateCreated, ascending),
			CustomLeaderboardSorting.SpawnsetName => customLeaderboardData.OrderBy(cl => cl.CustomLeaderboard.Spawnset.Name.ToLower(), ascending),
			CustomLeaderboardSorting.TimeBronze => customLeaderboardData.OrderBy(cl => cl.CustomLeaderboard.IsFeatured ? cl.CustomLeaderboard.Bronze : 0, ascending),
			CustomLeaderboardSorting.TimeSilver => customLeaderboardData.OrderBy(cl => cl.CustomLeaderboard.IsFeatured ? cl.CustomLeaderboard.Silver : 0, ascending),
			CustomLeaderboardSorting.TimeGolden => customLeaderboardData.OrderBy(cl => cl.CustomLeaderboard.IsFeatured ? cl.CustomLeaderboard.Golden : 0, ascending),
			CustomLeaderboardSorting.TimeDevil => customLeaderboardData.OrderBy(cl => cl.CustomLeaderboard.IsFeatured ? cl.CustomLeaderboard.Devil : 0, ascending),
			CustomLeaderboardSorting.TimeLeviathan => customLeaderboardData.OrderBy(cl => cl.CustomLeaderboard.IsFeatured ? cl.CustomLeaderboard.Leviathan : 0, ascending),
			CustomLeaderboardSorting.DateCreated => customLeaderboardData.OrderBy(cl => cl.CustomLeaderboard.DateCreated, ascending),
			CustomLeaderboardSorting.Players => customLeaderboardData.OrderBy(cl => cl.PlayerCount, ascending),
			CustomLeaderboardSorting.Submits => customLeaderboardData.OrderBy(cl => cl.CustomLeaderboard.TotalRunsSubmitted, ascending),
			CustomLeaderboardSorting.WorldRecord => customLeaderboardData.OrderBy(cl => cl.WorldRecord?.Time, ascending),
			CustomLeaderboardSorting.TopPlayer => customLeaderboardData.OrderBy(cl => cl.WorldRecord?.PlayerName.ToLower(), ascending),
			_ => customLeaderboardData.OrderBy(cl => cl.CustomLeaderboard.Id, ascending),
		}).ToList();

		int totalCustomLeaderboards = customLeaderboards.Count;
		int lastPageIndex = totalCustomLeaderboards / pageSize;

		return new(
			Results: customLeaderboardData
				.Skip(Math.Min(pageIndex, lastPageIndex) * pageSize)
				.Take(pageSize)
				.Select(ToOverview)
				.ToList(),
			TotalResults: totalCustomLeaderboards);
	}

	public async Task<SortedCustomLeaderboard> GetSortedCustomLeaderboardByIdAsync(int id)
	{
		CustomLeaderboardEntity? customLeaderboard = await _dbContext.CustomLeaderboards
			.AsNoTracking()
			.Include(cl => cl.CustomEntries!)
				.ThenInclude(ce => ce.Player)
			.Include(cl => cl.Spawnset)
				.ThenInclude(sf => sf.Player)
			.FirstOrDefaultAsync(cl => cl.Id == id);
		if (customLeaderboard == null)
			throw new NotFoundException($"Custom leaderboard '{id}' could not be found.");

		List<int> existingReplayIds = _customEntryRepository.GetExistingCustomEntryReplayIds(customLeaderboard.CustomEntries!.ConvertAll(ce => ce.Id));

		List<CustomLeaderboardCriteria> criteria = new()
		{
			AddCriteria(CustomLeaderboardCriteriaType.GemsCollected, customLeaderboard.GemsCollectedCriteria),
			AddCriteria(CustomLeaderboardCriteriaType.GemsDespawned, customLeaderboard.GemsDespawnedCriteria),
			AddCriteria(CustomLeaderboardCriteriaType.GemsEaten, customLeaderboard.GemsEatenCriteria),
			AddCriteria(CustomLeaderboardCriteriaType.EnemiesKilled, customLeaderboard.EnemiesKilledCriteria),
			AddCriteria(CustomLeaderboardCriteriaType.DaggersFired, customLeaderboard.DaggersFiredCriteria),
			AddCriteria(CustomLeaderboardCriteriaType.DaggersHit, customLeaderboard.DaggersHitCriteria),
			AddCriteria(CustomLeaderboardCriteriaType.HomingStored, customLeaderboard.HomingStoredCriteria),
			AddCriteria(CustomLeaderboardCriteriaType.HomingEaten, customLeaderboard.HomingEatenCriteria),
			AddEnemyCriteria(CustomLeaderboardCriteriaType.Skull1Kills, customLeaderboard.Skull1KillsCriteria),
			AddEnemyCriteria(CustomLeaderboardCriteriaType.Skull2Kills, customLeaderboard.Skull2KillsCriteria),
			AddEnemyCriteria(CustomLeaderboardCriteriaType.Skull3Kills, customLeaderboard.Skull3KillsCriteria),
			AddEnemyCriteria(CustomLeaderboardCriteriaType.Skull4Kills, customLeaderboard.Skull4KillsCriteria),
			AddEnemyCriteria(CustomLeaderboardCriteriaType.SpiderlingKills, customLeaderboard.SpiderlingKillsCriteria),
			AddEnemyCriteria(CustomLeaderboardCriteriaType.SpiderEggKills, customLeaderboard.SpiderEggKillsCriteria),
			AddEnemyCriteria(CustomLeaderboardCriteriaType.Squid1Kills, customLeaderboard.Squid1KillsCriteria),
			AddEnemyCriteria(CustomLeaderboardCriteriaType.Squid2Kills, customLeaderboard.Squid2KillsCriteria),
			AddEnemyCriteria(CustomLeaderboardCriteriaType.Squid3Kills, customLeaderboard.Squid3KillsCriteria),
			AddEnemyCriteria(CustomLeaderboardCriteriaType.CentipedeKills, customLeaderboard.CentipedeKillsCriteria),
			AddEnemyCriteria(CustomLeaderboardCriteriaType.GigapedeKills, customLeaderboard.GigapedeKillsCriteria),
			AddEnemyCriteria(CustomLeaderboardCriteriaType.GhostpedeKills, customLeaderboard.GhostpedeKillsCriteria),
			AddEnemyCriteria(CustomLeaderboardCriteriaType.Spider1Kills, customLeaderboard.Spider1KillsCriteria),
			AddEnemyCriteria(CustomLeaderboardCriteriaType.Spider2Kills, customLeaderboard.Spider2KillsCriteria),
			AddEnemyCriteria(CustomLeaderboardCriteriaType.LeviathanKills, customLeaderboard.LeviathanKillsCriteria),
			AddEnemyCriteria(CustomLeaderboardCriteriaType.OrbKills, customLeaderboard.OrbKillsCriteria),
			AddEnemyCriteria(CustomLeaderboardCriteriaType.ThornKills, customLeaderboard.ThornKillsCriteria),
		};

		static CustomLeaderboardCriteria AddCriteria(CustomLeaderboardCriteriaType criteriaType, CustomLeaderboardCriteriaEntityValue criteria) => new()
		{
			Type = criteriaType,
			Operator = criteria.Operator,
			Value = criteria.Value,
		};

		static CustomLeaderboardCriteria AddEnemyCriteria(CustomLeaderboardCriteriaType criteriaType, CustomLeaderboardEnemyCriteriaEntityValue criteria) => new()
		{
			Type = criteriaType,
			Operator = criteria.Operator,
			Value = criteria.Value,
		};

		return new()
		{
			Category = customLeaderboard.Category,
			Criteria = criteria.Where(c => c.Operator != CustomLeaderboardCriteriaOperator.Any).ToList(),
			CustomEntries = customLeaderboard.CustomEntries
				.Sort(customLeaderboard.Category)
				.Select((ce, i) =>
				{
					bool isDdcl = ce.Client == CustomLeaderboardsClient.DevilDaggersCustomLeaderboards;
					AppVersion clientVersionParsed = AppVersion.TryParse(ce.ClientVersion, out AppVersion? version) ? version : new(0, 0, 0);
					bool hasV3_1Values = !isDdcl || clientVersionParsed >= FeatureConstants.DdclV3_1;
					bool hasHomingEatenValue = !isDdcl || clientVersionParsed >= FeatureConstants.DdclHomingEaten;

					return new CustomEntry
					{
						Client = ce.Client,
						ClientVersion = ce.ClientVersion,
						CountryCode = ce.Player.CountryCode,
						CustomLeaderboardDagger = customLeaderboard.GetDaggerFromTime(ce.Time),
						DaggersFired = ce.DaggersFired,
						DaggersHit = ce.DaggersHit,
						DeathType = ce.DeathType,
						EnemiesAlive = ce.EnemiesAlive,
						EnemiesKilled = ce.EnemiesKilled,
						GemsCollected = ce.GemsCollected,
						GemsDespawned = hasV3_1Values ? ce.GemsDespawned : null,
						GemsEaten = hasV3_1Values ? ce.GemsEaten : null,
						GemsTotal = hasV3_1Values ? ce.GemsTotal : null,
						HasReplay = existingReplayIds.Contains(ce.Id),
						HomingEaten = hasHomingEatenValue ? ce.HomingEaten : null,
						HomingStored = ce.HomingStored,
						Id = ce.Id,
						LevelUpTime2 = ce.LevelUpTime2,
						LevelUpTime3 = ce.LevelUpTime3,
						LevelUpTime4 = ce.LevelUpTime4,
						PlayerId = ce.PlayerId,
						PlayerName = ce.Player.PlayerName,
						Rank = i + 1,
						SubmitDate = ce.SubmitDate,
						Time = ce.Time,
					};
				})
				.ToList(),
			Daggers = !customLeaderboard.IsFeatured ? null : new()
			{
				Bronze = customLeaderboard.Bronze,
				Silver = customLeaderboard.Silver,
				Golden = customLeaderboard.Golden,
				Devil = customLeaderboard.Devil,
				Leviathan = customLeaderboard.Leviathan,
			},
			DateCreated = customLeaderboard.DateCreated,
			DateLastPlayed = customLeaderboard.DateLastPlayed,
			Id = customLeaderboard.Id,
			SpawnsetHtmlDescription = customLeaderboard.Spawnset.HtmlDescription,
			SpawnsetName = customLeaderboard.Spawnset.Name,
			SpawnsetAuthorId = customLeaderboard.Spawnset.PlayerId,
			SpawnsetAuthorName = customLeaderboard.Spawnset.Player.PlayerName,
			SpawnsetId = customLeaderboard.SpawnsetId,
			TotalRunsSubmitted = customLeaderboard.TotalRunsSubmitted,
		};
	}

	public async Task<CustomLeaderboardsTotalData> GetCustomLeaderboardsTotalDataAsync()
	{
		var customLeaderboards = await _dbContext.CustomLeaderboards.AsNoTracking().Select(cl => new { cl.Id, cl.Category, cl.TotalRunsSubmitted }).ToListAsync();
		var customEntries = await _dbContext.CustomEntries.AsNoTracking().Select(ce => new { ce.PlayerId, ce.CustomLeaderboard.Category }).ToListAsync();

		Dictionary<CustomLeaderboardCategory, int> leaderboardsPerCategory = new();
		Dictionary<CustomLeaderboardCategory, int> scoresPerCategory = new();
		Dictionary<CustomLeaderboardCategory, int> submitsPerCategory = new();
		Dictionary<CustomLeaderboardCategory, int> playersPerCategory = new();
		foreach (CustomLeaderboardCategory category in Enum.GetValues<CustomLeaderboardCategory>())
		{
			leaderboardsPerCategory[category] = customLeaderboards.Count(cl => cl.Category == category);
			scoresPerCategory[category] = customEntries.Count(cl => cl.Category == category);
			submitsPerCategory[category] = customLeaderboards.Where(cl => cl.Category == category).Sum(cl => cl.TotalRunsSubmitted);
			playersPerCategory[category] = customEntries.Where(cl => cl.Category == category).DistinctBy(cl => cl.PlayerId).Count();
		}

		return new()
		{
			LeaderboardsPerCategory = leaderboardsPerCategory,
			ScoresPerCategory = scoresPerCategory,
			SubmitsPerCategory = submitsPerCategory,
			PlayersPerCategory = playersPerCategory,
			TotalPlayers = customEntries.DistinctBy(ce => ce.PlayerId).Count(),
		};
	}

	public async Task<GlobalCustomLeaderboard> GetGlobalCustomLeaderboardAsync(CustomLeaderboardCategory category)
	{
		List<CustomLeaderboardEntity> customLeaderboards = await _dbContext.CustomLeaderboards
			.AsNoTracking()
			.Include(cl => cl.CustomEntries!)
				.ThenInclude(ce => ce.Player)
			.Where(ce => ce.Category == category && ce.IsFeatured)
			.ToListAsync();

		Dictionary<int, (string Name, GlobalCustomLeaderboardEntryData Data)> globalData = new();
		foreach (CustomLeaderboardEntity customLeaderboard in customLeaderboards)
		{
			List<CustomEntryEntity> customEntries = customLeaderboard.CustomEntries!.Sort(category).ToList();

			for (int i = 0; i < customEntries.Count; i++)
			{
				CustomEntryEntity customEntry = customEntries[i];

				GlobalCustomLeaderboardEntryData data;
				if (!globalData.ContainsKey(customEntry.PlayerId))
				{
					data = new();
					globalData.Add(customEntry.PlayerId, (customEntry.Player.PlayerName, data));
				}
				else
				{
					data = globalData[customEntry.PlayerId].Data;
				}

				data.Rankings.Add(new(i + 1, customEntries.Count));

				switch (customLeaderboard.GetDaggerFromTime(customEntry.Time) ?? throw new InvalidOperationException("Custom leaderboard without daggers may not be used for processing global custom leaderboard data."))
				{
					case CustomLeaderboardDagger.Leviathan: data.LeviathanCount++; break;
					case CustomLeaderboardDagger.Devil: data.DevilCount++; break;
					case CustomLeaderboardDagger.Golden: data.GoldenCount++; break;
					case CustomLeaderboardDagger.Silver: data.SilverCount++; break;
					case CustomLeaderboardDagger.Bronze: data.BronzeCount++; break;
					default: data.DefaultCount++; break;
				}
			}
		}

		return new GlobalCustomLeaderboard
		{
			Entries = globalData
				.Select(kvp => new GlobalCustomLeaderboardEntry
				{
					DefaultDaggerCount = kvp.Value.Data.DefaultCount,
					BronzeDaggerCount = kvp.Value.Data.BronzeCount,
					SilverDaggerCount = kvp.Value.Data.SilverCount,
					GoldenDaggerCount = kvp.Value.Data.GoldenCount,
					DevilDaggerCount = kvp.Value.Data.DevilCount,
					LeviathanDaggerCount = kvp.Value.Data.LeviathanCount,
					LeaderboardsPlayedCount = kvp.Value.Data.TotalPlayed,
					PlayerId = kvp.Key,
					PlayerName = kvp.Value.Name,
					Points = GlobalCustomLeaderboardUtils.GetPoints(kvp.Value.Data),
				})
				.OrderByDescending(ce => ce.Points)
				.ThenByDescending(ce => ce.LeaderboardsPlayedCount)
				.ToList(),
			TotalLeaderboards = customLeaderboards.Count,
			TotalPoints = customLeaderboards.Sum(cl => cl.CustomEntries!.Count * GlobalCustomLeaderboardUtils.RankingMultiplier + GlobalCustomLeaderboardUtils.LeviathanBonus),
		};
	}

	public async Task<int> GetCustomLeaderboardIdBySpawnsetHashAsync(byte[] hash)
	{
		SpawnsetHashCacheData? data = _spawnsetHashCache.GetSpawnset(hash);
		if (data == null)
			throw new NotFoundException();

		var spawnset = await _dbContext.Spawnsets
			.Select(s => new { s.Id, s.Name })
			.FirstOrDefaultAsync(s => s.Name == data.Name);
		if (spawnset == null)
			throw new NotFoundException();

		var customLeaderboard = await _dbContext.CustomLeaderboards
			.Select(cl => new { cl.Id, cl.SpawnsetId })
			.FirstOrDefaultAsync(cl => cl.SpawnsetId == spawnset.Id);
		if (customLeaderboard == null)
			throw new NotFoundException();

		return customLeaderboard.Id;
	}

	private static CustomLeaderboardOverview ToOverview(CustomLeaderboardData cl) => new()
	{
		Category = cl.CustomLeaderboard.Category,
		Daggers = !cl.CustomLeaderboard.IsFeatured ? null : new()
		{
			Bronze = cl.CustomLeaderboard.Bronze,
			Silver = cl.CustomLeaderboard.Silver,
			Golden = cl.CustomLeaderboard.Golden,
			Devil = cl.CustomLeaderboard.Devil,
			Leviathan = cl.CustomLeaderboard.Leviathan,
		},
		DateCreated = cl.CustomLeaderboard.DateCreated,
		DateLastPlayed = cl.CustomLeaderboard.DateLastPlayed,
		Id = cl.CustomLeaderboard.Id,
		PlayerCount = cl.PlayerCount,
		SelectedPlayerStats = cl.SelectedPlayerStats,
		SpawnsetAuthorId = cl.CustomLeaderboard.Spawnset.PlayerId,
		SpawnsetAuthorName = cl.CustomLeaderboard.Spawnset.Player.PlayerName,
		SpawnsetId = cl.CustomLeaderboard.SpawnsetId,
		SpawnsetName = cl.CustomLeaderboard.Spawnset.Name,
		TotalRunsSubmitted = cl.CustomLeaderboard.TotalRunsSubmitted,
		WorldRecord = cl.WorldRecord == null ? null : new()
		{
			PlayerId = cl.WorldRecord.PlayerId,
			PlayerName = cl.WorldRecord.PlayerName,
			Time = cl.WorldRecord.Time,
			Dagger = cl.CustomLeaderboard.GetDaggerFromTime(cl.WorldRecord.Time),
		},
	};

	[return: NotNullIfNotNull("selectedEntry")]
	private static CustomLeaderboardOverviewSelectedPlayerStats? GetSelectedStats(CustomLeaderboardEntity cl, List<CustomEntrySummary> sortedCustomEntries, CustomEntrySummary? selectedEntry)
	{
		if (selectedEntry == null)
			return null;

		CustomLeaderboardDagger? dagger = cl.GetDaggerFromTime(selectedEntry.Time);
		return new()
		{
			Dagger = dagger,
			Rank = sortedCustomEntries.IndexOf(selectedEntry) + 1,
			Time = selectedEntry.Time,
			NextDagger = dagger switch
			{
				CustomLeaderboardDagger.Default => new() { Dagger = CustomLeaderboardDagger.Bronze, Time = cl.Bronze },
				CustomLeaderboardDagger.Bronze => new() { Dagger = CustomLeaderboardDagger.Silver, Time = cl.Silver },
				CustomLeaderboardDagger.Silver => new() { Dagger = CustomLeaderboardDagger.Golden, Time = cl.Golden },
				CustomLeaderboardDagger.Golden => new() { Dagger = CustomLeaderboardDagger.Devil, Time = cl.Devil },
				CustomLeaderboardDagger.Devil => new() { Dagger = CustomLeaderboardDagger.Leviathan, Time = cl.Leviathan },
				_ => null,
			},
		};
	}

	private sealed class CustomLeaderboardData
	{
		public CustomLeaderboardData(CustomLeaderboardEntity customLeaderboard, CustomLeaderboardOverviewWorldRecord? worldRecord, CustomLeaderboardOverviewSelectedPlayerStats? selectedPlayerStats, int playerCount)
		{
			CustomLeaderboard = customLeaderboard;
			WorldRecord = worldRecord;
			SelectedPlayerStats = selectedPlayerStats;
			PlayerCount = playerCount;
		}

		public CustomLeaderboardEntity CustomLeaderboard { get; }
		public CustomLeaderboardOverviewWorldRecord? WorldRecord { get; }
		public CustomLeaderboardOverviewSelectedPlayerStats? SelectedPlayerStats { get; }
		public int PlayerCount { get; }
	}
}
