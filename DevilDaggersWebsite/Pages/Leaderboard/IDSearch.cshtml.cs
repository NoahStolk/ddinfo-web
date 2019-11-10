using DevilDaggersCore.Leaderboards;
using DevilDaggersWebsite.Code.Utils.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Leaderboard
{
	public class IDSearchModel : PageModel
	{
		[BindProperty]
		public DevilDaggersCore.Leaderboards.Leaderboard Leaderboard { get; set; } = new DevilDaggersCore.Leaderboards.Leaderboard();

		public int ID { get; set; }

		public async Task OnGetAsync(int id)
		{
			if (id < 1)
				id = 1;

			Entry entry = await Hasmodai.GetUserByID(id);
			Leaderboard = new DevilDaggersCore.Leaderboards.Leaderboard()
			{
				Entries = new List<Entry> { entry }
			};
		}
	}
}