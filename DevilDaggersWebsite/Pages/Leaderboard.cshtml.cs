using DevilDaggersWebsite.Models.Leaderboard;
using DevilDaggersWebsite.Utils;
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
			if (Leaderboard.IsUserSearch)
				await LeaderboardUtils.LoadLeaderboardSearch(Leaderboard);
			else
				await LeaderboardUtils.LoadLeaderboard(Leaderboard);
		}

		public async Task OnPostAsync(string submitAction, int offsetPrevious)
		{
			if (Leaderboard.IsUserSearch)
			{
				await LeaderboardUtils.LoadLeaderboardSearch(Leaderboard);
			}
			else
			{
				switch (submitAction)
				{
					case ">":
						Leaderboard.Offset = offsetPrevious + 100;
						break;
					case "<":
						Leaderboard.Offset = offsetPrevious - 100;
						break;
				}

				Leaderboard.Offset = Math.Max(1, Leaderboard.Offset);
				await LeaderboardUtils.LoadLeaderboard(Leaderboard);

				if (Leaderboard.Offset > Leaderboard.Players - 99)
				{
					Leaderboard.Offset = Leaderboard.Players - 99;
					Leaderboard.Entries.Clear();
					await LeaderboardUtils.LoadLeaderboard(Leaderboard);
				}
				Leaderboard.OffsetPrevious = Leaderboard.Offset;
			}
		}
	}
}