using DevilDaggersCore.Extensions;
using DevilDaggersWebsite.Code.Database;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Admin.Players
{
	public class IndexModel : PageModel
	{
		private readonly ApplicationDbContext context;

		public IndexModel(ApplicationDbContext context)
		{
			this.context = context;
		}

		public IList<Player> Players { get; private set; }

		public async Task OnGetAsync(string? sortMemberName, bool ascending)
		{
			IQueryable<Player> query = context.Players;
			if (!string.IsNullOrEmpty(sortMemberName))
				query = query.OrderByMember(sortMemberName, ascending);

			Players = await query.ToListAsync();
		}
	}
}