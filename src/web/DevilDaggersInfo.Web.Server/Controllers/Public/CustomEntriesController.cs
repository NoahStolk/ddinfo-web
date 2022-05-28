using DevilDaggersInfo.Web.Server.Caches.SpawnsetSummaries;
using DevilDaggersInfo.Web.Server.Converters.Public;
using DevilDaggersInfo.Web.Shared.Dto.Public.CustomEntries;

namespace DevilDaggersInfo.Web.Server.Controllers.Public;

[Route("api/custom-entries")]
[ApiController]
public class CustomEntriesController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;
	private readonly SpawnsetSummaryCache _spawnsetSummaryCache;
	private readonly IFileSystemService _fileSystemService;

	public CustomEntriesController(ApplicationDbContext dbContext, SpawnsetSummaryCache spawnsetSummaryCache, IFileSystemService fileSystemService)
	{
		_dbContext = dbContext;
		_spawnsetSummaryCache = spawnsetSummaryCache;
		_fileSystemService = fileSystemService;
	}

	[HttpGet("{id}/replay-buffer")]
	[ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public ActionResult<byte[]> GetCustomEntryReplayBufferById([Required] int id)
	{
		string path = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.CustomEntryReplays), $"{id}.ddreplay");
		if (!IoFile.Exists(path))
			return NotFound();

		return IoFile.ReadAllBytes(path);
	}

	// FORBIDDEN: Used by DDLIVE.
	// FORBIDDEN: Used by DDCL 1.8.3.0.
	[HttpGet("{id}/replay")]
	[ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public ActionResult GetCustomEntryReplayById([Required] int id)
	{
		string path = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.CustomEntryReplays), $"{id}.ddreplay");
		if (!IoFile.Exists(path))
			return NotFound();

		var customEntry = _dbContext.CustomEntries
			.AsNoTracking()
			.Select(ce => new
			{
				ce.Id,
				ce.CustomLeaderboard.SpawnsetId,
				SpawnsetName = ce.CustomLeaderboard.Spawnset.Name,
				ce.PlayerId,
				ce.Player.PlayerName,
			})
			.FirstOrDefault(ce => ce.Id == id);
		if (customEntry == null)
			return NotFound();

		string fileName = $"{customEntry.SpawnsetId}-{customEntry.SpawnsetName}-{customEntry.PlayerId}-{customEntry.PlayerName}.ddreplay";
		return File(IoFile.ReadAllBytes(path), MediaTypeNames.Application.Octet, fileName);
	}

	[HttpGet("{id}/data")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public ActionResult<GetCustomEntryData> GetCustomEntryDataById([Required] int id)
	{
		CustomEntryEntity? customEntry = _dbContext.CustomEntries
			.AsNoTracking()
			.Include(ce => ce.Player)
			.Include(ce => ce.CustomLeaderboard)
				.ThenInclude(cl => cl.Spawnset)
			.FirstOrDefault(cl => cl.Id == id);
		if (customEntry == null)
			return NotFound();

		CustomEntryDataEntity? customEntryData = _dbContext.CustomEntryData
			.AsNoTracking()
			.FirstOrDefault(ced => ced.CustomEntryId == id);

		SpawnsetSummary ss = _spawnsetSummaryCache.GetSpawnsetSummaryByFilePath(Path.Combine(_fileSystemService.GetPath(DataSubDirectory.Spawnsets), customEntry.CustomLeaderboard.Spawnset.Name));
		EffectivePlayerSettings eps = SpawnsetBinary.GetEffectivePlayerSettings(ss.HandLevel, ss.AdditionalGems);
		return customEntry.ToGetCustomEntryData(customEntryData, eps.HandLevel, IoFile.Exists(Path.Combine(_fileSystemService.GetPath(DataSubDirectory.CustomEntryReplays), $"{id}.ddreplay")));
	}
}
