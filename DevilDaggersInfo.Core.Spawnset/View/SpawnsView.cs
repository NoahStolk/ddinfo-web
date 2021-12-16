namespace DevilDaggersInfo.Core.Spawnset.View;

// TODO: Write unit tests.
public class SpawnsView
{
	public SpawnsView(SpawnsetBinary spawnsetBinary, GameVersion gameVersion, int waveCount = 40)
		: this(spawnsetBinary.GameMode, gameVersion, spawnsetBinary.Spawns, waveCount, spawnsetBinary.HandLevel, spawnsetBinary.AdditionalGems, spawnsetBinary.TimerStart)
	{
	}

	public SpawnsView(GameMode gameMode, GameVersion gameVersion, Spawn[] spawns, int waveCount = 40, HandLevel handLevel = HandLevel.Level1, int additionalGems = 0, float timerStart = 0)
	{
		PreLoop = new();
		Waves = new List<SpawnView>[waveCount];
		for (int i = 0; i < waveCount; i++)
			Waves[i] = new();

		if (spawns.Length == 0)
			return;

		double totalSeconds = timerStart;
		int totalGems = handLevel.GetStartGems() + additionalGems;

		if (gameMode is GameMode.TimeAttack or GameMode.Race)
		{
			BuildPreLoop(ref totalSeconds, ref totalGems, spawns);
		}
		else
		{
			int loopStartIndex = SpawnsetBinary.GetLoopStartIndex(spawns);
			Spawn[] preLoopSpawns = spawns.Take(loopStartIndex).ToArray();
			Spawn[] loopSpawns = spawns.Skip(loopStartIndex).ToArray();

			BuildPreLoop(ref totalSeconds, ref totalGems, preLoopSpawns);

			if (waveCount > 0 && loopSpawns.Any(s => s.EnemyType != EnemyType.Empty))
				BuildLoop(gameVersion, waveCount, ref totalSeconds, ref totalGems, loopSpawns);
		}
	}

	public List<SpawnView> PreLoop { get; }
	public List<SpawnView>[] Waves { get; }

	public bool HasPreLoopSpawns { get; private set; }
	public bool HasLoopSpawns { get; private set; }

	private void BuildPreLoop(ref double totalSeconds, ref int totalGems, Spawn[] preLoopSpawns)
	{
		HasPreLoopSpawns = preLoopSpawns.Length > 0;
		if (!HasPreLoopSpawns)
			return;

		foreach (Spawn spawn in preLoopSpawns)
		{
			totalSeconds += spawn.Delay;
			int noFarmGems = spawn.EnemyType.GetNoFarmGems();
			totalGems += noFarmGems;
			if (spawn.EnemyType != EnemyType.Empty)
				PreLoop.Add(new(spawn.EnemyType, totalSeconds, noFarmGems, totalGems));
		}
	}

	private void BuildLoop(GameVersion gameVersion, int waveCount, ref double totalSeconds, ref int totalGems, Spawn[] loopSpawns)
	{
		HasLoopSpawns = loopSpawns.Length > 0;
		if (!HasLoopSpawns)
			return;

		for (int i = 0; i < waveCount; i++)
		{
			double enemyTimer = 0;
			double delay = 0;
			foreach (Spawn spawn in loopSpawns)
			{
				delay += spawn.Delay;
				while (enemyTimer < delay)
				{
					totalSeconds += 1f / 60f;
					enemyTimer += 1f / 60f + 1f / 60f / 8f * i;
				}

				if (spawn.EnemyType != EnemyType.Empty)
				{
					EnemyType finalEnemy = spawn.EnemyType;
					if (i % 3 == 2 && !(gameVersion is GameVersion.V1_0 or GameVersion.V2_0) && finalEnemy == EnemyType.Gigapede)
						finalEnemy = EnemyType.Ghostpede;

					int gems = finalEnemy.GetNoFarmGems();
					totalGems += gems;

					Waves[i].Add(new(finalEnemy, totalSeconds, gems, totalGems));
				}
			}
		}
	}
}
