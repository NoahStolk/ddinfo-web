using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace DevilDaggersInfo.Core.Spawnset
{
	// TODO: Log exceptions.
	// TODO: Add unit tests.
	public class Spawnset
	{
		public const int HeaderBufferSize = 36;
		public const int ArenaBufferSize = 10404; // ArenaWidth * ArenaHeight * TileBufferSize (51 * 51 * 4 = 10404)
		public const int SpawnBufferSize = 28; // The amount of bytes per spawn

		public const int ArenaWidth = 51;
		public const int ArenaHeight = 51;

		public Spawnset(
			int spawnVersion,
			int worldVersion,
			float shrinkStart,
			float shrinkEnd,
			float shrinkRate,
			float brightness,
			GameMode gameMode,
			float[,] arenaTiles,
			Spawn[] spawns,
			HandLevel handLevel,
			int additionalGems,
			float timerStart)
		{
			if (arenaTiles.GetLength(0) != ArenaWidth || arenaTiles.GetLength(1) != ArenaHeight)
				throw new ArgumentOutOfRangeException(nameof(arenaTiles), $"Arena array must be {ArenaWidth} by {ArenaHeight}.");

			SpawnVersion = spawnVersion;
			WorldVersion = worldVersion;
			ShrinkStart = shrinkStart;
			ShrinkEnd = shrinkEnd;
			ShrinkRate = shrinkRate;
			Brightness = brightness;
			GameMode = gameMode;
			ArenaTiles = arenaTiles;
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

		public float[,] ArenaTiles { get; }
		public Spawn[] Spawns { get; }

		public HandLevel HandLevel { get; }
		public int AdditionalGems { get; }
		public float TimerStart { get; }

		#region Utilities

		public static Spawnset CreateDefault()
			=> new(6, 9, 50, 20, 0.025f, 60, GameMode.Default, new float[ArenaWidth, ArenaHeight], Array.Empty<Spawn>(), HandLevel.Level1, 0, 0);

		public static int GetSpawnsHeaderBufferSize(int worldVersion) => worldVersion switch
		{
			8 => 36,
			_ => 40,
		};

		public static int GetSettingsBufferSize(int spawnVersion) => spawnVersion switch
		{
			5 => 5,
			6 => 9,
			_ => 0,
		};

		public static bool HasSpawns(Spawn[] spawns)
			=> spawns.Any(s => s.EnemyType != EnemyType.Empty);

		public static int GetEndLoopStartIndex(Spawn[] spawns)
		{
			for (int i = spawns.Length - 1; i >= 0; i--)
			{
				if (spawns[i].EnemyType == EnemyType.Empty)
					return i;
			}

			return 0;
		}

		public static IEnumerable<double> GenerateEndWaveTimes(Spawn[] spawns, double endGameSecond, int waveIndex)
		{
			double enemyTimer = 0;
			double delay = 0;

			foreach (Spawn spawn in spawns.Skip(GetEndLoopStartIndex(spawns)))
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

		public static bool IsEmptySpawn(int enemyType)
			=> enemyType < 0 || enemyType > 9;

		public int GetInitialGems() => AdditionalGems + HandLevel switch
		{
			HandLevel.Level2 => 10,
			HandLevel.Level3 => 70,
			HandLevel.Level4 => 220,
			_ => 0,
		};

		public string GetGameVersionString()
			=> GetGameVersionString(WorldVersion, SpawnVersion);

		public static string GetGameVersionString(int worldVersion, int spawnVersion)
			=> worldVersion == 8 ? "V0 / V1" : spawnVersion == 4 ? "V2 / V3" : "V3.1";

		public (double LoopLength, double EndLoopSpawns) GetEndLoopData()
		{
			double loopLength = 0;
			int endLoopSpawns = 0;
			for (int i = Spawns.Length - 1; i >= 0; i--)
			{
				loopLength += Spawns[i].Delay;
				if (Spawns[i].EnemyType == EnemyType.Empty || i == 0)
					break;

				endLoopSpawns++;
			}

			if (!Spawns.Any(s => s.EnemyType == EnemyType.Empty) && Spawns.Length > 0)
				endLoopSpawns++;

			return (loopLength, endLoopSpawns);
		}

		#endregion Utilities

		#region Parsing

		// TODO: Throw clear exceptions when parsing fails, remove try-catch and rename method to Parse. Write extra method TryParse which is a wrapper around this.
		// TODO: Seek instead of setting BaseStream.Position.
		public static bool TryParse(Stream stream, [NotNullWhen(true)] out Spawnset? spawnset)
		{
			try
			{
				using BinaryReader br = new(stream);
				br.BaseStream.Position = 0;

				// Header
				int spawnVersion = br.ReadInt32();
				int worldVersion = br.ReadInt32();
				float shrinkEnd = br.ReadSingle();
				float shrinkStart = br.ReadSingle();
				float shrinkRate = br.ReadSingle();
				float brightness = br.ReadSingle();
				GameMode gameMode = (GameMode)br.ReadInt32();

				// Arena
				br.BaseStream.Position = HeaderBufferSize;
				float[,] arenaTiles = new float[ArenaWidth, ArenaHeight];
				for (int i = 0; i < ArenaWidth * ArenaHeight; i++)
				{
					int x = i % ArenaHeight;
					int y = i / ArenaWidth;
					arenaTiles[x, y] = br.ReadSingle();
				}

				// Spawns header
				int spawnsHeaderBufferSize = GetSpawnsHeaderBufferSize(worldVersion);
				br.BaseStream.Position = HeaderBufferSize + ArenaBufferSize + spawnsHeaderBufferSize - sizeof(int);
				int spawnCount = br.ReadInt32();

				// Spawns
				Spawn[] spawns = new Spawn[spawnCount];
				for (int i = 0; i < spawnCount; i++)
				{
					EnemyType enemyType = (EnemyType)br.ReadInt32();
					float delay = br.ReadSingle();
					spawns[i] = new(enemyType, delay);

					br.BaseStream.Seek(20, SeekOrigin.Current);
				}

				// Settings
				int settingsBufferSize = GetSettingsBufferSize(spawnVersion);
				HandLevel handLevel = HandLevel.Level1;
				int additionalGems = 0;
				float timerStart = 0;
				if (settingsBufferSize >= 5)
				{
					handLevel = (HandLevel)br.ReadByte();
					additionalGems = br.ReadInt32();

					if (settingsBufferSize >= 9)
						timerStart = br.ReadSingle();
				}

				spawnset = new(spawnVersion, worldVersion, shrinkStart, shrinkEnd, shrinkRate, brightness, gameMode, arenaTiles, spawns, handLevel, additionalGems, timerStart);

				return true;
			}
			catch
			{
				spawnset = null;

				return false;
			}
		}

		public static bool TryGetSpawnsetData(Stream stream, [NotNullWhen(true)] out SpawnsetSummary? spawnsetSummary)
		{
			try
			{
				using BinaryReader br = new(stream);
				br.BaseStream.Position = 0;

				// Header
				int spawnVersion = br.ReadInt32();
				int worldVersion = br.ReadInt32();
				br.BaseStream.Seek(16, SeekOrigin.Current);
				GameMode gameMode = (GameMode)br.ReadInt32();

				// Spawns header
				int spawnsHeaderBufferSize = GetSpawnsHeaderBufferSize(worldVersion);
				br.BaseStream.Position = HeaderBufferSize + ArenaBufferSize + spawnsHeaderBufferSize - sizeof(int);
				int spawnCount = br.ReadInt32();

				// Spawns
				Spawn[] spawns = new Spawn[spawnCount];
				for (int i = 0; i < spawnCount; i++)
				{
					EnemyType enemyType = (EnemyType)br.ReadInt32();
					float delay = br.ReadSingle();
					spawns[i] = new(enemyType, delay);

					br.BaseStream.Seek(20, SeekOrigin.Current);
				}

				int endLoopStartIndex = GetEndLoopStartIndex(spawns);

				int nonLoopSpawns = 0;
				int loopSpawns = 0;
				float nonLoopSeconds = 0;
				float loopSeconds = 0;
				for (int i = 0; i < spawns.Length; i++)
				{
					Spawn spawn = spawns[i];

					if (i < endLoopStartIndex)
						nonLoopSeconds += spawn.Delay; // TODO: Trim EMPTY spawns at the end (test with Delirium_Stop)
					else
						loopSeconds += spawn.Delay;

					if (spawn.EnemyType != EnemyType.Empty)
					{
						if (i < endLoopStartIndex)
							nonLoopSpawns++;
						else
							loopSpawns++;
					}
				}

				// Settings
				int settingsBufferSize = GetSettingsBufferSize(spawnVersion);
				HandLevel handLevel = HandLevel.Level1;
				int additionalGems = 0;
				float timerStart = 0;
				if (settingsBufferSize >= 5)
				{
					handLevel = (HandLevel)br.ReadByte();
					additionalGems = br.ReadInt32();

					if (settingsBufferSize >= 9)
						timerStart = br.ReadSingle();
				}

				spawnsetSummary = new(spawnVersion, worldVersion, gameMode, nonLoopSpawns, loopSpawns, nonLoopSpawns == 0 ? null : nonLoopSeconds, loopSpawns == 0 ? null : loopSeconds, handLevel, additionalGems, timerStart);

				return true;
			}
			catch
			{
				spawnsetSummary = null;

				return false;
			}
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
			bw.Write(0x33);
			bw.Write(0x01);

			// Arena
			for (int i = 0; i < ArenaWidth * ArenaHeight; i++)
			{
				int x = i % ArenaHeight;
				int y = i / ArenaWidth;

				bw.Write(ArenaTiles[x, y]);
			}

			// Spawns header
			bw.BaseStream.Seek(12, SeekOrigin.Current);
			bw.Write(0x01);
			bw.Write(WorldVersion == 8 ? (byte)0x90 : (byte)0xF4);
			bw.Write((byte)0x01);
			bw.BaseStream.Seek(2, SeekOrigin.Current);
			bw.Write(0xFA);
			bw.Write(0x78);
			bw.Write(0x3C);
			bw.BaseStream.Seek(WorldVersion == 8 ? 8 : 12, SeekOrigin.Current);

			// Spawns
			foreach (Spawn spawn in Spawns)
			{
				bw.Write((int)spawn.EnemyType);
				bw.Write(spawn.Delay);
				bw.BaseStream.Seek(1, SeekOrigin.Current);
				bw.Write(0x03);
				bw.BaseStream.Seek(6, SeekOrigin.Current);
				bw.Write((byte)0xF0);
				bw.Write((byte)0x41);
				bw.Write((byte)0x0A);
				bw.BaseStream.Seek(4, SeekOrigin.Current);
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
	}
}
