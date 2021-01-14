using DevilDaggersWebsite.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Razor.Pages.Admin.Titles
{
	public class DetailsModel : PageModel
	{
		private readonly ApplicationDbContext _dbContext;

		public DetailsModel(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

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
	}
}
