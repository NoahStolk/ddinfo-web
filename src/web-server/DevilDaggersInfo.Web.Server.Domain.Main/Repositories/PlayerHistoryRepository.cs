using DevilDaggersInfo.Api.Main.Players;
using DevilDaggersInfo.Common.Extensions;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Models.LeaderboardHistory;
using DevilDaggersInfo.Web.Server.Domain.Services.Caching;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using Microsoft.EntityFrameworkCore;

namespace DevilDaggersInfo.Web.Server.Domain.Main.Repositories;

public class PlayerHistoryRepository
{
	private readonly ApplicationDbContext _dbContext;
	private readonly IFileSystemService _fileSystemService;
	private readonly LeaderboardHistoryCache _leaderboardHistoryCache;

	public PlayerHistoryRepository(ApplicationDbContext dbContext, IFileSystemService fileSystemService, LeaderboardHistoryCache leaderboardHistoryCache)
	{
		_dbContext = dbContext;
		_fileSystemService = fileSystemService;
		_leaderboardHistoryCache = leaderboardHistoryCache;
	}

	// TODO: Also move to Domain project since this is used by DDLIVE. We'll also need a separate return type for DDLIVE.
	public GetPlayerHistory GetPlayerHistoryById(int id)
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
 #pragma warning disable CA1854
				if (usernamesHistory.ContainsKey(entry.Username))
 #pragma warning restore CA1854
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

		return new()
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
