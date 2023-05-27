using DevilDaggersInfo.Core.Spawnset.Exceptions;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace DevilDaggersInfo.Core.Spawnset;

public record SpawnsetBinary
{
	/// <summary>
	/// The game supports different arena dimensions, however anything other than the default causes the spawnset to glitch out.
	/// The default dimension is also the max possible; any higher value will crash the game.
	/// </summary>
	public const int ArenaDimensionMax = 51;

	public SpawnsetBinary(
		int spawnVersion,
		int worldVersion,
		float shrinkStart,
		float shrinkEnd,
		float shrinkRate,
		float brightness,
		GameMode gameMode,
		int arenaDimension,
		float[,] arenaTiles,
		Vector2 raceDaggerPosition,
		int unusedDevilTime,
		int unusedGoldenTime,
		int unusedSilverTime,
		int unusedBronzeTime,
		ImmutableArray<Spawn> spawns,
		HandLevel handLevel,
		int additionalGems,
		float timerStart)
	{
		SpawnVersion = spawnVersion;
		WorldVersion = worldVersion;
		ShrinkStart = shrinkStart;
		ShrinkEnd = shrinkEnd;
		ShrinkRate = shrinkRate;
		Brightness = brightness;
		GameMode = gameMode;
		ArenaDimension = arenaDimension;
		ArenaTiles = new(arenaDimension, arenaTiles);

		RaceDaggerPosition = raceDaggerPosition;
		UnusedDevilTime = unusedDevilTime;
		UnusedGoldenTime = unusedGoldenTime;
		UnusedSilverTime = unusedSilverTime;
		UnusedBronzeTime = unusedBronzeTime;
		Spawns = spawns;

		HandLevel = handLevel;
		AdditionalGems = additionalGems;
		TimerStart = timerStart;
	}

	public int SpawnVersion { get; init; }
	public int WorldVersion { get; init; }
	public float ShrinkStart { get; init; }
	public float ShrinkEnd { get; init; }
	public float ShrinkRate { get; init; }
	public float Brightness { get; init; }
	public GameMode GameMode { get; init; }
	public int ArenaDimension { get; init; }
	public ImmutableArena ArenaTiles { get; init; }

	public Vector2 RaceDaggerPosition { get; init; }
	public int UnusedDevilTime { get; init; }
	public int UnusedGoldenTime { get; init; }
	public int UnusedSilverTime { get; init; }
	public int UnusedBronzeTime { get; init; }

	public ImmutableArray<Spawn> Spawns { get; init; }

	public HandLevel HandLevel { get; init; }
	public int AdditionalGems { get; init; }
	public float TimerStart { get; init; }

	#region Parsing

	public static bool TryParse(byte[] fileContents, [NotNullWhen(true)] out SpawnsetBinary? spawnsetBinary)
	{
		try
		{
			spawnsetBinary = Parse(fileContents);
			return true;
		}
		catch
		{
			// TODO: Log exceptions.
			spawnsetBinary = null;
			return false;
		}
	}

	// TODO: Throw clear exceptions when parsing fails.
	public static SpawnsetBinary Parse(byte[] fileContents)
	{
		using MemoryStream ms = new(fileContents);
		using BinaryReader br = new(ms);
		br.BaseStream.Position = 0;

		// Header
		int spawnVersion = br.ReadInt32();
		if (spawnVersion > 6)
			throw new InvalidSpawnsetBinaryException($"Spawn version {spawnVersion} is not supported.");

		int worldVersion = br.ReadInt32();
		if (worldVersion > 9)
			throw new InvalidSpawnsetBinaryException($"World version {worldVersion} is not supported.");

		float shrinkEnd = br.ReadSingle();
		float shrinkStart = br.ReadSingle();
		float shrinkRate = br.ReadSingle();
		float brightness = br.ReadSingle();
		GameMode gameMode = br.ReadInt32().ToGameMode();
		int arenaDimension = br.ReadInt32();
		if (arenaDimension is < 0 or > ArenaDimensionMax)
			throw new InvalidSpawnsetBinaryException($"Arena dimension cannot be {arenaDimension}.");

		br.Seek(4);

		// Arena
		float[,] arenaTiles = new float[arenaDimension, arenaDimension];
		for (int i = 0; i < arenaDimension * arenaDimension; i++)
		{
			int x = i % arenaDimension;
			int y = i / arenaDimension;
			arenaTiles[x, y] = br.ReadSingle();
		}

		// Spawns header
		float raceDaggerX = br.ReadSingle();
		float raceDaggerZ = br.ReadSingle();
		br.Seek(8);
		int unusedDevilTime = br.ReadInt32();
		int unusedGoldenTime = br.ReadInt32();
		int unusedSilverTime = br.ReadInt32();
		int unusedBronzeTime = br.ReadInt32();
		if (worldVersion >= 9)
			br.Seek(4);
		int spawnCount = br.ReadInt32();

		// Spawns
		Spawn[] spawns = new Spawn[spawnCount];
		for (int i = 0; i < spawnCount; i++)
		{
			EnemyType enemyType = br.ReadInt32().ToEnemyType();
			float delay = br.ReadSingle();
			spawns[i] = new(enemyType, delay);

			br.Seek(20);
		}

		// Settings
		HandLevel handLevel = HandLevel.Level1;
		int additionalGems = 0;
		float timerStart = 0;
		if (spawnVersion >= 5)
		{
			handLevel = br.ReadByte().ToHandLevel();
			additionalGems = br.ReadInt32();

			if (spawnVersion >= 6)
				timerStart = br.ReadSingle();
		}

		return new(
			spawnVersion,
			worldVersion,
			shrinkStart,
			shrinkEnd,
			shrinkRate,
			brightness,
			gameMode,
			arenaDimension,
			arenaTiles,
			new(raceDaggerX, raceDaggerZ),
			unusedDevilTime,
			unusedGoldenTime,
			unusedSilverTime,
			unusedBronzeTime,
			spawns.ToImmutableArray(),
			handLevel,
			additionalGems,
			timerStart);
	}

	#endregion Parsing

	#region Converting

	public byte[] ToBytes()
	{
		using MemoryStream ms = new();
		using BinaryWriter bw = new(ms);

		// Header
		bw.Write(SpawnVersion);
		bw.Write(WorldVersion);
		bw.Write(ShrinkEnd);
		bw.Write(ShrinkStart);
		bw.Write(ShrinkRate);
		bw.Write(Brightness);
		bw.Write((int)GameMode);
		bw.Write(ArenaDimension);
		bw.Write(0x01);

		// Arena
		for (int i = 0; i < ArenaDimension * ArenaDimension; i++)
		{
			int x = i % ArenaDimension;
			int y = i / ArenaDimension;

			bw.Write(ArenaTiles[x, y]);
		}

		// Spawns header
		bw.Write(RaceDaggerPosition.X);
		bw.Write(RaceDaggerPosition.Y);
		bw.Seek(4, SeekOrigin.Current);
		bw.Write(0x01);
		bw.Write(UnusedDevilTime);
		bw.Write(UnusedGoldenTime);
		bw.Write(UnusedSilverTime);
		bw.Write(UnusedBronzeTime);
		if (WorldVersion >= 9)
			bw.Seek(4, SeekOrigin.Current);
		bw.Write(Spawns.Length);

		// Spawns
		foreach (Spawn spawn in Spawns)
		{
			bw.Write((int)spawn.EnemyType);
			bw.Write(spawn.Delay);
			bw.Seek(4, SeekOrigin.Current);
			bw.Write(0x03);
			bw.Seek(6, SeekOrigin.Current);
			bw.Write((byte)0xF0);
			bw.Write((byte)0x41);
			bw.Write(0x0A);
		}

		// Settings
		if (SpawnVersion >= 5)
		{
			bw.Write((byte)HandLevel);
			bw.Write(AdditionalGems);
			if (SpawnVersion >= 6)
				bw.Write(TimerStart);
		}

		return ms.ToArray();
	}

	#endregion Converting

	#region Utilities

	/// <summary>
	/// Creates a new spawnset, in the latest format, containing no spawns, with an arena that is roughly the same as the default one in the game.
	/// </summary>
	public static SpawnsetBinary CreateDefault()
	{
		const int center = ArenaDimensionMax / 2;
		const float shrinkStart = 50;

		Vector2 centerPoint = new(center);

		float[,] arena = new float[ArenaDimensionMax, ArenaDimensionMax];

		for (int i = 0; i < ArenaDimensionMax; i++)
		{
			for (int j = 0; j < ArenaDimensionMax; j++)
			{
				const int tileSize = 4;
				const int halfTile = tileSize / 2;
				const float radius = (shrinkStart - halfTile) / tileSize;
				const float radiusSquared = radius * radius;
				bool inside = Vector2.DistanceSquared(centerPoint, new(i, j)) < radiusSquared;
				arena[i, j] = inside ? 0 : -1000;
			}
		}

		return new(6, 9, shrinkStart, 20, 0.025f, 60, GameMode.Survival, ArenaDimensionMax, arena, default, 500, 250, 120, 60, ImmutableArray<Spawn>.Empty, HandLevel.Level1, 0, 0);
	}

	public SpawnsetBinary DeepCopy()
	{
		Spawn[] spawns = new Spawn[Spawns.Length];
		for (int i = 0; i < Spawns.Length; i++)
			spawns[i] = Spawns[i];

		return new(
			SpawnVersion,
			WorldVersion,
			ShrinkStart,
			ShrinkEnd,
			ShrinkRate,
			Brightness,
			GameMode,
			ArenaDimension,
			ArenaTiles.GetMutableClone(),
			RaceDaggerPosition,
			UnusedDevilTime,
			UnusedGoldenTime,
			UnusedSilverTime,
			UnusedBronzeTime,
			spawns.ToImmutableArray(),
			HandLevel,
			AdditionalGems,
			TimerStart);
	}

	public bool HasSpawns()
	{
		return Spawns.Any(s => s.EnemyType != EnemyType.Empty);
	}

	public bool HasEndLoop()
	{
		if (GameMode != GameMode.Survival || !HasSpawns())
			return false;

		for (int i = GetLoopStartIndex(); i < Spawns.Length; i++)
		{
			if (Spawns[i].EnemyType != EnemyType.Empty)
				return true;
		}

		return false;
	}

	public int GetLoopStartIndex()
	{
		if (GameMode != GameMode.Survival)
			throw new InvalidOperationException("Cannot get loop start index from non-survival spawnset.");

		for (int i = Spawns.Length - 1; i >= 0; i--)
		{
			if (Spawns[i].EnemyType == EnemyType.Empty)
				return i;
		}

		return 0;
	}

	public SpawnsetBinary GetWithHardcodedEndLoop(int amountOfGeneratedWaves)
	{
		if (amountOfGeneratedWaves < 1)
			throw new ArgumentOutOfRangeException(nameof(amountOfGeneratedWaves), "Amount of generated waves must be at least 1.");

		if (!HasEndLoop())
			throw new InvalidOperationException("Spawnset does not have an end loop.");

		// The new spawnset cannot have an actual end loop, so we must remove all empty spawns and add their delay to the next spawn.
		List<Spawn> newSpawns = new();
		for (int i = Spawns.Length - 1; i >= 0; i--)
		{
			if (Spawns[i].EnemyType == EnemyType.Empty)
				continue;

			float extraDelayFromPreviousEmptySpawns = 0;
			if (i > 0)
			{
				int previousIndex = 1;
				while (true)
				{
					Spawn previousSpawn = Spawns[i - previousIndex];
					if (previousSpawn.EnemyType != EnemyType.Empty)
						break;

					extraDelayFromPreviousEmptySpawns += previousSpawn.Delay;
					previousIndex++;
				}
			}

			newSpawns.Insert(0, new(Spawns[i].EnemyType, Spawns[i].Delay + extraDelayFromPreviousEmptySpawns));
		}

		// Add the end loop, including the original last empty spawn.
		// TODO: Check if using doubles is correct.
		double enemyTimer = 0;
		double delay = 0;

		List<Spawn> originalEndLoop = Spawns.Skip(GetLoopStartIndex()).ToList();

		// Start at index 1 because the first wave is already added.
		for (int waveIndex = 1; waveIndex < amountOfGeneratedWaves; waveIndex++)
		{
			foreach (Spawn spawn in originalEndLoop)
			{
				double spawnDelay = 0;
				delay += spawn.Delay;
				while (enemyTimer < delay)
				{
					spawnDelay += 1f / 60f;
					enemyTimer += 1f / 60f + 1f / 60f / 8f * waveIndex;
				}

				newSpawns.Add(new(spawn.EnemyType, (float)spawnDelay));
			}
		}

		// We don't want an actual loop to happen after these hardcoded waves, so we add an empty spawn.
		newSpawns.Add(new(EnemyType.Empty, 0));

		return this with
		{
			Spawns = newSpawns.ToImmutableArray(),
		};
	}

	public EffectivePlayerSettings GetEffectivePlayerSettings()
	{
		if (SpawnVersion < 5)
			return new(HandLevel.Level1, 0, HandLevel.Level1);

		return HandLevel switch
		{
			HandLevel.Level1 when AdditionalGems < 10 => new(HandLevel.Level1, AdditionalGems, HandLevel.Level1),
			HandLevel.Level1 when AdditionalGems < 70 => new(HandLevel.Level2, AdditionalGems, HandLevel.Level2),
			HandLevel.Level1 when AdditionalGems == 70 => new(HandLevel.Level3, 0, HandLevel.Level3),
			HandLevel.Level1 when AdditionalGems == 71 => new(HandLevel.Level4, 0, HandLevel.Level4),
			HandLevel.Level1 => new(HandLevel.Level4, 0, HandLevel.Level3),
			HandLevel.Level2 when AdditionalGems < 0 => new(HandLevel.Level1, AdditionalGems + 10, HandLevel.Level1),
			HandLevel.Level2 => new(HandLevel.Level2, Math.Min(59, AdditionalGems) + 10, HandLevel.Level2),
			HandLevel.Level3 => new(HandLevel.Level3, Math.Min(149, AdditionalGems), HandLevel.Level3),
			_ => new(HandLevel.Level4, AdditionalGems, HandLevel.Level4),
		};
	}

	public float GetEffectiveTimerStart()
	{
		return SpawnVersion < 6 ? 0 : TimerStart;
	}

	public SpawnsetSupportedGameVersion GetSupportedGameVersion()
	{
		if (SpawnVersion >= 5)
			return SpawnsetSupportedGameVersion.V3_1AndLater; // Practice mode (spawn version 5+) is only available in V3.1+.

		if (WorldVersion >= 9)
			return SpawnsetSupportedGameVersion.V2AndLater; // World version 9 was added in V2.

		return SpawnsetSupportedGameVersion.V0AndLater;
	}

	public float? GetRaceDaggerHeight()
	{
		int raceDaggerTileX = WorldToTileCoordinate(RaceDaggerPosition.X);
		int raceDaggerTileZ = WorldToTileCoordinate(RaceDaggerPosition.Y);

		if (raceDaggerTileX >= 0 && raceDaggerTileX < ArenaDimension && raceDaggerTileZ >= 0 && raceDaggerTileZ < ArenaDimension)
			return ArenaTiles[raceDaggerTileX, raceDaggerTileZ];

		return null;
	}

	public int WorldToTileCoordinate(float worldCoordinate)
	{
		int arenaMiddle = ArenaDimension / 2;
		return (int)MathF.Round(worldCoordinate / 4) + arenaMiddle;
	}

	public int TileToWorldCoordinate(float tileCoordinate)
	{
		int arenaMiddle = ArenaDimension / 2;
		return (int)MathF.Round((tileCoordinate - arenaMiddle) * 4);
	}

	public float GetShrinkEndTime()
	{
		float shrinkStart = Math.Max(ShrinkStart, 0);
		float shrinkEnd = Math.Max(ShrinkEnd, 0);

		if (ShrinkRate <= 0 || shrinkEnd > shrinkStart)
			return 0;

		return (shrinkStart - shrinkEnd) / ShrinkRate;
	}

	public float GetShrinkTimeForTile(int x, int y)
	{
		const int tileUnit = 4;
		float shrinkStartInTiles = ShrinkStart / tileUnit;
		float shrinkEndInTiles = ShrinkEnd / tileUnit;

		int arenaMiddle = ArenaDimension / 2;
		Vector2 middle = new(arenaMiddle, arenaMiddle);
		float distance = Vector2.Distance(new(x, y), middle);
		if (distance > Math.Max(shrinkStartInTiles, shrinkEndInTiles))
			return 0;

		if (distance <= shrinkEndInTiles)
			return float.MaxValue;

		// TODO: Prevent this from overflowing.
		return (shrinkStartInTiles - distance) / ShrinkRate * tileUnit;
	}

	public float GetActualTileHeight(int x, int y, float currentTime)
	{
		if (x < 0 || x >= ArenaDimension)
			throw new ArgumentOutOfRangeException(nameof(x));
		if (y < 0 || y >= ArenaDimension)
			throw new ArgumentOutOfRangeException(nameof(y));

		float tileHeight = ArenaTiles[x, y];
		float shrinkTime = GetShrinkTimeForTile(x, y);
		float shrinkHeight = Math.Max(0, currentTime - shrinkTime) / 4;
		return tileHeight - shrinkHeight;
	}

	public float? GetActualRaceDaggerHeight(float currentTime)
	{
		int raceDaggerTileX = WorldToTileCoordinate(RaceDaggerPosition.X);
		int raceDaggerTileZ = WorldToTileCoordinate(RaceDaggerPosition.Y);
		if (raceDaggerTileX >= 0 && raceDaggerTileX < ArenaDimension && raceDaggerTileZ >= 0 && raceDaggerTileZ < ArenaDimension)
			return GetActualTileHeight(raceDaggerTileX, raceDaggerTileZ, currentTime);

		return null;
	}

	public float GetSliderMaxSeconds()
	{
		// Determine the max tile height to add additional time to the slider.
		// For example, when the shrink ends at 200, but there is a tile at height 20, we want to add another 88 seconds ((20 + 2) * 4) to the slider so the shrink transition is always fully visible for all tiles.
		// Add 2 heights to make sure it is still visible until the height is -2 (the palette should still show something until a height of at least -1 or lower).
		// Multiply by 4 because a tile falls by 1 unit every 4 seconds.
		float maxTileHeight = 0;
		for (int i = 0; i < ArenaDimension; i++)
		{
			for (int j = 0; j < ArenaDimension; j++)
			{
				float tileHeight = ArenaTiles[i, j];
				if (maxTileHeight < tileHeight)
					maxTileHeight = tileHeight;
			}
		}

		return GetShrinkEndTime() + (maxTileHeight + 2) * 4;
	}

	public (SpawnSectionInfo PreLoopSection, SpawnSectionInfo LoopSection) CalculateSections()
	{
		return GameMode != GameMode.Survival ? CalculateSectionsForNonDefaultGameMode() : CalculateSectionsForDefaultGameMode();

		(SpawnSectionInfo PreLoopSection, SpawnSectionInfo LoopSection) CalculateSectionsForNonDefaultGameMode()
		{
			int spawnCount = 0;
			float seconds = 0;
			for (int i = 0; i < Spawns.Length; i++)
			{
				Spawn spawn = Spawns[i];

				// If the rest of the spawns are empty, break loop.
				if (Spawns.Skip(i).All(s => s.EnemyType == EnemyType.Empty))
					break;

				seconds += spawn.Delay;
				if (spawn.EnemyType != EnemyType.Empty)
					spawnCount++;
			}

			return (new(spawnCount, spawnCount == 0 ? null : seconds), default);
		}

		(SpawnSectionInfo PreLoopSection, SpawnSectionInfo LoopSection) CalculateSectionsForDefaultGameMode()
		{
			int loopStartIndex = GetLoopStartIndex();

			int preLoopSpawnCount = 0;
			int loopSpawnCount = 0;
			float preLoopSeconds = 0;
			float loopSeconds = 0;
			for (int i = 0; i < Spawns.Length; i++)
			{
				Spawn spawn = Spawns[i];

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

	#endregion Utilities
}
