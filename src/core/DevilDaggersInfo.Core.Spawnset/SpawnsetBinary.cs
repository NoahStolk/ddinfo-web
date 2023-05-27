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
		return SpawnsetUtils.HasSpawns(Spawns);
	}

	public bool HasEndLoop()
	{
		return SpawnsetUtils.HasEndLoop(Spawns, GameMode);
	}

	public int GetLoopStartIndex()
	{
		return SpawnsetUtils.GetLoopStartIndex(Spawns);
	}

	public IEnumerable<double> GenerateEndWaveTimes(double endGameSecond, int waveIndex)
	{
		return SpawnsetUtils.GenerateEndWaveTimes(Spawns, endGameSecond, waveIndex);
	}

	public EffectivePlayerSettings GetEffectivePlayerSettings()
	{
		return SpawnsetUtils.GetEffectivePlayerSettings(SpawnVersion, HandLevel, AdditionalGems);
	}

	public float GetEffectiveTimerStart()
	{
		return SpawnsetUtils.GetEffectiveTimerStart(SpawnVersion, TimerStart);
	}

	public string GetGameVersionString()
	{
		return SpawnsetUtils.GetGameVersionString(WorldVersion, SpawnVersion);
	}

	public (int X, float? Y, int Z) GetRaceDaggerTilePosition()
	{
		return SpawnsetUtils.GetRaceDaggerTilePosition(ArenaDimension, ArenaTiles, RaceDaggerPosition);
	}

	public float? GetRaceDaggerHeight()
	{
		int raceDaggerTileX = SpawnsetUtils.WorldToTileCoordinate(ArenaDimension, RaceDaggerPosition.X);
		int raceDaggerTileZ = SpawnsetUtils.WorldToTileCoordinate(ArenaDimension, RaceDaggerPosition.Y);
		return SpawnsetUtils.GetRaceDaggerHeight(ArenaDimension, ArenaTiles, raceDaggerTileX, raceDaggerTileZ);
	}

	public int WorldToTileCoordinate(float worldCoordinate)
	{
		return SpawnsetUtils.WorldToTileCoordinate(ArenaDimension, worldCoordinate);
	}

	public int TileToWorldCoordinate(float tileCoordinate)
	{
		return SpawnsetUtils.TileToWorldCoordinate(ArenaDimension, tileCoordinate);
	}

	public float GetShrinkEndTime()
	{
		return SpawnsetUtils.GetShrinkEndTime(ShrinkStart, ShrinkEnd, ShrinkRate);
	}

	public float GetShrinkTimeForTile(int x, int y)
	{
		return SpawnsetUtils.GetShrinkTimeForTile(ArenaDimension, ShrinkStart, ShrinkEnd, ShrinkRate, x, y);
	}

	public float GetActualTileHeight(int x, int y, float currentTime)
	{
		return SpawnsetUtils.GetActualTileHeight(ArenaDimension, ArenaTiles, ShrinkStart, ShrinkEnd, ShrinkRate, x, y, currentTime);
	}

	public float? GetActualRaceDaggerHeight(float currentTime)
	{
		int raceDaggerTileX = SpawnsetUtils.WorldToTileCoordinate(ArenaDimension, RaceDaggerPosition.X);
		int raceDaggerTileZ = SpawnsetUtils.WorldToTileCoordinate(ArenaDimension, RaceDaggerPosition.Y);
		return SpawnsetUtils.GetActualRaceDaggerHeight(ArenaDimension, ArenaTiles, ShrinkStart, ShrinkEnd, ShrinkRate, raceDaggerTileX, raceDaggerTileZ, currentTime);
	}

	public float GetSliderMaxSeconds()
	{
		return SpawnsetUtils.GetSliderMaxSeconds(ArenaDimension, ArenaTiles, ShrinkStart, ShrinkEnd, ShrinkRate);
	}

	public (SpawnSectionInfo PreLoopSection, SpawnSectionInfo LoopSection) CalculateSections()
	{
		return SpawnsetUtils.CalculateSections(Spawns, GameMode);
	}

	#endregion Utilities
}
