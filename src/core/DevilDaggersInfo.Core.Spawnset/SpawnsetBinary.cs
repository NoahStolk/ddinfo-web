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

	public static bool IsEmptySpawn(int enemyType)
		=> enemyType is < 0 or > 9;

	public bool HasSpawns()
		=> HasSpawns(Spawns);

	public static bool HasSpawns(ImmutableArray<Spawn> spawns)
		=> spawns.Any(s => s.EnemyType != EnemyType.Empty);

	public bool HasEndLoop()
		=> HasEndLoop(Spawns, GameMode);

	public static bool HasEndLoop(ImmutableArray<Spawn> spawns, GameMode gameMode)
	{
		if (gameMode != GameMode.Survival || !HasSpawns(spawns))
			return false;

		int loopStartIndex = GetLoopStartIndex(spawns);
		Spawn[] loopSpawns = spawns.Skip(loopStartIndex).ToArray();

		return loopSpawns.Any(s => s.EnemyType != EnemyType.Empty);
	}

	public int GetLoopStartIndex()
		=> GetLoopStartIndex(Spawns);

	public static int GetLoopStartIndex(ImmutableArray<Spawn> spawns)
	{
		for (int i = spawns.Length - 1; i >= 0; i--)
		{
			if (spawns[i].EnemyType == EnemyType.Empty)
				return i;
		}

		return 0;
	}

	public IEnumerable<double> GenerateEndWaveTimes(double endGameSecond, int waveIndex)
		=> GenerateEndWaveTimes(Spawns, endGameSecond, waveIndex);

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

	public EffectivePlayerSettings GetEffectivePlayerSettings()
		=> GetEffectivePlayerSettings(SpawnVersion, HandLevel, AdditionalGems);

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

	public float GetEffectiveTimerStart()
		=> GetEffectiveTimerStart(SpawnVersion, TimerStart);

	public static float GetEffectiveTimerStart(int spawnVersion, float timerStart)
		=> spawnVersion < 6 ? 0 : timerStart;

	public string GetGameVersionString()
		=> GetGameVersionString(WorldVersion, SpawnVersion);

	// TODO: Add enum for recognized spawnset format versions.
	public static string GetGameVersionString(int worldVersion, int spawnVersion)
		=> worldVersion == 8 ? "V0/V1" : spawnVersion == 4 ? "V2/V3" : "V3.1+";

	public (int X, float? Y, int Z) GetRaceDaggerTilePosition()
		=> GetRaceDaggerTilePosition(ArenaDimension, ArenaTiles, RaceDaggerPosition);

	// TODO: Remove the Y component from this method. Use GetRaceDaggerHeight or GetActualRaceDaggerHeight instead.
	public static (int X, float? Y, int Z) GetRaceDaggerTilePosition(int arenaDimension, ImmutableArena arenaTiles, Vector2 raceDaggerPosition)
	{
		int x = WorldToTileCoordinate(arenaDimension, raceDaggerPosition.X);
		int z = WorldToTileCoordinate(arenaDimension, raceDaggerPosition.Y);
		float? y = GetRaceDaggerHeight(arenaDimension, arenaTiles, x, z);
		return (x, y, z);
	}

	public float? GetRaceDaggerHeight()
	{
		int raceDaggerTileX = WorldToTileCoordinate(ArenaDimension, RaceDaggerPosition.X);
		int raceDaggerTileZ = WorldToTileCoordinate(ArenaDimension, RaceDaggerPosition.Y);
		return GetRaceDaggerHeight(ArenaDimension, ArenaTiles, raceDaggerTileX, raceDaggerTileZ);
	}

	public static float? GetRaceDaggerHeight(int arenaDimension, ImmutableArena arenaTiles, int raceDaggerTileX, int raceDaggerTileZ)
	{
		if (raceDaggerTileX >= 0 && raceDaggerTileX < arenaDimension && raceDaggerTileZ >= 0 && raceDaggerTileZ < arenaDimension)
			return arenaTiles[raceDaggerTileX, raceDaggerTileZ];

		return null;
	}

	public int WorldToTileCoordinate(float worldCoordinate)
		=> WorldToTileCoordinate(ArenaDimension, worldCoordinate);

	public static int WorldToTileCoordinate(int arenaDimension, float worldCoordinate)
	{
		int arenaMiddle = arenaDimension / 2;
		return (int)MathF.Round(worldCoordinate / 4) + arenaMiddle;
	}

	public int TileToWorldCoordinate(float tileCoordinate)
		=> TileToWorldCoordinate(ArenaDimension, tileCoordinate);

	public static int TileToWorldCoordinate(int arenaDimension, float tileCoordinate)
	{
		int arenaMiddle = arenaDimension / 2;
		return (int)MathF.Round((tileCoordinate - arenaMiddle) * 4);
	}

	public float GetShrinkEndTime()
		=> GetShrinkEndTime(ShrinkStart, ShrinkEnd, ShrinkRate);

	public static float GetShrinkEndTime(float shrinkStart, float shrinkEnd, float shrinkRate)
	{
		shrinkStart = Math.Max(shrinkStart, 0);
		shrinkEnd = Math.Max(shrinkEnd, 0);

		if (shrinkRate <= 0 || shrinkEnd > shrinkStart)
			return 0;

		return (shrinkStart - shrinkEnd) / shrinkRate;
	}

	public float GetShrinkTimeForTile(int x, int y)
		=> GetShrinkTimeForTile(ArenaDimension, ShrinkStart, ShrinkEnd, ShrinkRate, x, y);

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

	public float GetActualTileHeight(int x, int y, float currentTime)
		=> GetActualTileHeight(ArenaDimension, ArenaTiles, ShrinkStart, ShrinkEnd, ShrinkRate, x, y, currentTime);

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

	public float? GetActualRaceDaggerHeight(float currentTime)
	{
		int raceDaggerTileX = WorldToTileCoordinate(ArenaDimension, RaceDaggerPosition.X);
		int raceDaggerTileZ = WorldToTileCoordinate(ArenaDimension, RaceDaggerPosition.Y);
		return GetActualRaceDaggerHeight(ArenaDimension, ArenaTiles, ShrinkStart, ShrinkEnd, ShrinkRate, raceDaggerTileX, raceDaggerTileZ, currentTime);
	}

	public static float? GetActualRaceDaggerHeight(int arenaDimension, ImmutableArena arenaTiles, float shrinkStart, float shrinkEnd, float shrinkRate, int raceDaggerTileX, int raceDaggerTileZ, float currentTime)
	{
		if (raceDaggerTileX >= 0 && raceDaggerTileX < arenaDimension && raceDaggerTileZ >= 0 && raceDaggerTileZ < arenaDimension)
			return GetActualTileHeight(arenaDimension, arenaTiles, shrinkStart, shrinkEnd, shrinkRate, raceDaggerTileX, raceDaggerTileZ, currentTime);

		return null;
	}

	public float GetSliderMaxSeconds()
		=> GetSliderMaxSeconds(ArenaDimension, ArenaTiles, ShrinkStart, ShrinkEnd, ShrinkRate);

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

	public (SpawnSectionInfo PreLoopSection, SpawnSectionInfo LoopSection) CalculateSections()
		=> CalculateSections(Spawns, GameMode);

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

	#endregion Utilities
}
