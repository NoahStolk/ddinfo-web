using DevilDaggersInfo.Api.Main.WorldRecords;
using DevilDaggersInfo.Common.Extensions;
using DevilDaggersInfo.Core.Wiki;
using DevilDaggersInfo.Types.Core.Wiki;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Models.LeaderboardHistory;
using DevilDaggersInfo.Web.Server.Domain.Services.Caching;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using Microsoft.EntityFrameworkCore;

namespace DevilDaggersInfo.Web.Server.Domain.Main.Repositories;

public class WorldRecordRepository
{
	private static readonly DateTime _automationStart = new(2019, 10, 26);

	private readonly ApplicationDbContext _dbContext;
	private readonly IFileSystemService _fileSystemService;
	private readonly LeaderboardHistoryCache _leaderboardHistoryCache;

	public WorldRecordRepository(ApplicationDbContext dbContext, IFileSystemService fileSystemService, LeaderboardHistoryCache leaderboardHistoryCache)
	{
		_dbContext = dbContext;
		_fileSystemService = fileSystemService;
		_leaderboardHistoryCache = leaderboardHistoryCache;
	}

	public GetWorldRecordDataContainer GetWorldRecordData()
	{
		List<BaseWorldRecord> baseWorldRecords = GetBaseWorldRecords();
		List<BaseWorldRecordHolder> worldRecordHolders = new();

		List<GetWorldRecord> worldRecords = new();

		TimeSpan heldConsecutively = default;
		for (int i = 0; i < baseWorldRecords.Count; i++)
		{
			BaseWorldRecord wr = baseWorldRecords[i];
			BaseWorldRecord? previousWrSameLeaderboard = baseWorldRecords.OrderByDescending(w => w.DateTime).FirstOrDefault(w => w.DateTime < wr.DateTime && GetMajorGameVersion(w.GameVersion) == GetMajorGameVersion(wr.GameVersion));

			// Only display an improvement when this world record is not the first on this leaderboard, and also only when the previous world record did not have a higher time (the world record was reset once during V1 because of an exploit (b0necarver 485.3422)).
			double? improvement = previousWrSameLeaderboard == null || previousWrSameLeaderboard.Entry.Time > wr.Entry.Time ? null : wr.Entry.Time - previousWrSameLeaderboard.Entry.Time;

			TimeSpan duration;
			DateTime firstHeld;
			DateTime lastHeld;
			if (i == baseWorldRecords.Count - 1)
			{
				duration = DateTime.UtcNow - wr.DateTime;
				firstHeld = wr.DateTime;
				lastHeld = DateTime.UtcNow;
			}
			else
			{
				BaseWorldRecord nextWr = baseWorldRecords[i + 1];
				duration = nextWr.DateTime - wr.DateTime;
				firstHeld = wr.DateTime;
				lastHeld = nextWr.DateTime;
			}

			if (i != 0 && wr.Entry.Id != baseWorldRecords[i - 1].Entry.Id)
				heldConsecutively = default;

			heldConsecutively += duration;

			worldRecords.Add(new()
			{
				DateTime = wr.DateTime,
				Entry = wr.Entry,
				GameVersion = wr.GameVersion,
				WorldRecordDuration = duration,
				WorldRecordImprovement = improvement,
			});

			BaseWorldRecordHolder? holder = worldRecordHolders.Find(wrh => wrh.Id == wr.Entry.Id);
			if (holder == null)
			{
				worldRecordHolders.Add(new(wr.Entry.Id, wr.Entry.Username, duration, heldConsecutively, 1, firstHeld, lastHeld));
			}
			else
			{
				holder.MostRecentUsername = wr.Entry.Username;
				if (!holder.Usernames.Contains(wr.Entry.Username))
					holder.Usernames.Add(wr.Entry.Username);

				if (heldConsecutively > holder.LongestTimeHeldConsecutively)
					holder.LongestTimeHeldConsecutively = heldConsecutively;

				holder.TotalTimeHeld += duration;
				holder.WorldRecordCount++;
				if (firstHeld < holder.FirstHeld)
					holder.FirstHeld = firstHeld;
				holder.LastHeld = lastHeld;
			}
		}

		return new()
		{
			WorldRecordHolders = worldRecordHolders
				.OrderByDescending(wrh => wrh.TotalTimeHeld)
				.Select(bwrh => new GetWorldRecordHolder
				{
					FirstHeld = bwrh.FirstHeld,
					Id = bwrh.Id,
					LastHeld = bwrh.LastHeld,
					LongestTimeHeldConsecutively = bwrh.LongestTimeHeldConsecutively,
					MostRecentUsername = bwrh.MostRecentUsername,
					TotalTimeHeld = bwrh.TotalTimeHeld,
					Usernames = bwrh.Usernames,
					WorldRecordCount = bwrh.WorldRecordCount,
				})
				.ToList(),
			WorldRecords = worldRecords,
		};

		// Used for determining when the leaderboard was reset.
		static int GetMajorGameVersion(GameVersion? gameVersion) => gameVersion switch
		{
			GameVersion.V1_0 => 1,
			GameVersion.V2_0 => 2,
			GameVersion.V3_0 or GameVersion.V3_1 or GameVersion.V3_2 => 3,
			_ => 0,
		};
	}

	private List<BaseWorldRecord> GetBaseWorldRecords()
	{
		DateTime? previousDate = null;
		List<BaseWorldRecord> worldRecords = new();
		int worldRecord = 0;

		List<LeaderboardHistory> history = _fileSystemService.TryGetFiles(DataSubDirectory.LeaderboardHistory).Where(p => p.EndsWith(".bin")).Select(f => _leaderboardHistoryCache.GetLeaderboardHistoryByFilePath(f)).OrderBy(lbh => lbh.DateTime).ToList();
		for (int i = 0; i < history.Count; i++)
		{
			LeaderboardHistory leaderboard = history[i];
			EntryHistory? firstPlace = leaderboard.Entries.Find(e => e.Rank == 1);
			if (firstPlace == null)
				continue;

			if (firstPlace.Time != worldRecord)
			{
				worldRecord = firstPlace.Time;

				DateTime date;

				// If history dates are only one day apart (which is assumed to be every day after _automationStart), use the average of the previous and the current date.
				// This is because leaderboard history is recorded exactly at 00:00 UTC, and the date will therefore be one day ahead in all cases.
				// For older history, use the literal leaderboard DateTime.
				if (previousDate.HasValue && leaderboard.DateTime >= _automationStart)
					date = GetAverage(previousDate.Value, leaderboard.DateTime);
				else
					date = leaderboard.DateTime;

				// If the WR was submitted by an alt, we need to manually fix the ID by looking up the main ID in the database.
				int? mainPlayerId = _dbContext.Players.AsNoTracking().Select(p => new { p.Id, p.BanResponsibleId }).FirstOrDefault(p => p.Id == firstPlace.Id)?.BanResponsibleId;

				GetWorldRecordEntry getWorldRecordEntry = new()
				{
					DateTime = leaderboard.DateTime,
					Id = mainPlayerId ?? firstPlace.Id,
					Username = firstPlace.Username,
					Time = firstPlace.Time.ToSecondsTime(),
					Kills = firstPlace.Kills,
					Gems = firstPlace.Gems,
					DeathType = firstPlace.DeathType,
					DaggersHit = firstPlace.DaggersHit,
					DaggersFired = firstPlace.DaggersFired,
				};
				worldRecords.Add(new(date, getWorldRecordEntry, GameVersions.GetGameVersionFromDate(date)));
			}

			previousDate = leaderboard.DateTime;
		}

		return worldRecords;

		static DateTime GetAverage(DateTime a, DateTime b)
			=> new((a.Ticks + b.Ticks) / 2);
	}

	private sealed record BaseWorldRecord(DateTime DateTime, GetWorldRecordEntry Entry, GameVersion? GameVersion);

	private sealed class BaseWorldRecordHolder
	{
		public BaseWorldRecordHolder(int id, string username, TimeSpan totalTimeHeld, TimeSpan longestTimeHeldConsecutively, int worldRecordCount, DateTime firstHeld, DateTime lastHeld)
		{
			Id = id;
			Usernames = new() { username };
			TotalTimeHeld = totalTimeHeld;
			LongestTimeHeldConsecutively = longestTimeHeldConsecutively;
			WorldRecordCount = worldRecordCount;
			FirstHeld = firstHeld;
			LastHeld = lastHeld;

			MostRecentUsername = username;
		}

		public int Id { get; }
		public List<string> Usernames { get; }
		public TimeSpan TotalTimeHeld { get; set; }
		public TimeSpan LongestTimeHeldConsecutively { get; set; }
		public int WorldRecordCount { get; set; }
		public DateTime FirstHeld { get; set; }
		public DateTime LastHeld { get; set; }

		public string MostRecentUsername { get; set; }
	}
}
