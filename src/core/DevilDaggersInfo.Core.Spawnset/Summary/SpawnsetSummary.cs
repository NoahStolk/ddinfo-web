using DevilDaggersInfo.Types.Core.Spawnsets;

namespace DevilDaggersInfo.Core.Spawnset.Summary;

public class SpawnsetSummary
{
	public SpawnsetSummary(int spawnVersion, int worldVersion, GameMode gameMode, SpawnSectionInfo preLoopSection, SpawnSectionInfo loopSection, HandLevel handLevel, int additionalGems, float timerStart)
	{
		SpawnVersion = spawnVersion;
		WorldVersion = worldVersion;
		GameMode = gameMode;
		PreLoopSection = preLoopSection;
		LoopSection = loopSection;
		HandLevel = handLevel;
		AdditionalGems = additionalGems;
		TimerStart = timerStart;

		EffectivePlayerSettings = SpawnsetBinary.GetEffectivePlayerSettings(handLevel, additionalGems);
	}

	public SpawnsetSummary(SpawnsetBinary spawnsetBinary)
	{
		SpawnVersion = spawnsetBinary.SpawnVersion;
		WorldVersion = spawnsetBinary.WorldVersion;
		GameMode = spawnsetBinary.GameMode;

		(SpawnSectionInfo preLoopSection, SpawnSectionInfo loopSection) = CalculateSections(spawnsetBinary.Spawns, spawnsetBinary.GameMode);
		PreLoopSection = preLoopSection;
		LoopSection = loopSection;

		HandLevel = spawnsetBinary.HandLevel;
		AdditionalGems = spawnsetBinary.AdditionalGems;
		TimerStart = spawnsetBinary.TimerStart;

		EffectivePlayerSettings = spawnsetBinary.GetEffectivePlayerSettings();
	}

	public int SpawnVersion { get; }
	public int WorldVersion { get; }
	public GameMode GameMode { get; }
	public SpawnSectionInfo PreLoopSection { get; }
	public SpawnSectionInfo LoopSection { get; }
	public HandLevel HandLevel { get; }
	public int AdditionalGems { get; }
	public float TimerStart { get; }

	public EffectivePlayerSettings EffectivePlayerSettings { get; }

	public static bool TryParse(byte[] fileContents, [NotNullWhen(true)] out SpawnsetSummary? spawnsetSummary)
	{
		try
		{
			spawnsetSummary = Parse(fileContents);
			return true;
		}
		catch
		{
			// TODO: Log exceptions.
			spawnsetSummary = null;
			return false;
		}
	}

	// TODO: Throw clear exceptions when parsing fails.
	public static SpawnsetSummary Parse(byte[] fileContents)
	{
		using MemoryStream ms = new(fileContents);
		using BinaryReader br = new(ms);
		br.BaseStream.Position = 0;

		// Header
		int spawnVersion = br.ReadInt32();
		int worldVersion = br.ReadInt32();
		br.Seek(16);
		GameMode gameMode = br.ReadInt32().ToGameMode();
		int arenaDimension = br.ReadInt32();
		br.Seek(4 + arenaDimension * arenaDimension * sizeof(float) + (worldVersion == 8 ? 32 : 36));

		// Spawns header
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

		(SpawnSectionInfo preLoopSection, SpawnSectionInfo loopSection) = CalculateSections(spawns, gameMode);

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

		return new(spawnVersion, worldVersion, gameMode, preLoopSection, loopSection, handLevel, additionalGems, timerStart);
	}

	private static (SpawnSectionInfo PreLoopSection, SpawnSectionInfo LoopSection) CalculateSections(Spawn[] spawns, GameMode gameMode)
		=> gameMode != GameMode.Survival ? CalculateSectionsForNonDefaultGameMode(spawns) : CalculateSectionsForDefaultGameMode(spawns);

	private static (SpawnSectionInfo PreLoopSection, SpawnSectionInfo LoopSection) CalculateSectionsForNonDefaultGameMode(Spawn[] spawns)
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

	private static (SpawnSectionInfo PreLoopSection, SpawnSectionInfo LoopSection) CalculateSectionsForDefaultGameMode(Spawn[] spawns)
	{
		int loopStartIndex = SpawnsetBinary.GetLoopStartIndex(spawns);

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
