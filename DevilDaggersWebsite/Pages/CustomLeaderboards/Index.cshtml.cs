﻿using DevilDaggersWebsite.Code.Database;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.Pages.CustomLeaderboards
{
	public class IndexModel : PageModel
	{
		private readonly ApplicationDbContext context;

		public IndexModel(ApplicationDbContext context)
		{
			this.context = context;
		}

		public List<SelectListItem> CategoryListItems { get; private set; } = new List<SelectListItem>();

		public CustomLeaderboardCategory Category { get; private set; }
		public List<CustomLeaderboard> Leaderboards { get; private set; }

		public void OnGet(string category)
		{
			foreach (CustomLeaderboardCategory clc in context.CustomLeaderboardCategories)
			{
				if (clc.Name != "Challenge" && clc.Name != "Archive")
					CategoryListItems.Add(new SelectListItem($"Category: {clc.Name}", clc.Name));
			}

			Category = context.CustomLeaderboardCategories.FirstOrDefault(clc => clc.Name == category);
			if (Category == null)
				Category = context.CustomLeaderboardCategories.FirstOrDefault(clc => clc.Name == "Default");

			Leaderboards = context.CustomLeaderboards.Where(l => l.Category == Category).ToList();
		}
	}
}