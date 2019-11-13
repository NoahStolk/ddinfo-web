using DevilDaggersWebsite.Code.Database;
using DevilDaggersWebsite.Code.Database.CustomLeaderboards;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.Pages.CustomLeaderboards
{
	public class IndexModel : PageModel
	{
		public List<SelectListItem> CategoryListItems { get; private set; } = new List<SelectListItem>();

		public CustomLeaderboardCategory Category { get; private set; }
		public List<CustomLeaderboard> Leaderboards { get; private set; }

		private readonly ApplicationDbContext _context;

		public IndexModel(ApplicationDbContext context)
		{
			_context = context;
		}

		public void OnGet(string category)
		{
			foreach (CustomLeaderboardCategory clc in _context.CustomLeaderboardCategories)
				if (clc.Name != "Challenge" && clc.Name != "Archive")
					CategoryListItems.Add(new SelectListItem($"Category: {clc.Name}", clc.Name));

			Category = _context.CustomLeaderboardCategories.FirstOrDefault(clc => clc.Name == category);
			if (Category == null)
				Category = _context.CustomLeaderboardCategories.FirstOrDefault(clc => clc.Name == "Default");

			Leaderboards = _context.CustomLeaderboards.Where(l => l.Category == Category).ToList();
		}
	}
}