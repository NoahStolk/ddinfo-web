using DevilDaggersWebsite.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Admin.Titles
{
	public class DeleteModel : PageModel
	{
		private readonly ApplicationDbContext context;

		public DeleteModel(ApplicationDbContext context)
		{
			this.context = context;
		}

		[BindProperty]
		public Title Title { get; set; }

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
				return NotFound();

			Title = await context.Titles.FirstOrDefaultAsync(m => m.Id == id);

			if (Title == null)
				return NotFound();
			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int? id)
		{
			if (id == null)
				return NotFound();

			Title = await context.Titles.FindAsync(id);

			if (Title != null)
			{
				context.Titles.Remove(Title);
				await context.SaveChangesAsync();
			}

			return RedirectToPage("./Index");
		}
	}
}