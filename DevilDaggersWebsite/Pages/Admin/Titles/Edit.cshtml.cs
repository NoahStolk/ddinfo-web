using DevilDaggersWebsite.Code.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Admin.Titles
{
	public class EditModel : PageModel
	{
		private readonly ApplicationDbContext _context;

		public EditModel(ApplicationDbContext context)
		{
			_context = context;
		}

		[BindProperty]
		public Title Title { get; set; }

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			Title = await _context.Titles.FirstOrDefaultAsync(m => m.Id == id);

			if (Title == null)
			{
				return NotFound();
			}
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			_context.Attach(Title).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!TitleExists(Title.Id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return RedirectToPage("./Index");
		}

		private bool TitleExists(int id)
		{
			return _context.Titles.Any(e => e.Id == id);
		}
	}
}