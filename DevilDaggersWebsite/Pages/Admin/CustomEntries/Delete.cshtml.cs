using DevilDaggersWebsite.Code.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Admin.CustomEntries
{
	public class DeleteModel : PageModel
	{
		private readonly ApplicationDbContext context;

		public DeleteModel(ApplicationDbContext context)
		{
			this.context = context;
		}

		[BindProperty]
		public CustomEntry CustomEntry { get; set; }

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
				return NotFound();

			CustomEntry = await context.CustomEntries.Include(c => c.CustomLeaderboard).FirstOrDefaultAsync(m => m.Id == id);

			if (CustomEntry == null)
				return NotFound();
			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int? id)
		{
			if (id == null)
				return NotFound();

			CustomEntry = await context.CustomEntries.FindAsync(id);

			if (CustomEntry != null)
			{
				context.CustomEntries.Remove(CustomEntry);
				await context.SaveChangesAsync();
			}

			return RedirectToPage("./Index");
		}
	}
}