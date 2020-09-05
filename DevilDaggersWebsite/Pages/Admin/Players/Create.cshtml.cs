using DevilDaggersWebsite.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Admin.Players
{
	public class CreateModel : PageModel
	{
		private readonly ApplicationDbContext context;

		public CreateModel(ApplicationDbContext context)
		{
			this.context = context;
		}

		[BindProperty]
		public Player Player { get; set; }

		public IActionResult OnGet() => Page();

		public async Task<IActionResult> OnPostAsync()
		{
			ModelState.Remove("Player.PlayerAssetMods");
			ModelState.Remove("Player.PlayerTitles");

			if (!ModelState.IsValid)
				return Page();

			context.Players.Add(Player);
			await context.SaveChangesAsync();

			return RedirectToPage("./Index");
		}
	}
}