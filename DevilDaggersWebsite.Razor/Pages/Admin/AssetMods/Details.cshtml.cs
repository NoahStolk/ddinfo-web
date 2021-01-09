using DevilDaggersWebsite.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Razor.Pages.Admin.AssetMods
{
	public class DetailsModel : PageModel
	{
		private readonly ApplicationDbContext _dbContext;

		public DetailsModel(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public AssetMod AssetMod { get; set; } = null!;

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
				return NotFound();

			AssetMod = await _dbContext.AssetMods.Include(am => am.PlayerAssetMods).ThenInclude(pam => pam.Player).FirstOrDefaultAsync(m => m.Id == id);

			if (AssetMod == null)
				return NotFound();
			return Page();
		}
	}
}