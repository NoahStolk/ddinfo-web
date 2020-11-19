using DevilDaggersWebsite.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Admin.Players
{
	public class EditModel : PageModel
	{
		private readonly ApplicationDbContext _dbContext;

		public EditModel(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		[BindProperty]
		public Player Player { get; set; } = null!;

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
				return NotFound();

			Player = await _dbContext.Players.FirstOrDefaultAsync(m => m.Id == id);

			if (Player == null)
				return NotFound();
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			ModelState.Remove("Player.PlayerAssetMods");
			ModelState.Remove("Player.PlayerTitles");

			if (!ModelState.IsValid)
				return Page();

			_dbContext.Attach(Player).State = EntityState.Modified;

			try
			{
				await _dbContext.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException) when (!PlayerExists(Player.Id))
			{
				return NotFound();
			}

			return RedirectToPage("./Index");
		}

		private bool PlayerExists(int id) => _dbContext.Players.Any(e => e.Id == id);
	}
}