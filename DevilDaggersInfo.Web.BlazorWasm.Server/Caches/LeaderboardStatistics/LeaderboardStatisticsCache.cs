namespace DevilDaggersInfo.Web.BlazorWasm.Server.Caches.LeaderboardStatistics;

public class LeaderboardStatisticsCache : IStaticCache
{
	private readonly List<CompressedEntry> _entries = new();

	private readonly IFileSystemService _fileSystemService;
	private readonly ILogger<LeaderboardStatisticsCache> _logger;

	public LeaderboardStatisticsCache(IFileSystemService fileSystemService, ILogger<LeaderboardStatisticsCache> logger)
	{
		_fileSystemService = fileSystemService;
		_logger = logger;
	}

	public string? FileName { get; private set; }
	public bool IsFetched { get; private set; }

	public Dictionary<Dagger, int> DaggersStatistics { get; } = new();
	public Dictionary<Death, int> DeathsStatistics { get; } = new();
	public Dictionary<Enemy, int> EnemiesStatistics { get; } = new();
	public Dictionary<int, int> TimesStatistics { get; } = new();
	public Dictionary<int, int> KillsStatistics { get; } = new();
	public Dictionary<int, int> GemsStatistics { get; } = new();
	public Dictionary<int, int> DaggersFiredStatistics { get; } = new();
	public Dictionary<int, int> DaggersHitStatistics { get; } = new();
	public int PlayersWithLevel1 { get; private set; }
	public int PlayersWithLevel2 { get; private set; }
	public int PlayersWithLevel3Or4 { get; private set; }

	public ArrayStatistic Times { get; } = new();
	public ArrayStatistic Kills { get; } = new();
	public ArrayStatistic Gems { get; } = new();
	public ArrayStatistic DaggersFired { get; } = new();
	public ArrayStatistic DaggersHit { get; } = new();

	public IReadOnlyList<CompressedEntry> Entries => _entries;

	public void Initiate()
	{
		string[] paths = _fileSystemService.TryGetFiles(DataSubDirectory.LeaderboardStatistics);
		if (paths.Length == 0)
		{
			_logger.LogError("No files found in leaderboard statistics directory.");
			return;
		}

		string path = paths.OrderByDescending(p => p).First();
		FileName = Path.GetFileNameWithoutExtension(path);

		IsFetched = false;

		_entries.Clear();
		DaggersStatistics.Clear();
		DeathsStatistics.Clear();
		EnemiesStatistics.Clear();
		TimesStatistics.Clear();
		KillsStatistics.Clear();
		GemsStatistics.Clear();
		DaggersFiredStatistics.Clear();
		DaggersHitStatistics.Clear();

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

		foreach (Death death in Deaths.GetDeaths(GameConstants.CurrentVersion))
			DeathsStatistics.Add(death, 0);

		foreach (Dagger dagger in Daggers.GetDaggers(GameConstants.CurrentVersion))
			DaggersStatistics.Add(dagger, 0);

		IEnumerable<Enemy> enemies = Enemies.GetEnemies(GameConstants.CurrentVersion).Where(e => e.FirstSpawnSecond.HasValue);
		foreach (Enemy enemy in enemies)
			EnemiesStatistics.Add(enemy, 0);

		foreach (CompressedEntry entry in _entries)
		{
			Dagger dagger = Daggers.GetDaggerFromTenthsOfMilliseconds(GameConstants.CurrentVersion, (int)entry.Time);
			if (DaggersStatistics.ContainsKey(dagger))
				DaggersStatistics[dagger]++;

			Death? death = Deaths.GetDeathByLeaderboardType(GameConstants.CurrentVersion, entry.DeathType);
			if (!death.HasValue)
				_logger.LogError("Invalid death type 0x{death} for entry with time {time} in leaderboard-statistics.", entry.DeathType.ToString("X"), entry.Time);
			else if (DeathsStatistics.ContainsKey(death.Value))
				DeathsStatistics[death.Value]++;

			foreach (Enemy enemy in enemies)
			{
				if (entry.Time >= ((double?)enemy.FirstSpawnSecond).To10thMilliTime() && EnemiesStatistics.ContainsKey(enemy))
					EnemiesStatistics[enemy]++;
			}

			const int step = 10;

			int timesStep = (int)(entry.Time / 10000 / step * step);
			AddToStatisticsDictionary(TimesStatistics, step, timesStep);

			int killsStep = entry.Kills / step * step;
			AddToStatisticsDictionary(KillsStatistics, step, killsStep);

			int gemsStep = entry.Gems / step * step;
			AddToStatisticsDictionary(GemsStatistics, step, gemsStep);

			const int daggerStep = 100;

			int daggersFiredStep = (int)entry.DaggersFired / daggerStep * daggerStep;
			AddToStatisticsDictionary(DaggersFiredStatistics, daggerStep, daggersFiredStep);

			int daggersHitStep = entry.DaggersHit / daggerStep * daggerStep;
			AddToStatisticsDictionary(DaggersHitStatistics, daggerStep, daggersHitStep);
		}

		Times.Populate(_entries.Select(e => (int)e.Time));
		Kills.Populate(_entries.Select(e => (int)e.Kills));
		Gems.Populate(_entries.Select(e => (int)e.Gems));
		DaggersFired.Populate(_entries.Select(e => (int)e.DaggersFired));
		DaggersHit.Populate(_entries.Select(e => (int)e.DaggersHit));

		PlayersWithLevel1 = _entries.Count(e => e.Gems < 10);
		PlayersWithLevel2 = _entries.Count(e => e.Gems >= 10 && e.Gems < 70);
		PlayersWithLevel3Or4 = _entries.Count(e => e.Gems > 70);

		IsFetched = true;
	}

	private static void AddToStatisticsDictionary(Dictionary<int, int> dictionary, int step, int currentKey)
	{
		if (dictionary.ContainsKey(currentKey))
		{
			dictionary[currentKey]++;
		}
		else
		{
			dictionary.Add(currentKey, 1);

			// Fill and set remaining keys to 0 as needed.
			int previous = currentKey - step;
			while (!dictionary.ContainsKey(previous) && previous >= 0)
			{
				dictionary.Add(previous, 0);
				previous -= step;
			}
		}
	}

	public string LogState()
		=> $"`{_entries.Count}` in memory";
}
