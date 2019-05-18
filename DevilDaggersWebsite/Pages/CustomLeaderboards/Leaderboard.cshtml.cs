using DevilDaggersWebsite.Code.Database;
using DevilDaggersWebsite.Code.Database.CustomLeaderboards;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.Pages.CustomLeaderboards
{
	public class LeaderboardModel : PageModel
	{
		[BindProperty]
		public CustomLeaderboard Leaderboard { get; set; }

		[BindProperty]
		public List<CustomEntry> Entries { get; set; }

		private readonly ApplicationDbContext _context;

		public LeaderboardModel(ApplicationDbContext context)
		{
			_context = context;
		}

		public ActionResult OnGet(string spawnset)
		{
			Leaderboard = _context.CustomLeaderboards.Where(l => l.SpawnsetFileName == spawnset).FirstOrDefault();

			if (Leaderboard == null)
				return RedirectToPage("Index");

			Entries = _context.CustomEntries.Where(e => e.CustomLeaderboard == Leaderboard).OrderByDescending(e => e.Time).ToList();

			return null;
		}
	}
}