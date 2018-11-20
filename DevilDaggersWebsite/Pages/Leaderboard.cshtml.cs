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

		public int Rank { get; set; }
		public string Search { get; set; }
		public bool IsUserSearch => !string.IsNullOrEmpty(Search) && Search.Length >= 3;

		public async Task OnGetAsync(int rank, string search)
		{
			Rank = Math.Max(rank, 1);
			Search = search;

			if (IsUserSearch)
			{
				await LeaderboardUtils.LoadLeaderboardSearch(Leaderboard, Search);
			}
			else
			{
				await LeaderboardUtils.LoadLeaderboard(Leaderboard, Rank);

				if (Rank > Leaderboard.Players - 99)
				{
					Rank = Leaderboard.Players - 99;
					Leaderboard.Entries.Clear();
					await LeaderboardUtils.LoadLeaderboard(Leaderboard, Rank);
				}
			}
		}
	}
}