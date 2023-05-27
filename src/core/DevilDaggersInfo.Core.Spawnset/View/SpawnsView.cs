using System.Collections.Immutable;

namespace DevilDaggersInfo.Core.Spawnset.View;

public class SpawnsView
{
	public SpawnsView(SpawnsetBinary spawnsetBinary, GameVersion gameVersion, int waveCount = 40)
		: this(gameVersion, spawnsetBinary.SpawnVersion, spawnsetBinary.GameMode, spawnsetBinary.Spawns, spawnsetBinary.HandLevel, spawnsetBinary.AdditionalGems, spawnsetBinary.TimerStart, waveCount)
	{
	}

	public SpawnsView(GameVersion gameVersion, int spawnVersion, GameMode gameMode, ImmutableArray<Spawn> spawns, HandLevel handLevel, int additionalGems, float timerStart, int waveCount)
	{
		PreLoop = new();
		Waves = new List<SpawnView>[waveCount];
		for (int i = 0; i < waveCount; i++)
			Waves[i] = new();

		if (spawns.Length == 0)
			return;

		double totalSeconds = SpawnsetUtils.GetEffectiveTimerStart(spawnVersion, timerStart);
		EffectivePlayerSettings effectivePlayerSettings = SpawnsetUtils.GetEffectivePlayerSettings(spawnVersion, handLevel, additionalGems);
		GemState gemState = new(effectivePlayerSettings.HandLevel, effectivePlayerSettings.GemsOrHoming, 0);

		if (gameMode is GameMode.TimeAttack or GameMode.Race)
		{
			BuildPreLoop(ref totalSeconds, ref gemState, spawns);
		}
		else
		{
			int loopStartIndex = SpawnsetUtils.GetLoopStartIndex(spawns);
			ImmutableArray<Spawn> preLoopSpawns = spawns.Take(loopStartIndex).ToImmutableArray();
			ImmutableArray<Spawn> loopSpawns = spawns.Skip(loopStartIndex).ToImmutableArray();

			BuildPreLoop(ref totalSeconds, ref gemState, preLoopSpawns);

			if (waveCount > 0 && loopSpawns.Any(s => s.EnemyType != EnemyType.Empty))
				BuildLoop(gameVersion, waveCount, ref totalSeconds, ref gemState, loopSpawns);
		}
	}

	public List<SpawnView> PreLoop { get; }
	public List<SpawnView>[] Waves { get; }

	public bool HasPreLoopSpawns { get; private set; }
	public bool HasLoopSpawns { get; private set; }

	private void BuildPreLoop(ref double totalSeconds, ref GemState gemState, ImmutableArray<Spawn> preLoopSpawns)
	{
		HasPreLoopSpawns = preLoopSpawns.Any(s => s.EnemyType != EnemyType.Empty);
		if (!HasPreLoopSpawns)
			return;

		foreach (Spawn spawn in preLoopSpawns)
		{
			totalSeconds += spawn.Delay;
			int noFarmGems = spawn.EnemyType.GetNoFarmGems();
			gemState = gemState.Add(noFarmGems);
			if (spawn.EnemyType != EnemyType.Empty)
				PreLoop.Add(new(spawn.EnemyType, totalSeconds, noFarmGems, gemState));
		}
	}

	private void BuildLoop(GameVersion gameVersion, int waveCount, ref double totalSeconds, ref GemState gemState, ImmutableArray<Spawn> loopSpawns)
	{
		HasLoopSpawns = loopSpawns.Any(s => s.EnemyType != EnemyType.Empty);
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

				if (spawn.EnemyType == EnemyType.Empty)
					continue;

				EnemyType finalEnemy = spawn.EnemyType;
				if (i % 3 == 2 && gameVersion is not (GameVersion.V1_0 or GameVersion.V2_0) && finalEnemy == EnemyType.Gigapede)
					finalEnemy = EnemyType.Ghostpede;

				int noFarmGems = finalEnemy.GetNoFarmGems();
				gemState = gemState.Add(noFarmGems);

				Waves[i].Add(new(finalEnemy, totalSeconds, noFarmGems, gemState));
			}
		}
	}
}
