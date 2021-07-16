using DevilDaggersCore.Game;
using DevilDaggersWebsite.BlazorWasm.Server.Caches.LeaderboardHistory;
using DevilDaggersWebsite.BlazorWasm.Server.Clients.Leaderboard;
using DevilDaggersWebsite.BlazorWasm.Server.Controllers.Attributes;
using DevilDaggersWebsite.BlazorWasm.Server.Converters;
using DevilDaggersWebsite.BlazorWasm.Server.Utils;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto.LeaderboardHistory;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto.Leaderboards;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DevilDaggersWebsite.BlazorWasm.Server.Controllers
{
	[Route("api/leaderboard-history")]
	[ApiController]
	public class LeaderboardHistoryController : ControllerBase
	{
		private static readonly DateTime _automationStart = new(2019, 10, 26);

		private readonly LeaderboardHistoryCache _leaderboardHistoryCache;

		public LeaderboardHistoryController(LeaderboardHistoryCache leaderboardHistoryCache)
		{
			_leaderboardHistoryCache = leaderboardHistoryCache;
		}

		[HttpGet("user-progression")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[EndpointConsumer(EndpointConsumers.Website)]
		public SortedDictionary<DateTime, GetEntryPublic> GetUserProgressionById([Required] int userId)
		{
			SortedDictionary<DateTime, GetEntryPublic> data = new();
			if (userId < 1)
				return data;

			foreach (string leaderboardHistoryPath in DataUtils.GetLeaderboardHistoryPaths())
			{
				LeaderboardResponse leaderboard = _leaderboardHistoryCache.GetLeaderboardHistoryByFilePath(leaderboardHistoryPath);
				EntryResponse? entry = leaderboard.Entries.Find(e => e.Id == userId);

				// + 1 and - 1 are used to fix off-by-one errors in the history based on screenshots and videos. This is due to a rounding error in Devil Daggers itself.
				if (entry != null && !data.Values.Any(e =>
					e.Time == entry.Time ||
					e.Time == entry.Time + 1 ||
					e.Time == entry.Time - 1))
				{
					data[leaderboard.DateTime] = entry.ToGetEntryPublic();
				}
			}

			return data;
		}

		[HttpGet("user-activity")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[EndpointConsumer(EndpointConsumers.Website)]
		public Dictionary<DateTime, ulong> GetUserActivity([Required] int userId)
		{
			Dictionary<DateTime, ulong> data = new();
			foreach (string leaderboardHistoryPath in DataUtils.GetLeaderboardHistoryPaths())
			{
				LeaderboardResponse leaderboard = _leaderboardHistoryCache.GetLeaderboardHistoryByFilePath(leaderboardHistoryPath);
				EntryResponse? entry = leaderboard.Entries.Find(e => e.Id == userId);
				if (entry?.DeathsTotal > 0)
					data.Add(leaderboard.DateTime, entry.DeathsTotal);
			}

			return data;
		}

		[HttpGet("world-records")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[EndpointConsumer(EndpointConsumers.Website)]
		public List<GetWorldRecordPublic> GetWorldRecords()
			=> GetWorldRecordsPrivate();

		[HttpGet("world-record-data")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[EndpointConsumer(EndpointConsumers.Website)]
		public (List<GetWorldRecordHolderPublic> WorldRecordHolders, Dictionary<GetWorldRecordPublic, GetWorldRecordDataPublic> WorldRecordData) GetWorldRecordData()
		{
			List<GetWorldRecordPublic> worldRecords = GetWorldRecordsPrivate();

			List<GetWorldRecordHolderPublic> worldRecordHolders = new();
			Dictionary<GetWorldRecordPublic, GetWorldRecordDataPublic> worldRecordData = new();

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

				GetWorldRecordHolderPublic? holder = worldRecordHolders.Find(wrh => wrh.Id == wr.Entry.Id);
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

		private List<GetWorldRecordPublic> GetWorldRecordsPrivate()
		{
			DateTime? previousDate = null;
			List<GetWorldRecordPublic> worldRecords = new();
			int worldRecord = 0;
			foreach (string leaderboardHistoryPath in DataUtils.GetLeaderboardHistoryPaths())
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
	}
}
