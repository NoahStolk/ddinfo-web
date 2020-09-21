using DevilDaggersWebsite.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Admin.CustomLeaderboards
{
	public class CreateModel : PageModel
	{
		private readonly ApplicationDbContext _context;

		public CreateModel(ApplicationDbContext context)
		{
			_context = context;
		}

		[BindProperty]
		public CustomLeaderboard CustomLeaderboard { get; set; }

		public IActionResult OnGet()
		{
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

			CustomLeaderboard.DateCreated = DateTime.Now;
			_context.CustomLeaderboards.Add(CustomLeaderboard);
			await _context.SaveChangesAsync();

			return RedirectToPage("./Index");
		}
	}
}