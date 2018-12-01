using CoreBase.Services;
using DevilDaggersWebsite.Models.Leaderboard;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetBase.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace DevilDaggersWebsite.Pages.Leaderboard
{
	public class WorldRecordProgressionModel : PageModel
	{
		private ICommonObjects _commonObjects;

		public Dictionary<DateTime, Entry> Data { get; set; } = new Dictionary<DateTime, Entry>();

		public WorldRecordProgressionModel(ICommonObjects commonObjects)
		{
			_commonObjects = commonObjects;
		}

		public void OnGet()
		{
			int worldRecord = 0;

			foreach (string leaderboardHistoryPath in Directory.GetFiles(Path.Combine(_commonObjects.Env.WebRootPath, "leaderboard-history")))
			{
				Models.Leaderboard.Leaderboard leaderboard = JsonConvert.DeserializeObject<Models.Leaderboard.Leaderboard>(FileUtils.GetContents(leaderboardHistoryPath));
				if (leaderboard.Entries[0].Time > worldRecord)
				{
					worldRecord = leaderboard.Entries[0].Time;
					Data.Add(leaderboard.DateTime, leaderboard.Entries[0]);
				}
			}
		}
	}
}