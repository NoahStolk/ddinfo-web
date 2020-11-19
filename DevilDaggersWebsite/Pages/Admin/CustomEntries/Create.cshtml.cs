using DevilDaggersWebsite.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Admin.CustomEntries
{
	public class CreateModel : PageModel
	{
		private readonly ApplicationDbContext _dbContext;

		public CreateModel(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		[BindProperty]
		public CustomEntry CustomEntry { get; set; }

		public IActionResult OnGet()
		{
			ViewData["CustomLeaderboardId"] = new SelectList(_dbContext.CustomLeaderboards, "Id", "SpawnsetFileName");
			return Page();
		}

		// To protect from overposting attacks, enable the specific properties you want to bind to, for
		// more details, see https://aka.ms/RazorPagesCRUD.
		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
				return Page();

			_dbContext.CustomEntries.Add(CustomEntry);
			await _dbContext.SaveChangesAsync();

			return RedirectToPage("./Index");
		}
	}
}