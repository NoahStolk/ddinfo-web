using DevilDaggersCore.Game;
using DevilDaggersWebsite.HostedServices.DdInfoDiscordBot;
using DevilDaggersWebsite.Singletons;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Caches.LeaderboardStatistics
{
	public class LeaderboardStatisticsCache : IStaticCache
	{
		private static readonly int _timeStep = 100000; // 10 seconds

		private readonly List<CompressedEntry> _entries = new();

		private readonly DiscordLogger _discordLogger;

		public LeaderboardStatisticsCache(DiscordLogger discordLogger)
		{
			_discordLogger = discordLogger;
		}

		public string FileName { get; private set; } = string.Empty;
		public bool IsFetched { get; private set; }

		public Dictionary<Dagger, int> DaggerStats { get; } = new();
		public Dictionary<Death, int> DeathStats { get; } = new();
		public Dictionary<Enemy, int> EnemyStats { get; } = new();
		public Dictionary<int, int> TimeStats { get; private set; } = new();

		public ulong AllTime { get; private set; }
		public ulong AllKills { get; private set; }
		public ulong AllGems { get; private set; }

		public int AverageTimeInTenthsOfMilliseconds { get; private set; }
		public float AverageKills { get; private set; }
		public float AverageGems { get; private set; }

		public IReadOnlyList<CompressedEntry> Entries => _entries;

		public async Task Initiate()
		{
			string leaderboardStatisticsDirectory = Path.Combine("Content", "LeaderboardStatistics");
			if (!Directory.Exists(leaderboardStatisticsDirectory))
			{
				await _discordLogger.TryLog(Channel.MonitoringError, ":x: Directory `LeaderboardStatistics` does not exist.");
				return;
			}

			string[] paths = Directory.GetFiles(leaderboardStatisticsDirectory);
			if (paths.Length == 0)
			{
				await _discordLogger.TryLog(Channel.MonitoringError, ":x: No files found in `LeaderboardStatistics`.");
				return;
			}

			FileName = Path.GetFileNameWithoutExtension(paths[0]);

			IsFetched = false;

			_entries.Clear();
			DaggerStats.Clear();
			DeathStats.Clear();
			TimeStats = Enumerable.Range(0, 120).ToDictionary(i => i * 10, _ => 0);

			using (FileStream fs = new(paths[0], FileMode.Open))
			{
				using BinaryReader br = new(fs);
				while (br.BaseStream.Position <= br.BaseStream.Length - 15)
				{
					_entries.Add(new()
					{
						Time = br.ReadUInt32(),
						Kills = br.ReadUInt16(),
						Gems = br.ReadUInt16(),
						DaggersHit = br.ReadUInt16(),
						DaggersFired = br.ReadUInt32(),
						DeathType = br.ReadByte(),
					});
				}
			}

			foreach (Death death in GameInfo.GetDeaths(GameVersion.V31))
				DeathStats.Add(death, 0);

			foreach (Dagger dagger in GameInfo.GetDaggers(GameVersion.V31))
				DaggerStats.Add(dagger, 0);

			IEnumerable<Enemy> enemies = GameInfo.GetEnemies(GameVersion.V31).Where(e => e.FirstSpawnSecond.HasValue);
			foreach (Enemy enemy in enemies)
				EnemyStats.Add(enemy, 0);

			foreach (CompressedEntry entry in _entries)
			{
				Dagger dagger = GameInfo.GetDaggerFromTenthsOfMilliseconds(GameVersion.V31, (int)entry.Time);
				if (DaggerStats.ContainsKey(dagger))
					DaggerStats[dagger]++;

				Death? death = GameInfo.GetDeathByType(GameVersion.V31, entry.DeathType);
				if (death == null)
					await _discordLogger.TryLog(Channel.MonitoringError, $":x: Invalid death type 0x{entry.DeathType:X} for entry with time {entry.Time} in leaderboard-statistics.");
				else if (DeathStats.ContainsKey(death))
					DeathStats[death]++;

				foreach (Enemy enemy in enemies)
				{
					if (entry.Time >= enemy.FirstSpawnSecond * 10000 && EnemyStats.ContainsKey(enemy))
						EnemyStats[enemy]++;
				}

				int step = (int)(entry.Time / _timeStep * 10);
				if (TimeStats.ContainsKey(step))
					TimeStats[step]++;

				AllTime += entry.Time;
				AllKills += entry.Kills;
				AllGems += entry.Gems;
			}

			AverageTimeInTenthsOfMilliseconds = (int)(AllTime / (float)_entries.Count);
			AverageKills = AllKills / (float)_entries.Count;
			AverageGems = AllGems / (float)_entries.Count;

			IsFetched = true;
		}

		public string LogState()
			=> $"`{_entries.Count}` in memory";
	}
}
