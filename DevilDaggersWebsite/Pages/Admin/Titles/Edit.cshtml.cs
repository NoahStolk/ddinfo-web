using DevilDaggersWebsite.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Admin.Titles
{
	public class EditModel : PageModel
	{
		private readonly ApplicationDbContext context;

		public EditModel(ApplicationDbContext context)
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

		public async Task<IActionResult> OnPostAsync()
		{
			ModelState.Remove("Title.PlayerTitles");

			if (!ModelState.IsValid)
				return Page();

			context.Attach(Title).State = EntityState.Modified;

			try
			{
				await context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!TitleExists(Title.Id))
					return NotFound();
				else
					throw;
			}

			return RedirectToPage("./Index");
		}

		private bool TitleExists(int id) => context.Titles.Any(e => e.Id == id);
	}
}