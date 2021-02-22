using DevilDaggersCore.Utils;
using DSharpPlus.Entities;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace DiscordBotDdInfo.Commands
{
	public class BotStatsCommand : AbstractCommand
	{
		public override string Name => "botstats";

		public CommandResult Execute()
		{
			DiscordEmbedBuilder builder = new()
			{
				Title = "Bot stats for ddinfo",
			};

			builder.AddField("Host source", NetworkUtils.BaseUrl);
			builder.AddField("Build date", File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location).ToString(FormatUtils.DateTimeFullFormat, CultureInfo.InvariantCulture));

			return new(null, builder.Build());
		}
	}
}
