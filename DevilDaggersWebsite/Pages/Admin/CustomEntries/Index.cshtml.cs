using DevilDaggersCore.Extensions;
using DevilDaggersWebsite.Code.Database;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Admin.CustomEntries
{
	public class IndexModel : PageModel
	{
		private readonly ApplicationDbContext context;

		public IndexModel(ApplicationDbContext context)
		{
			this.context = context;
		}

		public IList<CustomEntry> CustomEntries { get; private set; }

		public async Task OnGetAsync(string? sortMemberName, bool ascending)
		{
			IQueryable<CustomEntry> query = context.CustomEntries.Include(ce => ce.CustomLeaderboard);
			if (!string.IsNullOrEmpty(sortMemberName))
				query = query.OrderByMember(sortMemberName, ascending);

			CustomEntries = await query.ToListAsync();
		}
	}
}