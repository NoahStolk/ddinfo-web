using DevilDaggersInfo.Api.Ddse.Spawnsets;
using DevilDaggersInfo.Web.Server.Caches.SpawnsetSummaries;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.Ddse;

namespace DevilDaggersInfo.Web.Server.Controllers.Ddse;

[Route("api/ddse/spawnsets")]
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

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public List<GetSpawnsetDdse> GetSpawnsets(string? authorFilter = null, string? nameFilter = null)
	{
		return GetSpawnsetsRepo(authorFilter, nameFilter);
	}

	// Used by DDSE 2.45.0.0.
	[Obsolete("Use the new route instead.")]
	[HttpGet("/api/spawnsets/ddse")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public List<GetSpawnsetDdse> GetSpawnsetsObsolete(string? authorFilter = null, string? nameFilter = null)
	{
		return GetSpawnsetsRepo(authorFilter, nameFilter);
	}

	// TODO: Move to repository.
	private List<GetSpawnsetDdse> GetSpawnsetsRepo(string? authorFilter, string? nameFilter)
	{
		IEnumerable<SpawnsetEntity> query = _dbContext.Spawnsets.AsNoTracking().Include(sf => sf.Player);

		if (!string.IsNullOrWhiteSpace(authorFilter))
			query = query.Where(sf => sf.Player.PlayerName.Contains(authorFilter, StringComparison.InvariantCultureIgnoreCase));
		if (!string.IsNullOrWhiteSpace(nameFilter))
			query = query.Where(sf => sf.Name.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase));

		List<int> spawnsetsWithCustomLeaderboardIds = _dbContext.CustomLeaderboards
			.AsNoTracking()
			.Select(cl => cl.SpawnsetId)
			.ToList();

		return query
			.Where(s => IoFile.Exists(Path.Combine(_fileSystemService.GetPath(DataSubDirectory.Spawnsets), s.Name)))
			.Select(s =>
			{
				SpawnsetSummary spawnsetSummary = _spawnsetSummaryCache.GetSpawnsetSummaryByFilePath(Path.Combine(_fileSystemService.GetPath(DataSubDirectory.Spawnsets), s.Name));
				return s.ToDdseApi(spawnsetSummary, spawnsetsWithCustomLeaderboardIds.Contains(s.Id));
			})
			.ToList();
	}

	// Not used yet.
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
}
