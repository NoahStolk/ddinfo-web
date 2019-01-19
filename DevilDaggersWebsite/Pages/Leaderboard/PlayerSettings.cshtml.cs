using DevilDaggersWebsite.Utils;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Leaderboard
{
	public class PlayerSettingsModel : PageModel
	{
		public Models.Leaderboard.Leaderboard Leaderboard { get; set; } = new Models.Leaderboard.Leaderboard();

		public async Task OnGetAsync()
		{
			Leaderboard = await LeaderboardUtils.LoadLeaderboard(1);
		}
	}
}