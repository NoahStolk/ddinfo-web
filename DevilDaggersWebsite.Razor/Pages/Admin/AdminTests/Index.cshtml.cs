using DevilDaggersDiscordBot.Logging;
using DevilDaggersWebsite.Caches;
using DevilDaggersWebsite.LeaderboardStatistics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Razor.Pages.Admin.AdminTests
{
	public class IndexModel : PageModel
	{
		private readonly IWebHostEnvironment _env;

		public IndexModel(IWebHostEnvironment env)
			=> _env = env;

		public async Task OnPostTestBotAsync()
			=> await DiscordLogger.Instance.TryLog(Channel.TestMonitoring, _env.EnvironmentName, "Hello, this is a test message sent from an external environment.");

		public void OnPostTestException()
			=> throw new("TEST EXCEPTION with 3 inner exceptions", new("Inner exception message", new("Another inner exception message", new("Big Discord embed"))));

		public void OnPostUpdateStatisticsDataHolder()
			=> StatisticsDataHolder.Instance.Update(_env);

		public async Task OnPostClearSpawnsetHashCache()
			=> await SpawnsetHashCache.Instance.Clear(_env);

		public void OnPostClearLeaderboardHistoryCache()
			=> LeaderboardHistoryCache.Instance.Clear();

		public void OnPostClearSpawnsetDataCache()
			=> SpawnsetDataCache.Instance.Clear();
	}
}
