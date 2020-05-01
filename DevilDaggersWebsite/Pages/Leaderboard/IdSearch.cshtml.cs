using DevilDaggersCore.Leaderboards;
using DevilDaggersWebsite.Code.Utils.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lb = DevilDaggersCore.Leaderboards.Leaderboard;

namespace DevilDaggersWebsite.Pages.Leaderboard
{
	public class IdSearchModel : PageModel
	{
		[BindProperty]
		public Lb Leaderboard { get; set; } = new Lb();

		public int Id { get; set; }

		public async Task OnGetAsync(int id)
		{
			if (id < 1)
				id = 1;

			Entry entry = await Hasmodai.GetUserById(id);
			Leaderboard = new Lb
			{
				Entries = new List<Entry> { entry }
			};
		}
	}
}