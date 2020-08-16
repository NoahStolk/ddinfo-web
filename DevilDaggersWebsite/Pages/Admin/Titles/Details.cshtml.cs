using DevilDaggersWebsite.Code.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Admin.Titles
{
	public class DetailsModel : PageModel
	{
		private readonly ApplicationDbContext _context;

		public DetailsModel(ApplicationDbContext context)
		{
			_context = context;
		}

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
	}
}