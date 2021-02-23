using DevilDaggersCore.Utils;
using DevilDaggersWebsite.Dto;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DevilDaggersWebsite.Transients
{
	public class LeaderboardHistoryHelper
	{
		private readonly IWebHostEnvironment _env;

		public LeaderboardHistoryHelper(IWebHostEnvironment env)
		{
			_env = env;
		}

		public List<WorldRecord> GetWorldRecords(DateTime? date)
		{
			bool isDateParameterValid = date.HasValue && date <= DateTime.UtcNow;

			List<WorldRecord> worldRecords = new();

			int worldRecord = 0;
			foreach (string leaderboardHistoryPath in Directory.GetFiles(Path.Combine(_env.WebRootPath, "leaderboard-history"), "*.json"))
			{
				Leaderboard leaderboard = JsonConvert.DeserializeObject<Leaderboard>(File.ReadAllText(leaderboardHistoryPath, Encoding.UTF8));
				if (leaderboard.Entries[0].Time != worldRecord)
				{
					worldRecord = leaderboard.Entries[0].Time;
					if (isDateParameterValid)
					{
						if (HistoryUtils.HistoryJsonFileNameToDateTime(Path.GetFileNameWithoutExtension(leaderboardHistoryPath)) > date)
							break;
						worldRecords.Clear();
					}

					worldRecords.Add(new(leaderboard.DateTime, leaderboard.Entries[0]));
				}
			}

			return worldRecords;
		}

		public (List<WorldRecordHolder> WorldRecordHolders, Dictionary<WorldRecord, TimeSpan> WorldRecordsByTimeLasted) GetWorldRecordData()
		{
			List<WorldRecord> worldRecords = GetWorldRecords(null);
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
					difference = worldRecords[i + 1].DateTime - wr.DateTime;
					firstHeld = wr.DateTime;
					lastHeld = worldRecords[i + 1].DateTime;
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
