using DevilDaggersWebsite.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Razor.Pages.Admin.AssetMods
{
	public class DeleteModel : PageModel
	{
		private readonly ApplicationDbContext _dbcontext;

		public DeleteModel(ApplicationDbContext context)
		{
			_dbcontext = context;
		}

		[BindProperty]
		public AssetMod AssetMod { get; set; }

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
				return NotFound();

			AssetMod = await _dbcontext.AssetMods.Include(am => am.PlayerAssetMods).ThenInclude(pam => pam.Player).FirstOrDefaultAsync(m => m.Id == id);

			if (AssetMod == null)
				return NotFound();
			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int? id)
		{
			if (id == null)
				return NotFound();

			AssetMod = await _dbcontext.AssetMods.FindAsync(id);

			if (AssetMod != null)
			{
				_dbcontext.AssetMods.Remove(AssetMod);
				await _dbcontext.SaveChangesAsync();
			}

			return RedirectToPage("./Index");
		}
	}
}