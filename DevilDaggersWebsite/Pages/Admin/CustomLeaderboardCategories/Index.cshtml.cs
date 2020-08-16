using DevilDaggersWebsite.Code.Database;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Admin.CustomLeaderboardCategories
{
	public class IndexModel : PageModel
	{
		private readonly ApplicationDbContext context;

		public IndexModel(ApplicationDbContext context)
		{
			this.context = context;
		}

		public IList<CustomLeaderboardCategory> CustomLeaderboardCategory { get; set; }

		public async Task OnGetAsync()
		{
			CustomLeaderboardCategory = await context.CustomLeaderboardCategories.ToListAsync();
		}
	}
}