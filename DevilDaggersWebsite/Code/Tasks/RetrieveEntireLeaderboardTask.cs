using DevilDaggersCore.Leaderboards;
using DevilDaggersWebsite.Code.Utils.Web;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Code.Tasks
{
	public class RetrieveEntireLeaderboardTask : AbstractTask
	{
		public int MaxPages => Leaderboard.Players / 100 + 1;

		public override string Schedule => "0 0 * * *";

		public Leaderboard Leaderboard { get; private set; } = new Leaderboard();

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
	}
}