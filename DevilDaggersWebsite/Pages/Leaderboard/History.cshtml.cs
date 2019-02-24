using CoreBase.Services;
using DevilDaggersUtilities.Website;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using NetBase.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DevilDaggersWebsite.Pages.Leaderboard
{
	public class HistoryModel : PageModel
	{
		private ICommonObjects _commonObjects;

		public DevilDaggersCore.Leaderboard.Leaderboard Leaderboard { get; set; } = new DevilDaggersCore.Leaderboard.Leaderboard();

		public List<SelectListItem> JsonFiles { get; set; } = new List<SelectListItem>();
		public string From { get; set; }
		public string FromPrevious { get; set; }
		public string FromNext { get; set; }

		public HistoryModel(ICommonObjects commonObjects)
		{
			_commonObjects = commonObjects;

			foreach (string leaderboardHistoryPath in Directory.GetFiles(Path.Combine(_commonObjects.Env.WebRootPath, "leaderboard-history")))
			{
				DevilDaggersCore.Leaderboard.Leaderboard leaderboard = JsonConvert.DeserializeObject<DevilDaggersCore.Leaderboard.Leaderboard>(FileUtils.GetContents(leaderboardHistoryPath));
				JsonFiles.Add(new SelectListItem($"{LeaderboardHistoryUtils.HistoryJsonFileNameToDateString(Path.GetFileNameWithoutExtension(leaderboardHistoryPath))} UTC ({leaderboard.GetCompletionRate().ToString("0.0%")} complete)", Path.GetFileName(leaderboardHistoryPath)));
			}

			JsonFiles.Reverse();
		}

		public void OnGet(string from)
		{
			From = from;
			if (string.IsNullOrEmpty(From))
				From = JsonFiles[0].Value;

			for (int i = 0; i < JsonFiles.Count; i++)
			{
				if (From == JsonFiles[i].Value)
				{
					if (i != 0)
						FromPrevious = JsonFiles[i - 1].Value;
					if (i != JsonFiles.Count - 1)
						FromNext = JsonFiles[i + 1].Value;
				}
			}

			string jsonString = null;
			try
			{
				jsonString = FileUtils.GetContents(Path.Combine(_commonObjects.Env.WebRootPath, "leaderboard-history", From), Encoding.UTF8);
			}
			catch (Exception)
			{
				From = JsonFiles[0].Value;
				jsonString = FileUtils.GetContents(Path.Combine(_commonObjects.Env.WebRootPath, "leaderboard-history", From), Encoding.UTF8);
			}
			Leaderboard = JsonConvert.DeserializeObject<DevilDaggersCore.Leaderboard.Leaderboard>(jsonString);
		}
	}
}