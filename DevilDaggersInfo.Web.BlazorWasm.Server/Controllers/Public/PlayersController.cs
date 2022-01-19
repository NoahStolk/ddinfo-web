using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.LeaderboardHistory;
using DevilDaggersInfo.Web.BlazorWasm.Server.Converters.Public;
using DevilDaggersInfo.Web.BlazorWasm.Server.InternalModels.Json;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Players;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Controllers.Public;

[Route("api/players")]
[ApiController]
public class PlayersController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;
	private readonly IFileSystemService _fileSystemService;
	private readonly LeaderboardHistoryCache _leaderboardHistoryCache;

	public PlayersController(ApplicationDbContext dbContext, IFileSystemService fileSystemService, LeaderboardHistoryCache leaderboardHistoryCache)
	{
		_dbContext = dbContext;
		_fileSystemService = fileSystemService;
		_leaderboardHistoryCache = leaderboardHistoryCache;
	}

	[HttpGet("leaderboard")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<List<GetPlayerForLeaderboard>> GetPlayersForLeaderboard()
	{
		return _dbContext.Players
			.AsNoTracking()

			// TODO: Check if this can be combined without querying entire entity.
			.Select(p => new { p.Id, p.BanType, p.BanDescription, p.BanResponsibleId, p.CountryCode })
			.Select(p => new GetPlayerForLeaderboard
			{
				Id = p.Id,
				BanType = p.BanType,
				BanDescription = p.BanDescription,
				BanResponsibleId = p.BanResponsibleId,
				CountryCode = p.CountryCode,
			})
			.ToList();
	}

	[HttpGet("settings")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<List<GetPlayerForSettings>> GetPlayersForSettings()
	{
		List<PlayerEntity> players = _dbContext.Players
			.AsNoTracking()
			.Where(p => p.BanType == BanType.NotBanned && !p.HideSettings)
			.ToList();

		// Note; cannot evaluate HasSettings() against database (IQueryable).
		return players.Where(p => p.HasSettings()).Select(p => p.ToGetPlayerForSettings()).ToList();
	}

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public ActionResult<GetPlayer> GetPlayerById([Required] int id)
	{
		PlayerEntity? player = _dbContext.Players
			.AsNoTracking()
			.FirstOrDefault(p => p.Id == id);
		if (player == null)
			return NotFound();

		bool isPublicDonator = !player.HideDonations && _dbContext.Donations.Any(d => d.PlayerId == id && !d.IsRefunded && d.ConvertedEuroCentsReceived > 0);

		return player.ToGetPlayer(isPublicDonator);
	}

	[HttpGet("{id}/flag")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public ActionResult<string> GetPlayerFlagById([Required] int id)
	{
		var player = _dbContext.Players.AsNoTracking().Select(p => new { p.Id, p.CountryCode }).FirstOrDefault(p => p.Id == id);
		return player?.CountryCode ?? string.Empty;
	}

	[HttpGet("{id}/history")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public GetPlayerHistory GetPlayerHistoryById([Required, Range(1, int.MaxValue)] int id)
	{
		var player = _dbContext.Players
			.AsNoTracking()
			.Select(p => new { p.Id, p.HidePastUsernames })
			.FirstOrDefault(p => p.Id == id);

		int? bestRank = null;

		bool hideUsernames = player?.HidePastUsernames ?? false;
		Dictionary<string, int> usernamesHistory = new();

		int? scorePreviousForScoreHistory = null;
		List<GetPlayerHistoryScoreEntry> scoreHistory = new();

		int? rankPreviousForRankHistory = null;
		List<GetPlayerHistoryRankEntry> rankHistory = new();

		ulong? totalDeathsForActivityHistory = null;
		ulong? totalTimeForActivityHistory = null;
		DateTime? datePreviousForActivityHistory = null;
		List<GetPlayerHistoryActivityEntry> activityHistory = new();

		foreach (string leaderboardHistoryPath in _fileSystemService.TryGetFiles(DataSubDirectory.LeaderboardHistory))
		{
			LeaderboardHistory leaderboard = _leaderboardHistoryCache.GetLeaderboardHistoryByFilePath(leaderboardHistoryPath);
			EntryHistory? entry = leaderboard.Entries.Find(e => e.Id == id);
			if (entry == null)
				continue;

			if (!bestRank.HasValue || entry.Rank < bestRank)
				bestRank = entry.Rank;

			if (!hideUsernames && !string.IsNullOrWhiteSpace(entry.Username))
			{
				if (usernamesHistory.ContainsKey(entry.Username))
					usernamesHistory[entry.Username]++;
				else
					usernamesHistory.Add(entry.Username, 1);
			}

			// + 1 and - 1 are used to fix off-by-one errors in the history based on screenshots and videos. This is due to a rounding error in Devil Daggers itself.
			if (!scorePreviousForScoreHistory.HasValue || scorePreviousForScoreHistory < entry.Time - 1 || scorePreviousForScoreHistory > entry.Time + 1)
			{
				scoreHistory.Add(new()
				{
					DaggersFired = entry.DaggersFired,
					DaggersHit = entry.DaggersHit,
					DateTime = leaderboard.DateTime,
					DeathType = entry.DeathType,
					Gems = entry.Gems,
					Kills = entry.Kills,
					Rank = entry.Rank,
					Time = entry.Time.ToSecondsTime(),
					Username = entry.Username,
				});

				scorePreviousForScoreHistory = entry.Time;
			}

			if (!rankPreviousForRankHistory.HasValue || rankPreviousForRankHistory != entry.Rank)
			{
				rankHistory.Add(new()
				{
					DateTime = leaderboard.DateTime,
					Rank = entry.Rank,
				});

				rankPreviousForRankHistory = entry.Rank;
			}

			if (entry.DeathsTotal > 0)
			{
				TimeSpan? span = datePreviousForActivityHistory == null ? null : leaderboard.DateTime - datePreviousForActivityHistory.Value;

				activityHistory.Add(new()
				{
					DeathsIncrement = totalDeathsForActivityHistory.HasValue && span.HasValue ? (entry.DeathsTotal - totalDeathsForActivityHistory.Value) / span.Value.TotalDays : 0,
					DateTime = leaderboard.DateTime,
				});

				totalDeathsForActivityHistory = entry.DeathsTotal;
				totalTimeForActivityHistory = entry.TimeTotal;
				datePreviousForActivityHistory = leaderboard.DateTime;
			}
		}

		return new GetPlayerHistory
		{
			ActivityHistory = activityHistory,
			BestRank = bestRank,
			HidePastUsernames = hideUsernames,
			RankHistory = rankHistory,
			ScoreHistory = scoreHistory,
			Usernames = usernamesHistory.OrderByDescending(kvp => kvp.Value).Select(kvp => kvp.Key).ToList(),
		};
	}
}
