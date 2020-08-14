using DevilDaggersWebsite.Code.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Admin.CustomLeaderboards
{
	[Authorize]
	public class CreateModel : PageModel
	{
		private readonly ApplicationDbContext _context;

		public CreateModel(ApplicationDbContext context)
		{
			_context = context;
		}

		public IActionResult OnGet()
		{
			ViewData["CategoryId"] = new SelectList(_context.CustomLeaderboardCategories, "Id", "LayoutPartialName");
			return Page();
		}

		[BindProperty]
		public CustomLeaderboard CustomLeaderboard { get; set; }

		// To protect from overposting attacks, enable the specific properties you want to bind to, for
		// more details, see https://aka.ms/RazorPagesCRUD.
		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			_context.CustomLeaderboards.Add(CustomLeaderboard);
			await _context.SaveChangesAsync();

			return RedirectToPage("./Index");
		}
	}
}