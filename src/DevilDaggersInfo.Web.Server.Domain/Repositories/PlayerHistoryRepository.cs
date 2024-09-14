using DevilDaggersInfo.Core.Common;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Models.LeaderboardHistory;
using DevilDaggersInfo.Web.Server.Domain.Models.Players;
using DevilDaggersInfo.Web.Server.Domain.Services.Caching;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using Microsoft.EntityFrameworkCore;

namespace DevilDaggersInfo.Web.Server.Domain.Repositories;

public class PlayerHistoryRepository
{
	private readonly ApplicationDbContext _dbContext;
	private readonly IFileSystemService _fileSystemService;
	private readonly ILeaderboardHistoryCache _leaderboardHistoryCache;

	public PlayerHistoryRepository(ApplicationDbContext dbContext, IFileSystemService fileSystemService, ILeaderboardHistoryCache leaderboardHistoryCache)
	{
		_dbContext = dbContext;
		_fileSystemService = fileSystemService;
		_leaderboardHistoryCache = leaderboardHistoryCache;
	}

	public PlayerHistory GetPlayerHistoryById(int id)
	{
		// TODO: Add caching.
		// TODO: Alts may be valid. We would need to check if the main account is below the current player and the alt is above it, then it should not be included in illegitimateScoresAbove.
		// This is kind of annoying to do, so we'll just ignore it for now.
		List<int> bannedPlayerIds = _dbContext.Players.Select(p => new { p.Id, p.BanType }).Where(p => p.BanType != BanType.NotBanned).Select(p => p.Id).ToList();

		var player = _dbContext.Players
			.AsNoTracking()
			.Select(p => new { p.Id, p.HidePastUsernames })
			.FirstOrDefault(p => p.Id == id);

		int? bestRank = null;

		bool hideUsernames = player?.HidePastUsernames ?? false;
		Dictionary<string, int> usernamesHistory = new();

		int? scorePreviousForScoreHistory = null;
		List<PlayerHistoryScoreEntry> scoreHistory = [];

		int? rankPreviousForRankHistory = null;
		List<PlayerHistoryRankEntry> rankHistory = [];

		ulong? totalDeathsForActivityHistory = null;
		ulong? totalTimeForActivityHistory = null;
		DateTime? datePreviousForActivityHistory = null;
		List<PlayerHistoryActivityEntry> activityHistory = [];

		foreach (string leaderboardHistoryPath in _fileSystemService.TryGetFiles(DataSubDirectory.LeaderboardHistory).Where(p => p.EndsWith(".bin")))
		{
			LeaderboardHistory leaderboard = _leaderboardHistoryCache.GetLeaderboardHistoryByFilePath(leaderboardHistoryPath);
			EntryHistory? entry = leaderboard.Entries.Find(e => e.Id == id);
			if (entry == null)
				continue;

			int illegitimateScoresAbove = leaderboard.Entries.Count(e => e.Rank < entry.Rank && bannedPlayerIds.Contains(e.Id));
			int correctedRank = entry.Rank - illegitimateScoresAbove;
			if (!bestRank.HasValue || correctedRank < bestRank)
				bestRank = correctedRank;

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
				scoreHistory.Add(new PlayerHistoryScoreEntry
				{
					DaggersFired = entry.DaggersFired,
					DaggersHit = entry.DaggersHit,
					DateTime = leaderboard.DateTime,
					DeathType = entry.DeathType,
					Gems = entry.Gems,
					Kills = entry.Kills,
					Rank = correctedRank,
					Time = GameTime.FromGameUnits(entry.Time).Seconds,
					Username = entry.Username,
				});

				scorePreviousForScoreHistory = entry.Time;
			}

			if (!rankPreviousForRankHistory.HasValue || rankPreviousForRankHistory != correctedRank)
			{
				rankHistory.Add(new PlayerHistoryRankEntry
				{
					DateTime = leaderboard.DateTime,
					Rank = correctedRank,
				});

				rankPreviousForRankHistory = correctedRank;
			}

			if (entry.DeathsTotal > 0)
			{
				TimeSpan? timeSpan = datePreviousForActivityHistory.HasValue ? leaderboard.DateTime - datePreviousForActivityHistory.Value : null;

				activityHistory.Add(new PlayerHistoryActivityEntry
				{
					DeathsIncrement = totalDeathsForActivityHistory.HasValue && timeSpan.HasValue ? GetDeathsIncrement(entry.DeathsTotal, totalDeathsForActivityHistory.Value, timeSpan.Value) : 0,
					TimeIncrement = totalTimeForActivityHistory.HasValue && timeSpan.HasValue ? GetTimeIncrement(entry.TimeTotal, totalTimeForActivityHistory.Value, timeSpan.Value) : 0,
					DateTime = leaderboard.DateTime,
				});

				totalDeathsForActivityHistory = entry.DeathsTotal;
				totalTimeForActivityHistory = entry.TimeTotal;
				datePreviousForActivityHistory = leaderboard.DateTime;

				static double GetDeathsIncrement(ulong deathsTotal, ulong totalDeathsForActivityHistory, TimeSpan timeSpan)
				{
					ulong deathsTotalDifference = deathsTotal - totalDeathsForActivityHistory;
					if (deathsTotalDifference > long.MaxValue)
						deathsTotalDifference = 0; // Ignore negative differences.

					return deathsTotalDifference / timeSpan.TotalDays;
				}

				static double GetTimeIncrement(ulong timeTotal, ulong totalTimeForActivityHistory, TimeSpan timeSpan)
				{
					ulong timeTotalDifference = timeTotal - totalTimeForActivityHistory;
					if (timeTotalDifference > long.MaxValue)
						timeTotalDifference = 0; // Ignore negative differences.

					return GameTime.FromGameUnits(timeTotalDifference).Seconds / timeSpan.TotalDays;
				}
			}
		}

		return new PlayerHistory
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
