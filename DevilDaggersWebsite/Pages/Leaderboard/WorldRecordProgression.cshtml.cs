using DevilDaggersCore.Leaderboards.History;
using DevilDaggersWebsite.Code.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.Pages.Leaderboard
{
	public class WorldRecordProgressionModel : PageModel
	{
		private readonly IWebHostEnvironment env;

		public List<WorldRecordHolder> WorldRecordHolders { get; private set; } = new List<WorldRecordHolder>();

		public Dictionary<WorldRecord, TimeSpan> WorldRecords { get; private set; } = new Dictionary<WorldRecord, TimeSpan>();

		public WorldRecordProgressionModel(IWebHostEnvironment env)
		{
			this.env = env;
		}

		public void OnGet()
		{
			List<WorldRecord> worldRecords = LeaderboardHistoryUtils.GetWorldRecords(env, null);

			TimeSpan heldConsecutively = new TimeSpan();
			for (int i = 0; i < worldRecords.Count; i++)
			{
				WorldRecord wr = worldRecords[i];

				TimeSpan difference;
				DateTime lastHeld;
				if (i == worldRecords.Count - 1)
				{
					difference = DateTime.Now - wr.DateTime;
					lastHeld = DateTime.Now;
				}
				else
				{
					difference = worldRecords[i + 1].DateTime - wr.DateTime;
					lastHeld = worldRecords[i + 1].DateTime;
				}

				if (i != 0 && wr.Entry.Id != worldRecords[i - 1].Entry.Id)
					heldConsecutively = new TimeSpan();

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
						wrh.LastHeld = lastHeld;
						added = true;
						break;
					}
				}

				if (!added)
					WorldRecordHolders.Add(new WorldRecordHolder(wr.Entry.Id, wr.Entry.Username, difference, heldConsecutively, 1, lastHeld));
			}

			WorldRecordHolders = WorldRecordHolders.OrderByDescending(wrh => wrh.TotalTimeHeld).ToList();
		}
	}
}