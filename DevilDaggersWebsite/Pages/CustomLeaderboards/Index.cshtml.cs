using DevilDaggersWebsite.Models.Database;
using DevilDaggersWebsite.Models.Database.CustomLeaderboards;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.Pages.CustomLeaderboards
{
	public class IndexModel : PageModel
	{
		public List<CustomLeaderboard> Leaderboards { get; set; }

		private readonly ApplicationDbContext _context;

		public IndexModel(ApplicationDbContext context)
		{
			_context = context;
		}

		public void OnGet()
		{
			Leaderboards = _context.CustomLeaderboards.ToList();
		}
	}
}