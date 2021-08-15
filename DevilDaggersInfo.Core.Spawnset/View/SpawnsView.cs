namespace DevilDaggersInfo.Core.Spawnset.View;

// TODO: Write unit tests.
public class SpawnsView
{
	public SpawnsView(GameMode gameMode, MajorGameVersion majorGameVersion, Spawn[] spawns, int waveCount = 30, HandLevel handLevel = HandLevel.Level1, int additionalGems = 0, float timerStart = 0)
	{
		if (spawns.Length == 0)
			return;

		double totalSeconds = timerStart;
		int gemsTotal = handLevel.GetStartGems() + additionalGems;

		if (gameMode is GameMode.TimeAttack or GameMode.Race)
		{
			PreLoop = BuildPreLoop(ref totalSeconds, ref gemsTotal, spawns);
		}
		else
		{
			int loopIndex = Spawnset.GetLoopStartIndex(spawns);
			Spawn[] preLoopSpawns = spawns.Take(loopIndex).ToArray();
			Spawn[] loopSpawns = spawns.Skip(loopIndex).ToArray();

			PreLoop = BuildPreLoop(ref totalSeconds, ref gemsTotal, preLoopSpawns);

			if (waveCount > 0 && loopSpawns.Any(s => s.EnemyType != EnemyType.Empty))
				Waves = BuildLoop(majorGameVersion, waveCount, ref totalSeconds, ref gemsTotal, loopSpawns);
		}
	}

	public List<SpawnView>? PreLoop { get; }
	public List<SpawnView>[]? Waves { get; }

	private static List<SpawnView> BuildPreLoop(ref double totalSeconds, ref int gemsTotal, Spawn[] preLoopSpawns)
	{
		List<SpawnView> preLoop = new();
		foreach (Spawn spawn in preLoopSpawns)
		{
			totalSeconds += spawn.Delay;
			int gems = spawn.EnemyType.GetNoFarmGems();
			gemsTotal += gems;
			if (spawn.EnemyType != EnemyType.Empty)
				preLoop.Add(new(spawn.EnemyType, totalSeconds, gems, gemsTotal));
		}

		return preLoop;
	}

	private static List<SpawnView>[] BuildLoop(MajorGameVersion majorGameVersion, int waveCount, ref double totalSeconds, ref int gemsTotal, Spawn[] loopSpawns)
	{
		List<SpawnView>[] waves = new List<SpawnView>[waveCount];
		for (int i = 0; i < waveCount; i++)
		{
			waves[i] = new();
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
					if (i % 3 == 2 && majorGameVersion == MajorGameVersion.V3 && finalEnemy == EnemyType.Gigapede)
						finalEnemy = EnemyType.Ghostpede;

					int gems = finalEnemy.GetNoFarmGems();
					gemsTotal += gems;

					waves[i].Add(new(finalEnemy, totalSeconds, gems, gemsTotal));
				}
			}
		}

		return waves;
	}
}
