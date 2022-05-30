using DevilDaggersInfo.Web.Server.InternalModels.CustomLeaderboards;
using DevilDaggersInfo.Web.Shared.Enums.Sortings.Public;

namespace DevilDaggersInfo.Web.Server.Repositories;

public class CustomLeaderboardRepository
{
	private readonly ApplicationDbContext _dbContext;

	public CustomLeaderboardRepository(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<(List<CustomLeaderboardOverview> CustomLeaderboards, int TotalCount)> GetSortedCustomLeaderboardOverviewsAsync(
		CustomLeaderboardCategory category,
		string? spawnsetFilter = null,
		string? authorFilter = null,
		int pageIndex = 0,
		int pageSize = PagingConstants.PageSizeDefault,
		CustomLeaderboardSorting? sortBy = null,
		bool ascending = false)
	{
		// Build query.
		IQueryable<CustomLeaderboardEntity> customLeaderboardsQuery = _dbContext.CustomLeaderboards
			.AsNoTracking()
			.Include(cl => cl.Spawnset)
				.ThenInclude(sf => sf.Player)
			.Where(cl => category == cl.Category);

		// Casing is ignored by default because of IQueryable.
		if (!string.IsNullOrWhiteSpace(spawnsetFilter))
			customLeaderboardsQuery = customLeaderboardsQuery.Where(cl => cl.Spawnset.Name.Contains(spawnsetFilter));

		if (!string.IsNullOrWhiteSpace(authorFilter))
			customLeaderboardsQuery = customLeaderboardsQuery.Where(cl => cl.Spawnset.Player.PlayerName.Contains(authorFilter));

		// Execute query.
		List<CustomLeaderboardEntity> customLeaderboards = customLeaderboardsQuery.ToList();

		// Query custom entries for world record and amount of players.
		List<int> customLeaderboardIds = customLeaderboards.ConvertAll(cl => cl.Id);
		List<CustomEntryEntity> customEntries = await _dbContext.CustomEntries
			.AsNoTracking()
			.Where(ce => customLeaderboardIds.Contains(ce.CustomLeaderboardId))
			.Include(ce => ce.Player)
			.ToListAsync();

		// Determine world records.
		customEntries = customEntries.Sort(category).ToList();

		// Map custom leaderboards with world record data.
		List<CustomLeaderboardWorldRecord> customLeaderboardWrs = customLeaderboards.ConvertAll(cl =>
		{
			CustomEntryEntity? worldRecord = customEntries.Find(clwr => clwr.CustomLeaderboardId == cl.Id);
			CustomLeaderboardOverviewWorldRecord? worldRecordModel = worldRecord == null ? null : new()
			{
				Time = worldRecord.Time,
				PlayerId = worldRecord.PlayerId,
				PlayerName = worldRecord.Player.PlayerName,
				Dagger = cl.GetDaggerFromTime(worldRecord.Time),
			};
			return new CustomLeaderboardWorldRecord(cl, worldRecordModel);
		});

		// Build dictionary for amount of players.
		Dictionary<int, int> customEntryCountByCustomLeaderboardId = new();
		foreach (int customLeaderboardId in customEntries.Select(ce => ce.CustomLeaderboardId))
		{
			if (customEntryCountByCustomLeaderboardId.ContainsKey(customLeaderboardId))
				customEntryCountByCustomLeaderboardId[customLeaderboardId]++;
			else
				customEntryCountByCustomLeaderboardId.Add(customLeaderboardId, 1);
		}

		customLeaderboardWrs = (sortBy switch
		{
			CustomLeaderboardSorting.AuthorName => customLeaderboardWrs.OrderBy(cl => cl.CustomLeaderboard.Spawnset.Player.PlayerName, ascending),
			CustomLeaderboardSorting.DateLastPlayed => customLeaderboardWrs.OrderBy(cl => cl.CustomLeaderboard.DateLastPlayed, ascending),
			CustomLeaderboardSorting.SpawnsetName => customLeaderboardWrs.OrderBy(cl => cl.CustomLeaderboard.Spawnset.Name, ascending),
			CustomLeaderboardSorting.TimeBronze => customLeaderboardWrs.OrderBy(cl => cl.CustomLeaderboard.IsFeatured ? cl.CustomLeaderboard.TimeBronze : 0, ascending),
			CustomLeaderboardSorting.TimeSilver => customLeaderboardWrs.OrderBy(cl => cl.CustomLeaderboard.IsFeatured ? cl.CustomLeaderboard.TimeSilver : 0, ascending),
			CustomLeaderboardSorting.TimeGolden => customLeaderboardWrs.OrderBy(cl => cl.CustomLeaderboard.IsFeatured ? cl.CustomLeaderboard.TimeGolden : 0, ascending),
			CustomLeaderboardSorting.TimeDevil => customLeaderboardWrs.OrderBy(cl => cl.CustomLeaderboard.IsFeatured ? cl.CustomLeaderboard.TimeDevil : 0, ascending),
			CustomLeaderboardSorting.TimeLeviathan => customLeaderboardWrs.OrderBy(cl => cl.CustomLeaderboard.IsFeatured ? cl.CustomLeaderboard.TimeLeviathan : 0, ascending),
			CustomLeaderboardSorting.DateCreated => customLeaderboardWrs.OrderBy(cl => cl.CustomLeaderboard.DateCreated, ascending),
			CustomLeaderboardSorting.Players => customLeaderboardWrs.OrderBy(cl => customEntryCountByCustomLeaderboardId.ContainsKey(cl.CustomLeaderboard.Id) ? customEntryCountByCustomLeaderboardId[cl.CustomLeaderboard.Id] : 0, ascending),
			CustomLeaderboardSorting.Submits => customLeaderboardWrs.OrderBy(cl => cl.CustomLeaderboard.TotalRunsSubmitted, ascending),
			CustomLeaderboardSorting.WorldRecord => customLeaderboardWrs.OrderBy(cl => cl.WorldRecord?.Time, ascending),
			CustomLeaderboardSorting.TopPlayer => customLeaderboardWrs.OrderBy(cl => cl.WorldRecord?.PlayerName, ascending),
			_ => customLeaderboardWrs.OrderBy(cl => cl.CustomLeaderboard.Id, ascending),
		}).ToList();

		int totalCustomLeaderboards = customLeaderboards.Count;
		int lastPageIndex = totalCustomLeaderboards / pageSize;

		return new()
		{
			CustomLeaderboards = customLeaderboardWrs
				.Skip(Math.Min(pageIndex, lastPageIndex) * pageSize)
				.Take(pageSize)
				.Select(cl => ToOverview(cl, customEntryCountByCustomLeaderboardId))
				.ToList(),
			TotalCount = totalCustomLeaderboards,
		};
	}

	public async Task<List<CustomLeaderboardOverview>> GetCustomLeaderboardOverviewsAsync()
	{
		List<CustomLeaderboardEntity> customLeaderboards = await _dbContext.CustomLeaderboards
			.AsNoTracking()
			.Include(cl => cl.Spawnset)
				.ThenInclude(sf => sf.Player)
			.ToListAsync();

		// Query custom entries for world record and amount of players.
		List<int> customLeaderboardIds = customLeaderboards.ConvertAll(cl => cl.Id);
		List<CustomEntryEntity> customEntries = await _dbContext.CustomEntries
			.AsNoTracking()
			.Where(ce => customLeaderboardIds.Contains(ce.CustomLeaderboardId))
			.Include(ce => ce.Player)
			.ToListAsync();

		// Map custom leaderboards with world record data.
		List<CustomLeaderboardWorldRecord> customLeaderboardWrs = new();
		foreach (CustomLeaderboardEntity cl in customLeaderboards)
		{
			CustomEntryEntity? worldRecord = customEntries.Where(ce => ce.CustomLeaderboardId == cl.Id).Sort(cl.Category).FirstOrDefault();
			CustomLeaderboardOverviewWorldRecord? worldRecordModel = worldRecord == null ? null : new()
			{
				Time = worldRecord.Time,
				PlayerId = worldRecord.PlayerId,
				PlayerName = worldRecord.Player.PlayerName,
				Dagger = cl.GetDaggerFromTime(worldRecord.Time),
			};
			customLeaderboardWrs.Add(new(cl, worldRecordModel));
		}

		// Build dictionary for amount of players.
		Dictionary<int, int> customEntryCountByCustomLeaderboardId = new();
		foreach (int customLeaderboardId in customEntries.Select(ce => ce.CustomLeaderboardId))
		{
			if (customEntryCountByCustomLeaderboardId.ContainsKey(customLeaderboardId))
				customEntryCountByCustomLeaderboardId[customLeaderboardId]++;
			else
				customEntryCountByCustomLeaderboardId.Add(customLeaderboardId, 1);
		}

		return customLeaderboardWrs
			.OrderByDescending(clwr => clwr.CustomLeaderboard.DateLastPlayed ?? clwr.CustomLeaderboard.DateCreated)
			.Select(cl => ToOverview(cl, customEntryCountByCustomLeaderboardId))
			.ToList();
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

		return new()
		{
			Category = customLeaderboard.Category,
			CustomEntries = customLeaderboard.CustomEntries!
				.Sort(customLeaderboard.Category)
				.Select((ce, i) =>
				{
					bool isDdcl = ce.Client == CustomLeaderboardsClient.DevilDaggersCustomLeaderboards;
					Version clientVersionParsed = Version.TryParse(ce.ClientVersion, out Version? version) ? version : new(0, 0, 0, 0);
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
				Bronze = customLeaderboard.TimeBronze,
				Silver = customLeaderboard.TimeSilver,
				Golden = customLeaderboard.TimeGolden,
				Devil = customLeaderboard.TimeDevil,
				Leviathan = customLeaderboard.TimeLeviathan,
			},
			DateCreated = customLeaderboard.DateCreated,
			DateLastPlayed = customLeaderboard.DateLastPlayed,
			Id = customLeaderboard.Id,
			SpawnsetAuthorId = customLeaderboard.Spawnset.PlayerId,
			SpawnsetAuthorName = customLeaderboard.Spawnset.Player.PlayerName,
			SpawnsetId = customLeaderboard.SpawnsetId,
			TotalRunsSubmitted = customLeaderboard.TotalRunsSubmitted,
		};
	}

	private static CustomLeaderboardOverview ToOverview(CustomLeaderboardWorldRecord cl, Dictionary<int, int> customEntryCountByCustomLeaderboardId) => new()
	{
		Category = cl.CustomLeaderboard.Category,
		Daggers = !cl.CustomLeaderboard.IsFeatured ? null : new()
		{
			Bronze = cl.CustomLeaderboard.TimeBronze,
			Silver = cl.CustomLeaderboard.TimeSilver,
			Golden = cl.CustomLeaderboard.TimeGolden,
			Devil = cl.CustomLeaderboard.TimeDevil,
			Leviathan = cl.CustomLeaderboard.TimeLeviathan,
		},
		DateCreated = cl.CustomLeaderboard.DateCreated,
		DateLastPlayed = cl.CustomLeaderboard.DateLastPlayed,
		Id = cl.CustomLeaderboard.Id,
		PlayerCount = customEntryCountByCustomLeaderboardId.ContainsKey(cl.CustomLeaderboard.Id) ? customEntryCountByCustomLeaderboardId[cl.CustomLeaderboard.Id] : 0,
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

	private sealed class CustomLeaderboardWorldRecord
	{
		public CustomLeaderboardWorldRecord(CustomLeaderboardEntity customLeaderboard, CustomLeaderboardOverviewWorldRecord? worldRecord)
		{
			CustomLeaderboard = customLeaderboard;
			WorldRecord = worldRecord;
		}

		public CustomLeaderboardEntity CustomLeaderboard { get; }
		public CustomLeaderboardOverviewWorldRecord? WorldRecord { get; }
	}
}
