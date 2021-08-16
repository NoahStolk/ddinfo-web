﻿using DevilDaggersInfo.Core.Wiki;
using DevilDaggersInfo.Core.Wiki.Enums;
using DevilDaggersInfo.Core.Wiki.Objects;
using DevilDaggersInfo.Web.BlazorWasm.Server.Enums;
using DevilDaggersInfo.Web.BlazorWasm.Server.HostedServices.DdInfoDiscordBot;
using DevilDaggersInfo.Web.BlazorWasm.Server.Singletons;
using DevilDaggersInfo.Web.BlazorWasm.Server.Transients;
using DevilDaggersInfo.Web.BlazorWasm.Shared;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Extensions;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Caches.LeaderboardStatistics;

public class LeaderboardStatisticsCache : IStaticCache
{
	private readonly List<CompressedEntry> _entries = new();

	private readonly IFileSystemService _fileSystemService;
	private readonly DiscordLogger _discordLogger;

	public LeaderboardStatisticsCache(IFileSystemService fileSystemService, DiscordLogger discordLogger)
	{
		_fileSystemService = fileSystemService;
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

	public ArrayData Time { get; } = new();
	public ArrayData Kills { get; } = new();
	public ArrayData Gems { get; } = new();

	public IReadOnlyList<CompressedEntry> Entries => _entries;

	public async Task Initiate()
	{
		string[] paths = _fileSystemService.TryGetFiles(DataSubDirectory.LeaderboardStatistics);
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

		foreach (Death death in Deaths.GetDeaths(GameVersion.V3_1).DistinctBy(d => d.Name))
			DeathStats.Add(death, 0);

		foreach (Dagger dagger in Daggers.GetDaggers(GameVersion.V3_1))
			DaggerStats.Add(dagger, 0);

		IEnumerable<Enemy> enemies = EnemiesV3_1.All.Where(e => e.FirstSpawnSecond.HasValue);
		foreach (Enemy enemy in enemies)
			EnemyStats.Add(enemy, 0);

		foreach (CompressedEntry entry in _entries)
		{
			Dagger dagger = Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_1, (int)entry.Time);
			if (DaggerStats.ContainsKey(dagger))
				DaggerStats[dagger]++;

			Death? death = Deaths.GetDeathByType(GameVersion.V3_1, entry.DeathType);
			if (death == null)
				await _discordLogger.TryLog(Channel.MonitoringError, $":x: Invalid death type 0x{entry.DeathType:X} for entry with time {entry.Time} in leaderboard-statistics.");
			else if (DeathStats.ContainsKey(death))
				DeathStats[death]++;

			foreach (Enemy enemy in enemies)
			{
				if (entry.Time >= ((double?)enemy.FirstSpawnSecond).To10thMilliTime() && EnemyStats.ContainsKey(enemy))
					EnemyStats[enemy]++;
			}

			const int step = 10;

			int timeStep = (int)(entry.Time / 10000 / step * step);
			if (TimeStats.ContainsKey(timeStep))
			{
				TimeStats[timeStep]++;
			}
			else
			{
				TimeStats.Add(timeStep, 1);

				int previous = timeStep - step;
				while (!TimeStats.ContainsKey(previous) && previous >= 0)
				{
					TimeStats.Add(previous, 0);
					previous -= step;
				}
			}

			int killStep = entry.Kills / step * step;
			if (KillStats.ContainsKey(killStep))
			{
				KillStats[killStep]++;
			}
			else
			{
				KillStats.Add(killStep, 1);

				int previous = killStep - step;
				while (!KillStats.ContainsKey(previous) && previous >= 0)
				{
					KillStats.Add(previous, 0);
					previous -= step;
				}
			}

			int gemStep = entry.Gems / step * step;
			if (GemStats.ContainsKey(gemStep))
			{
				GemStats[gemStep]++;
			}
			else
			{
				GemStats.Add(gemStep, 1);

				int previous = gemStep - step;
				while (!GemStats.ContainsKey(previous) && previous >= 0)
				{
					GemStats.Add(previous, 0);
					previous -= step;
				}
			}
		}

		Time.Populate(_entries.Select(e => (int)e.Time));
		Kills.Populate(_entries.Select(e => (int)e.Kills));
		Gems.Populate(_entries.Select(e => (int)e.Gems));

		Level1 = _entries.Count(e => e.Gems < 10);
		Level2 = _entries.Count(e => e.Gems >= 10 && e.Gems < 70);
		Level3Or4 = _entries.Count(e => e.Gems > 70);

		IsFetched = true;
	}

	public string LogState()
		=> $"`{_entries.Count}` in memory";
}
