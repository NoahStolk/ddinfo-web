﻿using DevilDaggersCore.Leaderboards;
using DevilDaggersWebsite.Code.Utils.Web;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lb = DevilDaggersCore.Leaderboards.Leaderboard;

namespace DevilDaggersWebsite.Pages.Leaderboard
{
	public class UserSettingsModel : PageModel
	{
		public Lb Leaderboard { get; set; } = new Lb();

		public async Task OnGetAsync()
		{
			IEnumerable<Task> tasks = Enumerable.Range(0, 2).Select(async i =>
			{
				Lb nextLeaderboard = await Hasmodai.GetScores(i * 100 + 1);
				foreach (Entry entry in nextLeaderboard.Entries)
					Leaderboard.Entries.Add(entry);
			});
			await Task.WhenAll(tasks);
		}
	}
}