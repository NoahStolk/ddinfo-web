using DevilDaggersDiscordBot.Logging;
using DevilDaggersWebsite.Caches.LeaderboardHistory;
using DevilDaggersWebsite.Caches.LeaderboardStatistics;
using DevilDaggersWebsite.Caches.ModArchive;
using DevilDaggersWebsite.Caches.SpawnsetData;
using DevilDaggersWebsite.Caches.SpawnsetHash;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
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

		public async Task OnPostInitiateLeaderboardStatisticsCache()
			=> await LeaderboardStatisticsCache.Instance.Initiate(_env);

		public async Task OnPostClearSpawnsetHashCache()
			=> await SpawnsetHashCache.Instance.Clear(_env);

		public async Task OnPostClearLeaderboardHistoryCache()
			=> await LeaderboardHistoryCache.Instance.Clear(_env);

		public async Task OnPostClearSpawnsetDataCache()
			=> await SpawnsetDataCache.Instance.Clear(_env);

		public async Task OnPostClearModDataCache()
			=> await ModArchiveCache.Instance.Clear(_env);

		public async Task OnPostLogCaches()
		{
			StringBuilder sb = new("\n");
			sb.AppendLine("**Static caches:**");
			sb.AppendLine(LeaderboardStatisticsCache.Instance.LogState());
			sb.AppendLine("**Dynamic caches:**");
			sb.AppendLine(LeaderboardHistoryCache.Instance.LogState());
			sb.AppendLine(ModArchiveCache.Instance.LogState());
			sb.AppendLine(SpawnsetDataCache.Instance.LogState());
			sb.AppendLine(SpawnsetHashCache.Instance.LogState());
			await DiscordLogger.Instance.TryLog(Channel.CacheMonitoring, _env.EnvironmentName, sb.ToString());
		}
	}
}
