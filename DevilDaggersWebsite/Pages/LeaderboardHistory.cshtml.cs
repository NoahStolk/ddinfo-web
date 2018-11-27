using CoreBase.Services;
using DevilDaggersWebsite.Models.Leaderboard;
using DevilDaggersWebsite.Utils;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using NetBase.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DevilDaggersWebsite.Pages
{
	public class LeaderboardHistoryModel : PageModel
	{
		private ICommonObjects _commonObjects;

		public Leaderboard Leaderboard { get; set; } = new Leaderboard();

		public List<SelectListItem> JsonFiles { get; set; } = new List<SelectListItem>();
		public string From { get; set; }

		public LeaderboardHistoryModel(ICommonObjects commonObjects)
		{
			_commonObjects = commonObjects;

			foreach (string leaderboardHistoryPath in Directory.GetFiles(Path.Combine(_commonObjects.Env.WebRootPath, "leaderboard-history")))
			{
				Leaderboard leaderboard = JsonConvert.DeserializeObject<Leaderboard>(FileUtils.GetContents(leaderboardHistoryPath));
				JsonFiles.Add(new SelectListItem($"{LeaderboardHistoryUtils.HistoryJsonFileNameToDateString(Path.GetFileNameWithoutExtension(leaderboardHistoryPath))} UTC ({leaderboard.GetCompletionRate().ToString("##0")}% complete)", Path.GetFileName(leaderboardHistoryPath)));
			}

			JsonFiles.Reverse();
		}

		public void OnGetAsync(string from)
		{
			From = from;
			if (string.IsNullOrEmpty(From))
				From = JsonFiles[0].Value;

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
			Leaderboard = JsonConvert.DeserializeObject<Leaderboard>(jsonString);
		}
	}
}