using DevilDaggersWebsite.Extensions;
using DevilDaggersWebsite.Models.Leaderboard;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages
{
	public class LeaderboardModel : PageModel
	{
		[BindProperty]
		public Leaderboard Leaderboard { get; set; } = new Leaderboard();

		public async Task OnGetAsync()
		{
			await LeaderboardParser.LoadLeaderboard(Leaderboard);
		}

		public async Task<IActionResult> OnPostAsync(string submitAction)
		{
			//ModelState.Remove("OffsetPrevious");

			switch (submitAction)
			{
				case ">":
					Leaderboard.Offset = Leaderboard.OffsetPrevious + 100;
					break;
				case "<":
					Leaderboard.Offset = Leaderboard.OffsetPrevious - 100;
					break;
			}

			Leaderboard.Offset = Math.Max(1, Leaderboard.Offset);
			await LeaderboardParser.LoadLeaderboard(Leaderboard);

			if (Leaderboard.Offset > Leaderboard.Players - 99)
			{
				Leaderboard.Offset = Leaderboard.Players - 99;
				Leaderboard.Entries.Clear();
				await LeaderboardParser.LoadLeaderboard(Leaderboard);
			}
			Leaderboard.OffsetPrevious = Leaderboard.Offset;

			return RedirectToPage();
		}
	}
}