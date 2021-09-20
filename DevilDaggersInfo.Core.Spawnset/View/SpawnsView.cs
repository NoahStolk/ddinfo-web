namespace DevilDaggersInfo.Core.Spawnset.View;

// TODO: Write unit tests.
public class SpawnsView
{
	public SpawnsView(Spawnset spawnset, GameVersion gameVersion, int waveCount = 30)
		: this(spawnset.GameMode, gameVersion, spawnset.Spawns, waveCount, spawnset.HandLevel, spawnset.AdditionalGems, spawnset.TimerStart)
	{
	}

	public SpawnsView(GameMode gameMode, GameVersion gameVersion, Spawn[] spawns, int waveCount = 30, HandLevel handLevel = HandLevel.Level1, int additionalGems = 0, float timerStart = 0)
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
			int loopStartIndex = Spawnset.GetLoopStartIndex(spawns);
			Spawn[] preLoopSpawns = spawns.Take(loopStartIndex).ToArray();
			Spawn[] loopSpawns = spawns.Skip(loopStartIndex).ToArray();

			BuildPreLoop(ref totalSeconds, ref totalGems, preLoopSpawns);

			if (waveCount > 0 && loopSpawns.Any(s => s.EnemyType != EnemyType.Empty))
				BuildLoop(gameVersion, waveCount, ref totalSeconds, ref totalGems, loopSpawns);
		}
	}

	public List<SpawnView> PreLoop { get; }
	public List<SpawnView>[] Waves { get; }

	private void BuildPreLoop(ref double totalSeconds, ref int totalGems, Spawn[] preLoopSpawns)
	{
		foreach (Spawn spawn in preLoopSpawns)
		{
			totalSeconds += spawn.Delay;
			int gems = spawn.EnemyType.GetNoFarmGems();
			totalGems += gems;
			if (spawn.EnemyType != EnemyType.Empty)
				PreLoop.Add(new(spawn.EnemyType, totalSeconds, gems, totalGems));
		}
	}

	private void BuildLoop(GameVersion gameVersion, int waveCount, ref double totalSeconds, ref int totalGems, Spawn[] loopSpawns)
	{
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
					if (i % 3 == 2 && gameVersion is GameVersion.V3_0 or GameVersion.V3_1 && finalEnemy == EnemyType.Gigapede)
						finalEnemy = EnemyType.Ghostpede;

					int gems = finalEnemy.GetNoFarmGems();
					totalGems += gems;

					Waves[i].Add(new(finalEnemy, totalSeconds, gems, totalGems));
				}
			}
		}
	}
}
