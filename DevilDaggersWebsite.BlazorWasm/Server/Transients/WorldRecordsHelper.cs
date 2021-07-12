using DevilDaggersCore.Game;
using DevilDaggersWebsite.BlazorWasm.Server.Caches.LeaderboardHistory;
using DevilDaggersWebsite.BlazorWasm.Server.Clients.Leaderboard;
using DevilDaggersWebsite.BlazorWasm.Server.Converters;
using DevilDaggersWebsite.BlazorWasm.Server.WorldRecords;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto.LeaderboardHistory;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DevilDaggersWebsite.BlazorWasm.Server.Transients
{
	public class WorldRecordsHelper
	{
		private static readonly DateTime _automationStart = new(2019, 10, 26);

		private readonly IWebHostEnvironment _environment;
		private readonly LeaderboardHistoryCache _leaderboardHistoryCache;

		public WorldRecordsHelper(IWebHostEnvironment environment, LeaderboardHistoryCache leaderboardHistoryCache)
		{
			_environment = environment;
			_leaderboardHistoryCache = leaderboardHistoryCache;
		}

		public List<GetWorldRecordPublic> GetWorldRecords()
		{
			DateTime? previousDate = null;
			List<GetWorldRecordPublic> worldRecords = new();
			int worldRecord = 0;
			foreach (string leaderboardHistoryPath in Directory.GetFiles(Path.Combine(_environment.WebRootPath, "leaderboard-history"), "*.json"))
			{
				LeaderboardResponse leaderboard = _leaderboardHistoryCache.GetLeaderboardHistoryByFilePath(leaderboardHistoryPath);
				EntryResponse? firstPlace = leaderboard.Entries.Find(e => e.Rank == 1);
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

					worldRecords.Add(new()
					{
						DateTime = date,
						Entry = firstPlace.ToGetEntryPublic(),
						GameVersion = GameInfo.GetGameVersionFromDate(date),
					});
				}

				previousDate = leaderboard.DateTime;
			}

			return worldRecords;

			static DateTime GetAverage(DateTime a, DateTime b)
				=> new((a.Ticks + b.Ticks) / 2);
		}

		public (List<WorldRecordHolder> WorldRecordHolders, Dictionary<GetWorldRecordPublic, WorldRecordData> WorldRecordData) GetWorldRecordData()
		{
			List<GetWorldRecordPublic> worldRecords = GetWorldRecords();

			List<WorldRecordHolder> worldRecordHolders = new();
			Dictionary<GetWorldRecordPublic, WorldRecordData> worldRecordData = new();

			TimeSpan heldConsecutively = default;
			for (int i = 0; i < worldRecords.Count; i++)
			{
				GetWorldRecordPublic wr = worldRecords[i];

				GetWorldRecordPublic? previousWrSameLeaderboard = worldRecords.OrderByDescending(w => w.Entry.Time).FirstOrDefault(w => w.Entry.Time < wr.Entry.Time && GetMajorGameVersion(w.GameVersion) == GetMajorGameVersion(wr.GameVersion));

				TimeSpan duration;
				DateTime firstHeld;
				DateTime lastHeld;
				if (i == worldRecords.Count - 1)
				{
					duration = DateTime.UtcNow - wr.DateTime;
					firstHeld = wr.DateTime;
					lastHeld = DateTime.UtcNow;
				}
				else
				{
					GetWorldRecordPublic nextWr = worldRecords[i + 1];
					duration = nextWr.DateTime - wr.DateTime;
					firstHeld = wr.DateTime;
					lastHeld = nextWr.DateTime;
				}

				if (i != 0 && wr.Entry.Id != worldRecords[i - 1].Entry.Id)
					heldConsecutively = default;

				heldConsecutively += duration;
				worldRecordData.Add(wr, new(duration, previousWrSameLeaderboard == null ? null : wr.Entry.Time - previousWrSameLeaderboard.Entry.Time));

				WorldRecordHolder? holder = worldRecordHolders.Find(wrh => wrh.Id == wr.Entry.Id);
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

			return (worldRecordHolders.OrderByDescending(wrh => wrh.TotalTimeHeld).ToList(), worldRecordData);

			// Used for determining when the leaderboard was reset.
			static int GetMajorGameVersion(GameVersion? gameVersion) => gameVersion switch
			{
				GameVersion.V1 => 1,
				GameVersion.V2 => 2,
				GameVersion.V3 => 3,
				GameVersion.V31 => 3,
				_ => 0,
			};
		}
	}
}
