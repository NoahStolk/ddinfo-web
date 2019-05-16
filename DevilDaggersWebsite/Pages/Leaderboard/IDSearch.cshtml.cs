using DevilDaggersCore.Leaderboard;
using DevilDaggersWebsite.Utils.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Leaderboard
{
	public class IDSearchModel : PageModel
	{
		[BindProperty]
		public DevilDaggersCore.Leaderboard.Leaderboard Leaderboard { get; set; } = new DevilDaggersCore.Leaderboard.Leaderboard();

		public int ID { get; set; }

		public async Task OnGetAsync(int id)
		{
			Entry entry = await Hasmodai.GetUserByID(id);
			Leaderboard = new DevilDaggersCore.Leaderboard.Leaderboard()
			{
				Entries = new List<Entry> { entry }
			};
		}
	}
}