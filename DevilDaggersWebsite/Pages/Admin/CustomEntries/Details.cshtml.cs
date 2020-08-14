using DevilDaggersWebsite.Code.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Admin.CustomEntries
{
	[Authorize]
	public class DetailsModel : PageModel
	{
		private readonly ApplicationDbContext _context;

		public DetailsModel(ApplicationDbContext context)
		{
			_context = context;
		}

		public CustomEntry CustomEntry { get; set; }

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			CustomEntry = await _context.CustomEntries
				.Include(c => c.CustomLeaderboard).FirstOrDefaultAsync(m => m.Id == id);

			if (CustomEntry == null)
			{
				return NotFound();
			}
			return Page();
		}
	}
}