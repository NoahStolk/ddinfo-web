using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.SpawnsetSummaries;
using DevilDaggersInfo.Web.BlazorWasm.Server.Converters.Public;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Spawnsets;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Enums.Sortings.Public;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Controllers.Public;

[Route("api/spawnsets")]
[ApiController]
public class SpawnsetsController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;
	private readonly IFileSystemService _fileSystemService;
	private readonly SpawnsetSummaryCache _spawnsetSummaryCache;

	public SpawnsetsController(ApplicationDbContext dbContext, IFileSystemService fileSystemService, SpawnsetSummaryCache spawnsetSummaryCache)
	{
		_dbContext = dbContext;
		_fileSystemService = fileSystemService;
		_spawnsetSummaryCache = spawnsetSummaryCache;
	}

	[HttpGet("overview")] // Can't use default route because already in use by DDSE.
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public ActionResult<Page<GetSpawnsetOverview>> GetSpawnsets(
		bool onlyPractice,
		bool onlyWithLeaderboard,
		[Range(0, 1000)] int pageIndex = 0,
		[Range(PublicPagingConstants.PageSizeMin, PublicPagingConstants.PageSizeMax)] int pageSize = PublicPagingConstants.PageSizeDefault,
		SpawnsetSorting? sortBy = null,
		bool ascending = false)
	{
		IQueryable<SpawnsetEntity> spawnsetsQuery = _dbContext.Spawnsets.AsNoTracking().Include(s => s.Player);

		if (onlyPractice)
			spawnsetsQuery = spawnsetsQuery.Where(s => s.IsPractice);

		if (onlyWithLeaderboard)
		{
			List<int> spawnsetsWithCustomLeaderboardIds = _dbContext.CustomLeaderboards
				.AsNoTracking()
				.Where(cl => !cl.IsArchived)
				.Select(cl => cl.SpawnsetId)
				.ToList();

			spawnsetsQuery = spawnsetsQuery.Where(s => spawnsetsWithCustomLeaderboardIds.Contains(s.Id));
		}

		List<SpawnsetEntity> spawnsets = spawnsetsQuery.ToList();

		Dictionary<int, SpawnsetSummary> summaries = new();
		foreach (string filePath in _fileSystemService.TryGetFiles(DataSubDirectory.Spawnsets))
		{
			string name = Path.GetFileNameWithoutExtension(filePath);
			SpawnsetEntity? spawnset = spawnsets.Find(s => s.Name == name);
			if (spawnset == null)
				continue;

			summaries[spawnset.Id] = _spawnsetSummaryCache.GetSpawnsetSummaryByFilePath(filePath);
		}

		// In case a spawnset doesn't have a summary; remove it.
		spawnsets = spawnsets.Where(s => summaries.ContainsKey(s.Id)).ToList();

		spawnsets = sortBy switch
		{
			SpawnsetSorting.Name => spawnsets.OrderBy(s => s.Name, ascending).ToList(),
			SpawnsetSorting.AuthorName => spawnsets.OrderBy(s => s.Player.PlayerName, ascending).ToList(),
			SpawnsetSorting.LastUpdated => spawnsets.OrderBy(s => s.LastUpdated, ascending).ToList(),
			SpawnsetSorting.GameVersion => spawnsets.OrderBy(s => Spawnset.GetGameVersionString(summaries[s.Id].WorldVersion, summaries[s.Id].SpawnVersion), ascending).ToList(),
			SpawnsetSorting.GameMode => spawnsets.OrderBy(s => summaries[s.Id].GameMode, ascending).ToList(),
			SpawnsetSorting.LoopLength => spawnsets.OrderBy(s => summaries[s.Id].LoopSection.Length, ascending).ToList(),
			SpawnsetSorting.LoopSpawnCount => spawnsets.OrderBy(s => summaries[s.Id].LoopSection.SpawnCount, ascending).ToList(),
			SpawnsetSorting.PreLoopLength => spawnsets.OrderBy(s => summaries[s.Id].PreLoopSection.Length, ascending).ToList(),
			SpawnsetSorting.PreLoopSpawnCount => spawnsets.OrderBy(s => summaries[s.Id].PreLoopSection.SpawnCount, ascending).ToList(),
			SpawnsetSorting.Hand => spawnsets.OrderBy(s => summaries[s.Id].HandLevel, ascending).ToList(),
			SpawnsetSorting.AdditionalGems => spawnsets.OrderBy(s => summaries[s.Id].AdditionalGems, ascending).ToList(),
			SpawnsetSorting.TimerStart => spawnsets.OrderBy(s => summaries[s.Id].TimerStart, ascending).ToList(),
			_ => spawnsets.OrderBy(s => s.Id, ascending).ToList(),
		};

		spawnsets = spawnsets
			.Skip(pageIndex * pageSize)
			.Take(pageSize)
			.ToList();

		return new Page<GetSpawnsetOverview>
		{
			Results = spawnsets.ConvertAll(s => s.ToGetSpawnsetOverview(summaries[s.Id])),
			TotalResults = spawnsetsQuery.Count(),
		};
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public List<GetSpawnsetDdse> GetSpawnsetsForDdse(string? authorFilter = null, string? nameFilter = null)
	{
		List<int> spawnsetsWithCustomLeaderboardIds = _dbContext.CustomLeaderboards
			.AsNoTracking()
			.Where(cl => !cl.IsArchived)
			.Select(cl => cl.SpawnsetId)
			.ToList();

		IEnumerable<SpawnsetEntity> query = _dbContext.Spawnsets.AsNoTracking().Include(sf => sf.Player);

		if (!string.IsNullOrWhiteSpace(authorFilter))
			query = query.Where(sf => sf.Player.PlayerName.Contains(authorFilter, StringComparison.InvariantCultureIgnoreCase));

		if (!string.IsNullOrWhiteSpace(nameFilter))
			query = query.Where(sf => sf.Name.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase));

		return query
			.Where(sf => IoFile.Exists(Path.Combine(_fileSystemService.GetPath(DataSubDirectory.Spawnsets), sf.Name)))
			.Select(sf => Map(sf))
			.ToList();

		GetSpawnsetDdse Map(SpawnsetEntity spawnset)
		{
			SpawnsetSummary spawnsetSummary = _spawnsetSummaryCache.GetSpawnsetSummaryByFilePath(Path.Combine(_fileSystemService.GetPath(DataSubDirectory.Spawnsets), spawnset.Name));
			return spawnset.ToGetSpawnsetDdse(spawnsetSummary, spawnsetsWithCustomLeaderboardIds.Contains(spawnset.Id));
		}
	}

	[HttpGet("total-data")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<GetTotalSpawnsetData> GetTotalSpawnsetData()
	{
		return new GetTotalSpawnsetData
		{
			Count = _dbContext.Spawnsets.AsNoTracking().Select(s => s.Id).Count(),
		};
	}

	[HttpGet("{fileName}/file")]
	[ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public ActionResult GetSpawnsetFile([Required] string fileName)
	{
		string path = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.Spawnsets), fileName);
		if (!IoFile.Exists(path))
			return NotFound();

		return File(IoFile.ReadAllBytes(path), MediaTypeNames.Application.Octet, fileName);
	}

	[HttpGet("{id}")]
	[ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public ActionResult<GetSpawnset> GetSpawnsetById([Required] int id)
	{
		SpawnsetEntity? spawnsetEntity = _dbContext.Spawnsets
			.AsNoTracking()
			.Include(s => s.Player)
			.FirstOrDefault(s => s.Id == id);
		if (spawnsetEntity == null)
			return NotFound();

		string path = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.Spawnsets), spawnsetEntity.Name);
		if (!IoFile.Exists(path))
			return NotFound();

		return spawnsetEntity.ToGetSpawnset(IoFile.ReadAllBytes(path));
	}
}
