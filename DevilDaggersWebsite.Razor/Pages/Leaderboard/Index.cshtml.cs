using DevilDaggersWebsite.Clients;
using DevilDaggersWebsite.Razor.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;
using Lb = DevilDaggersWebsite.Clients.Leaderboard;

namespace DevilDaggersWebsite.Razor.Pages.Leaderboard
{
	public class IndexModel : PageModel, IDefaultLeaderboardPageModel
	{
		[BindProperty]
		public Lb? Leaderboard { get; set; }

		public int Rank { get; set; }

		public async Task OnGetAsync(int rank)
		{
			Rank = Math.Max(rank, 1);

			Leaderboard = await LeaderboardClient.Instance.GetScores(Rank);
			if (Leaderboard != null && Rank > Leaderboard.Players - 99)
			{
				Rank = Leaderboard.Players - 99;
				Leaderboard.Entries.Clear();
				Leaderboard = await LeaderboardClient.Instance.GetScores(Rank);
			}
		}
	}
}
