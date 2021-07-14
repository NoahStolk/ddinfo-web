using DevilDaggersCore.Game;
using DevilDaggersWebsite.BlazorWasm.Server.HostedServices.DdInfoDiscordBot;
using DevilDaggersWebsite.BlazorWasm.Server.Singletons;
using DevilDaggersWebsite.BlazorWasm.Shared;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.BlazorWasm.Server.Caches.LeaderboardStatistics
{
	public class LeaderboardStatisticsCache : IStaticCache
	{
		private const int _timeStep = 100000; // 10 seconds

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
		public Dictionary<int, int> TimeStats { get; } = new();
		public Dictionary<int, int> KillStats { get; } = new();
		public Dictionary<int, int> GemStats { get; } = new();
		public int Level1 { get; private set; }
		public int Level2 { get; private set; }
		public int Level3Or4 { get; private set; }

		public ArrayData Time { get; private set; }
		public ArrayData Kills { get; private set; }
		public ArrayData Gems { get; private set; }

		public IReadOnlyList<CompressedEntry> Entries => _entries;

		public async Task Initiate()
		{
			string leaderboardStatisticsDirectory = Path.Combine("Content", "LeaderboardStatistics");
			if (!Directory.Exists(leaderboardStatisticsDirectory))
			{
				await _discordLogger.TryLog(Channel.MonitoringError, ":x: Leaderboard statistics directory does not exist.");
				return;
			}

			string[] paths = Directory.GetFiles(leaderboardStatisticsDirectory);
			if (paths.Length == 0)
			{
				await _discordLogger.TryLog(Channel.MonitoringError, ":x: No files found in leaderboard statistics directory.");
				return;
			}

			string path = paths.OrderByDescending(p => p).First();
			FileName = Path.GetFileNameWithoutExtension(path);

			IsFetched = false;

			_entries.Clear();
			DaggerStats.Clear();
			DeathStats.Clear();
			EnemyStats.Clear();
			TimeStats.Clear();
			KillStats.Clear();
			GemStats.Clear();

			using (FileStream fs = new(path, FileMode.Open))
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
				{
					TimeStats[step]++;
				}
				else
				{
					TimeStats.Add(step, 1);

					int previous = step - 10;
					while (!TimeStats.ContainsKey(previous) && previous >= 0)
					{
						TimeStats.Add(previous, 0);
						previous -= 10;
					}
				}

				if (KillStats.ContainsKey(entry.Kills))
					KillStats[entry.Kills]++;
				else
					KillStats.Add(entry.Kills, 1);

				if (GemStats.ContainsKey(entry.Gems))
					GemStats[entry.Gems]++;
				else
					GemStats.Add(entry.Gems, 1);
			}

			Time = new ArrayData(_entries.Select(e => (int)e.Time));
			Kills = new ArrayData(_entries.Select(e => (int)e.Kills));
			Gems = new ArrayData(_entries.Select(e => (int)e.Gems));

			Level1 = _entries.Count(e => e.Gems < 10);
			Level2 = _entries.Count(e => e.Gems >= 10 && e.Gems < 70);
			Level3Or4 = _entries.Count(e => e.Gems > 70);

			IsFetched = true;
		}

		public string LogState()
			=> $"`{_entries.Count}` in memory";
	}
}
