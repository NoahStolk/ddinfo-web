using DevilDaggersWebsite.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Admin.CustomLeaderboards
{
	public class CreateModel : PageModel
	{
		private readonly ApplicationDbContext context;

		public CreateModel(ApplicationDbContext context)
		{
			this.context = context;
		}

		[BindProperty]
		public CustomLeaderboard CustomLeaderboard { get; set; }

		public IActionResult OnGet()
		{
			ViewData["CategoryId"] = new SelectList(context.CustomLeaderboardCategories, "Id", "LayoutPartialName");
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
				return Page();

			context.CustomLeaderboards.Add(CustomLeaderboard);
			await context.SaveChangesAsync();

			return RedirectToPage("./Index");
		}
	}
}