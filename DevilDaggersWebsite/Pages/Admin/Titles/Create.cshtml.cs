using DevilDaggersWebsite.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Admin.Titles
{
	public class CreateModel : PageModel
	{
		private readonly ApplicationDbContext context;

		public CreateModel(ApplicationDbContext context)
		{
			this.context = context;
		}

		[BindProperty]
		public Title Title { get; set; }

		public IActionResult OnGet() => Page();

		public async Task<IActionResult> OnPostAsync()
		{
			ModelState.Remove("Title.PlayerTitles");

			if (!ModelState.IsValid)
				return Page();

			context.Titles.Add(Title);
			await context.SaveChangesAsync();

			return RedirectToPage("./Index");
		}
	}
}