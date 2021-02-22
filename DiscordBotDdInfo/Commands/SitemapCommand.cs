using System.Net;

namespace DiscordBotDdInfo.Commands
{
	public class SitemapCommand : AbstractCommand
	{
		public override string Name => "sitemap";

		public CommandResult Execute()
		{
			using WebClient client = new();
			return new($"```xml\n{client.DownloadString($"{NetworkUtils.BaseUrl}/GenerateSitemap")}\n```");
		}
	}
}
