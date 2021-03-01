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

		public int? Id { get; private set; }

		[BindProperty]
		public Title Title { get; set; } = null!;

		public bool IsEditing => Id.HasValue;

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			Id = id;

			Title = await _dbContext.Titles.FirstOrDefaultAsync(m => m.Id == id);

			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int? id)
		{
			if (!ModelState.IsValid)
				return Page();

			Id = id;

			if (IsEditing)
			{
				_dbContext.Attach(Title).State = EntityState.Modified;

				try
				{
					await _dbContext.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException) when (!_dbContext.Titles.Any(e => e.Id == Title.Id))
				{
					return NotFound();
				}
			}
			else
			{
				_dbContext.Titles.Add(Title);
				await _dbContext.SaveChangesAsync();
			}

			return RedirectToPage("./Index");
		}
	}
}
