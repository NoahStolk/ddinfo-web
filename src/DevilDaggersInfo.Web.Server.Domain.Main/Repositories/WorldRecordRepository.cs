using DevilDaggersInfo.Core.Common;
using DevilDaggersInfo.Core.Wiki;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using DevilDaggersInfo.Web.Server.Domain.Main.Converters.DomainToApi;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Models.LeaderboardHistory;
using DevilDaggersInfo.Web.Server.Domain.Services.Caching;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using ApiMain = DevilDaggersInfo.Web.ApiSpec.Main.WorldRecords;

namespace DevilDaggersInfo.Web.Server.Domain.Main.Repositories;

public class WorldRecordRepository
{
	private static readonly DateTime _automationStart = new(2019, 10, 26, 0, 0, 0, DateTimeKind.Utc);

	private readonly ApplicationDbContext _dbContext;
	private readonly IFileSystemService _fileSystemService;
	private readonly ILeaderboardHistoryCache _leaderboardHistoryCache;

	public WorldRecordRepository(ApplicationDbContext dbContext, IFileSystemService fileSystemService, ILeaderboardHistoryCache leaderboardHistoryCache)
	{
		_dbContext = dbContext;
		_fileSystemService = fileSystemService;
		_leaderboardHistoryCache = leaderboardHistoryCache;
	}

	public ApiMain.GetWorldRecordDataContainer GetWorldRecordData()
	{
		List<BaseWorldRecord> baseWorldRecords = GetBaseWorldRecords();
		List<BaseWorldRecordHolder> worldRecordHolders = [];

		List<ApiMain.GetWorldRecord> worldRecords = [];

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
				GameVersion = wr.GameVersion?.ToMainApi(),
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
				.Select(wrh => new ApiMain.GetWorldRecordHolder
				{
					FirstHeld = wrh.FirstHeld,
					Id = wrh.Id,
					LastHeld = wrh.LastHeld,
					LongestTimeHeldConsecutively = wrh.LongestTimeHeldConsecutively,
					MostRecentUsername = wrh.MostRecentUsername,
					TotalTimeHeld = wrh.TotalTimeHeld,
					Usernames = wrh.Usernames,
					WorldRecordCount = wrh.WorldRecordCount,
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
		// WRs made on an alt can be legit, we'll just swap it with the main account.
		List<int> bannedPlayerIds = _dbContext.Players.Select(p => new { p.Id, p.BanType }).Where(p => p.BanType != BanType.Alt && p.BanType != BanType.NotBanned).Select(p => p.Id).ToList();

		DateTime? previousDate = null;
		List<BaseWorldRecord> worldRecords = [];
		int worldRecord = 0;

		List<LeaderboardHistory> history = _fileSystemService.TryGetFiles(DataSubDirectory.LeaderboardHistory).Where(p => p.EndsWith(".bin")).Select(f => _leaderboardHistoryCache.GetLeaderboardHistoryByFilePath(f)).OrderBy(lbh => lbh.DateTime).ToList();
		foreach (LeaderboardHistory leaderboard in history)
		{
			// Find the WR, if the actual first place is not legit, get second place, etc.
			EntryHistory? firstLegitPlace = null;

			// ReSharper disable once LoopCanBeConvertedToQuery
			for (int i = 0; i < leaderboard.Entries.Count; i++)
			{
				int rank = i + 1;
				EntryHistory? entry = leaderboard.Entries.Find(e => e.Rank == rank);
				if (entry == null)
					break; // Entry with current rank is not present on this leaderboard, meaning we cannot determine what the WR was.

				if (bannedPlayerIds.Contains(entry.Id))
					continue; // The actual WR is not legit, find the next entry.

				firstLegitPlace = entry;
				break;
			}

			if (firstLegitPlace == null)
				continue;

			if (firstLegitPlace.Time != worldRecord)
			{
				worldRecord = firstLegitPlace.Time;

				DateTime date;

				// If history dates are only one day apart (which is assumed to be every day after _automationStart), use the average of the previous and the current date.
				// This is because leaderboard history is recorded exactly at 00:00 UTC, and the date will therefore be one day ahead in all cases.
				// For older history, use the literal leaderboard DateTime.
				if (previousDate.HasValue && leaderboard.DateTime >= _automationStart)
					date = GetAverage(previousDate.Value, leaderboard.DateTime);
				else
					date = leaderboard.DateTime;

				// If the WR was submitted by an alt, we need to manually fix the ID by looking up the main ID in the database.
				int? mainPlayerId = _dbContext.Players.Select(p => new { p.Id, p.BanResponsibleId }).FirstOrDefault(p => p.Id == firstLegitPlace.Id)?.BanResponsibleId;

				ApiMain.GetWorldRecordEntry getWorldRecordEntry = new()
				{
					DateTime = leaderboard.DateTime,
					Id = mainPlayerId ?? firstLegitPlace.Id,
					Username = firstLegitPlace.Username,
					Time = GameTime.FromGameUnits(firstLegitPlace.Time).Seconds,
					Kills = firstLegitPlace.Kills,
					Gems = firstLegitPlace.Gems,
					DeathType = firstLegitPlace.DeathType,
					DaggersHit = firstLegitPlace.DaggersHit,
					DaggersFired = firstLegitPlace.DaggersFired,
				};
				worldRecords.Add(new(date, getWorldRecordEntry, GameVersions.GetGameVersionFromDate(date)));
			}

			previousDate = leaderboard.DateTime;
		}

		return worldRecords;

		static DateTime GetAverage(DateTime a, DateTime b)
		{
			return new((a.Ticks + b.Ticks) / 2, DateTimeKind.Utc);
		}
	}

	private sealed record BaseWorldRecord(DateTime DateTime, ApiMain.GetWorldRecordEntry Entry, GameVersion? GameVersion);

	private sealed class BaseWorldRecordHolder
	{
		public BaseWorldRecordHolder(int id, string username, TimeSpan totalTimeHeld, TimeSpan longestTimeHeldConsecutively, int worldRecordCount, DateTime firstHeld, DateTime lastHeld)
		{
			Id = id;
			Usernames = [username];
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
