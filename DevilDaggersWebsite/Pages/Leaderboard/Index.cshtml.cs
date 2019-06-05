using DevilDaggersWebsite.Code.Utils.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Leaderboard
{
	public class IndexModel : PageModel
	{
		[BindProperty]
		public DevilDaggersCore.Leaderboard.Leaderboard Leaderboard { get; set; } = new DevilDaggersCore.Leaderboard.Leaderboard();

		public int Rank { get; set; }

		private string search = string.Empty;
		public string Search
		{
			get => search;
			set
			{
				if (string.IsNullOrEmpty(value))
					return;
				search = value.Length >= 16 ? value.Substring(0, 16) : value;
			}
		}
		
		public bool IsUserSearch => !string.IsNullOrEmpty(Search) && Search.Length >= 3;

		public async Task OnGetAsync(int rank, string search)
		{
			Rank = Math.Max(rank, 1);
			Search = search;

			if (IsUserSearch)
			{
				Leaderboard = await Hasmodai.GetUserSearch(Search);
			}
			else
			{
				Leaderboard = await Hasmodai.GetScores(Rank);

				if (Rank > Leaderboard.Players - 99)
				{
					Rank = Leaderboard.Players - 99;
					Leaderboard.Entries.Clear();
					Leaderboard = await Hasmodai.GetScores(Rank);
				}
			}
		}
	}
}