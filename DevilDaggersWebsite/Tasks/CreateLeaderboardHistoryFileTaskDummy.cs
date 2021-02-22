using DiscordBotDdInfo.Logging;
using System.Threading.Tasks;
using BotLogger = DiscordBotDdInfo.Logging.DiscordLogger;

namespace DevilDaggersWebsite.Tasks
{
	public class CreateLeaderboardHistoryFileTaskDummy : AbstractTask
	{
		public override string Schedule => "* * * * *";

		protected override async Task Execute()
		{
			await BotLogger.Instance.TryLog(LoggingChannel.Task, $"{nameof(CreateLeaderboardHistoryFileTaskDummy)} executed.");
			await Task.CompletedTask;
		}
	}
}
