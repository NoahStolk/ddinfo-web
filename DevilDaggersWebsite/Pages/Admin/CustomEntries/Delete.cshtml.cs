using DevilDaggersWebsite.Code.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Admin.CustomEntries
{
	public class DeleteModel : PageModel
	{
		private readonly ApplicationDbContext _context;

		public DeleteModel(ApplicationDbContext context)
		{
			_context = context;
		}

		[BindProperty]
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

		public async Task<IActionResult> OnPostAsync(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			CustomEntry = await _context.CustomEntries.FindAsync(id);

			if (CustomEntry != null)
			{
				_context.CustomEntries.Remove(CustomEntry);
				await _context.SaveChangesAsync();
			}

			return RedirectToPage("./Index");
		}
	}
}