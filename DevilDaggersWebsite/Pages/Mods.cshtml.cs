using DevilDaggersWebsite.Code.Database;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.Pages
{
	public class ModsModel : PageModel
	{
		private readonly ApplicationDbContext dbContext;

		public ModsModel(ApplicationDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public List<AssetMod> AssetMods { get; private set; } = new List<AssetMod>();

		public void OnGet()
		{
			AssetMods = dbContext.AssetMods.Include(am => am.PlayerAssetMods).ThenInclude(pam => pam.Player).ToList();
		}
	}
}