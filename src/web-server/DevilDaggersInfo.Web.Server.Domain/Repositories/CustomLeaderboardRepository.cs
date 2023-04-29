using DevilDaggersInfo.Common.Extensions;
using DevilDaggersInfo.Core.CriteriaExpression;
using DevilDaggersInfo.Core.Versioning;
using DevilDaggersInfo.Web.Server.Domain.Constants;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Extensions;
using DevilDaggersInfo.Web.Server.Domain.Models;
using DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;
using DevilDaggersInfo.Web.Server.Domain.Utils;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace DevilDaggersInfo.Web.Server.Domain.Repositories;

public class CustomLeaderboardRepository
{
	private readonly ApplicationDbContext _dbContext;
	private readonly CustomEntryRepository _customEntryRepository;

	public CustomLeaderboardRepository(ApplicationDbContext dbContext, CustomEntryRepository customEntryRepository)
	{
		_dbContext = dbContext;
		_customEntryRepository = customEntryRepository;
	}

	public async Task<Page<CustomLeaderboardOverview>> GetCustomLeaderboardOverviewsAsync(
		CustomLeaderboardRankSorting? rankSorting,
		SpawnsetGameMode? gameMode,
		string? spawnsetFilter,
		string? authorFilter,
		int pageIndex,
		int pageSize,
		CustomLeaderboardSorting? sortBy,
		bool ascending,
		int? selectedPlayerId,
		bool onlyFeatured)
	{
		// ! Navigation property.
		IQueryable<CustomLeaderboardEntity> customLeaderboardsQuery = _dbContext.CustomLeaderboards
			.AsNoTracking()
			.Include(cl => cl.Spawnset)
				.ThenInclude(sf => sf!.Player);

		if (rankSorting.HasValue)
			customLeaderboardsQuery = customLeaderboardsQuery.Where(cl => rankSorting == cl.RankSorting);

		if (gameMode.HasValue)
		{
			// ! Navigation property.
			customLeaderboardsQuery = customLeaderboardsQuery.Where(cl => gameMode == cl.Spawnset!.GameMode);
		}

		if (onlyFeatured)
			customLeaderboardsQuery = customLeaderboardsQuery.Where(cl => cl.IsFeatured);

		// Casing is ignored by default because of IQueryable.
		if (!string.IsNullOrWhiteSpace(spawnsetFilter))
		{
			// ! Navigation property.
			customLeaderboardsQuery = customLeaderboardsQuery.Where(cl => cl.Spawnset!.Name.Contains(spawnsetFilter));
		}

		if (!string.IsNullOrWhiteSpace(authorFilter))
		{
			// ! Navigation property.
			customLeaderboardsQuery = customLeaderboardsQuery.Where(cl => cl.Spawnset!.Player!.PlayerName.Contains(authorFilter));
		}

		// Execute query.
		List<CustomLeaderboardEntity> customLeaderboards = await customLeaderboardsQuery.ToListAsync();

		// Query custom entries for additional data.
		List<int> customLeaderboardIds = customLeaderboards.ConvertAll(cl => cl.Id);

		// ! Navigation property.
		List<CustomEntrySummary> customEntries = await _dbContext.CustomEntries
			.AsNoTracking()
			.Include(ce => ce.Player)
			.Select(ce => new CustomEntrySummary { CustomLeaderboardId = ce.CustomLeaderboardId, PlayerId = ce.PlayerId, PlayerName = ce.Player!.PlayerName, Time = ce.Time, SubmitDate = ce.SubmitDate })
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
				Dagger = cl.DaggerFromStat(worldRecord.Time),
			};

			CustomEntrySummary? selectedEntry = sortedCustomEntries.Find(ce => ce.PlayerId == selectedPlayerId);
			CustomLeaderboardOverviewSelectedPlayerStats? selectedPlayerStatsModel = GetSelectedStats(cl, sortedCustomEntries, selectedEntry);
			customLeaderboardData.Add(new(cl, worldRecordModel, selectedPlayerStatsModel, sortedCustomEntries.Count));
		}

		// ! Navigation property.
		customLeaderboardData = (sortBy switch
		{
			CustomLeaderboardSorting.AuthorName => customLeaderboardData.OrderBy(cl => cl.CustomLeaderboard.Spawnset!.Player!.PlayerName.ToLower(), ascending),
			CustomLeaderboardSorting.DateLastPlayed => customLeaderboardData.OrderBy(cl => cl.CustomLeaderboard.DateLastPlayed ?? cl.CustomLeaderboard.DateCreated, ascending),
			CustomLeaderboardSorting.SpawnsetName => customLeaderboardData.OrderBy(cl => cl.CustomLeaderboard.Spawnset!.Name.ToLower(), ascending),
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
		// ! Navigation property.
		CustomLeaderboardEntity? customLeaderboard = await _dbContext.CustomLeaderboards
			.AsNoTracking()
			.Include(cl => cl.CustomEntries!)
				.ThenInclude(ce => ce.Player)
			.Include(cl => cl.Spawnset)
				.ThenInclude(sf => sf!.Player)
			.FirstOrDefaultAsync(cl => cl.Id == id);
		if (customLeaderboard == null)
			throw new NotFoundException($"Custom leaderboard '{id}' could not be found.");

		// ! Navigation property.
		List<int> existingReplayIds = _customEntryRepository.GetExistingCustomEntryReplayIds(customLeaderboard.CustomEntries!.ConvertAll(ce => ce.Id));

		// ! Navigation property.
		return new()
		{
			Criteria = GetCriteria(customLeaderboard),
			CustomEntries = customLeaderboard.CustomEntries
				.Sort(customLeaderboard.Category)
				.Select((ce, i) =>
				{
					bool isOldDdcl = ce.Client == CustomLeaderboardsClient.DevilDaggersCustomLeaderboards;
					AppVersion clientVersionParsed = AppVersion.TryParse(ce.ClientVersion, out AppVersion? version) ? version : new(0, 0, 0);
					bool hasV3_1Values = !isOldDdcl || clientVersionParsed >= FeatureConstants.OldDdclV3_1;
					bool hasHomingEatenValue = !isOldDdcl || clientVersionParsed >= FeatureConstants.OldDdclHomingEaten;

					// ! Navigation property.
					return new CustomEntry
					{
						Client = ce.Client,
						ClientVersion = ce.ClientVersion,
						CountryCode = ce.Player!.CountryCode,
						CustomLeaderboardDagger = customLeaderboard.DaggerFromStat(ce.Time),
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
			GameMode = customLeaderboard.Spawnset!.GameMode,
			Id = customLeaderboard.Id,
			RankSorting = customLeaderboard.RankSorting,
			SpawnsetHtmlDescription = customLeaderboard.Spawnset!.HtmlDescription,
			SpawnsetName = customLeaderboard.Spawnset.Name,
			SpawnsetAuthorId = customLeaderboard.Spawnset.PlayerId,
			SpawnsetAuthorName = customLeaderboard.Spawnset.Player!.PlayerName,
			SpawnsetId = customLeaderboard.SpawnsetId,
			TotalRunsSubmitted = customLeaderboard.TotalRunsSubmitted,
		};
	}

	public async Task<CustomLeaderboardsTotalData> GetCustomLeaderboardsTotalDataAsync()
	{
		var customLeaderboards = await _dbContext.CustomLeaderboards.AsNoTracking().Select(cl => new { cl.Id, cl.Category, cl.TotalRunsSubmitted }).ToListAsync();

		// ! Navigation property.
		var customEntries = await _dbContext.CustomEntries.AsNoTracking().Select(ce => new { ce.PlayerId, ce.CustomLeaderboard!.Category }).ToListAsync();

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
		// ! Navigation property.
		List<CustomLeaderboardEntity> customLeaderboards = await _dbContext.CustomLeaderboards
			.AsNoTracking()
			.Include(cl => cl.CustomEntries!)
				.ThenInclude(ce => ce.Player)
			.Where(ce => ce.Category == category && ce.IsFeatured)
			.ToListAsync();

		Dictionary<int, (string Name, GlobalCustomLeaderboardEntryData Data)> globalData = new();
		foreach (CustomLeaderboardEntity customLeaderboard in customLeaderboards)
		{
			// ! Navigation property.
			List<CustomEntryEntity> customEntries = customLeaderboard.CustomEntries!.Sort(category).ToList();

			for (int i = 0; i < customEntries.Count; i++)
			{
				CustomEntryEntity customEntry = customEntries[i];

				GlobalCustomLeaderboardEntryData data;
				if (!globalData.ContainsKey(customEntry.PlayerId))
				{
					data = new();

					// ! Navigation property.
					globalData.Add(customEntry.PlayerId, (customEntry.Player!.PlayerName, data));
				}
				else
				{
					data = globalData[customEntry.PlayerId].Data;
				}

				data.Rankings.Add(new() { Rank = i + 1, TotalPlayers = customEntries.Count });

				switch (customLeaderboard.DaggerFromStat(customEntry.Time) ?? throw new InvalidOperationException("Custom leaderboard without daggers may not be used for processing global custom leaderboard data."))
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

		// ! Navigation property.
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
		var spawnset = await _dbContext.Spawnsets.Select(s => new { s.Id, s.Md5Hash }).FirstOrDefaultAsync(s => s.Md5Hash == hash);
		if (spawnset == null)
			throw new NotFoundException();

		var customLeaderboard = await _dbContext.CustomLeaderboards
			.Select(cl => new { cl.Id, cl.SpawnsetId })
			.FirstOrDefaultAsync(cl => cl.SpawnsetId == spawnset.Id);
		if (customLeaderboard == null)
			throw new NotFoundException();

		return customLeaderboard.Id;
	}

	private static CustomLeaderboardOverview ToOverview(CustomLeaderboardData cl)
	{
		if (cl.CustomLeaderboard.Spawnset == null)
			throw new InvalidOperationException("Spawnset is not included.");

		if (cl.CustomLeaderboard.Spawnset.Player == null)
			throw new InvalidOperationException("Spawnset author is not included.");

		return new()
		{
			Criteria = GetCriteria(cl.CustomLeaderboard),
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
			GameMode = cl.CustomLeaderboard.Spawnset.GameMode,
			Id = cl.CustomLeaderboard.Id,
			PlayerCount = cl.PlayerCount,
			RankSorting = cl.CustomLeaderboard.RankSorting,
			SelectedPlayerStats = cl.SelectedPlayerStats,
			SpawnsetAuthorId = cl.CustomLeaderboard.Spawnset.PlayerId,
			SpawnsetAuthorName = cl.CustomLeaderboard.Spawnset.Player.PlayerName,
			SpawnsetId = cl.CustomLeaderboard.SpawnsetId,
			SpawnsetName = cl.CustomLeaderboard.Spawnset.Name,
			TotalRunsSubmitted = cl.CustomLeaderboard.TotalRunsSubmitted,
			WorldRecord = cl.WorldRecord == null ? null : new()
			{
				PlayerId = cl.WorldRecord.PlayerId, PlayerName = cl.WorldRecord.PlayerName, Time = cl.WorldRecord.Time, Dagger = cl.CustomLeaderboard.DaggerFromStat(cl.WorldRecord.Time),
			},
		};
	}

	[return: NotNullIfNotNull(nameof(selectedEntry))]
	private static CustomLeaderboardOverviewSelectedPlayerStats? GetSelectedStats(CustomLeaderboardEntity cl, List<CustomEntrySummary> sortedCustomEntries, CustomEntrySummary? selectedEntry)
	{
		if (selectedEntry == null)
			return null;

		CustomLeaderboardDagger? dagger = cl.DaggerFromStat(selectedEntry.Time);
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

	private static List<CustomLeaderboardCriteria> GetCriteria(CustomLeaderboardEntity customLeaderboard)
	{
		List<CustomLeaderboardCriteria> criteria = new();
		AddCriteria(CustomLeaderboardCriteriaType.GemsCollected, customLeaderboard.GemsCollectedCriteria.Operator, customLeaderboard.GemsCollectedCriteria.Expression);
		AddCriteria(CustomLeaderboardCriteriaType.GemsDespawned, customLeaderboard.GemsDespawnedCriteria.Operator, customLeaderboard.GemsDespawnedCriteria.Expression);
		AddCriteria(CustomLeaderboardCriteriaType.GemsEaten, customLeaderboard.GemsEatenCriteria.Operator, customLeaderboard.GemsEatenCriteria.Expression);
		AddCriteria(CustomLeaderboardCriteriaType.EnemiesKilled, customLeaderboard.EnemiesKilledCriteria.Operator, customLeaderboard.EnemiesKilledCriteria.Expression);
		AddCriteria(CustomLeaderboardCriteriaType.DaggersFired, customLeaderboard.DaggersFiredCriteria.Operator, customLeaderboard.DaggersFiredCriteria.Expression);
		AddCriteria(CustomLeaderboardCriteriaType.DaggersHit, customLeaderboard.DaggersHitCriteria.Operator, customLeaderboard.DaggersHitCriteria.Expression);
		AddCriteria(CustomLeaderboardCriteriaType.HomingStored, customLeaderboard.HomingStoredCriteria.Operator, customLeaderboard.HomingStoredCriteria.Expression);
		AddCriteria(CustomLeaderboardCriteriaType.HomingEaten, customLeaderboard.HomingEatenCriteria.Operator, customLeaderboard.HomingEatenCriteria.Expression);
		AddCriteria(CustomLeaderboardCriteriaType.DeathType, customLeaderboard.DeathTypeCriteria.Operator, customLeaderboard.DeathTypeCriteria.Expression);
		AddCriteria(CustomLeaderboardCriteriaType.Time, customLeaderboard.TimeCriteria.Operator, customLeaderboard.TimeCriteria.Expression);
		AddCriteria(CustomLeaderboardCriteriaType.LevelUpTime2, customLeaderboard.LevelUpTime2Criteria.Operator, customLeaderboard.LevelUpTime2Criteria.Expression);
		AddCriteria(CustomLeaderboardCriteriaType.LevelUpTime3, customLeaderboard.LevelUpTime3Criteria.Operator, customLeaderboard.LevelUpTime3Criteria.Expression);
		AddCriteria(CustomLeaderboardCriteriaType.LevelUpTime4, customLeaderboard.LevelUpTime4Criteria.Operator, customLeaderboard.LevelUpTime4Criteria.Expression);
		AddCriteria(CustomLeaderboardCriteriaType.EnemiesAlive, customLeaderboard.EnemiesAliveCriteria.Operator, customLeaderboard.EnemiesAliveCriteria.Expression);
		AddCriteria(CustomLeaderboardCriteriaType.Skull1Kills, customLeaderboard.Skull1KillsCriteria.Operator, customLeaderboard.Skull1KillsCriteria.Expression);
		AddCriteria(CustomLeaderboardCriteriaType.Skull2Kills, customLeaderboard.Skull2KillsCriteria.Operator, customLeaderboard.Skull2KillsCriteria.Expression);
		AddCriteria(CustomLeaderboardCriteriaType.Skull3Kills, customLeaderboard.Skull3KillsCriteria.Operator, customLeaderboard.Skull3KillsCriteria.Expression);
		AddCriteria(CustomLeaderboardCriteriaType.Skull4Kills, customLeaderboard.Skull4KillsCriteria.Operator, customLeaderboard.Skull4KillsCriteria.Expression);
		AddCriteria(CustomLeaderboardCriteriaType.SpiderlingKills, customLeaderboard.SpiderlingKillsCriteria.Operator, customLeaderboard.SpiderlingKillsCriteria.Expression);
		AddCriteria(CustomLeaderboardCriteriaType.SpiderEggKills, customLeaderboard.SpiderEggKillsCriteria.Operator, customLeaderboard.SpiderEggKillsCriteria.Expression);
		AddCriteria(CustomLeaderboardCriteriaType.Squid1Kills, customLeaderboard.Squid1KillsCriteria.Operator, customLeaderboard.Squid1KillsCriteria.Expression);
		AddCriteria(CustomLeaderboardCriteriaType.Squid2Kills, customLeaderboard.Squid2KillsCriteria.Operator, customLeaderboard.Squid2KillsCriteria.Expression);
		AddCriteria(CustomLeaderboardCriteriaType.Squid3Kills, customLeaderboard.Squid3KillsCriteria.Operator, customLeaderboard.Squid3KillsCriteria.Expression);
		AddCriteria(CustomLeaderboardCriteriaType.CentipedeKills, customLeaderboard.CentipedeKillsCriteria.Operator, customLeaderboard.CentipedeKillsCriteria.Expression);
		AddCriteria(CustomLeaderboardCriteriaType.GigapedeKills, customLeaderboard.GigapedeKillsCriteria.Operator, customLeaderboard.GigapedeKillsCriteria.Expression);
		AddCriteria(CustomLeaderboardCriteriaType.GhostpedeKills, customLeaderboard.GhostpedeKillsCriteria.Operator, customLeaderboard.GhostpedeKillsCriteria.Expression);
		AddCriteria(CustomLeaderboardCriteriaType.Spider1Kills, customLeaderboard.Spider1KillsCriteria.Operator, customLeaderboard.Spider1KillsCriteria.Expression);
		AddCriteria(CustomLeaderboardCriteriaType.Spider2Kills, customLeaderboard.Spider2KillsCriteria.Operator, customLeaderboard.Spider2KillsCriteria.Expression);
		AddCriteria(CustomLeaderboardCriteriaType.LeviathanKills, customLeaderboard.LeviathanKillsCriteria.Operator, customLeaderboard.LeviathanKillsCriteria.Expression);
		AddCriteria(CustomLeaderboardCriteriaType.OrbKills, customLeaderboard.OrbKillsCriteria.Operator, customLeaderboard.OrbKillsCriteria.Expression);
		AddCriteria(CustomLeaderboardCriteriaType.ThornKills, customLeaderboard.ThornKillsCriteria.Operator, customLeaderboard.ThornKillsCriteria.Expression);
		AddCriteria(CustomLeaderboardCriteriaType.Skull1sAlive, customLeaderboard.Skull1sAliveCriteria.Operator, customLeaderboard.Skull1sAliveCriteria.Expression);
		AddCriteria(CustomLeaderboardCriteriaType.Skull2sAlive, customLeaderboard.Skull2sAliveCriteria.Operator, customLeaderboard.Skull2sAliveCriteria.Expression);
		AddCriteria(CustomLeaderboardCriteriaType.Skull3sAlive, customLeaderboard.Skull3sAliveCriteria.Operator, customLeaderboard.Skull3sAliveCriteria.Expression);
		AddCriteria(CustomLeaderboardCriteriaType.Skull4sAlive, customLeaderboard.Skull4sAliveCriteria.Operator, customLeaderboard.Skull4sAliveCriteria.Expression);
		AddCriteria(CustomLeaderboardCriteriaType.SpiderlingsAlive, customLeaderboard.SpiderlingsAliveCriteria.Operator, customLeaderboard.SpiderlingsAliveCriteria.Expression);
		AddCriteria(CustomLeaderboardCriteriaType.SpiderEggsAlive, customLeaderboard.SpiderEggsAliveCriteria.Operator, customLeaderboard.SpiderEggsAliveCriteria.Expression);
		AddCriteria(CustomLeaderboardCriteriaType.Squid1sAlive, customLeaderboard.Squid1sAliveCriteria.Operator, customLeaderboard.Squid1sAliveCriteria.Expression);
		AddCriteria(CustomLeaderboardCriteriaType.Squid2sAlive, customLeaderboard.Squid2sAliveCriteria.Operator, customLeaderboard.Squid2sAliveCriteria.Expression);
		AddCriteria(CustomLeaderboardCriteriaType.Squid3sAlive, customLeaderboard.Squid3sAliveCriteria.Operator, customLeaderboard.Squid3sAliveCriteria.Expression);
		AddCriteria(CustomLeaderboardCriteriaType.CentipedesAlive, customLeaderboard.CentipedesAliveCriteria.Operator, customLeaderboard.CentipedesAliveCriteria.Expression);
		AddCriteria(CustomLeaderboardCriteriaType.GigapedesAlive, customLeaderboard.GigapedesAliveCriteria.Operator, customLeaderboard.GigapedesAliveCriteria.Expression);
		AddCriteria(CustomLeaderboardCriteriaType.GhostpedesAlive, customLeaderboard.GhostpedesAliveCriteria.Operator, customLeaderboard.GhostpedesAliveCriteria.Expression);
		AddCriteria(CustomLeaderboardCriteriaType.Spider1sAlive, customLeaderboard.Spider1sAliveCriteria.Operator, customLeaderboard.Spider1sAliveCriteria.Expression);
		AddCriteria(CustomLeaderboardCriteriaType.Spider2sAlive, customLeaderboard.Spider2sAliveCriteria.Operator, customLeaderboard.Spider2sAliveCriteria.Expression);
		AddCriteria(CustomLeaderboardCriteriaType.LeviathansAlive, customLeaderboard.LeviathansAliveCriteria.Operator, customLeaderboard.LeviathansAliveCriteria.Expression);
		AddCriteria(CustomLeaderboardCriteriaType.OrbsAlive, customLeaderboard.OrbsAliveCriteria.Operator, customLeaderboard.OrbsAliveCriteria.Expression);
		AddCriteria(CustomLeaderboardCriteriaType.ThornsAlive, customLeaderboard.ThornsAliveCriteria.Operator, customLeaderboard.ThornsAliveCriteria.Expression);
		return criteria.Where(c => c.Operator != CustomLeaderboardCriteriaOperator.Any).ToList();

		void AddCriteria(CustomLeaderboardCriteriaType criteriaType, CustomLeaderboardCriteriaOperator op, byte[]? expression)
		{
			if (expression == null)
				return;

			criteria.Add(new()
			{
				Type = criteriaType,
				Operator = op,
				Expression = expression,
			});
		}
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
