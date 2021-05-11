using DevilDaggersDiscordBot.Logging;
using DevilDaggersWebsite.Caches.LeaderboardHistory;
using DevilDaggersWebsite.Caches.LeaderboardStatistics;
using DevilDaggersWebsite.Caches.ModArchive;
using DevilDaggersWebsite.Caches.SpawnsetData;
using DevilDaggersWebsite.Caches.SpawnsetHash;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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

		public async Task OnPostFixModBinaryNames()
		{
			const string n = @"dd-spidorlingudark
dd_paperplane
audiolossofbraincells";

			const string nn = @"dd-Spidorlingu-spidorlingudark
dd-LocoCaesar's mini mods-paperplane
audio-lossofbraincells-lossofbraincells";

			List<(string Old, string New)> names = new();
			string[] nSplit = n.Split("\r\n");
			string[] nnSplit = nn.Split("\r\n");
			for (int i = 0; i < nSplit.Length; i++)
				names.Add((nSplit[i], nnSplit[i]));

			List<string> toFix = new() { "Spidorlingu", "LocoCaesar's mini mods", "lossofbraincells", "Nifty Daggers", "Underline" };

			foreach (string path in Directory.GetFiles(Path.Combine(_env.WebRootPath, "mods"), "*.zip"))
			{
				string modName = Path.GetFileNameWithoutExtension(path);
				if (!toFix.Contains(modName))
					continue;

				using ZipArchive archive = new(System.IO.File.Open(path, FileMode.Open, FileAccess.ReadWrite), ZipArchiveMode.Update);

				List<ZipArchiveEntry> originalEntries = new(archive.Entries);
				for (int i = 0; i < originalEntries.Count; i++)
				{
					ZipArchiveEntry entry = originalEntries[i];

					string? newName = null;
					if (entry.Name == "dd-Braden's Gfxpacks-tiny" && modName == "Nifty Daggers")
					{
						newName = "dd-Nifty Daggers-tiny";
					}
					else if (entry.Name == "dd-LocoCaesar's post_luts-(underline)-darker-vanilla-tile" && modName == "Underline")
					{
						newName = "dd-Underline-(underline)-darker-vanilla-tile";
					}
					else
					{
						newName = names.Find(n => n.Old == entry.Name).New;
					}

					if (newName == null)
					{
						await DiscordLogger.Instance.TryLog(Channel.TestMonitoring, _env.EnvironmentName, $"Skipping `{entry.Name}`.");
						continue;
					}

					ZipArchiveEntry newEntry = archive.CreateEntry(newName);
					using (Stream a = entry.Open())
					using (Stream b = newEntry.Open())
						a.CopyTo(b);
					entry.Delete();

					await DiscordLogger.Instance.TryLog(Channel.TestMonitoring, _env.EnvironmentName, $"Renaming `{entry.Name}` to `{newName}`.");
				}
			}
		}
	}
}
