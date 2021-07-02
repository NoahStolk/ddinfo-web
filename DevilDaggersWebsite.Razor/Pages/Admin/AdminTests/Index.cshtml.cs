using DevilDaggersWebsite.Caches.LeaderboardHistory;
using DevilDaggersWebsite.Caches.LeaderboardStatistics;
using DevilDaggersWebsite.Caches.ModArchive;
using DevilDaggersWebsite.Caches.SpawnsetData;
using DevilDaggersWebsite.Caches.SpawnsetHash;
using DevilDaggersWebsite.HostedServices.DdInfoDiscordBot;
using DSharpPlus.Entities;
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
			=> await DiscordLogger.TryLog(Channel.MonitoringTest, _env.EnvironmentName, "Hello, this is a test message sent from an external environment.");

		public void OnPostTestException()
			=> throw new("TEST EXCEPTION with 3 inner exceptions", new("Inner exception message", new("Another inner exception message", new("Big Discord embed"))));

		public async Task OnPostInitiateLeaderboardStatisticsCache()
			=> await LeaderboardStatisticsCache.Instance.Initiate(_env);

		public void OnPostClearSpawnsetHashCache()
			=> SpawnsetHashCache.Instance.Clear();

		public void OnPostClearLeaderboardHistoryCache()
			=> LeaderboardHistoryCache.Instance.Clear();

		public void OnPostClearSpawnsetDataCache()
			=> SpawnsetDataCache.Instance.Clear();

		public void OnPostClearModDataCache()
			=> ModArchiveCache.Instance.Clear();

		public async Task OnPostTestColors()
		{
			for (int i = 0; i < 6; i++)
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
					Title = "Test colors",
					Color = color,
				};
				await DiscordLogger.TryLog(Channel.MonitoringTest, _env.EnvironmentName, null, builder.Build());
			}
		}
	}
}
