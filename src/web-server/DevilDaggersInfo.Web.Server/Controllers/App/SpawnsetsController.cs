using DevilDaggersInfo.Api.App.Spawnsets;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.App;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Models.Spawnsets;
using DevilDaggersInfo.Web.Server.Domain.Services.Caching;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;

namespace DevilDaggersInfo.Web.Server.Controllers.App;

[Route("api/app/spawnsets")]
[ApiController]
public class SpawnsetsController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;
	private readonly IFileSystemService _fileSystemService;
	private readonly ILogger<SpawnsetsController> _logger;
	private readonly SpawnsetHashCache _spawnsetHashCache;

	public SpawnsetsController(ApplicationDbContext dbContext, IFileSystemService fileSystemService, ILogger<SpawnsetsController> logger, SpawnsetHashCache spawnsetHashCache)
	{
		_dbContext = dbContext;
		_fileSystemService = fileSystemService;
		_logger = logger;
		_spawnsetHashCache = spawnsetHashCache;
	}

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
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

		var customLeaderboard = _dbContext.CustomLeaderboards
			.AsNoTracking()
			.Select(cl => new { cl.Id, cl.SpawnsetId })
			.FirstOrDefault(cl => cl.SpawnsetId == spawnsetEntity.Id);

		return spawnsetEntity.ToGetSpawnset(customLeaderboard?.Id, IoFile.ReadAllBytes(path));
	}

	[HttpGet("{id}/buffer")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetSpawnsetBuffer>> GetSpawnsetBufferById([Required] int id)
	{
		var spawnset = await _dbContext.Spawnsets
			.AsNoTracking()
			.Select(s => new { s.Id, s.Name })
			.FirstOrDefaultAsync(s => s.Id == id);
		if (spawnset == null)
			throw new NotFoundException();

		string path = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.Spawnsets), spawnset.Name);
		if (!IoFile.Exists(path))
			throw new NotFoundException();

		return new GetSpawnsetBuffer
		{
			Data = await IoFile.ReadAllBytesAsync(path),
		};
	}

	[HttpGet("by-hash")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public ActionResult<GetSpawnsetByHash> GetSpawnsetByHash([FromQuery] byte[] hash)
	{
		SpawnsetHashCacheData? data = _spawnsetHashCache.GetSpawnset(hash);
		if (data == null)
			return NotFound();

		SpawnsetEntity? spawnset = _dbContext.Spawnsets
			.AsNoTracking()
			.Include(s => s.Player)
			.FirstOrDefault(s => s.Name == data.Name);
		if (spawnset == null)
		{
			_logger.LogWarning("Spawnset {name} was found in hash cache but not in database.", data.Name);
			return NotFound();
		}

		CustomLeaderboardEntity? customLeaderboard = _dbContext.CustomLeaderboards
			.AsNoTracking()
			.FirstOrDefault(cl => cl.SpawnsetId == spawnset.Id);

		var customEntries = customLeaderboard == null ? null : _dbContext.CustomEntries
			.AsNoTracking()
			.Select(ce => new { ce.Id, ce.CustomLeaderboardId, ce.Time })
			.Where(ce => ce.CustomLeaderboardId == customLeaderboard.Id)
			.ToList();

		return new GetSpawnsetByHash
		{
			AuthorName = spawnset.Player.PlayerName,
			CustomLeaderboard = customLeaderboard == null ? null : new GetSpawnsetByHashCustomLeaderboard
			{
				CustomLeaderboardId = customLeaderboard.Id,
				CustomEntries = customEntries?.ConvertAll(ce => new GetSpawnsetByHashCustomEntry
				{
					HasReplay = false,
					CustomEntryId = ce.Id,
					Time = ce.Time,
				}) ?? new(),
			},
			SpawnsetId = spawnset.Id,
			Name = spawnset.Name,
		};
	}
}
