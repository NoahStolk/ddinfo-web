using DevilDaggersWebsite.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Razor.Pages.Admin.CustomLeaderboards
{
	public class DetailsModel : PageModel
	{
		private readonly ApplicationDbContext _dbContext;

		public DetailsModel(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public CustomLeaderboard CustomLeaderboard { get; set; } = null!;

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
				return NotFound();

			CustomLeaderboard = await _dbContext.CustomLeaderboards.Include(c => c.SpawnsetFile).FirstOrDefaultAsync(m => m.Id == id);

			if (CustomLeaderboard == null)
				return NotFound();
			return Page();
		}
	}
}
