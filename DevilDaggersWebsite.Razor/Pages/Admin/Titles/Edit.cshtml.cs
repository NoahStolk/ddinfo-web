using DevilDaggersWebsite.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Razor.Pages.Admin.Titles
{
	public class EditModel : PageModel
	{
		private readonly ApplicationDbContext _dbContext;

		public EditModel(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		[BindProperty]
		public Title Title { get; set; } = null!;

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
				return NotFound();

			Title = await _dbContext.Titles.FirstOrDefaultAsync(m => m.Id == id);

			if (Title == null)
				return NotFound();
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			ModelState.Remove("Title.PlayerTitles");

			if (!ModelState.IsValid)
				return Page();

			_dbContext.Attach(Title).State = EntityState.Modified;

			try
			{
				await _dbContext.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException) when (!TitleExists(Title.Id))
			{
				return NotFound();
			}

			return RedirectToPage("./Index");
		}

		private bool TitleExists(int id) => _dbContext.Titles.Any(e => e.Id == id);
	}
}