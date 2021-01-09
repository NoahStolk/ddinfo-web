using DevilDaggersWebsite.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Razor.Pages.Admin.CustomEntries
{
	public class DeleteModel : PageModel
	{
		private readonly ApplicationDbContext _dbContext;

		public DeleteModel(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		[BindProperty]
		public CustomEntry CustomEntry { get; set; } = null!;

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
				return NotFound();

			CustomEntry = await _dbContext.CustomEntries.Include(c => c.CustomLeaderboard).FirstOrDefaultAsync(m => m.Id == id);

			if (CustomEntry == null)
				return NotFound();
			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int? id)
		{
			if (id == null)
				return NotFound();

			CustomEntry = await _dbContext.CustomEntries.FindAsync(id);

			if (CustomEntry != null)
			{
				_dbContext.CustomEntries.Remove(CustomEntry);
				await _dbContext.SaveChangesAsync();
			}

			return RedirectToPage("./Index");
		}
	}
}