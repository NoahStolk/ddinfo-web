using DevilDaggersWebsite.Dto;
using DevilDaggersWebsite.Razor.Utils;
using DevilDaggersWebsite.Transients;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.Razor.Pages.Leaderboard
{
	public class WorldRecordProgressionModel : PageModel
	{
		private readonly LeaderboardHistoryHelper _leaderboardHistoryHelper;

		public WorldRecordProgressionModel(LeaderboardHistoryHelper leaderboardHistoryHelper)
		{
			_leaderboardHistoryHelper = leaderboardHistoryHelper;
		}

		public List<WorldRecordHolder> WorldRecordHolders { get; private set; } = new();

		public Dictionary<WorldRecord, TimeSpan> WorldRecords { get; } = new();

		public void OnGet()
		{
			List<WorldRecord> worldRecords = _leaderboardHistoryHelper.GetWorldRecords(null);

			TimeSpan heldConsecutively = default;
			for (int i = 0; i < worldRecords.Count; i++)
			{
				WorldRecord wr = worldRecords[i];

				TimeSpan difference;
				DateTime firstHeld;
				DateTime lastHeld;
				if (i == worldRecords.Count - 1)
				{
					difference = DateTime.Now - wr.DateTime;
					firstHeld = wr.DateTime;
					lastHeld = DateTime.Now;
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

				WorldRecords[wr] = difference;

				bool added = false;
				foreach (WorldRecordHolder wrh in WorldRecordHolders)
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
					WorldRecordHolders.Add(new(wr.Entry.Id, wr.Entry.Username, difference, heldConsecutively, 1, firstHeld, lastHeld));
			}

			WorldRecordHolders = WorldRecordHolders.OrderByDescending(wrh => wrh.TotalTimeHeld).ToList();
		}

		public string GetHistoryDateString(DateTime dateTime)
		{
			int daysAgo = (int)Math.Round((DateTime.Now - dateTime).TotalDays);
			return $"{dateTime:MMM dd} '{dateTime:yy} ({daysAgo} day{daysAgo.S()} ago)";
		}
	}
}