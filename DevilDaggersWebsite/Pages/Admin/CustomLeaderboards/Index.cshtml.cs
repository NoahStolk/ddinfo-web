using DevilDaggersCore.Extensions;
using DevilDaggersWebsite.Core.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Admin.CustomLeaderboards
{
	public class IndexModel : PageModel
	{
		private readonly ApplicationDbContext _context;

		public IndexModel(ApplicationDbContext context)
		{
			_context = context;
		}

		public IList<CustomLeaderboard> CustomLeaderboards { get; private set; }

		public async Task OnGetAsync(string? sortMemberName, bool ascending)
		{
			IQueryable<CustomLeaderboard> query = _context.CustomLeaderboards.Include(cl => cl.Category).Include(cl => cl.SpawnsetFile);
			if (!string.IsNullOrEmpty(sortMemberName))
				query = query.OrderByMember(sortMemberName, ascending);

			CustomLeaderboards = await query.ToListAsync();
		}
	}
}