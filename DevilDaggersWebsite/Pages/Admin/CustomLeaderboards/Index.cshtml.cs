using DevilDaggersWebsite.Code.Database;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Admin.CustomLeaderboards
{
	public class IndexModel : PageModel
	{
		private readonly ApplicationDbContext _context;

		public IndexModel(ApplicationDbContext context)
		{
			_context = context;
		}

		public IList<CustomLeaderboard> CustomLeaderboard { get; set; }

		public async Task OnGetAsync()
		{
			CustomLeaderboard = await _context.CustomLeaderboards
				.Include(c => c.Category).ToListAsync();
		}
	}
}