using DevilDaggersWebsite.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Admin.CustomEntries
{
	public class DetailsModel : PageModel
	{
		private readonly ApplicationDbContext context;

		public DetailsModel(ApplicationDbContext context)
		{
			this.context = context;
		}

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
	}
}