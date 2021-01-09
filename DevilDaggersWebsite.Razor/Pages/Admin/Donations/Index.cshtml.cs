using DevilDaggersCore.Extensions;
using DevilDaggersWebsite.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Razor.Pages.Admin.Donations
{
	public class IndexModel : PageModel
	{
		private readonly ApplicationDbContext _dbContext;

		public IndexModel(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public IList<Donation> Donations { get; private set; } = null!;

		public async Task OnGetAsync(string? sortMemberName, bool ascending)
		{
			IQueryable<Donation> query = _dbContext.Donations;
			if (!string.IsNullOrEmpty(sortMemberName))
				query = query.OrderByMember(sortMemberName, ascending);

			Donations = await query.ToListAsync();
		}
	}
}