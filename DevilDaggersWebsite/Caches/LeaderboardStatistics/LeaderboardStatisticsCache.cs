using DevilDaggersCore.Game;
using DevilDaggersDiscordBot.Logging;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Caches.LeaderboardStatistics
{
	public sealed class LeaderboardStatisticsCache : IStaticCache
	{
		private readonly int _timeStep = 100000; // 10 seconds

		private readonly List<CompressedEntry> _entries = new();

		private static readonly Lazy<LeaderboardStatisticsCache> _lazy = new(() => new());

		private LeaderboardStatisticsCache()
		{
		}

		public static LeaderboardStatisticsCache Instance => _lazy.Value;

		public string FileName { get; private set; } = string.Empty;
		public bool IsFetched { get; private set; }

		public Dictionary<Dagger, int> DaggerStats { get; } = new();
		public Dictionary<Death, int> DeathStats { get; } = new();
		public Dictionary<int, int> TimeStats { get; private set; } = new();

		public async Task Refresh(IWebHostEnvironment env)
		{
			Initiate(env);

			await DiscordLogger.Instance.TryLog(Channel.CacheMonitoring, env.EnvironmentName, $"Successfully refreshed `{nameof(LeaderboardStatisticsCache)}`.");
		}

		public void Initiate(IWebHostEnvironment env)
		{
			string[] paths = Directory.GetFiles(Path.Combine(env.WebRootPath, "leaderboard-statistics"));
			if (paths.Length == 0)
				throw new("Statistics file not found.");
			if (paths.Length > 1)
				throw new("Multiple statistics files found.");

			FileName = Path.GetFileNameWithoutExtension(paths[0]);
			Update(paths[0]);
		}

		private void Update(string fileName)
		{
			IsFetched = false;

			_entries.Clear();
			DaggerStats.Clear();
			DeathStats.Clear();
			TimeStats = Enumerable.Range(0, 120).ToDictionary(i => i * 10, _ => 0);

			byte[] bytes = File.ReadAllBytes(fileName);
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

				Death death = GameInfo.GetDeathByType(GameVersion.V31, entry.DeathType) ?? throw new($"Invalid death type for entry with time {entry.Time}.");
				if (DeathStats.ContainsKey(death))
					DeathStats[death]++;

				int step = (int)(entry.Time / _timeStep * 10);
				if (TimeStats.ContainsKey(step))
					TimeStats[step]++;
			}

			IsFetched = true;
		}
	}
}
