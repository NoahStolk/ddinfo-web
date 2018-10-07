using CoreBase.Services;
using DevilDaggersWebsite.Models.Leaderboard;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using NetBase.Utils;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DevilDaggersWebsite.Pages
{
	public class LeaderboardHistoryModel : PageModel
	{
		ICommonObjects CommonObjects;

		[BindProperty]
		public Leaderboard Leaderboard { get; set; } = new Leaderboard();

		public List<SelectListItem> JsonFiles { get; set; } = new List<SelectListItem>();
		public string From { get; set; }

		public LeaderboardHistoryModel(ICommonObjects commonObjects)
		{
			CommonObjects = commonObjects;

			foreach (string s in Directory.GetFiles(Path.Combine(CommonObjects.Env.WebRootPath, "leaderboard-history")))
				JsonFiles.Add(new SelectListItem($"{Path.GetFileNameWithoutExtension(s).Replace('.', ':')} UTC", Path.GetFileName(s)));
			JsonFiles.Reverse();
		}

		public void OnGetAsync(string from)
		{
			From = from;
			if (string.IsNullOrEmpty(From))
				From = JsonFiles[0].Value;

			string jsonString = FileUtils.GetContents(Path.Combine(CommonObjects.Env.WebRootPath, "leaderboard-history", From), Encoding.UTF8);
			Leaderboard = JsonConvert.DeserializeObject<Leaderboard>(jsonString);
		}
	}
}