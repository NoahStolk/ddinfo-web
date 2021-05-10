﻿using DevilDaggersCore.Game;
using DevilDaggersWebsite.Caches.LeaderboardHistory;
using DevilDaggersWebsite.Dto;
using DevilDaggersWebsite.WorldRecords;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DevilDaggersWebsite.Transients
{
	public class WorldRecordsHelper
	{
		private static readonly DateTime _automationStart = new(2019, 10, 26);

		private readonly IWebHostEnvironment _env;

		public WorldRecordsHelper(IWebHostEnvironment env)
		{
			_env = env;
		}

		public List<WorldRecord> GetWorldRecords()
		{
			DateTime? previousDate = null;
			List<WorldRecord> worldRecords = new();
			int worldRecord = 0;
			foreach (string leaderboardHistoryPath in Directory.GetFiles(Path.Combine(_env.WebRootPath, "leaderboard-history"), "*.json"))
			{
				Leaderboard leaderboard = LeaderboardHistoryCache.Instance.GetLeaderboardHistoryByFilePath(leaderboardHistoryPath);
				Entry? firstPlace = leaderboard.Entries.Find(e => e.Rank == 1);
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

					worldRecords.Add(new(date, firstPlace, GameInfo.GetGameVersionFromDate(date)));
				}

				previousDate = leaderboard.DateTime;
			}

			return worldRecords;

			static DateTime GetAverage(DateTime a, DateTime b)
				=> new((a.Ticks + b.Ticks) / 2);
		}

		public (List<WorldRecordHolder> WorldRecordHolders, Dictionary<WorldRecord, WorldRecordData> WorldRecordData) GetWorldRecordData()
		{
			List<WorldRecord> worldRecords = GetWorldRecords();

			List<WorldRecordHolder> worldRecordHolders = new();
			Dictionary<WorldRecord, WorldRecordData> worldRecordData = new();

			TimeSpan heldConsecutively = default;
			for (int i = 0; i < worldRecords.Count; i++)
			{
				WorldRecord wr = worldRecords[i];

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
					WorldRecord nextWr = worldRecords[i + 1];
					duration = nextWr.DateTime - wr.DateTime;
					firstHeld = wr.DateTime;
					lastHeld = nextWr.DateTime;
				}

				if (i != 0 && wr.Entry.Id != worldRecords[i - 1].Entry.Id)
					heldConsecutively = default;

				heldConsecutively += duration;
				worldRecordData.Add(wr, new(duration, 0));

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
		}
	}
}
