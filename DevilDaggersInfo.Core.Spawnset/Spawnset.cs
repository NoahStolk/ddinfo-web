using DevilDaggersInfo.Core.Spawnset.Enums;
using DevilDaggersInfo.Core.Spawnset.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace DevilDaggersInfo.Core.Spawnset
{
	public class Spawnset
	{
		public const int HeaderBufferSize = 36;
		public const int ArenaBufferSize = ArenaWidth * ArenaHeight * sizeof(float);
		public const int SpawnBufferSize = 28;

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

		#region Parsing

		public static bool TryParse(Stream stream, [NotNullWhen(true)] out Spawnset? spawnset)
		{
			try
			{
				spawnset = Parse(stream);
				return true;
			}
			catch
			{
				// TODO: Log exceptions.
				spawnset = null;
				return false;
			}
		}

		// TODO: Throw clear exceptions when parsing fails.
		// TODO: Manually fix incorrect enum values.
		public static Spawnset Parse(Stream stream)
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
			br.Seek(8);

			// Arena
			float[,] arenaTiles = new float[ArenaWidth, ArenaHeight];
			for (int i = 0; i < ArenaWidth * ArenaHeight; i++)
			{
				int x = i % ArenaHeight;
				int y = i / ArenaWidth;
				arenaTiles[x, y] = br.ReadSingle();
			}

			// Spawns header
			br.Seek(worldVersion >= 9 ? 36 : 32);
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

			return new(spawnVersion, worldVersion, shrinkStart, shrinkEnd, shrinkRate, brightness, gameMode, arenaTiles, spawns, handLevel, additionalGems, timerStart);
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
			bw.Seek(12, SeekOrigin.Current);
			bw.Write(0x01);
			bw.Write(WorldVersion == 8 ? (byte)0x90 : (byte)0xF4);
			bw.Write((byte)0x01);
			bw.Seek(2, SeekOrigin.Current);
			bw.Write(0xFA);
			bw.Write(0x78);
			bw.Write(0x3C);
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

		public static Spawnset CreateDefault()
			=> new(6, 9, 50, 20, 0.025f, 60, GameMode.Default, new float[ArenaWidth, ArenaHeight], Array.Empty<Spawn>(), HandLevel.Level1, 0, 0);

		public static bool IsEmptySpawn(int enemyType)
			=> enemyType < 0 || enemyType > 9;

		public bool HasSpawns()
			=> HasSpawns(Spawns);

		public static bool HasSpawns(Spawn[] spawns)
			=> spawns.Any(s => s.EnemyType != EnemyType.Empty);

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

		public int GetInitialGems()
			=> GetInitialGems(AdditionalGems, HandLevel);

		public static int GetInitialGems(int additionalGems, HandLevel handLevel) => additionalGems + handLevel switch
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

		#endregion Utilities
	}
}
