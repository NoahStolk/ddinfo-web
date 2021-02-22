using System.Threading.Tasks;
using BotLogger = DiscordBotDdInfo.DiscordLogger;

namespace DevilDaggersWebsite.Tasks
{
	public class CreateLeaderboardHistoryFileTaskDummy : AbstractTask
	{
		public override string Schedule => "* * * * *";

		protected override async Task Execute()
		{
			await BotLogger.Instance.TryLog($"{nameof(CreateLeaderboardHistoryFileTaskDummy)} executed.");
			await Task.CompletedTask;
		}
	}
}
