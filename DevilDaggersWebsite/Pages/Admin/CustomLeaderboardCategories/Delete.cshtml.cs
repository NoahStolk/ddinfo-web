using DevilDaggersWebsite.Code.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Admin.CustomLeaderboardCategories
{
	[Authorize]
	public class DeleteModel : PageModel
	{
		private readonly ApplicationDbContext _context;

		public DeleteModel(ApplicationDbContext context)
		{
			_context = context;
		}

		[BindProperty]
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

		public async Task<IActionResult> OnPostAsync(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			CustomLeaderboardCategory = await _context.CustomLeaderboardCategories.FindAsync(id);

			if (CustomLeaderboardCategory != null)
			{
				_context.CustomLeaderboardCategories.Remove(CustomLeaderboardCategory);
				await _context.SaveChangesAsync();
			}

			return RedirectToPage("./Index");
		}
	}
}