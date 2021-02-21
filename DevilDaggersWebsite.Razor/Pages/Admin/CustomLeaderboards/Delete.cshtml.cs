using DevilDaggersWebsite.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Razor.Pages.Admin.CustomLeaderboards
{
	public class DeleteModel : PageModel
	{
		private readonly ApplicationDbContext _context;

		public DeleteModel(ApplicationDbContext context)
		{
			_context = context;
		}

		[BindProperty]
		public CustomLeaderboard CustomLeaderboard { get; set; }

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
				return NotFound();

			CustomLeaderboard = await _context.CustomLeaderboards.Include(c => c.SpawnsetFile).FirstOrDefaultAsync(m => m.Id == id);

			if (CustomLeaderboard == null)
				return NotFound();
			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int? id)
		{
			if (id == null)
				return NotFound();

			CustomLeaderboard = await _context.CustomLeaderboards.FindAsync(id);

			if (CustomLeaderboard != null)
			{
				_context.CustomLeaderboards.Remove(CustomLeaderboard);
				await _context.SaveChangesAsync();
			}

			return RedirectToPage("./Index");
		}
	}
}
