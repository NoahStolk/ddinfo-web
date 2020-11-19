using DevilDaggersWebsite.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Admin.CustomEntries
{
	public class EditModel : PageModel
	{
		private readonly ApplicationDbContext _dbContext;

		public EditModel(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		[BindProperty]
		public CustomEntry CustomEntry { get; set; }

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
				return NotFound();

			CustomEntry = await _dbContext.CustomEntries
				.Include(c => c.CustomLeaderboard).FirstOrDefaultAsync(m => m.Id == id);

			if (CustomEntry == null)
				return NotFound();
			ViewData["CustomLeaderboardId"] = new SelectList(_dbContext.CustomLeaderboards, "Id", "SpawnsetFileName");
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
				return Page();

			_dbContext.Attach(CustomEntry).State = EntityState.Modified;

			try
			{
				await _dbContext.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException) when (!CustomEntryExists(CustomEntry.Id))
			{
				return NotFound();
			}

			return RedirectToPage("./Index");
		}

		private bool CustomEntryExists(int id) => _dbContext.CustomEntries.Any(e => e.Id == id);
	}
}