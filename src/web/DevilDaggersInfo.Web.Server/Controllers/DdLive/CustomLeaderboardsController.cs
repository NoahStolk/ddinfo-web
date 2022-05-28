using DevilDaggersInfo.Web.Server.Converters.DdLive;
using DevilDaggersInfo.Web.Server.Entities.Views;
using DevilDaggersInfo.Web.Shared.Dto.DdLive.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Server.Controllers.DdLive;

[Route("api/ddlive/custom-leaderboards")]
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

	[HttpGet("/api/custom-leaderboards/ddlive")]
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

	[HttpGet("/api/custom-leaderboards/{id}/ddlive")]
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
