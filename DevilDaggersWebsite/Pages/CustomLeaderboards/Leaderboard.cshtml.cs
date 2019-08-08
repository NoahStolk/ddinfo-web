using DevilDaggersWebsite.Code.Database;
using DevilDaggersWebsite.Code.Database.CustomLeaderboards;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NetBase.Extensions;
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
			Leaderboard = _context.CustomLeaderboards
				.Include(l => l.Category)
				.Where(l => l.SpawnsetFileName == spawnset)
				.FirstOrDefault();

			if (Leaderboard == null)
				return RedirectToPage("Index");

			Entries = _context.CustomEntries
				.Where(e => e.CustomLeaderboard == Leaderboard)
				.OrderByMember(Leaderboard.Category.SortingPropertyName, Leaderboard.Category.Ascending)
				.ToList();

			return null;
		}

		public void GetDaggerInfo(int i, ref string daggerName, ref string seconds)
		{
			daggerName = "";
			seconds = "";
			switch (i)
			{
				case 0:
					daggerName = "Bronze";
					seconds = Leaderboard.Bronze.ToString("0.0000");
					break;
				case 1:
					daggerName = "Silver";
					seconds = Leaderboard.Silver.ToString("0.0000");
					break;
				case 2:
					daggerName = "Golden";
					seconds = Leaderboard.Golden.ToString("0.0000");
					break;
				case 3:
					daggerName = "Devil";
					seconds = Leaderboard.Devil.ToString("0.0000");
					break;
				case 4:
					daggerName = "Homing";
					seconds =
						Leaderboard.Category.Ascending ?
							Entries.Any(e => e.Time <= Leaderboard.Homing) ?
								Leaderboard.Homing.ToString("0.0000")
							:
								"???"
						:
							Entries.Any(e => e.Time >= Leaderboard.Homing) ?
								Leaderboard.Homing.ToString("0.0000")
							:
								"???";
					break;
			}
		}
	}
}