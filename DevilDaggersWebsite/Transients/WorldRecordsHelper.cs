using DevilDaggersCore.Game;
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

		public (List<WorldRecordHolder> WorldRecordHolders, Dictionary<WorldRecord, TimeSpan> WorldRecordsByTimeLasted) GetWorldRecordData()
		{
			List<WorldRecord> worldRecords = GetWorldRecords();
			Dictionary<WorldRecord, TimeSpan> worldRecordsByTimeLasted = new();
			List<WorldRecordHolder> worldRecordHolders = new();

			TimeSpan heldConsecutively = default;
			for (int i = 0; i < worldRecords.Count; i++)
			{
				WorldRecord wr = worldRecords[i];

				TimeSpan difference;
				DateTime firstHeld;
				DateTime lastHeld;
				if (i == worldRecords.Count - 1)
				{
					difference = DateTime.UtcNow - wr.DateTime;
					firstHeld = wr.DateTime;
					lastHeld = DateTime.UtcNow;
				}
				else
				{
					WorldRecord nextWr = worldRecords[i + 1];
					difference = nextWr.DateTime - wr.DateTime;
					firstHeld = wr.DateTime;
					lastHeld = nextWr.DateTime;
				}

				if (i != 0 && wr.Entry.Id != worldRecords[i - 1].Entry.Id)
					heldConsecutively = default;

				heldConsecutively += difference;

				worldRecordsByTimeLasted[wr] = difference;

				bool added = false;
				foreach (WorldRecordHolder wrh in worldRecordHolders)
				{
					if (wrh.Id == wr.Entry.Id)
					{
						wrh.MostRecentUsername = wr.Entry.Username;
						if (!wrh.Usernames.Contains(wr.Entry.Username))
							wrh.Usernames.Add(wr.Entry.Username);

						if (heldConsecutively > wrh.LongestTimeHeldConsecutively)
							wrh.LongestTimeHeldConsecutively = heldConsecutively;

						wrh.TotalTimeHeld += difference;
						wrh.WorldRecordCount++;
						if (firstHeld < wrh.FirstHeld)
							wrh.FirstHeld = firstHeld;
						wrh.LastHeld = lastHeld;
						added = true;
						break;
					}
				}

				if (!added)
					worldRecordHolders.Add(new(wr.Entry.Id, wr.Entry.Username, difference, heldConsecutively, 1, firstHeld, lastHeld));
			}

			return (worldRecordHolders.OrderByDescending(wrh => wrh.TotalTimeHeld).ToList(), worldRecordsByTimeLasted);
		}
	}
}
