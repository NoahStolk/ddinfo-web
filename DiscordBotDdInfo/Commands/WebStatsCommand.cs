using DevilDaggersCore.Utils;
using DiscordBotDdInfo.Clients;
using DSharpPlus.Entities;
using System.Globalization;
using System.Threading.Tasks;

namespace DiscordBotDdInfo.Commands
{
	public class WebStatsCommand : AbstractCommand
	{
		public override string Name => "webstats";

		public async Task<CommandResult> Execute()
		{
			WebStatsResult webStats = await NetworkUtils.ApiClient.Website_GetStatsAsync();

			DiscordEmbedBuilder builder = new()
			{
				Title = "Web stats for DevilDaggers.info",
				Thumbnail = new() { Url = $"{NetworkUtils.BaseUrl}/favicon.ico", },
			};

			builder.AddField("Build date", webStats.WebsiteBuildDateTime.ToString(FormatUtils.DateTimeFullFormat, CultureInfo.InvariantCulture));

			for (int i = 0; i < webStats.TaskResults.Count; i++)
			{
				TaskResult task = webStats.TaskResults[i];

				builder.AddField("Task", $"`{task.TypeName}`");
				builder.AddField("Last triggered", task.LastTriggered.ToString(FormatUtils.DateTimeFullFormat, CultureInfo.InvariantCulture), true);
				builder.AddField("Last finished", task.LastFinished.ToString(FormatUtils.DateTimeFullFormat, CultureInfo.InvariantCulture), true);
				builder.AddField("Last execution time", task.ExecutionTime.ToString(), true);
				builder.AddField("Cron schedule", task.Schedule, true);
			}

			return new(null, builder.Build());
		}
	}
}
