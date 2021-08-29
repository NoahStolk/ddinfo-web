using DevilDaggersInfo.Web.BlazorWasm.Server.Converters.Public;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomLeaderboards;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Mods;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Enums.Sortings.Public;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Controllers.Public;

[Route("api/custom-leaderboards")]
[ApiController]
public class CustomLeaderboardsController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;

	public CustomLeaderboardsController(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public ActionResult<Page<GetCustomLeaderboardOverview>> GetCustomLeaderboards(
		CustomLeaderboardCategory category,
		[Range(0, 1000)] int pageIndex = 0,
		[Range(PublicPagingConstants.PageSizeMin, PublicPagingConstants.PageSizeMax)] int pageSize = PublicPagingConstants.PageSizeDefault,
		CustomLeaderboardSorting? sortBy = null,
		bool ascending = false)
	{
		IQueryable<CustomLeaderboardEntity> customLeaderboardsQuery = _dbContext.CustomLeaderboards
			.AsNoTracking()
			.Where(cl => !cl.IsArchived)
			.Include(cl => cl.Spawnset)
				.ThenInclude(sf => sf.Player)
			.Where(cl => !cl.IsArchived && category == cl.Category);

		customLeaderboardsQuery = sortBy switch
		{
			CustomLeaderboardSorting.AuthorName => customLeaderboardsQuery.OrderBy(cl => cl.Spawnset.Player.PlayerName, ascending),
			CustomLeaderboardSorting.DateLastPlayed => customLeaderboardsQuery.OrderBy(cl => cl.DateLastPlayed, ascending),
			CustomLeaderboardSorting.SpawnsetName => customLeaderboardsQuery.OrderBy(cl => cl.Spawnset.Name, ascending),
			CustomLeaderboardSorting.TimeBronze => customLeaderboardsQuery.OrderBy(cl => cl.TimeBronze, ascending),
			CustomLeaderboardSorting.TimeSilver => customLeaderboardsQuery.OrderBy(cl => cl.TimeSilver, ascending),
			CustomLeaderboardSorting.TimeGolden => customLeaderboardsQuery.OrderBy(cl => cl.TimeGolden, ascending),
			CustomLeaderboardSorting.TimeDevil => customLeaderboardsQuery.OrderBy(cl => cl.TimeDevil, ascending),
			CustomLeaderboardSorting.TimeLeviathan => customLeaderboardsQuery.OrderBy(cl => cl.TimeLeviathan, ascending),
			CustomLeaderboardSorting.DateCreated => customLeaderboardsQuery.OrderBy(cl => cl.DateCreated, ascending),
			_ => customLeaderboardsQuery.OrderBy(cl => cl.Id, ascending),
		};

		List<CustomLeaderboardEntity> customLeaderboards = customLeaderboardsQuery.ToList();

		IEnumerable<int> customLeaderboardIds = customLeaderboards.Select(cl => cl.Id);
		var customEntries = _dbContext.CustomEntries
			.AsNoTracking()
			.Where(ce => customLeaderboardIds.Contains(ce.CustomLeaderboardId))
			.Include(ce => ce.Player)
			.Select(ce => new { ce.Time, ce.Player.PlayerName, ce.CustomLeaderboardId });

		if (category.IsAscending())
			customEntries = customEntries.OrderBy(wr => wr.Time);
		else
			customEntries = customEntries.OrderByDescending(wr => wr.Time);

		List<CustomLeaderboardWorldRecord> customLeaderboardWrs = customLeaderboards
			.ConvertAll(cl => new CustomLeaderboardWorldRecord(
				cl,
				customEntries.FirstOrDefault(clwr => clwr.CustomLeaderboardId == cl.Id)?.Time,
				customEntries.FirstOrDefault(clwr => clwr.CustomLeaderboardId == cl.Id)?.PlayerName));

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

		customLeaderboardWrs = customLeaderboardWrs
			.Skip(pageIndex * pageSize)
			.Take(pageSize)
			.ToList();

		return new Page<GetCustomLeaderboardOverview>
		{
			Results = customLeaderboardWrs.ConvertAll(cl => cl.CustomLeaderboard.ToGetCustomLeaderboardOverview(cl.TopPlayer, cl.WorldRecord)),
			TotalResults = customLeaderboardsQuery.Count(),
		};
	}

	[HttpGet("total-data")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<GetTotalCustomLeaderboardData> GetTotalCustomLeaderboardData()
	{
		var customLeaderboards = _dbContext.CustomLeaderboards.AsNoTracking().Select(cl => new { cl.Id, cl.Category, cl.TotalRunsSubmitted }).ToList();
		List<int> customEntryPlayerIds = _dbContext.CustomEntries.AsNoTracking().Select(ce => ce.PlayerId).ToList();

		return new GetTotalCustomLeaderboardData
		{
			CountDefault = customLeaderboards.Count(cl => cl.Category == CustomLeaderboardCategory.Default),
			CountSpeedrun = customLeaderboards.Count(cl => cl.Category == CustomLeaderboardCategory.Speedrun),
			TotalSubmits = customLeaderboards.Sum(cl => cl.TotalRunsSubmitted),
			UniqueScores = customEntryPlayerIds.Count,
			UniquePlayers = customEntryPlayerIds.Distinct().Count(),
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

		return customLeaderboard.ToGetCustomLeaderboard();
	}

	private class CustomLeaderboardWorldRecord
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
