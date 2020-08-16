using DevilDaggersWebsite.Code.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Admin.Players
{
	public class DeleteModel : PageModel
	{
		private readonly ApplicationDbContext context;

		public DeleteModel(ApplicationDbContext context)
		{
			this.context = context;
		}

		[BindProperty]
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

		public async Task<IActionResult> OnPostAsync(int? id)
		{
			if (id == null)
				return NotFound();

			Player = await context.Players.FindAsync(id);

			if (Player != null)
			{
				context.Players.Remove(Player);
				await context.SaveChangesAsync();
			}

			return RedirectToPage("./Index");
		}
	}
}