using DevilDaggersWebsite.Code.Utils;
using DevilDaggersWebsite.Core.Entities;
using DevilDaggersWebsite.Core.Enumerators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Admin.CustomLeaderboards
{
	public class CreateModel : PageModel
	{
		private readonly ApplicationDbContext _context;

		public CreateModel(ApplicationDbContext context)
		{
			_context = context;

			CategoryList = RazorUtils.EnumToSelectList<CustomLeaderboardCategory>();
		}

		public List<SelectListItem> CategoryList { get; }

		[BindProperty]
		public CustomLeaderboard CustomLeaderboard { get; set; }

		public IActionResult OnGet()
		{
			ViewData["SpawnsetFileId"] = new SelectList(_context.SpawnsetFiles, "Id", "Name");
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
			_context.CustomLeaderboards.Add(CustomLeaderboard);
			await _context.SaveChangesAsync();

			return RedirectToPage("./Index");
		}
	}
}