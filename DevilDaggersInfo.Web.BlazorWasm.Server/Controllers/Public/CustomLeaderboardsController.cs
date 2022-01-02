using DevilDaggersInfo.Web.BlazorWasm.Server.Converters.Public;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomLeaderboards;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Enums.Sortings.Public;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Controllers.Public;

[Route("api/custom-leaderboards")]
[ApiController]
public class CustomLeaderboardsController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;
	private readonly IFileSystemService _fileSystemService;

	public CustomLeaderboardsController(ApplicationDbContext dbContext, IFileSystemService fileSystemService)
	{
		_dbContext = dbContext;
		_fileSystemService = fileSystemService;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public ActionResult<Page<GetCustomLeaderboardOverview>> GetCustomLeaderboards(
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
			.Where(cl => !cl.IsArchived)
			.Include(cl => cl.Spawnset)
				.ThenInclude(sf => sf.Player)
			.Where(cl => !cl.IsArchived && category == cl.Category);

		// Casing is ignored by default because of IQueryable.
		if (!string.IsNullOrWhiteSpace(spawnsetFilter))
			customLeaderboardsQuery = customLeaderboardsQuery.Where(cl => cl.Spawnset.Name.Contains(spawnsetFilter));

		if (!string.IsNullOrWhiteSpace(authorFilter))
			customLeaderboardsQuery = customLeaderboardsQuery.Where(cl => cl.Spawnset.Player.PlayerName.Contains(authorFilter));

		// Execute query.
		List<CustomLeaderboardEntity> customLeaderboards = customLeaderboardsQuery.ToList();

		// Query custom entries for world record and amount of players.
		List<int> customLeaderboardIds = customLeaderboards.ConvertAll(cl => cl.Id);
		var customEntries = _dbContext.CustomEntries
			.AsNoTracking()
			.Where(ce => customLeaderboardIds.Contains(ce.CustomLeaderboardId))
			.Include(ce => ce.Player)
			.Select(ce => new { ce.Time, ce.Player.PlayerName, ce.CustomLeaderboardId });

		// Build dictionary for amount of players.
		Dictionary<int, int> customEntryCountByCustomLeaderboardId = new();
		foreach (int customLeaderboardId in customEntries.Select(ce => ce.CustomLeaderboardId))
		{
			if (customEntryCountByCustomLeaderboardId.ContainsKey(customLeaderboardId))
				customEntryCountByCustomLeaderboardId[customLeaderboardId]++;
			else
				customEntryCountByCustomLeaderboardId.Add(customLeaderboardId, 1);
		}

		// Apply regular sorting.
		customLeaderboards = (sortBy switch
		{
			CustomLeaderboardSorting.AuthorName => customLeaderboards.OrderBy(cl => cl.Spawnset.Player.PlayerName, ascending),
			CustomLeaderboardSorting.DateLastPlayed => customLeaderboards.OrderBy(cl => cl.DateLastPlayed, ascending),
			CustomLeaderboardSorting.SpawnsetName => customLeaderboards.OrderBy(cl => cl.Spawnset.Name, ascending),
			CustomLeaderboardSorting.TimeBronze => customLeaderboards.OrderBy(cl => cl.TimeBronze, ascending),
			CustomLeaderboardSorting.TimeSilver => customLeaderboards.OrderBy(cl => cl.TimeSilver, ascending),
			CustomLeaderboardSorting.TimeGolden => customLeaderboards.OrderBy(cl => cl.TimeGolden, ascending),
			CustomLeaderboardSorting.TimeDevil => customLeaderboards.OrderBy(cl => cl.TimeDevil, ascending),
			CustomLeaderboardSorting.TimeLeviathan => customLeaderboards.OrderBy(cl => cl.TimeLeviathan, ascending),
			CustomLeaderboardSorting.DateCreated => customLeaderboards.OrderBy(cl => cl.DateCreated, ascending),
			CustomLeaderboardSorting.Players => customLeaderboards.OrderBy(cl => customEntryCountByCustomLeaderboardId.ContainsKey(cl.Id) ? customEntryCountByCustomLeaderboardId[cl.Id] : 0, ascending),
			CustomLeaderboardSorting.Submits => customLeaderboards.OrderBy(cl => cl.TotalRunsSubmitted, ascending),
			_ => customLeaderboards.OrderBy(cl => cl.Id, ascending),
		}).ToList();

		// Determine world records.
		if (category.IsAscending())
			customEntries = customEntries.OrderBy(wr => wr.Time);
		else
			customEntries = customEntries.OrderByDescending(wr => wr.Time);

		// Map custom leaderboards with world record data.
		List<CustomLeaderboardWorldRecord> customLeaderboardWrs = customLeaderboards
			.ConvertAll(cl => new CustomLeaderboardWorldRecord(
				cl,
				customEntries.FirstOrDefault(clwr => clwr.CustomLeaderboardId == cl.Id)?.Time,
				customEntries.FirstOrDefault(clwr => clwr.CustomLeaderboardId == cl.Id)?.PlayerName));

		// Apply sorting for world records.
		if (sortBy == CustomLeaderboardSorting.WorldRecord)
		{
			customLeaderboardWrs = ascending
				? customLeaderboardWrs.OrderBy(clwr => clwr.WorldRecord).ToList()
				: customLeaderboardWrs.OrderByDescending(clwr => clwr.WorldRecord).ToList();
		}
		else if (sortBy == CustomLeaderboardSorting.TopPlayer)
		{
			customLeaderboardWrs = ascending
				? customLeaderboardWrs.OrderBy(clwr => clwr.TopPlayer).ToList()
				: customLeaderboardWrs.OrderByDescending(clwr => clwr.TopPlayer).ToList();
		}

		// Apply paging.
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
				cl.TopPlayer,
				cl.WorldRecord)),
			TotalResults = totalCustomLeaderboards,
		};
	}

	[HttpGet("ddlive")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public ActionResult<List<GetCustomLeaderboardDdLive>> GetCustomLeaderboardsDdLive()
	{
		List<CustomLeaderboardEntity> customLeaderboards = _dbContext.CustomLeaderboards
			.AsNoTracking()
			.Where(cl => !cl.IsArchived)
			.Include(cl => cl.Spawnset)
				.ThenInclude(sf => sf.Player)
			.Where(cl => !cl.IsArchived)
			.ToList();

		// Query custom entries for world record and amount of players.
		List<int> customLeaderboardIds = customLeaderboards.ConvertAll(cl => cl.Id);
		var customEntries = _dbContext.CustomEntries
			.AsNoTracking()
			.Where(ce => customLeaderboardIds.Contains(ce.CustomLeaderboardId))
			.Include(ce => ce.Player)
			.Select(ce => new { ce.Time, ce.Player.PlayerName, ce.CustomLeaderboardId });

		// Build dictionary for amount of players.
		Dictionary<int, int> customEntryCountByCustomLeaderboardId = new();
		foreach (int customLeaderboardId in customEntries.Select(ce => ce.CustomLeaderboardId))
		{
			if (customEntryCountByCustomLeaderboardId.ContainsKey(customLeaderboardId))
				customEntryCountByCustomLeaderboardId[customLeaderboardId]++;
			else
				customEntryCountByCustomLeaderboardId.Add(customLeaderboardId, 1);
		}

		// Map custom leaderboards with world record data.
		List<CustomLeaderboardWorldRecord> customLeaderboardWrs = new();
		foreach (CustomLeaderboardEntity cl in customLeaderboards)
		{
			var worldRecord = cl.Category.IsAscending()
				? customEntries.OrderBy(ce => ce.Time).FirstOrDefault(clwr => clwr.CustomLeaderboardId == cl.Id)
				: customEntries.OrderByDescending(ce => ce.Time).FirstOrDefault(clwr => clwr.CustomLeaderboardId == cl.Id);

			customLeaderboardWrs.Add(new(cl, worldRecord?.Time, worldRecord?.PlayerName));
		}

		return customLeaderboardWrs
			.OrderByDescending(clwr => clwr.CustomLeaderboard.DateLastPlayed ?? clwr.CustomLeaderboard.DateCreated)
			.Select(clwr => clwr.CustomLeaderboard.ToGetCustomLeaderboardDdLive(
				customEntryCountByCustomLeaderboardId.ContainsKey(clwr.CustomLeaderboard.Id) ? customEntryCountByCustomLeaderboardId[clwr.CustomLeaderboard.Id] : 0,
				clwr.TopPlayer,
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
		};
	}

	[HttpGet("number-of-custom-leaderboards")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<GetNumberOfCustomLeaderboards> GetNumberOfCustomLeaderboards()
	{
		var customLeaderboards = _dbContext.CustomLeaderboards.AsNoTracking().Select(cl => new { cl.Id, cl.Category }).ToList();

		Dictionary<CustomLeaderboardCategory, int> countPerCategory = new();
		foreach (CustomLeaderboardCategory category in Enum.GetValues<CustomLeaderboardCategory>())
			countPerCategory[category] = customLeaderboards.Count(cl => cl.Category == category);

		return new GetNumberOfCustomLeaderboards
		{
			CountPerCategory = countPerCategory,
		};
	}

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public ActionResult<GetCustomLeaderboard> GetCustomLeaderboardById(int id)
	{
		CustomLeaderboardEntity? customLeaderboard = _dbContext.CustomLeaderboards
			.AsNoTracking()
			.Include(cl => cl.CustomEntries!)
				.ThenInclude(ce => ce.Player)
			.Include(cl => cl.Spawnset)
				.ThenInclude(sf => sf.Player)
			.FirstOrDefault(cl => cl.Id == id);
		if (customLeaderboard == null)
			return NotFound();

		List<int> existingReplayIds = customLeaderboard.CustomEntries == null ? new() : customLeaderboard.CustomEntries
			.Where(ce => IoFile.Exists(Path.Combine(_fileSystemService.GetPath(DataSubDirectory.CustomEntryReplays), $"{ce.Id}.ddreplay")))
			.Select(ce => ce.Id)
			.ToList();
		return customLeaderboard.ToGetCustomLeaderboard(existingReplayIds);
	}

	private sealed class CustomLeaderboardWorldRecord
	{
		public CustomLeaderboardWorldRecord(CustomLeaderboardEntity customLeaderboard, int? worldRecord, string? topPlayer)
		{
			CustomLeaderboard = customLeaderboard;
			WorldRecord = worldRecord;
			TopPlayer = topPlayer;
		}

		public CustomLeaderboardEntity CustomLeaderboard { get; }
		public int? WorldRecord { get; }
		public string? TopPlayer { get; }
	}
}
