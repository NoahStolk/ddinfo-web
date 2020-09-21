using DevilDaggersWebsite.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Admin.CustomLeaderboards
{
	public class EditModel : PageModel
	{
		private readonly ApplicationDbContext _context;

		public EditModel(ApplicationDbContext context)
		{
			_context = context;
		}

		[BindProperty]
		public CustomLeaderboard CustomLeaderboard { get; set; }

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
				return NotFound();

			CustomLeaderboard = await _context.CustomLeaderboards
				.Include(c => c.Category)
				.Include(c => c.SpawnsetFile)
				.FirstOrDefaultAsync(m => m.Id == id);

			if (CustomLeaderboard == null)
				return NotFound();
			ViewData["CategoryId"] = new SelectList(_context.CustomLeaderboardCategories, "Id", "Name");
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			ModelState.Remove("CustomLeaderboard.Category");
			ModelState.Remove("CustomLeaderboard.SpawnsetFile.Player");
			ModelState.Remove("CustomLeaderboard.DateLastPlayed");
			ModelState.Remove("CustomLeaderboard.DateCreated");

			if (!ModelState.IsValid)
				return Page();

			_context.Attach(CustomLeaderboard).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!CustomLeaderboardExists(CustomLeaderboard.Id))
					return NotFound();
				else
					throw;
			}

			return RedirectToPage("./Index");
		}

		private bool CustomLeaderboardExists(int id)
			=> _context.CustomLeaderboards.Any(e => e.Id == id);
	}
}