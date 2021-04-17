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
		private readonly ApplicationDbContext _dbContext;

		public IndexModel(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;

			foreach (CustomLeaderboardCategory e in (CustomLeaderboardCategory[])Enum.GetValues(typeof(CustomLeaderboardCategory)))
			{
				if (e == CustomLeaderboardCategory.None || e == CustomLeaderboardCategory.Challenge)
					continue;

				CategoryListItems.Add(new SelectListItem($"Category: {e}", e.ToString()));
			}
		}

		public List<SelectListItem> CategoryListItems { get; } = new();

		public CustomLeaderboardCategory Category { get; private set; }
		public List<CustomLeaderboard> Leaderboards { get; private set; } = null!;

		public void OnGet(CustomLeaderboardCategory category = CustomLeaderboardCategory.Default)
		{
			if (category == CustomLeaderboardCategory.None || category == CustomLeaderboardCategory.Challenge)
				category = CustomLeaderboardCategory.Default;

			Category = category;

			Leaderboards = _dbContext.CustomLeaderboards
				.Where(cl => cl.Category == category && !cl.IsArchived)
				.Include(cl => cl.SpawnsetFile)
					.ThenInclude(sf => sf.Player)
				.OrderByDescending(cl => cl.DateLastPlayed)
				.ToList();
		}
	}
}
