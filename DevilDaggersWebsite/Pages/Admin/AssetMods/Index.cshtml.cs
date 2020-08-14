using DevilDaggersWebsite.Code.Database;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Admin.AssetMods
{
	public class IndexModel : PageModel
	{
		private readonly ApplicationDbContext _context;

		public IndexModel(ApplicationDbContext context)
		{
			_context = context;
		}

		public IList<AssetMod> AssetMod { get; set; }

		public async Task OnGetAsync()
		{
			AssetMod = await _context.AssetMods.ToListAsync();
		}
	}
}