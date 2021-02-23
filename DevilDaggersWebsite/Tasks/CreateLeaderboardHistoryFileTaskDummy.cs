using System.Threading.Tasks;

namespace DevilDaggersWebsite.Tasks
{
	public class CreateLeaderboardHistoryFileTaskDummy : AbstractTask
	{
		public override string Schedule => "* * * * *";

		protected override async Task Execute()
		{
			await Task.CompletedTask;
		}
	}
}
