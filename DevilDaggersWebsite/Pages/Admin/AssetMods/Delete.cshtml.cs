using DevilDaggersWebsite.Code.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Admin.AssetMods
{
	public class DeleteModel : PageModel
	{
		private readonly ApplicationDbContext context;

		public DeleteModel(ApplicationDbContext context)
		{
			this.context = context;
		}

		[BindProperty]
		public AssetMod AssetMod { get; set; }

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
				return NotFound();

			AssetMod = await context.AssetMods.Include(am => am.PlayerAssetMods).ThenInclude(pam => pam.Player).FirstOrDefaultAsync(m => m.Id == id);

			if (AssetMod == null)
				return NotFound();
			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int? id)
		{
			if (id == null)
				return NotFound();

			AssetMod = await context.AssetMods.FindAsync(id);

			if (AssetMod != null)
			{
				context.AssetMods.Remove(AssetMod);
				await context.SaveChangesAsync();
			}

			return RedirectToPage("./Index");
		}
	}
}