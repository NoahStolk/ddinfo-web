using DevilDaggersCore.Extensions;
using DevilDaggersWebsite.Core.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Razor.Pages.Admin.Titles
{
	public class IndexModel : PageModel
	{
		private readonly ApplicationDbContext _dbContext;

		public IndexModel(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public IList<Title> Titles { get; private set; } = null!;

		public async Task OnGetAsync(string? sortMemberName, bool ascending)
		{
			IQueryable<Title> query = _dbContext.Titles;
			if (!string.IsNullOrEmpty(sortMemberName))
				query = query.OrderByMember(sortMemberName, ascending);

			Titles = await query.ToListAsync();
		}
	}
}