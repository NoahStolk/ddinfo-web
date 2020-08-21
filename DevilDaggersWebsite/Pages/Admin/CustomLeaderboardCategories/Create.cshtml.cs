using DevilDaggersWebsite.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Admin.CustomLeaderboardCategories
{
	public class CreateModel : PageModel
	{
		private readonly ApplicationDbContext context;

		public CreateModel(ApplicationDbContext context)
		{
			this.context = context;
		}

		[BindProperty]
		public CustomLeaderboardCategory CustomLeaderboardCategory { get; set; }

		public IActionResult OnGet => Page();

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
				return Page();

			context.CustomLeaderboardCategories.Add(CustomLeaderboardCategory);
			await context.SaveChangesAsync();

			return RedirectToPage("./Index");
		}
	}
}