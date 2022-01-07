using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.SpawnsetSummaries;
using DevilDaggersInfo.Web.BlazorWasm.Server.Converters.Public;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomEntries;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Utils;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Controllers.Public;

[Route("api/custom-entries")]
[ApiController]
public class CustomEntriesController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;
	private readonly ILogger<CustomEntriesController> _logger;
	private readonly SpawnsetSummaryCache _spawnsetSummaryCache;
	private readonly IFileSystemService _fileSystemService;
	private readonly CustomEntryProcessor _customEntryProcessor;

	public CustomEntriesController(ApplicationDbContext dbContext, ILogger<CustomEntriesController> logger, SpawnsetSummaryCache spawnsetSummaryCache, IFileSystemService fileSystemService, CustomEntryProcessor customEntryProcessor)
	{
		_dbContext = dbContext;
		_logger = logger;
		_spawnsetSummaryCache = spawnsetSummaryCache;
		_fileSystemService = fileSystemService;
		_customEntryProcessor = customEntryProcessor;
	}

	[HttpGet("{id}/replay")]
	[ProducesResponseType(StatusCodes.Status200OK)]
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
		return customEntry.ToGetCustomEntryData(customEntryData, eps.HandLevel);
	}

	[HttpGet("player-stats")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<List<GetCustomLeaderboardStatisticsForPlayer>> GetCustomLeaderboardStatisticsByPlayerId([Required] int playerId)
	{
		var customEntries = _dbContext.CustomEntries
			.AsNoTracking()
			.Include(ce => ce.CustomLeaderboard)
			.Where(cl => cl.PlayerId == playerId)
			.Select(ce => new
			{
				ce.Time,
				ce.CustomLeaderboardId,
				ce.CustomLeaderboard.Category,
				ce.CustomLeaderboard.TimeLeviathan,
				ce.CustomLeaderboard.TimeDevil,
				ce.CustomLeaderboard.TimeGolden,
				ce.CustomLeaderboard.TimeSilver,
				ce.CustomLeaderboard.TimeBronze,
			})
			.ToList();

		List<GetCustomLeaderboardStatisticsForPlayer> stats = new();
		foreach (CustomLeaderboardCategory category in Enum.GetValues<CustomLeaderboardCategory>())
		{
			var customEntriesByCategory = customEntries.Where(c => c.Category == category);
			if (!customEntriesByCategory.Any())
				continue;

			int leviathanDaggers = 0;
			int devilDaggers = 0;
			int goldenDaggers = 0;
			int silverDaggers = 0;
			int bronzeDaggers = 0;
			int defaultDaggers = 0;
			int played = 0;
			foreach (var customEntry in customEntriesByCategory)
			{
				played++;
				switch (CustomLeaderboardUtils.GetDaggerFromTime(category, customEntry.Time, customEntry.TimeLeviathan, customEntry.TimeDevil, customEntry.TimeGolden, customEntry.TimeSilver, customEntry.TimeBronze))
				{
					case CustomLeaderboardDagger.Leviathan: leviathanDaggers++; break;
					case CustomLeaderboardDagger.Devil: devilDaggers++; break;
					case CustomLeaderboardDagger.Golden: goldenDaggers++; break;
					case CustomLeaderboardDagger.Silver: silverDaggers++; break;
					case CustomLeaderboardDagger.Bronze: bronzeDaggers++; break;
					default: defaultDaggers++; break;
				}
			}

			stats.Add(new()
			{
				CustomLeaderboardCategory = category,
				LeviathanDaggerCount = leviathanDaggers,
				DevilDaggerCount = devilDaggers,
				GoldenDaggerCount = goldenDaggers,
				SilverDaggerCount = silverDaggers,
				BronzeDaggerCount = bronzeDaggers,
				DefaultDaggerCount = defaultDaggers,
				LeaderboardsPlayedCount = played,
			});
		}

		return stats;
	}

	[HttpPost("submit")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<GetUploadSuccess>> SubmitScoreForDdcl([FromBody] AddUploadRequest uploadRequest)
	{
		try
		{
			return await _customEntryProcessor.ProcessUploadRequest(uploadRequest);
		}
		catch (Exception ex)
		{
			ex.Data[nameof(uploadRequest.ClientVersion)] = uploadRequest.ClientVersion;
			ex.Data[nameof(uploadRequest.OperatingSystem)] = uploadRequest.OperatingSystem;
			ex.Data[nameof(uploadRequest.BuildMode)] = uploadRequest.BuildMode;
			_logger.LogError(ex, "Upload failed for user `{playerName}` (`{playerId}`) for `{spawnset}`.", uploadRequest.PlayerName, uploadRequest.PlayerId, uploadRequest.SurvivalHashMd5.ByteArrayToHexString());
			throw;
		}
	}
}
