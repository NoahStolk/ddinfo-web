using DevilDaggersCore.Leaderboards;
using DevilDaggersWebsite.Code.Utils.Web;
using System.Threading.Tasks;
using DevilDaggersCore.Game;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Code.Tasks
{
	public class RetrieveEntireLeaderboardTask : AbstractTask
	{
		public readonly int timeStep = 100000;

		public int MaxPages => Leaderboard.Players / 100 + 1;
		public Leaderboard Leaderboard { get; private set; } = new Leaderboard();

		public Dictionary<Dagger, int> DaggerStats { get; private set; } = new Dictionary<Dagger, int>();
		public Dictionary<Death, int> DeathStats { get; private set; } = new Dictionary<Death, int>();
		public Dictionary<int, int> TimeStats { get; private set; } = new Dictionary<int, int>();

		public override string Schedule => "0 0 */2 * *";

		protected override async Task Execute()
		{
			if (Leaderboard.Players == 0) // This indicates that the object is empty, so we can assume that this is the first time the RetrieveEntireLeaderboardTask is triggered since start up.
			{
				await PopulateLeaderboardRealTime();
			}
			else
			{
				// If the Leaderboard property was not null, keep the object until populating is done (this is a lengthy task). We don't want the previous data to be lost instantly when the RetrieveEntireLeaderboardTask gets triggered.
				Leaderboard leaderboard = await PopulateLeaderboard();

				// When the task is done, set the property to the newly populated object.
				Leaderboard = leaderboard;
			}

			UpdateDaggerStats();
			UpdateDeathStats();
			UpdateTimeStats();
		}

		private async Task PopulateLeaderboardRealTime()
		{
			Leaderboard = await Hasmodai.GetScores(1);

			for (int i = 1; i < MaxPages; i++)
			{
				Leaderboard nextLeaderboard = await Hasmodai.GetScores(i * 100 + 1);

				foreach (Entry entry in nextLeaderboard.Entries)
					Leaderboard.Entries.Add(entry);
			}
		}

		private async Task<Leaderboard> PopulateLeaderboard()
		{
			Leaderboard leaderboard = await Hasmodai.GetScores(1);

			for (int i = 1; i < MaxPages; i++)
			{
				Leaderboard nextLeaderboard = await Hasmodai.GetScores(i * 100 + 1);

				foreach (Entry entry in nextLeaderboard.Entries)
					leaderboard.Entries.Add(entry);
			}

			return leaderboard;
		}

		private void UpdateDaggerStats()
		{
			DaggerStats = new Dictionary<Dagger, int>();

			foreach (Entry entry in Leaderboard.Entries)
			{
				Dagger dagger = GameInfo.GetDaggerFromTime(entry.Time);
				if (DaggerStats.ContainsKey(dagger))
					DaggerStats[dagger]++;
				else
					DaggerStats.Add(dagger, 1);
			}
		}

		private void UpdateDeathStats()
		{
			DeathStats = new Dictionary<Death, int>();

			foreach (Entry entry in Leaderboard.Entries)
			{
				Death death = GameInfo.GetDeathFromDeathType(entry.DeathType, GameInfo.GameVersions[GameInfo.DEFAULT_GAME_VERSION]);
				if (DeathStats.ContainsKey(death))
					DeathStats[death]++;
				else
					DeathStats.Add(death, 1);
			}
		}

		private void UpdateTimeStats()
		{
			TimeStats = new Dictionary<int, int>();

			foreach (Entry entry in Leaderboard.Entries)
			{
				int step = entry.Time / timeStep * 10;
				if (TimeStats.ContainsKey(step))
					TimeStats[step]++;
				else
					TimeStats.Add(step, 1);
			}
		}
	}
}