using CoreBase.Services;
using DevilDaggersCore.Game;
using DevilDaggersCore.Leaderboards;
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
				if (kvp.Key < GameInfo.GameVersions["V1"].ReleaseDate)
					continue;

				worldRecordsFiltered.Add(Tuple.Create(i, kvp.Key, kvp.Value));
				i++;
			}

			i = 0;
			foreach (Tuple<int, DateTime, Entry> tuple in worldRecordsFiltered)
			{
				TimeSpan diff;
				DateTime lastHeld;
				if (i == worldRecordsFiltered.Count - 1)
				{
					diff = DateTime.Now - tuple.Item2;
					lastHeld = DateTime.Now;
				}
				else
				{
					diff = worldRecordsFiltered.Where(t => t.Item1 == i + 1).FirstOrDefault().Item2 - tuple.Item2;
					lastHeld = worldRecordsFiltered.Where(t => t.Item1 == i + 1).FirstOrDefault().Item2;
				}
				i++;

				bool added = false;
				foreach (WorldRecordHolder w in WorldRecordHolders)
				{
					if (w.ID == tuple.Item3.ID)
					{
						w.Username = tuple.Item3.Username;
						w.TimeHeld += diff;
						w.LastHeld = lastHeld;
						added = true;
						break;
					}
				}

				if (!added)
				{
					WorldRecordHolders.Add(new WorldRecordHolder(tuple.Item3.ID, tuple.Item3.Username, diff, lastHeld));
				}
			}

			WorldRecordHolders = WorldRecordHolders.OrderByDescending(wrh => wrh.TimeHeld).ToList();
		}
	}
}