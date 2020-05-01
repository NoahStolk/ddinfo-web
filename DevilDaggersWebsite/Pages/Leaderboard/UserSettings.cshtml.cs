using DevilDaggersWebsite.Code.Utils.Web;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Lb = DevilDaggersCore.Leaderboards.Leaderboard;

namespace DevilDaggersWebsite.Pages.Leaderboard
{
	public class UserSettingsModel : PageModel
	{
		public Lb Leaderboard { get; set; } = new Lb();

		public async Task OnGetAsync()
		{
			Leaderboard = await Hasmodai.GetScores(1);
		}
	}
}