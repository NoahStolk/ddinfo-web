using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Extensions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Razor.Pages.Admin.CustomLeaderboards
{
	public class IndexModel : PageModel
	{
		private readonly ApplicationDbContext _dbContext;

		public IndexModel(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public IList<CustomLeaderboard> CustomLeaderboards { get; private set; } = new List<CustomLeaderboard>();

		public async Task OnGetAsync(string? sortMemberName, bool ascending)
		{
			IQueryable<CustomLeaderboard> query = _dbContext.CustomLeaderboards.Include(cl => cl.SpawnsetFile);
			if (!string.IsNullOrEmpty(sortMemberName))
				query = query.OrderByMember(sortMemberName, ascending);

			CustomLeaderboards = await query.ToListAsync();
		}
	}
}
