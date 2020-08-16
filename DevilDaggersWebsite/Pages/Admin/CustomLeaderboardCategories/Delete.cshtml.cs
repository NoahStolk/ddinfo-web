using DevilDaggersWebsite.Code.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Admin.CustomLeaderboardCategories
{
	public class DeleteModel : PageModel
	{
		private readonly ApplicationDbContext context;

		public DeleteModel(ApplicationDbContext context)
		{
			this.context = context;
		}

		[BindProperty]
		public CustomLeaderboardCategory CustomLeaderboardCategory { get; set; }

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
				return NotFound();

			CustomLeaderboardCategory = await context.CustomLeaderboardCategories.FirstOrDefaultAsync(m => m.Id == id);

			if (CustomLeaderboardCategory == null)
				return NotFound();
			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int? id)
		{
			if (id == null)
				return NotFound();

			CustomLeaderboardCategory = await context.CustomLeaderboardCategories.FindAsync(id);

			if (CustomLeaderboardCategory != null)
			{
				context.CustomLeaderboardCategories.Remove(CustomLeaderboardCategory);
				await context.SaveChangesAsync();
			}

			return RedirectToPage("./Index");
		}
	}
}