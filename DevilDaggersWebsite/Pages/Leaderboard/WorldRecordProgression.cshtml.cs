﻿using CoreBase.Services;
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

		public List<WorldRecordHolder> WorldRecordHolders { get; private set; } = new List<WorldRecordHolder>();

		public Dictionary<WorldRecord, TimeSpan> WorldRecords { get; private set; } = new Dictionary<WorldRecord, TimeSpan>();

		public WorldRecordProgressionModel(ICommonObjects commonObjects)
		{
			_commonObjects = commonObjects;
		}

		public void OnGet()
		{
			List<WorldRecord> worldRecords = new GetWorldRecordsModel(_commonObjects).GetWorldRecords().Where(w => w.DateTime >= GameInfo.GameVersions["V1"].ReleaseDate).ToList();

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

				WorldRecords[wr] = difference;

				bool added = false;
				foreach (WorldRecordHolder wrh in WorldRecordHolders)
				{
					if (wrh.ID == wr.Entry.ID)
					{
						wrh.Username = wr.Entry.Username;
						wrh.TimeHeld += difference;
						wrh.LastHeld = lastHeld;
						added = true;
						break;
					}
				}

				if (!added)
					WorldRecordHolders.Add(new WorldRecordHolder(wr.Entry.ID, wr.Entry.Username, difference, lastHeld));
			}

			WorldRecordHolders = WorldRecordHolders.OrderByDescending(wrh => wrh.TimeHeld).ToList();
		}
	}
}