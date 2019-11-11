using DevilDaggersCore.Leaderboards;
using DevilDaggersWebsite.Code.Utils.Web;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Code.Tasks
{
	public class RetrieveEntireLeaderboardTask : AbstractTask
	{
		public override string Schedule => "0 0 * * *";

		public Leaderboard Leaderboard { get; private set; } = new Leaderboard();

		protected override async Task Execute()
		{
			Leaderboard = await Hasmodai.GetScores(1);

			for (int i = 1; i < 25/*Leaderboard.Players / 100 + 1*/; i++)
			{
				Leaderboard nextLeaderboard = await Hasmodai.GetScores(i * 100 + 1);

				foreach (Entry entry in nextLeaderboard.Entries)
					Leaderboard.Entries.Add(entry);
			}
		}
	}
}