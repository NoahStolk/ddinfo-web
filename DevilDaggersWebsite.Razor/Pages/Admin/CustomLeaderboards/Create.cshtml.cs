using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Enumerators;
using DevilDaggersWebsite.Razor.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Razor.Pages.Admin.CustomLeaderboards
{
	public class CreateModel : PageModel
	{
		private readonly ApplicationDbContext _dbContext;

		public CreateModel(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;

			CategoryList = RazorUtils.EnumToSelectList<CustomLeaderboardCategory>();
		}

		public List<SelectListItem> CategoryList { get; }

		[BindProperty]
		public CustomLeaderboard CustomLeaderboard { get; set; } = null!;

		public IActionResult OnGet()
		{
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

			CustomLeaderboard.DateCreated = DateTime.Now;
			_dbContext.CustomLeaderboards.Add(CustomLeaderboard);
			await _dbContext.SaveChangesAsync();

			return RedirectToPage("./Index");
		}
	}
}
