using DevilDaggersWebsite.Code.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Admin.CustomEntries
{
	public class CreateModel : PageModel
	{
		private readonly ApplicationDbContext context;

		public CreateModel(ApplicationDbContext context)
		{
			this.context = context;
		}

		[BindProperty]
		public CustomEntry CustomEntry { get; set; }

		public IActionResult OnGet()
		{
			ViewData["CustomLeaderboardId"] = new SelectList(context.CustomLeaderboards, "Id", "SpawnsetFileName");
			return Page();
		}

		// To protect from overposting attacks, enable the specific properties you want to bind to, for
		// more details, see https://aka.ms/RazorPagesCRUD.
		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
				return Page();

			context.CustomEntries.Add(CustomEntry);
			await context.SaveChangesAsync();

			return RedirectToPage("./Index");
		}
	}
}