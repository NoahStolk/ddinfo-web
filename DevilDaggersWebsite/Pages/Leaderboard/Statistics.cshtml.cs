using DevilDaggersCore.Game;
using DevilDaggersCore.Leaderboard;
using DevilDaggersWebsite.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Leaderboard
{
	public class StatisticsModel : PageModel
	{
		public ActionResult OnGet()
		{
			return RedirectToPage("/Error/404");
		}

		public async Task<Dictionary<Dagger, int>> GetDaggerStats()
		{
			List<int> offsets = new List<int>
			{
				101,
				6201,
				43701,
				113001
			};

			Dictionary<Dagger, int> daggerStats = new Dictionary<Dagger, int>();

			int players = 0;
			foreach (int offset in offsets)
			{
				Entry entry = null;
				int off = offset;
				while (entry == null)
				{
					DevilDaggersCore.Leaderboard.Leaderboard lb = await LeaderboardUtils.LoadLeaderboard(off);
					players = lb.Players;

					for (int i = 0; i < 99; i++)
					{
						if (Game.GetDaggerFromTime(lb.Entries[i].Time) != Game.GetDaggerFromTime(lb.Entries[i + 1].Time))
						{
							entry = lb.Entries[i - 1];
							goto Done;
						}
					}
					off += 100;
				}
			Done:

				daggerStats.Add(Game.GetDaggerFromTime(entry.Time), entry.Rank + 1);
			}
			daggerStats.Add(Game.V3.Default, players);

			return daggerStats;
		}

		public async Task<Dictionary<Death, int>> GetDeathStats(params int[] pages)
		{
			Dictionary<Death, int> deathStats = new Dictionary<Death, int>();

			foreach (int page in pages)
			{
				DevilDaggersCore.Leaderboard.Leaderboard lb = await LeaderboardUtils.LoadLeaderboard(page * 100 + 1);

				foreach (Entry entry in lb.Entries)
				{
					Death death = Game.GetDeathFromDeathType(entry.DeathType);
					if (deathStats.ContainsKey(death))
						deathStats[death]++;
					else
						deathStats.Add(death, 1);
				}
			}

			return deathStats;
		}
	}
}