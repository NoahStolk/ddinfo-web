﻿using DevilDaggersCore.Game;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DevilDaggersWebsite.LeaderboardStatistics
{
	public sealed class StatisticsDataHolder
	{
		private readonly int _timeStep = 100000; // 10 seconds

		private readonly List<CompressedEntry> _entries = new();

		private static readonly Lazy<StatisticsDataHolder> _lazy = new(() => new());

		private StatisticsDataHolder()
		{
		}

		public static StatisticsDataHolder Instance => _lazy.Value;

		public string FileName { get; private set; } = string.Empty;

		public bool IsFetched { get; private set; }

		public Dictionary<Dagger, int> DaggerStats { get; } = new();
		public Dictionary<Death, int> DeathStats { get; } = new();
		public Dictionary<int, int> TimeStats { get; private set; } = new();

		public void Update(IWebHostEnvironment env)
		{
			FileName = Path.GetFileName(Directory.GetFiles(Path.Combine(env.WebRootPath, "leaderboard-statistics"))[0]);
			Update(Path.Combine(env.WebRootPath, "leaderboard-statistics", FileName));
		}

		public void Update(string fileName)
		{
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
