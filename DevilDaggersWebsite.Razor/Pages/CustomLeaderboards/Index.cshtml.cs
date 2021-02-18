using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Enumerators;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.Razor.Pages.CustomLeaderboards
{
	public class IndexModel : PageModel
	{
		private readonly ApplicationDbContext _context;

		public IndexModel(ApplicationDbContext context)
		{
			_context = context;

			foreach (CustomLeaderboardCategory e in (CustomLeaderboardCategory[])Enum.GetValues(typeof(CustomLeaderboardCategory)))
			{
				if (e != CustomLeaderboardCategory.None)
					CategoryListItems.Add(new SelectListItem($"Category: {e}", e.ToString()));
			}
		}

		public List<SelectListItem> CategoryListItems { get; } = new();

		public CustomLeaderboardCategory Category { get; private set; }
		public List<CustomLeaderboard> Leaderboards { get; private set; }

		public void OnGet(CustomLeaderboardCategory category = CustomLeaderboardCategory.Default)
		{
			Category = category;

			Leaderboards = _context.CustomLeaderboards.Where(cl => cl.Category == category).Include(cl => cl.SpawnsetFile).ThenInclude(sf => sf.Player).ToList();
		}
	}
}
