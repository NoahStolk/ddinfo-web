using DevilDaggersInfo.Api.Main.CustomLeaderboards;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.Main;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Repositories;
using DevilDaggersInfo.Web.Server.Domain.Services.Caching;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;

namespace DevilDaggersInfo.Web.Server.Controllers.Main;

[Route("api/custom-entries")]
[ApiController]
public class CustomEntriesController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;
	private readonly SpawnsetSummaryCache _spawnsetSummaryCache;
	private readonly IFileSystemService _fileSystemService;
	private readonly CustomEntryRepository _customEntryRepository;

	public CustomEntriesController(ApplicationDbContext dbContext, SpawnsetSummaryCache spawnsetSummaryCache, IFileSystemService fileSystemService, CustomEntryRepository customEntryRepository)
	{
		_dbContext = dbContext;
		_spawnsetSummaryCache = spawnsetSummaryCache;
		_fileSystemService = fileSystemService;
		_customEntryRepository = customEntryRepository;
	}

	[HttpGet("{id}/replay-buffer")]
	[ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)] // TODO: Remove incorrect response type FileContentResult.
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public ActionResult<byte[]> GetCustomEntryReplayBufferById([Required] int id)
	{
		return _customEntryRepository.GetCustomEntryReplayBufferById(id);
	}

	// FORBIDDEN: Used by DDLIVE.
	// FORBIDDEN: Used by DDCL 1.8.3.0.
	[HttpGet("{id}/replay")]
	[ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public ActionResult GetCustomEntryReplayById([Required] int id)
	{
		(string fileName, byte[] contents) = _customEntryRepository.GetCustomEntryReplayById(id);
		return File(contents, MediaTypeNames.Application.Octet, fileName);
	}

	[HttpGet("{id}/data")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public ActionResult<GetCustomEntryData> GetCustomEntryDataById([Required] int id)
	{
		// ! Navigation property.
		CustomEntryEntity? customEntry = _dbContext.CustomEntries
			.AsNoTracking()
			.Include(ce => ce.Player)
			.Include(ce => ce.CustomLeaderboard)
				.ThenInclude(cl => cl!.Spawnset)
			.FirstOrDefault(cl => cl.Id == id);
		if (customEntry == null)
			return NotFound();

		CustomEntryDataEntity? customEntryData = _dbContext.CustomEntryData
			.AsNoTracking()
			.FirstOrDefault(ced => ced.CustomEntryId == id);

		// ! Navigation property.
		SpawnsetSummary ss = _spawnsetSummaryCache.GetSpawnsetSummaryByFilePath(Path.Combine(_fileSystemService.GetPath(DataSubDirectory.Spawnsets), customEntry.CustomLeaderboard!.Spawnset!.Name));
		return customEntry.ToGetCustomEntryData(customEntryData, ss.EffectivePlayerSettings.HandLevel, IoFile.Exists(Path.Combine(_fileSystemService.GetPath(DataSubDirectory.CustomEntryReplays), $"{id}.ddreplay")));
	}
}
