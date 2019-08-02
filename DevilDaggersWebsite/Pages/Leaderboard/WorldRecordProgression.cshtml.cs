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
			List<WorldRecord> worldRecords = new GetWorldRecordsModel(_commonObjects).GetWorldRecords().Where(w => w.DateTime >= GameInfo.GameVersions["V1"].ReleaseDate).ToList();

			int i = 0;
			foreach (WorldRecord wr in worldRecords)
			{
				TimeSpan diff;
				DateTime lastHeld;
				if (i == worldRecords.Count - 1)
				{
					diff = DateTime.Now - wr.DateTime;
					lastHeld = DateTime.Now;
				}
				else
				{
					diff = worldRecords[i + 1].DateTime - wr.DateTime;
					lastHeld = worldRecords[i + 1].DateTime;
				}
				i++;

				bool added = false;
				foreach (WorldRecordHolder w in WorldRecordHolders)
				{
					if (w.ID == wr.Entry.ID)
					{
						w.Username = wr.Entry.Username;
						w.TimeHeld += diff;
						w.LastHeld = lastHeld;
						added = true;
						break;
					}
				}

				if (!added)
					WorldRecordHolders.Add(new WorldRecordHolder(wr.Entry.ID, wr.Entry.Username, diff, lastHeld));
			}

			WorldRecordHolders = WorldRecordHolders.OrderByDescending(wrh => wrh.TimeHeld).ToList();
		}
	}
}