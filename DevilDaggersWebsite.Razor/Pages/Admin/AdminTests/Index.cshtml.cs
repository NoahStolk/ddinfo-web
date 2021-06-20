using DevilDaggersDiscordBot;
using DevilDaggersWebsite.Caches.LeaderboardHistory;
using DevilDaggersWebsite.Caches.LeaderboardStatistics;
using DevilDaggersWebsite.Caches.ModArchive;
using DevilDaggersWebsite.Caches.SpawnsetData;
using DevilDaggersWebsite.Caches.SpawnsetHash;
using DSharpPlus.Entities;
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
			sb.AppendLine(LeaderboardStatisticsCache.Instance.LogState(_env));
			sb.AppendLine("**Dynamic caches:**");
			sb.AppendLine(LeaderboardHistoryCache.Instance.LogState(_env));
			sb.AppendLine(ModArchiveCache.Instance.LogState(_env));
			sb.AppendLine(SpawnsetDataCache.Instance.LogState(_env));
			sb.AppendLine(SpawnsetHashCache.Instance.LogState(_env));
			await DiscordLogger.Instance.TryLog(Channel.CacheMonitoring, _env.EnvironmentName, sb.ToString());
		}

		public async Task OnPostTestColors()
		{
			for (int i = 0; i < 7; i++)
			{
				DiscordColor color = DiscordColors.Default;
				int time = i * 10000;
				if (time >= 50000)
					color = DiscordColors.Leviathan;
				else if (time >= 40000)
					color = DiscordColors.Devil;
				else if (time >= 30000)
					color = DiscordColors.Golden;
				else if (time >= 20000)
					color = DiscordColors.Silver;
				else if (time >= 10000)
					color = DiscordColors.Bronze;

				DiscordEmbedBuilder builder = new()
				{
					Title = "hot reload",
					Color = color,
				};
				await DiscordLogger.Instance.TryLog(Channel.TestMonitoring, _env.EnvironmentName, null, builder.Build());
			}
		}
	}
}
