using DevilDaggersInfo.Types.Core.Spawnsets;
using System.Numerics;

namespace DevilDaggersInfo.Core.Spawnset;

public class SpawnsetBinary
{
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
		Spawn[] spawns,
		HandLevel handLevel,
		int additionalGems,
		float timerStart)
	{
		if (arenaTiles.GetLength(0) != arenaDimension || arenaTiles.GetLength(1) != arenaDimension)
			throw new ArgumentOutOfRangeException(nameof(arenaTiles), $"Arena array must be {arenaDimension} by {arenaDimension}.");

		SpawnVersion = spawnVersion;
		WorldVersion = worldVersion;
		ShrinkStart = shrinkStart;
		ShrinkEnd = shrinkEnd;
		ShrinkRate = shrinkRate;
		Brightness = brightness;
		GameMode = gameMode;
		ArenaDimension = arenaDimension;
		ArenaTiles = arenaTiles;

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

	public int SpawnVersion { get; }
	public int WorldVersion { get; }
	public float ShrinkStart { get; }
	public float ShrinkEnd { get; }
	public float ShrinkRate { get; }
	public float Brightness { get; }
	public GameMode GameMode { get; }
	public int ArenaDimension { get; }
	public float[,] ArenaTiles { get; }

	public Vector2 RaceDaggerPosition { get; }
	public int UnusedDevilTime { get; }
	public int UnusedGoldenTime { get; }
	public int UnusedSilverTime { get; }
	public int UnusedBronzeTime { get; }

	public Spawn[] Spawns { get; }

	public HandLevel HandLevel { get; }
	public int AdditionalGems { get; }
	public float TimerStart { get; }

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
		int worldVersion = br.ReadInt32();
		float shrinkEnd = br.ReadSingle();
		float shrinkStart = br.ReadSingle();
		float shrinkRate = br.ReadSingle();
		float brightness = br.ReadSingle();
		GameMode gameMode = br.ReadInt32().ToGameMode();
		int arenaDimension = br.ReadInt32();
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
			spawns,
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
		const int arenaSize = 51;
		const int center = arenaSize / 2;
		const float shrinkStart = 50;

		Vector2 centerPoint = new(center);

		float[,] arena = new float[arenaSize, arenaSize];

		for (int i = 0; i < arenaSize; i++)
		{
			for (int j = 0; j < arenaSize; j++)
			{
				const int tileSize = 4;
				const int halfTile = tileSize / 2;
				const float radius = (shrinkStart - halfTile) / tileSize;
				const float radiusSquared = radius * radius;
				bool inside = Vector2.DistanceSquared(centerPoint, new Vector2(i, j)) < radiusSquared;
				arena[i, j] = inside ? 0 : -1000;
			}
		}

		return new(6, 9, shrinkStart, 20, 0.025f, 60, GameMode.Survival, arenaSize, arena, default, 500, 250, 120, 60, Array.Empty<Spawn>(), HandLevel.Level1, 0, 0);
	}

	public static bool IsEmptySpawn(int enemyType)
		=> enemyType is < 0 or > 9;

	public bool HasSpawns()
		=> HasSpawns(Spawns);

	public static bool HasSpawns(Spawn[] spawns)
		=> spawns.Any(s => s.EnemyType != EnemyType.Empty);

	public bool HasEndLoop()
		=> HasEndLoop(Spawns, GameMode);

	public static bool HasEndLoop(Spawn[] spawns, GameMode gameMode)
	{
		if (gameMode != GameMode.Survival || !HasSpawns(spawns))
			return false;

		int loopStartIndex = GetLoopStartIndex(spawns);
		Spawn[] loopSpawns = spawns.Skip(loopStartIndex).ToArray();

		return loopSpawns.Any(s => s.EnemyType != EnemyType.Empty);
	}

	public int GetLoopStartIndex()
		=> GetLoopStartIndex(Spawns);

	public static int GetLoopStartIndex(Spawn[] spawns)
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

	public static IEnumerable<double> GenerateEndWaveTimes(Spawn[] spawns, double endGameSecond, int waveIndex)
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
		=> GetEffectivePlayerSettings(HandLevel, AdditionalGems);

	public static EffectivePlayerSettings GetEffectivePlayerSettings(HandLevel handLevel, int additionalGems)
	{
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
			_ => new(HandLevel.Level4, additionalGems, HandLevel.Level4)
		};
	}

	public string GetGameVersionString()
		=> GetGameVersionString(WorldVersion, SpawnVersion);

	public static string GetGameVersionString(int worldVersion, int spawnVersion)
		=> worldVersion == 8 ? "V0 / V1" : spawnVersion == 4 ? "V2 / V3" : "V3.1 / V3.2";

	public (int X, float? Y, int Z) GetRaceDaggerTilePosition()
		=> GetRaceDaggerTilePosition(ArenaDimension, ArenaTiles, RaceDaggerPosition);

	public static (int X, float? Y, int Z) GetRaceDaggerTilePosition(float arenaDimension, float[,] arenaTiles, Vector2 raceDaggerPosition)
	{
		int x = Convert(raceDaggerPosition.X);
		int z = Convert(raceDaggerPosition.Y);
		float? y = null;
		if (x >= 0 && x < arenaDimension && z >= 0 && z < arenaDimension)
			y = arenaTiles[x, z];

		return (x, y, z);

		static int Convert(float worldPosition) => (int)MathF.Round(worldPosition / 4) + 25;
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

		Vector2 middle = new(arenaDimension / 2, arenaDimension / 2);
		float distance = Vector2.Distance(new(x, y), middle);
		if (distance > Math.Max(shrinkStartInTiles, shrinkEndInTiles))
			return 0;

		if (distance <= shrinkEndInTiles)
			return float.MaxValue;

		return (shrinkStartInTiles - distance) / shrinkRate * tileUnit;
	}

	public float GetActualTileHeight(int x, int y, float currentTime)
		=> GetActualTileHeight(ArenaDimension, ArenaTiles, ShrinkStart, ShrinkEnd, ShrinkRate, x, y, currentTime);

	public static float GetActualTileHeight(int arenaDimension, float[,] arenaTiles, float shrinkStart, float shrinkEnd, float shrinkRate, int x, int y, float currentTime)
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

	#endregion Utilities
}
