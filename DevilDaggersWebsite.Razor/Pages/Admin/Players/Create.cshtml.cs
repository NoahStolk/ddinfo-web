using DevilDaggersWebsite.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Razor.Pages.Admin.Players
{
	public class CreateModel : PageModel
	{
		private readonly ApplicationDbContext _dbContext;

		public CreateModel(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		[BindProperty]
		public Player Player { get; set; } = null!;

		public IActionResult OnGet() => Page();

		public async Task<IActionResult> OnPostAsync()
		{
			ModelState.Remove("Player.PlayerAssetMods");
			ModelState.Remove("Player.PlayerTitles");

			if (!ModelState.IsValid)
				return Page();

			_dbContext.Players.Add(Player);
			await _dbContext.SaveChangesAsync();

			return RedirectToPage("./Index");
		}
	}
}
