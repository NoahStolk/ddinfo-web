using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Enumerators;
using DevilDaggersWebsite.Razor.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Razor.Pages.Admin.CustomLeaderboards
{
	public class EditModel : PageModel
	{
		private readonly ApplicationDbContext _dbContext;

		public EditModel(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;

			CategoryList = RazorUtils.EnumToSelectList<CustomLeaderboardCategory>();
		}

		public List<SelectListItem> CategoryList { get; }

		[BindProperty]
		public CustomLeaderboard CustomLeaderboard { get; set; } = null!;

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
				return NotFound();

			CustomLeaderboard = await _dbContext.CustomLeaderboards
				.Include(c => c.SpawnsetFile)
				.FirstOrDefaultAsync(m => m.Id == id);

			if (CustomLeaderboard == null)
				return NotFound();
			ViewData["SpawnsetFileId"] = new SelectList(_dbContext.SpawnsetFiles, "Id", "Name");
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			ModelState.Remove("CustomLeaderboard.SpawnsetFile");
			ModelState.Remove("CustomLeaderboard.SpawnsetFile.Player");
			ModelState.Remove("CustomLeaderboard.DateLastPlayed");
			ModelState.Remove("CustomLeaderboard.DateCreated");

			if (!ModelState.IsValid)
				return Page();

			_dbContext.Attach(CustomLeaderboard).State = EntityState.Modified;

			try
			{
				await _dbContext.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException) when (!CustomLeaderboardExists(CustomLeaderboard.Id))
			{
				return NotFound();
			}

			return RedirectToPage("./Index");
		}

		private bool CustomLeaderboardExists(int id)
			=> _dbContext.CustomLeaderboards.Any(e => e.Id == id);
	}
}
