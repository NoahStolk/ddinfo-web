using DevilDaggersWebsite.Code.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Admin.CustomEntries
{
	public class CreateModel : PageModel
	{
		private readonly ApplicationDbContext _context;

		public CreateModel(ApplicationDbContext context)
		{
			_context = context;
		}

		public IActionResult OnGet()
		{
			ViewData["CustomLeaderboardId"] = new SelectList(_context.CustomLeaderboards, "Id", "SpawnsetFileName");
			return Page();
		}

		[BindProperty]
		public CustomEntry CustomEntry { get; set; }

		// To protect from overposting attacks, enable the specific properties you want to bind to, for
		// more details, see https://aka.ms/RazorPagesCRUD.
		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			_context.CustomEntries.Add(CustomEntry);
			await _context.SaveChangesAsync();

			return RedirectToPage("./Index");
		}
	}
}