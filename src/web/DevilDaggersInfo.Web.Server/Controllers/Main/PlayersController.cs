using DevilDaggersInfo.Api.Main.Players;
using DevilDaggersInfo.Web.Server.Converters.ApiToDomain.Main;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.Main;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Models.LeaderboardHistory;
using DevilDaggersInfo.Web.Server.Domain.Models.Players;
using DevilDaggersInfo.Web.Server.Domain.Services;
using DevilDaggersInfo.Web.Server.Domain.Utils;
using Microsoft.AspNetCore.Authorization;

namespace DevilDaggersInfo.Web.Server.Controllers.Main;

[Route("api/players")]
[ApiController]
public class PlayersController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;
	private readonly IFileSystemService _fileSystemService;
	private readonly LeaderboardHistoryCache _leaderboardHistoryCache;
	private readonly ProfileService _profileService;

	public PlayersController(ApplicationDbContext dbContext, IFileSystemService fileSystemService, LeaderboardHistoryCache leaderboardHistoryCache, ProfileService profileService)
	{
		_dbContext = dbContext;
		_fileSystemService = fileSystemService;
		_leaderboardHistoryCache = leaderboardHistoryCache;
		_profileService = profileService;
	}

	[HttpGet("leaderboard")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<List<GetPlayerForLeaderboard>> GetPlayersForLeaderboard()
	{
		return _dbContext.Players
			.AsNoTracking()
			.Select(p => new GetPlayerForLeaderboard
			{
				Id = p.Id,
				BanType = p.BanType.ToMainApi(),
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
			.Where(p => p.BanType == Domain.Entities.Enums.BanType.NotBanned && !p.HideSettings)
			.ToList();

		// Note; cannot evaluate HasSettings() against database (IQueryable).
		return players.Where(p => p.HasSettings()).Select(p => p.ToGetPlayerForSettings()).ToList();
	}

	// FORBIDDEN: Used by DDLIVE.
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

	// FORBIDDEN: Used by DDLIVE.
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

		foreach (string leaderboardHistoryPath in _fileSystemService.TryGetFiles(DataSubDirectory.LeaderboardHistory).Where(p => p.EndsWith(".bin")))
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
					TimeIncrement = totalTimeForActivityHistory.HasValue && span.HasValue ? (entry.TimeTotal - totalTimeForActivityHistory.Value).ToSecondsTime() / span.Value.TotalDays : 0,
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

	[HttpGet("{id}/custom-leaderboard-statistics")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<List<GetPlayerCustomLeaderboardStatistics>>> GetCustomLeaderboardStatisticsByPlayerId([Required] int id)
	{
		var customEntries = await _dbContext.CustomEntries
			.AsNoTracking()
			.Include(ce => ce.CustomLeaderboard)
			.Where(cl => cl.PlayerId == id)
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
				ce.CustomLeaderboard.IsFeatured,
			})
			.Where(ce => ce.IsFeatured)
			.ToListAsync();

		Dictionary<CustomLeaderboardCategory, int> customLeaderboardsByCategory = await _dbContext.CustomLeaderboards
			.AsNoTracking()
			.Select(cl => new { cl.Category, cl.IsFeatured })
			.Where(cl => cl.IsFeatured)
			.GroupBy(cl => cl.Category)
			.Select(g => new { Category = g.Key, Count = g.Count() })
			.ToDictionaryAsync(a => a.Category, a => a.Count);

		List<GetPlayerCustomLeaderboardStatistics> stats = new();
		foreach (CustomLeaderboardCategory category in Enum.GetValues<CustomLeaderboardCategory>())
		{
			var customEntriesByCategory = customEntries.Where(c => c.Category == category);
			if (!customEntriesByCategory.Any() || !customLeaderboardsByCategory.ContainsKey(category))
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
				CustomLeaderboardCategory = category.ToMainApi(),
				LeviathanDaggerCount = leviathanDaggers,
				DevilDaggerCount = devilDaggers,
				GoldenDaggerCount = goldenDaggers,
				SilverDaggerCount = silverDaggers,
				BronzeDaggerCount = bronzeDaggers,
				DefaultDaggerCount = defaultDaggers,
				LeaderboardsPlayedCount = played,
				TotalCount = customLeaderboardsByCategory[category],
			});
		}

		return stats;
	}

	[Authorize]
	[HttpGet("{id}/profile")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetPlayerProfile>> GetProfileByPlayerId([Required] int id)
	{
		try
		{
			PlayerProfile playerProfile = await _profileService.GetProfileAsync(User, id);
			return playerProfile.ToMainApi();
		}
		catch (UnauthorizedAccessException)
		{
			return Unauthorized();
		}
	}

	[Authorize]
	[HttpPut("{id}/profile")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> UpdateProfileByPlayerId([Required] int id, EditPlayerProfile editPlayerProfile)
	{
		try
		{
			await _profileService.UpdateProfileAsync(User, id, editPlayerProfile.ToDomain());
			return Ok();
		}
		catch (UnauthorizedAccessException)
		{
			return Unauthorized();
		}
	}
}
