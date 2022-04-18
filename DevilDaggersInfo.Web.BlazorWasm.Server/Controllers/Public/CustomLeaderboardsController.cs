using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.SpawnsetHashes;
using DevilDaggersInfo.Web.BlazorWasm.Server.Converters.Public;
using DevilDaggersInfo.Web.BlazorWasm.Server.Entities.Views;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomLeaderboards;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Enums.Sortings.Public;
using DevilDaggersInfo.Web.BlazorWasm.Shared.InternalModels;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Utils;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Controllers.Public;

[Route("api/custom-leaderboards")]
[ApiController]
public class CustomLeaderboardsController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;
	private readonly SpawnsetHashCache _spawnsetHashCache;
	private readonly IFileSystemService _fileSystemService;

	public CustomLeaderboardsController(ApplicationDbContext dbContext, SpawnsetHashCache spawnsetHashCache, IFileSystemService fileSystemService)
	{
		_dbContext = dbContext;
		_spawnsetHashCache = spawnsetHashCache;
		_fileSystemService = fileSystemService;
	}

	[HttpHead]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> CustomLeaderboardExistsBySpawnsetHash([FromQuery] byte[] hash)
	{
		SpawnsetHashCacheData? data = _spawnsetHashCache.GetSpawnset(hash);
		if (data == null)
			return NotFound();

		var spawnset = await _dbContext.Spawnsets
			.Select(s => new { s.Id, s.Name })
			.FirstOrDefaultAsync(s => s.Name == data.Name);
		if (spawnset == null)
			return NotFound();

		if (!await _dbContext.CustomLeaderboards.AnyAsync(cl => cl.SpawnsetId == spawnset.Id))
			return NotFound();

		return Ok();
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<Page<GetCustomLeaderboardOverview>>> GetCustomLeaderboards(
		CustomLeaderboardCategory category,
		string? spawnsetFilter = null,
		string? authorFilter = null,
		[Range(0, 1000)] int pageIndex = 0,
		[Range(PagingConstants.PageSizeMin, PagingConstants.PageSizeMax)] int pageSize = PagingConstants.PageSizeDefault,
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
		List<CustomEntryBase> customEntries = await _dbContext.CustomEntries
			.AsNoTracking()
			.Where(ce => customLeaderboardIds.Contains(ce.CustomLeaderboardId))
			.Include(ce => ce.Player)
			.Select(ce => new CustomEntryBase(ce.Time, ce.SubmitDate, ce.CustomLeaderboardId, ce.Player.PlayerName))
			.ToListAsync();

		// Determine world records.
		customEntries = customEntries.Sort(category).ToList();

		// Map custom leaderboards with world record data.
		List<CustomLeaderboardWorldRecord> customLeaderboardWrs = customLeaderboards.ConvertAll(cl =>
		{
			CustomEntryBase? wr = customEntries.Find(clwr => clwr.CustomLeaderboardId == cl.Id);
			return new CustomLeaderboardWorldRecord(
				cl,
				wr?.Time,
				null, // We don't return the ID from this endpoint.
				wr?.PlayerName);
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
			CustomLeaderboardSorting.WorldRecord => customLeaderboardWrs.OrderBy(cl => cl.WorldRecord, ascending),
			CustomLeaderboardSorting.TopPlayer => customLeaderboardWrs.OrderBy(cl => cl.TopPlayerName, ascending),
			_ => customLeaderboardWrs.OrderBy(cl => cl.CustomLeaderboard.Id, ascending),
		}).ToList();

		int totalCustomLeaderboards = customLeaderboards.Count;
		int lastPageIndex = totalCustomLeaderboards / pageSize;
		customLeaderboardWrs = customLeaderboardWrs
			.Skip(Math.Min(pageIndex, lastPageIndex) * pageSize)
			.Take(pageSize)
			.ToList();

		return new Page<GetCustomLeaderboardOverview>
		{
			Results = customLeaderboardWrs.ConvertAll(cl => cl.CustomLeaderboard.ToGetCustomLeaderboardOverview(
				customEntryCountByCustomLeaderboardId.ContainsKey(cl.CustomLeaderboard.Id) ? customEntryCountByCustomLeaderboardId[cl.CustomLeaderboard.Id] : 0,
				cl.TopPlayerName,
				cl.WorldRecord)),
			TotalResults = totalCustomLeaderboards,
		};
	}

	[HttpGet("global-leaderboard")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<GetGlobalCustomLeaderboard>> GetGlobalCustomLeaderboardForCategory(CustomLeaderboardCategory category)
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

		return new GetGlobalCustomLeaderboard
		{
			Entries = globalData
				.Select(kvp => new GetGlobalCustomLeaderboardEntry
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

	[HttpGet("ddlive")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<List<GetCustomLeaderboardOverviewDdLive>>> GetCustomLeaderboardsOverviewDdLive()
	{
		List<CustomLeaderboardEntity> customLeaderboards = _dbContext.CustomLeaderboards
			.AsNoTracking()
			.Include(cl => cl.Spawnset)
				.ThenInclude(sf => sf.Player)
			.ToList();

		// Query custom entries for world record and amount of players.
		List<int> customLeaderboardIds = customLeaderboards.ConvertAll(cl => cl.Id);
		List<CustomEntryBaseDdLive> customEntries = await _dbContext.CustomEntries
			.AsNoTracking()
			.Where(ce => customLeaderboardIds.Contains(ce.CustomLeaderboardId))
			.Include(ce => ce.Player)
			.Select(ce => new CustomEntryBaseDdLive(ce.Time, ce.SubmitDate, ce.CustomLeaderboardId, ce.PlayerId, ce.Player.PlayerName))
			.ToListAsync();

		// Map custom leaderboards with world record data.
		List<CustomLeaderboardWorldRecord> customLeaderboardWrs = new();
		foreach (CustomLeaderboardEntity cl in customLeaderboards)
		{
			CustomEntryBaseDdLive? worldRecord = customEntries.Where(ce => ce.CustomLeaderboardId == cl.Id).Sort(cl.Category).FirstOrDefault();
			customLeaderboardWrs.Add(new(cl, worldRecord?.Time, worldRecord?.PlayerId, worldRecord?.PlayerName));
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
			.Select(clwr => clwr.CustomLeaderboard.ToGetCustomLeaderboardOverviewDdLive(
				customEntryCountByCustomLeaderboardId.ContainsKey(clwr.CustomLeaderboard.Id) ? customEntryCountByCustomLeaderboardId[clwr.CustomLeaderboard.Id] : 0,
				clwr.TopPlayerId,
				clwr.TopPlayerName,
				clwr.WorldRecord))
			.ToList();
	}

	[HttpGet("total-data")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<GetTotalCustomLeaderboardData> GetTotalCustomLeaderboardData()
	{
		var customLeaderboards = _dbContext.CustomLeaderboards.AsNoTracking().Select(cl => new { cl.Id, cl.Category, cl.TotalRunsSubmitted }).ToList();
		var customEntries = _dbContext.CustomEntries.AsNoTracking().Select(ce => new { ce.PlayerId, ce.CustomLeaderboard.Category }).ToList();

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

		return new GetTotalCustomLeaderboardData
		{
			LeaderboardsPerCategory = leaderboardsPerCategory,
			ScoresPerCategory = scoresPerCategory,
			SubmitsPerCategory = submitsPerCategory,
			PlayersPerCategory = playersPerCategory,
			TotalPlayers = customEntries.DistinctBy(ce => ce.PlayerId).Count(),
		};
	}

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetCustomLeaderboard>> GetCustomLeaderboardById(int id)
	{
		CustomLeaderboardEntity? customLeaderboard = await _dbContext.CustomLeaderboards
			.AsNoTracking()
			.Include(cl => cl.CustomEntries!)
				.ThenInclude(ce => ce.Player)
			.Include(cl => cl.Spawnset)
				.ThenInclude(sf => sf.Player)
			.FirstOrDefaultAsync(cl => cl.Id == id);
		if (customLeaderboard == null)
			return NotFound();

		List<CustomEntry> customEntries = customLeaderboard.CustomEntries!.ConvertAll(ce => new CustomEntry(
			ce.Time,
			ce.SubmitDate,
			ce.Id,
			ce.CustomLeaderboardId,
			ce.PlayerId,
			ce.Player.PlayerName,
			ce.Player.CountryCode,
			ce.GemsCollected,
			ce.GemsDespawned,
			ce.GemsEaten,
			ce.GemsTotal,
			ce.EnemiesAlive,
			ce.EnemiesKilled,
			ce.HomingStored,
			ce.HomingEaten,
			ce.DeathType,
			ce.DaggersFired,
			ce.DaggersHit,
			ce.LevelUpTime2,
			ce.LevelUpTime3,
			ce.LevelUpTime4,
			ce.Client,
			ce.ClientVersion));

		customEntries = customEntries.Sort(customLeaderboard.Category).ToList();

		return customLeaderboard.ToGetCustomLeaderboard(customEntries);
	}

	[HttpGet("{id}/ddlive")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetCustomLeaderboardDdLive>> GetCustomLeaderboardByIdDdLive(int id)
	{
		CustomLeaderboardEntity? customLeaderboard = await _dbContext.CustomLeaderboards
			.AsNoTracking()
			.Include(cl => cl.CustomEntries!)
				.ThenInclude(ce => ce.Player)
			.Include(cl => cl.Spawnset)
				.ThenInclude(sf => sf.Player)
			.FirstOrDefaultAsync(cl => cl.Id == id);
		if (customLeaderboard == null)
			return NotFound();

		List<CustomEntry> customEntries = customLeaderboard.CustomEntries!.ConvertAll(ce => new CustomEntry(
			ce.Time,
			ce.SubmitDate,
			ce.Id,
			ce.CustomLeaderboardId,
			ce.PlayerId,
			ce.Player.PlayerName,
			ce.Player.CountryCode,
			ce.GemsCollected,
			ce.GemsDespawned,
			ce.GemsEaten,
			ce.GemsTotal,
			ce.EnemiesAlive,
			ce.EnemiesKilled,
			ce.HomingStored,
			ce.HomingEaten,
			ce.DeathType,
			ce.DaggersFired,
			ce.DaggersHit,
			ce.LevelUpTime2,
			ce.LevelUpTime3,
			ce.LevelUpTime4,
			ce.Client,
			ce.ClientVersion));

		customEntries = customEntries.Sort(customLeaderboard.Category).ToList();

		List<int> customEntryReplayIds = customEntries
			.Where(ce => IoFile.Exists(Path.Combine(_fileSystemService.GetPath(DataSubDirectory.CustomEntryReplays), $"{ce.Id}.ddreplay")))
			.Select(ce => ce.Id)
			.ToList();

		return customLeaderboard.ToGetCustomLeaderboardDdLive(customEntries, customEntryReplayIds);
	}

	private sealed class CustomLeaderboardWorldRecord
	{
		public CustomLeaderboardWorldRecord(CustomLeaderboardEntity customLeaderboard, int? worldRecord, int? topPlayerId, string? topPlayerName)
		{
			CustomLeaderboard = customLeaderboard;
			WorldRecord = worldRecord;
			TopPlayerId = topPlayerId;
			TopPlayerName = topPlayerName;
		}

		public CustomLeaderboardEntity CustomLeaderboard { get; }
		public int? WorldRecord { get; }
		public int? TopPlayerId { get; }
		public string? TopPlayerName { get; }
	}
}
