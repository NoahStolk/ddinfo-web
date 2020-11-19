using DevilDaggersCore.Extensions;
using DevilDaggersWebsite.Core.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Admin.CustomEntries
{
	public class IndexModel : PageModel
	{
		private readonly ApplicationDbContext _dbContext;

		public IndexModel(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public IList<CustomEntry> CustomEntries { get; private set; }

		public async Task OnGetAsync(string? sortMemberName, bool ascending)
		{
			IQueryable<CustomEntry> query = _dbContext.CustomEntries.Include(ce => ce.CustomLeaderboard).ThenInclude(cl => cl.SpawnsetFile);
			if (!string.IsNullOrEmpty(sortMemberName))
				query = query.OrderByMember(sortMemberName, ascending);

			CustomEntries = await query.ToListAsync();
		}
	}
}