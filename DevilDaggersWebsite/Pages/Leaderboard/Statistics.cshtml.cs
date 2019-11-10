using DevilDaggersCore.Game;
using DevilDaggersCore.Leaderboards;
using DevilDaggersWebsite.Code.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Pages.Leaderboard
{
	public class StatisticsModel : PageModel
	{
		public RetrieveEntireLeaderboardTask Task => ((RetrieveEntireLeaderboardTask)TaskInstanceKeeper.Instances[typeof(RetrieveEntireLeaderboardTask)]);

		public Dictionary<Dagger, int> GetDaggerStats()
		{
			Dictionary<Dagger, int> daggerStats = new Dictionary<Dagger, int>();

			foreach (Entry entry in Task.Leaderboard.Entries)
			{
				Dagger dagger = GameInfo.GetDaggerFromTime(entry.Time);
				if (daggerStats.ContainsKey(dagger))
					daggerStats[dagger]++;
				else
					daggerStats.Add(dagger, 1);
			}

			return daggerStats;
		}

		public Dictionary<Death, int> GetDeathStats()
		{
			Dictionary<Death, int> deathStats = new Dictionary<Death, int>();

			foreach (Entry entry in Task.Leaderboard.Entries)
			{
				Death death = GameInfo.GetDeathFromDeathType(entry.DeathType);
				if (deathStats.ContainsKey(death))
					deathStats[death]++;
				else
					deathStats.Add(death, 1);
			}

			return deathStats;
		}
	}
}