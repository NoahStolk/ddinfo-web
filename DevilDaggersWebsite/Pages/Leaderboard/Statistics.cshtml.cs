using DevilDaggersCore.Game;
using DevilDaggersWebsite.Models.Leaderboard;
using DevilDaggersWebsite.Utils;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Leaderboard
{
	public class StatisticsModel : PageModel
	{
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
					Models.Leaderboard.Leaderboard lb = await LeaderboardUtils.LoadLeaderboard(off);
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
	}
}