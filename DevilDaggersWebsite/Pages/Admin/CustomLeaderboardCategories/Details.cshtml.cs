using DevilDaggersWebsite.Code.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Admin.CustomLeaderboardCategories
{
	[Authorize]
	public class DetailsModel : PageModel
	{
		private readonly ApplicationDbContext _context;

		public DetailsModel(ApplicationDbContext context)
		{
			_context = context;
		}

		public CustomLeaderboardCategory CustomLeaderboardCategory { get; set; }

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			CustomLeaderboardCategory = await _context.CustomLeaderboardCategories.FirstOrDefaultAsync(m => m.Id == id);

			if (CustomLeaderboardCategory == null)
			{
				return NotFound();
			}
			return Page();
		}
	}
}