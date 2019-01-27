using CoreBase.Services;
using DevilDaggersCore.Game;
using DevilDaggersWebsite.Models.Leaderboard;
using DevilDaggersWebsite.Pages.API;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.Pages.Leaderboard
{
	public class WorldRecordProgressionModel : PageModel
	{
		private readonly ICommonObjects _commonObjects;

		public List<WorldRecordHolder> WorldRecordHolders { get; set; } = new List<WorldRecordHolder>();

		public WorldRecordProgressionModel(ICommonObjects commonObjects)
		{
			_commonObjects = commonObjects;
		}

		public void OnGet()
		{
			SortedDictionary<DateTime, Entry> worldRecords = new GetWorldRecordsModel(_commonObjects).GetWorldRecords();

			List<Tuple<int, DateTime, Entry>> worldRecordsFiltered = new List<Tuple<int, DateTime, Entry>>();

			int i = 0;
			foreach (KeyValuePair<DateTime, Entry> kvp in worldRecords)
			{
				if (kvp.Key < Game.GameVersions["V1"].ReleaseDate)
					continue;

				worldRecordsFiltered.Add(Tuple.Create(i, kvp.Key, kvp.Value));
				i++;
			}

			i = 0;
			foreach (Tuple<int, DateTime, Entry> tuple in worldRecordsFiltered)
			{
				TimeSpan diff;
				if (i == worldRecordsFiltered.Count - 1)
					diff = DateTime.Now - tuple.Item2;
				else
					diff = worldRecordsFiltered.Where(t => t.Item1 == i + 1).FirstOrDefault().Item2 - tuple.Item2;
				i++;

				bool done = false;
				foreach (WorldRecordHolder w in WorldRecordHolders)
				{
					if (w.ID == tuple.Item3.ID)
					{
						w.Username = tuple.Item3.Username;
						w.TimeHeld += diff;
						done = true;
						break;
					}
				}

				if (!done)
				{
					WorldRecordHolders.Add(new WorldRecordHolder(tuple.Item3.ID, tuple.Item3.Username, diff));
				}
			}

			WorldRecordHolders = WorldRecordHolders.OrderByDescending(wrh => wrh.TimeHeld).ToList();
		}
	}
}