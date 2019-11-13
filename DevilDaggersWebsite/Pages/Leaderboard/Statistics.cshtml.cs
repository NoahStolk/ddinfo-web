using DevilDaggersCore.Game;
using DevilDaggersCore.Leaderboards;
using DevilDaggersWebsite.Code.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Pages.Leaderboard
{
	public class StatisticsModel : PageModel
	{
		public RetrieveEntireLeaderboardTask Task => (RetrieveEntireLeaderboardTask)TaskInstanceKeeper.Instances[typeof(RetrieveEntireLeaderboardTask)];

		public readonly int timeStep = 100000;

		public Dictionary<Dagger, int> GetDaggerStats()
		{
			Dictionary<Dagger, int> stats = new Dictionary<Dagger, int>();

			try
			{
				foreach (Entry entry in Task.Leaderboard.Entries)
				{
					Dagger dagger = GameInfo.GetDaggerFromTime(entry.Time);
					if (stats.ContainsKey(dagger))
						stats[dagger]++;
					else
						stats.Add(dagger, 1);
				}
			}
			catch
			{
				// An enumeration error due to asynchronous fetching prevented the stats from completing. Please try again or wait until the fetch is finished.
			}

			return stats;
		}

		public Dictionary<Death, int> GetDeathStats()
		{
			Dictionary<Death, int> stats = new Dictionary<Death, int>();

			try
			{
				foreach (Entry entry in Task.Leaderboard.Entries)
				{
					Death death = GameInfo.GetDeathFromDeathType(entry.DeathType, GameInfo.GameVersions[GameInfo.DEFAULT_GAME_VERSION]);
					if (stats.ContainsKey(death))
						stats[death]++;
					else
						stats.Add(death, 1);
				}
			}
			catch { }

			return stats;
		}

		public Dictionary<int, int> GetTimeStats()
		{
			Dictionary<int, int> stats = new Dictionary<int, int>();

			try
			{
				foreach (Entry entry in Task.Leaderboard.Entries)
				{
					int step = entry.Time / timeStep * 10;
					if (stats.ContainsKey(step))
						stats[step]++;
					else
						stats.Add(step, 1);
				}
			}
			catch { }

			return stats;
		}
	}
}