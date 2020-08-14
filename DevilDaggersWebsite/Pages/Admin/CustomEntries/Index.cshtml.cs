using DevilDaggersWebsite.Code.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Admin.CustomEntries
{
	[Authorize]
	public class IndexModel : PageModel
	{
		private readonly ApplicationDbContext _context;

		public IndexModel(ApplicationDbContext context)
		{
			_context = context;
		}

		public IList<CustomEntry> CustomEntry { get; set; }

		public async Task OnGetAsync()
		{
			CustomEntry = await _context.CustomEntries
				.Include(c => c.CustomLeaderboard).ToListAsync();
		}
	}
}