using DevilDaggersWebsite.Code.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Admin.Players
{
	public class DeleteModel : PageModel
	{
		private readonly ApplicationDbContext _context;

		public DeleteModel(ApplicationDbContext context)
		{
			_context = context;
		}

		[BindProperty]
		public Player Player { get; set; }

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			Player = await _context.Players.FirstOrDefaultAsync(m => m.Id == id);

			if (Player == null)
			{
				return NotFound();
			}
			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			Player = await _context.Players.FindAsync(id);

			if (Player != null)
			{
				_context.Players.Remove(Player);
				await _context.SaveChangesAsync();
			}

			return RedirectToPage("./Index");
		}
	}
}