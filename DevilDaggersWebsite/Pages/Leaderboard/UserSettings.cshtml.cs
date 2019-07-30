using DevilDaggersWebsite.Code.Utils.Web;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Leaderboard
{
	public class UserSettingsModel : PageModel
	{
		public DevilDaggersCore.Leaderboards.Leaderboard Leaderboard { get; set; } = new DevilDaggersCore.Leaderboards.Leaderboard();

		public async Task OnGetAsync()
		{
			Leaderboard = await Hasmodai.GetScores(1);
		}
	}
}