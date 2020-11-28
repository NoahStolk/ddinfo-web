using DevilDaggersCore.Extensions;
using DevilDaggersWebsite.Core.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Admin.AssetMods
{
	public class IndexModel : PageModel
	{
		private readonly ApplicationDbContext _dbContext;

		public IndexModel(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public IList<AssetMod> AssetMods { get; private set; } = null!;

		public async Task OnGetAsync(string? sortMemberName, bool ascending)
		{
			IQueryable<AssetMod> query = _dbContext.AssetMods.Include(am => am.PlayerAssetMods);
			if (!string.IsNullOrEmpty(sortMemberName))
				query = query.OrderByMember(sortMemberName, ascending);

			AssetMods = await query.ToListAsync();
		}
	}
}