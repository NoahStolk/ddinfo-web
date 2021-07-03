using DevilDaggersCore.Game;
using DevilDaggersWebsite.HostedServices.DdInfoDiscordBot;
using DevilDaggersWebsite.Singletons;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Caches.LeaderboardStatistics
{
	public class LeaderboardStatisticsCache : IStaticCache
	{
		private readonly int _timeStep = 100000; // 10 seconds

		private readonly List<CompressedEntry> _entries = new();

		private readonly DiscordLogger _discordLogger;
		private readonly IWebHostEnvironment _environment;

		public LeaderboardStatisticsCache(DiscordLogger discordLogger, IWebHostEnvironment environment)
		{
			_discordLogger = discordLogger;
			_environment = environment;
		}

		public string FileName { get; private set; } = string.Empty;
		public bool IsFetched { get; private set; }

		public Dictionary<Dagger, int> DaggerStats { get; } = new();
		public Dictionary<Death, int> DeathStats { get; } = new();
		public Dictionary<int, int> TimeStats { get; private set; } = new();

		public IReadOnlyList<CompressedEntry> Entries => _entries;

		public async Task Initiate()
		{
			string leaderboardStatisticsDirectory = Path.Combine(_environment.WebRootPath, "leaderboard-statistics");
			if (!Directory.Exists(leaderboardStatisticsDirectory))
			{
				await _discordLogger.TryLog(Channel.MonitoringError, _environment.EnvironmentName, ":x: Directory `leaderboard-statistics` does not exist.");
				return;
			}

			string[] paths = Directory.GetFiles(leaderboardStatisticsDirectory);
			if (paths.Length == 0)
			{
				await _discordLogger.TryLog(Channel.MonitoringError, _environment.EnvironmentName, ":x: No files found in `leaderboard-statistics`.");
				return;
			}

			FileName = Path.GetFileNameWithoutExtension(paths[0]);

			IsFetched = false;

			_entries.Clear();
			DaggerStats.Clear();
			DeathStats.Clear();
			TimeStats = Enumerable.Range(0, 120).ToDictionary(i => i * 10, _ => 0);

			byte[] bytes = File.ReadAllBytes(paths[0]);
			for (int i = 0; i < bytes.Length / 15; i++)
				_entries.Add(CompressedEntry.FromBytes(bytes[(i * 15)..((i + 1) * 15)]));

			foreach (Death death in GameInfo.GetDeaths(GameVersion.V31))
				DeathStats.Add(death, 0);

			foreach (Dagger dagger in GameInfo.GetDaggers(GameVersion.V31))
				DaggerStats.Add(dagger, 0);

			foreach (CompressedEntry entry in _entries)
			{
				Dagger dagger = GameInfo.GetDaggerFromTime(GameVersion.V31, (int)entry.Time);
				if (DaggerStats.ContainsKey(dagger))
					DaggerStats[dagger]++;

				Death? death = GameInfo.GetDeathByType(GameVersion.V31, entry.DeathType);
				if (death == null)
					await _discordLogger.TryLog(Channel.MonitoringError, _environment.EnvironmentName, $":x: Invalid death type 0x{entry.DeathType:X} for entry with time {entry.Time} in leaderboard-statistics.");
				else if (DeathStats.ContainsKey(death))
					DeathStats[death]++;

				int step = (int)(entry.Time / _timeStep * 10);
				if (TimeStats.ContainsKey(step))
					TimeStats[step]++;
			}

			IsFetched = true;
		}

		public string LogState()
			=> $"`{_entries.Count}` in memory";
	}
}
