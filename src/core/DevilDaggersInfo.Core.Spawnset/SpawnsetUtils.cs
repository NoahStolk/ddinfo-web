using System.Collections.Immutable;
using System.Numerics;

namespace DevilDaggersInfo.Core.Spawnset;

public static class SpawnsetUtils
{
	public static bool HasSpawns(ImmutableArray<Spawn> spawns)
	{
		return spawns.Any(s => s.EnemyType != EnemyType.Empty);
	}

	public static bool HasEndLoop(ImmutableArray<Spawn> spawns, GameMode gameMode)
	{
		if (gameMode != GameMode.Survival || !HasSpawns(spawns))
			return false;

		int loopStartIndex = GetLoopStartIndex(spawns);
		Spawn[] loopSpawns = spawns.Skip(loopStartIndex).ToArray();

		return loopSpawns.Any(s => s.EnemyType != EnemyType.Empty);
	}

	public static int GetLoopStartIndex(ImmutableArray<Spawn> spawns)
	{
		for (int i = spawns.Length - 1; i >= 0; i--)
		{
			if (spawns[i].EnemyType == EnemyType.Empty)
				return i;
		}

		return 0;
	}

	public static IEnumerable<double> GenerateEndWaveTimes(ImmutableArray<Spawn> spawns, double endGameSecond, int waveIndex)
	{
		double enemyTimer = 0;
		double delay = 0;

		foreach (Spawn spawn in spawns.Skip(GetLoopStartIndex(spawns)))
		{
			delay += spawn.Delay;
			while (enemyTimer < delay)
			{
				endGameSecond += 1f / 60f;
				enemyTimer += 1f / 60f + 1f / 60f / 8f * waveIndex;
			}

			yield return endGameSecond;
		}
	}

	public static EffectivePlayerSettings GetEffectivePlayerSettings(int spawnVersion, HandLevel handLevel, int additionalGems)
	{
		if (spawnVersion < 5)
			return new(HandLevel.Level1, 0, HandLevel.Level1);

		return handLevel switch
		{
			HandLevel.Level1 when additionalGems < 10 => new(HandLevel.Level1, additionalGems, HandLevel.Level1),
			HandLevel.Level1 when additionalGems < 70 => new(HandLevel.Level2, additionalGems, HandLevel.Level2),
			HandLevel.Level1 when additionalGems == 70 => new(HandLevel.Level3, 0, HandLevel.Level3),
			HandLevel.Level1 when additionalGems == 71 => new(HandLevel.Level4, 0, HandLevel.Level4),
			HandLevel.Level1 => new(HandLevel.Level4, 0, HandLevel.Level3),
			HandLevel.Level2 when additionalGems < 0 => new(HandLevel.Level1, additionalGems + 10, HandLevel.Level1),
			HandLevel.Level2 => new(HandLevel.Level2, Math.Min(59, additionalGems) + 10, HandLevel.Level2),
			HandLevel.Level3 => new(HandLevel.Level3, Math.Min(149, additionalGems), HandLevel.Level3),
			_ => new(HandLevel.Level4, additionalGems, HandLevel.Level4),
		};
	}

	public static float GetEffectiveTimerStart(int spawnVersion, float timerStart)
	{
		return spawnVersion < 6 ? 0 : timerStart;
	}

	public static SpawnsetSupportedGameVersion GetSupportedGameVersion(int worldVersion, int spawnVersion)
	{
		if (spawnVersion >= 5)
			return SpawnsetSupportedGameVersion.V3_1AndLater; // Practice mode (spawn version 5+) is only available in V3.1+.

		if (worldVersion >= 9)
			return SpawnsetSupportedGameVersion.V2AndLater; // World version 9 was added in V2.

		return SpawnsetSupportedGameVersion.V0AndLater;
	}

	public static float? GetRaceDaggerHeight(int arenaDimension, ImmutableArena arenaTiles, int raceDaggerTileX, int raceDaggerTileZ)
	{
		if (raceDaggerTileX >= 0 && raceDaggerTileX < arenaDimension && raceDaggerTileZ >= 0 && raceDaggerTileZ < arenaDimension)
			return arenaTiles[raceDaggerTileX, raceDaggerTileZ];

		return null;
	}

	public static int WorldToTileCoordinate(int arenaDimension, float worldCoordinate)
	{
		int arenaMiddle = arenaDimension / 2;
		return (int)MathF.Round(worldCoordinate / 4) + arenaMiddle;
	}

	public static int TileToWorldCoordinate(int arenaDimension, float tileCoordinate)
	{
		int arenaMiddle = arenaDimension / 2;
		return (int)MathF.Round((tileCoordinate - arenaMiddle) * 4);
	}

	public static float GetShrinkEndTime(float shrinkStart, float shrinkEnd, float shrinkRate)
	{
		shrinkStart = Math.Max(shrinkStart, 0);
		shrinkEnd = Math.Max(shrinkEnd, 0);

		if (shrinkRate <= 0 || shrinkEnd > shrinkStart)
			return 0;

		return (shrinkStart - shrinkEnd) / shrinkRate;
	}

	public static float GetShrinkTimeForTile(int arenaDimension, float shrinkStart, float shrinkEnd, float shrinkRate, int x, int y)
	{
		const int tileUnit = 4;
		float shrinkStartInTiles = shrinkStart / tileUnit;
		float shrinkEndInTiles = shrinkEnd / tileUnit;

		int arenaMiddle = arenaDimension / 2;
		Vector2 middle = new(arenaMiddle, arenaMiddle);
		float distance = Vector2.Distance(new(x, y), middle);
		if (distance > Math.Max(shrinkStartInTiles, shrinkEndInTiles))
			return 0;

		if (distance <= shrinkEndInTiles)
			return float.MaxValue;

		// TODO: Prevent this from overflowing.
		return (shrinkStartInTiles - distance) / shrinkRate * tileUnit;
	}

	public static float GetActualTileHeight(int arenaDimension, ImmutableArena arenaTiles, float shrinkStart, float shrinkEnd, float shrinkRate, int x, int y, float currentTime)
	{
		if (x < 0 || x >= arenaDimension)
			throw new ArgumentOutOfRangeException(nameof(x));
		if (y < 0 || y >= arenaDimension)
			throw new ArgumentOutOfRangeException(nameof(y));

		float tileHeight = arenaTiles[x, y];
		float shrinkTime = GetShrinkTimeForTile(arenaDimension, shrinkStart, shrinkEnd, shrinkRate, x, y);
		float shrinkHeight = Math.Max(0, currentTime - shrinkTime) / 4;
		return tileHeight - shrinkHeight;
	}

	public static float? GetActualRaceDaggerHeight(int arenaDimension, ImmutableArena arenaTiles, float shrinkStart, float shrinkEnd, float shrinkRate, int raceDaggerTileX, int raceDaggerTileZ, float currentTime)
	{
		if (raceDaggerTileX >= 0 && raceDaggerTileX < arenaDimension && raceDaggerTileZ >= 0 && raceDaggerTileZ < arenaDimension)
			return GetActualTileHeight(arenaDimension, arenaTiles, shrinkStart, shrinkEnd, shrinkRate, raceDaggerTileX, raceDaggerTileZ, currentTime);

		return null;
	}

	public static float GetSliderMaxSeconds(int arenaDimension, ImmutableArena arenaTiles, float shrinkStart, float shrinkEnd, float shrinkRate)
	{
		// Determine the max tile height to add additional time to the slider.
		// For example, when the shrink ends at 200, but there is a tile at height 20, we want to add another 88 seconds ((20 + 2) * 4) to the slider so the shrink transition is always fully visible for all tiles.
		// Add 2 heights to make sure it is still visible until the height is -2 (the palette should still show something until a height of at least -1 or lower).
		// Multiply by 4 because a tile falls by 1 unit every 4 seconds.
		float maxTileHeight = 0;
		for (int i = 0; i < arenaDimension; i++)
		{
			for (int j = 0; j < arenaDimension; j++)
			{
				float tileHeight = arenaTiles[i, j];
				if (maxTileHeight < tileHeight)
					maxTileHeight = tileHeight;
			}
		}

		return GetShrinkEndTime(shrinkStart, shrinkEnd, shrinkRate) + (maxTileHeight + 2) * 4;
	}

	public static (SpawnSectionInfo PreLoopSection, SpawnSectionInfo LoopSection) CalculateSections(ImmutableArray<Spawn> spawns, GameMode gameMode)
	{
		return gameMode != GameMode.Survival ? CalculateSectionsForNonDefaultGameMode(spawns) : CalculateSectionsForDefaultGameMode(spawns);

		static (SpawnSectionInfo PreLoopSection, SpawnSectionInfo LoopSection) CalculateSectionsForNonDefaultGameMode(ImmutableArray<Spawn> spawns)
		{
			int spawnCount = 0;
			float seconds = 0;
			for (int i = 0; i < spawns.Length; i++)
			{
				Spawn spawn = spawns[i];

				// If the rest of the spawns are empty, break loop.
				if (spawns.Skip(i).All(s => s.EnemyType == EnemyType.Empty))
					break;

				seconds += spawn.Delay;
				if (spawn.EnemyType != EnemyType.Empty)
					spawnCount++;
			}

			return (new(spawnCount, spawnCount == 0 ? null : seconds), default);
		}

		static (SpawnSectionInfo PreLoopSection, SpawnSectionInfo LoopSection) CalculateSectionsForDefaultGameMode(ImmutableArray<Spawn> spawns)
		{
			int loopStartIndex = GetLoopStartIndex(spawns);

			int preLoopSpawnCount = 0;
			int loopSpawnCount = 0;
			float preLoopSeconds = 0;
			float loopSeconds = 0;
			for (int i = 0; i < spawns.Length; i++)
			{
				Spawn spawn = spawns[i];

				if (i < loopStartIndex)
					preLoopSeconds += spawn.Delay;
				else
					loopSeconds += spawn.Delay;

				if (spawn.EnemyType != EnemyType.Empty)
				{
					if (i < loopStartIndex)
						preLoopSpawnCount++;
					else
						loopSpawnCount++;
				}
			}

			return (new(preLoopSpawnCount, preLoopSpawnCount == 0 ? null : preLoopSeconds), new(loopSpawnCount, loopSpawnCount == 0 ? null : loopSeconds));
		}
	}
}
