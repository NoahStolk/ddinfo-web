using DevilDaggersWebsite.Code.Database;
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
		private readonly ApplicationDbContext _context;

		public EditModel(ApplicationDbContext context)
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
			ViewData["CustomLeaderboardId"] = new SelectList(_context.CustomLeaderboards, "Id", "SpawnsetFileName");
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			_context.Attach(CustomEntry).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!CustomEntryExists(CustomEntry.Id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return RedirectToPage("./Index");
		}

		private bool CustomEntryExists(int id)
		{
			return _context.CustomEntries.Any(e => e.Id == id);
		}
	}
}