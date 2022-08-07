using DevilDaggersInfo.Common.Extensions;
using DevilDaggersInfo.Core.Wiki;
using DevilDaggersInfo.Core.Wiki.Objects;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Models.LeaderboardStatistics;
using Microsoft.Extensions.Logging;

namespace DevilDaggersInfo.Web.Server.Domain.Services;

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

	public Dictionary<Death, int> DeathsStatistics { get; } = new();
	public int[] DaggersStatistics { get; } = new int[StatDaggers.Count];
	public int[] EnemiesStatistics { get; } = new int[StatEnemies.Count];
	public Dictionary<int, int> TimesStatistics { get; } = new();
	public Dictionary<int, int> KillsStatistics { get; } = new();
	public Dictionary<int, int> GemsStatistics { get; } = new();
	public Dictionary<int, int> DaggersFiredStatistics { get; } = new();
	public Dictionary<int, int> DaggersHitStatistics { get; } = new();
	public int PlayersWithLevel1 { get; private set; }
	public int PlayersWithLevel2 { get; private set; }
	public int PlayersWithLevel3Or4 { get; private set; }

	public ArrayStatistics GlobalArrayStatistics { get; } = new();
	public ArrayStatistics Top10ArrayStatistics { get; } = new();
	public ArrayStatistics Top100ArrayStatistics { get; } = new();
	public ArrayStatistics Top1000ArrayStatistics { get; } = new();

	public IReadOnlyList<CompressedEntry> Entries => _entries;

	public static IReadOnlyList<Dagger> StatDaggers { get; } = Daggers.GetDaggers(GameConstants.CurrentVersion).OrderByDescending(d => d.UnlockSecond).ToList();

	public static IReadOnlyList<Enemy> StatEnemies { get; } = Enemies.GetEnemies(GameConstants.CurrentVersion).Where(e => e.FirstSpawnSecond.HasValue).OrderByDescending(e => e.FirstSpawnSecond).ToList();

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
		Array.Clear(DaggersStatistics);
		DeathsStatistics.Clear();
		Array.Clear(EnemiesStatistics);
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

		_entries.Sort((x, y) => -x.Time.CompareTo(y.Time));

		foreach (Death death in Deaths.GetDeaths(GameConstants.CurrentVersion))
			DeathsStatistics.Add(death, 0);

		int enemyIndex = 0;
		int currentFirstSpawnSecond = StatEnemies[enemyIndex].FirstSpawnSecond!.Value;

		int daggerIndex = 0;
		int currentDaggerTime = StatDaggers[daggerIndex].UnlockSecond;

		bool countingEnemies = true;
		foreach (CompressedEntry entry in _entries)
		{
			Death? death = Deaths.GetDeathByLeaderboardType(GameConstants.CurrentVersion, entry.DeathType);
			if (!death.HasValue)
				_logger.LogError("Invalid death type 0x{death} for entry with time {time} in leaderboard statistics.", entry.DeathType.ToString("X"), entry.Time);
			else if (DeathsStatistics.ContainsKey(death.Value))
				DeathsStatistics[death.Value]++;

			double seconds = entry.Time.ToSecondsTime();

			if (seconds < currentDaggerTime)
			{
				daggerIndex++;
				currentDaggerTime = StatDaggers[daggerIndex].UnlockSecond;
			}

			DaggersStatistics[daggerIndex]++;

			CountEnemies(seconds);

			const int step = 10;

			int timesStep = (int)(entry.Time / 10000 / step * step);
			int killsStep = entry.Kills / step * step;
			int gemsStep = entry.Gems / step * step;

			AddToStatisticsDictionary(TimesStatistics, step, timesStep);
			AddToStatisticsDictionary(KillsStatistics, step, killsStep);
			AddToStatisticsDictionary(GemsStatistics, step, gemsStep);

			const int daggerStep = 100;

			int daggersFiredStep = (int)entry.DaggersFired / daggerStep * daggerStep;
			int daggersHitStep = entry.DaggersHit / daggerStep * daggerStep;

			AddToStatisticsDictionary(DaggersFiredStatistics, daggerStep, daggersFiredStep);
			AddToStatisticsDictionary(DaggersHitStatistics, daggerStep, daggersHitStep);
		}

		GlobalArrayStatistics.Populate(_entries);
		Top10ArrayStatistics.Populate(_entries, 10);
		Top100ArrayStatistics.Populate(_entries, 100);
		Top1000ArrayStatistics.Populate(_entries, 1000);

		PlayersWithLevel1 = _entries.Count(e => e.Gems < 10);
		PlayersWithLevel2 = _entries.Count(e => e.Gems >= 10 && e.Gems < 70);
		PlayersWithLevel3Or4 = _entries.Count(e => e.Gems > 70);

		IsFetched = true;

		void CountEnemies(double seconds)
		{
			if (!countingEnemies)
				return;

			while (seconds < currentFirstSpawnSecond)
			{
				enemyIndex++;
				if (enemyIndex >= StatEnemies.Count)
				{
					countingEnemies = false;
					return;
				}

				currentFirstSpawnSecond = StatEnemies[enemyIndex].FirstSpawnSecond!.Value;
			}

			EnemiesStatistics[enemyIndex]++;
		}
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

	public int GetCount()
		=> _entries.Count;
}
