using DevilDaggersWebsite.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Admin.Players
{
	public class DetailsModel : PageModel
	{
		private readonly ApplicationDbContext context;

		public DetailsModel(ApplicationDbContext context)
		{
			this.context = context;
		}

		public Player Player { get; set; }

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
				return NotFound();

			Player = await context.Players.FirstOrDefaultAsync(m => m.Id == id);

			if (Player == null)
				return NotFound();
			return Page();
		}
	}
}