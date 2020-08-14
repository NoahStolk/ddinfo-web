using DevilDaggersWebsite.Code.Database;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Admin.Titles
{
	public class IndexModel : PageModel
	{
		private readonly ApplicationDbContext _context;

		public IndexModel(ApplicationDbContext context)
		{
			_context = context;
		}

		public IList<Title> Title { get; set; }

		public async Task OnGetAsync()
		{
			Title = await _context.Titles.ToListAsync();
		}
	}
}