using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.LeaderboardHistory;
using DevilDaggersInfo.Web.BlazorWasm.Server.Converters.Public;
using DevilDaggersInfo.Web.BlazorWasm.Server.InternalModels.Json;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.LeaderboardHistory;
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
			.Select(p => new { p.Id, p.BanDescription, p.BanType, p.CountryCode })
			.Select(p => new GetPlayerForLeaderboard
			{
				Id = p.Id,
				BanDescription = p.BanDescription,
				IsBanned = p.BanType != BanType.NotBanned,
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

		List<string> playerTitles = _dbContext.PlayerTitles
			.AsNoTracking()
			.Include(pt => pt.Title)
			.Select(pt => new { TitleName = pt.Title.Name, pt.PlayerId })
			.Where(pt => pt.PlayerId == player.Id)
			.Select(pt => pt.TitleName)
			.ToList();

		return player.ToGetPlayer(isPublicDonator, playerTitles);
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

		bool hideUsernames = player?.HidePastUsernames ?? false;

		int? bestRank = null;
		Dictionary<string, int> usernames = new();
		List<GetEntryHistory> entryHistory = new();
		List<GetPlayerActivity> activity = new();

		ulong? deaths = null;
		DateTime? datePrevious = null;

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
				if (usernames.ContainsKey(entry.Username))
					usernames[entry.Username]++;
				else
					usernames.Add(entry.Username, 1);
			}

			// + 1 and - 1 are used to fix off-by-one errors in the history based on screenshots and videos. This is due to a rounding error in Devil Daggers itself.
			if (!entryHistory.Any(e =>
				e.Time.To10thMilliTime() == entry.Time ||
				e.Time.To10thMilliTime() == entry.Time + 1 ||
				e.Time.To10thMilliTime() == entry.Time - 1))
			{
				entryHistory.Add(entry.ToDto(leaderboard.DateTime));
			}

			if (entry.DeathsTotal > 0)
			{
				TimeSpan? span = datePrevious == null ? null : leaderboard.DateTime - datePrevious.Value;

				activity.Add(new()
				{
					DateTime = leaderboard.DateTime,
					DeathsIncrement = deaths.HasValue && span.HasValue ? (entry.DeathsTotal - deaths.Value) / span.Value.TotalDays : 0,
				});

				deaths = entry.DeathsTotal;
				datePrevious = leaderboard.DateTime;
			}
		}

		return new GetPlayerHistory
		{
			Activity = activity,
			History = entryHistory,
			BestRank = bestRank,
			Usernames = usernames.OrderByDescending(kvp => kvp.Value).Select(kvp => kvp.Key).ToList(),
		};
	}
}
