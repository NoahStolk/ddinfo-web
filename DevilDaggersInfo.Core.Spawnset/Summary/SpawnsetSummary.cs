using DevilDaggersInfo.Core.Spawnset.Enums;
using DevilDaggersInfo.Core.Spawnset.Extensions;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace DevilDaggersInfo.Core.Spawnset.Summary
{
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
		}

		public SpawnsetSummary(Spawnset spawnset)
		{
			SpawnVersion = spawnset.SpawnVersion;
			WorldVersion = spawnset.WorldVersion;
			GameMode = spawnset.GameMode;

			(SpawnSectionInfo preLoopSection, SpawnSectionInfo loopSection) = CalculateSections(spawnset.Spawns);
			PreLoopSection = preLoopSection;
			LoopSection = loopSection;

			HandLevel = spawnset.HandLevel;
			AdditionalGems = spawnset.AdditionalGems;
			TimerStart = spawnset.TimerStart;
		}

		public int SpawnVersion { get; }
		public int WorldVersion { get; }
		public GameMode GameMode { get; }
		public SpawnSectionInfo PreLoopSection { get; }
		public SpawnSectionInfo LoopSection { get; }
		public HandLevel HandLevel { get; }
		public int AdditionalGems { get; }
		public float TimerStart { get; }

		public static bool TryParse(Stream stream, [NotNullWhen(true)] out SpawnsetSummary? spawnsetSummary)
		{
			try
			{
				spawnsetSummary = Parse(stream);
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
		public static SpawnsetSummary Parse(Stream stream)
		{
			using BinaryReader br = new(stream);
			br.BaseStream.Position = 0;

			// Header
			int spawnVersion = br.ReadInt32();
			int worldVersion = br.ReadInt32();
			br.Seek(16);
			GameMode gameMode = (GameMode)br.ReadInt32();
			br.Seek(8 + Spawnset.ArenaBufferSize + (worldVersion == 8 ? 32 : 36));

			// Spawns header
			int spawnCount = br.ReadInt32();

			// Spawns
			Spawn[] spawns = new Spawn[spawnCount];
			for (int i = 0; i < spawnCount; i++)
			{
				EnemyType enemyType = (EnemyType)br.ReadInt32();
				float delay = br.ReadSingle();
				spawns[i] = new(enemyType, delay);

				br.Seek(20);
			}

			(SpawnSectionInfo preLoopSection, SpawnSectionInfo loopSection) = CalculateSections(spawns);

			// Settings
			HandLevel handLevel = HandLevel.Level1;
			int additionalGems = 0;
			float timerStart = 0;
			if (spawnVersion >= 5)
			{
				handLevel = (HandLevel)br.ReadByte();
				additionalGems = br.ReadInt32();

				if (spawnVersion >= 6)
					timerStart = br.ReadSingle();
			}

			return new(spawnVersion, worldVersion, gameMode, preLoopSection, loopSection, handLevel, additionalGems, timerStart);
		}

		private static (SpawnSectionInfo PreLoopSection, SpawnSectionInfo LoopSection) CalculateSections(Spawn[] spawns)
		{
			int loopStartIndex = Spawnset.GetLoopStartIndex(spawns);

			int preLoopSpawns = 0;
			int loopSpawns = 0;
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
						preLoopSpawns++;
					else
						loopSpawns++;
				}
			}

			return (new(preLoopSpawns, preLoopSpawns == 0 ? null : preLoopSeconds), new(loopSpawns, loopSpawns == 0 ? null : loopSeconds));
		}
	}
}
