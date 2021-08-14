using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace DevilDaggersInfo.Core.Spawnset.Test
{
	[TestClass]
	public class SpawnsetParseTests
	{
		[TestMethod]
		public void ParseV0()
		{
			using FileStream fs = new(Path.Combine("Data", "V0"), FileMode.Open);
			Spawnset spawnset = Spawnset.ParseSpawnset(fs);

			Assert.AreEqual(4, spawnset.SpawnVersion);
			Assert.AreEqual(8, spawnset.WorldVersion);
			Assert.AreEqual(50, spawnset.ShrinkStart, 0.001f);
			Assert.AreEqual(20, spawnset.ShrinkEnd, 0.001f);
			Assert.AreEqual(0.025f, spawnset.ShrinkRate, 0.001f);
			Assert.AreEqual(60, spawnset.Brightness, 0.001f);
			Assert.AreEqual(GameMode.Default, spawnset.GameMode);

			Assert.AreEqual(82, spawnset.Spawns.Length);

			Assert.AreEqual(new(EnemyType.Squid1, 3), spawnset.Spawns[0]);
			Assert.AreEqual(new(EnemyType.Empty, 6), spawnset.Spawns[1]);
		}

		[TestMethod]
		public void ParseV1()
		{
			using FileStream fs = new(Path.Combine("Data", "V1"), FileMode.Open);
			Spawnset spawnset = Spawnset.ParseSpawnset(fs);

			Assert.AreEqual(4, spawnset.SpawnVersion);
			Assert.AreEqual(8, spawnset.WorldVersion);
			Assert.AreEqual(50, spawnset.ShrinkStart, 0.001f);
			Assert.AreEqual(20, spawnset.ShrinkEnd, 0.001f);
			Assert.AreEqual(0.025f, spawnset.ShrinkRate, 0.001f);
			Assert.AreEqual(60, spawnset.Brightness, 0.001f);
			Assert.AreEqual(GameMode.Default, spawnset.GameMode);

			Assert.AreEqual(130, spawnset.Spawns.Length);

			Assert.AreEqual(new(EnemyType.Squid1, 3), spawnset.Spawns[0]);
			Assert.AreEqual(new(EnemyType.Empty, 6), spawnset.Spawns[1]);
		}

		[TestMethod]
		public void ParseV2()
		{
			using FileStream fs = new(Path.Combine("Data", "V2"), FileMode.Open);
			Spawnset spawnset = Spawnset.ParseSpawnset(fs);

			Assert.AreEqual(4, spawnset.SpawnVersion);
			Assert.AreEqual(9, spawnset.WorldVersion);
			Assert.AreEqual(50, spawnset.ShrinkStart, 0.001f);
			Assert.AreEqual(20, spawnset.ShrinkEnd, 0.001f);
			Assert.AreEqual(0.025f, spawnset.ShrinkRate, 0.001f);
			Assert.AreEqual(60, spawnset.Brightness, 0.001f);
			Assert.AreEqual(GameMode.Default, spawnset.GameMode);

			Assert.AreEqual(87, spawnset.Spawns.Length);

			Assert.AreEqual(new(EnemyType.Squid1, 3), spawnset.Spawns[0]);
			Assert.AreEqual(new(EnemyType.Empty, 6), spawnset.Spawns[1]);
		}

		[TestMethod]
		public void ParseV3()
		{
			using FileStream fs = new(Path.Combine("Data", "V3"), FileMode.Open);
			Spawnset spawnset = Spawnset.ParseSpawnset(fs);

			Assert.AreEqual(4, spawnset.SpawnVersion);
			Assert.AreEqual(9, spawnset.WorldVersion);
			Assert.AreEqual(50, spawnset.ShrinkStart, 0.001f);
			Assert.AreEqual(20, spawnset.ShrinkEnd, 0.001f);
			Assert.AreEqual(0.025f, spawnset.ShrinkRate, 0.001f);
			Assert.AreEqual(60, spawnset.Brightness, 0.001f);
			Assert.AreEqual(GameMode.Default, spawnset.GameMode);

			Assert.AreEqual(118, spawnset.Spawns.Length);

			Assert.AreEqual(new(EnemyType.Squid1, 3), spawnset.Spawns[0]);
			Assert.AreEqual(new(EnemyType.Empty, 6), spawnset.Spawns[1]);

			Assert.AreEqual(HandLevel.Level1, spawnset.HandLevel);
			Assert.AreEqual(0, spawnset.AdditionalGems);
			Assert.AreEqual(0, spawnset.TimerStart);
		}

		[TestMethod]
		public void ParseV3_229()
		{
			using FileStream fs = new(Path.Combine("Data", "V3_229"), FileMode.Open);
			Spawnset spawnset = Spawnset.ParseSpawnset(fs);

			Assert.AreEqual(6, spawnset.SpawnVersion);
			Assert.AreEqual(9, spawnset.WorldVersion);
			Assert.AreEqual(44.275f, spawnset.ShrinkStart, 0.001f);
			Assert.AreEqual(20, spawnset.ShrinkEnd, 0.001f);
			Assert.AreEqual(0.025f, spawnset.ShrinkRate, 0.001f);
			Assert.AreEqual(60, spawnset.Brightness, 0.001f);
			Assert.AreEqual(GameMode.Default, spawnset.GameMode);

			Assert.AreEqual(75, spawnset.Spawns.Length);

			Assert.AreEqual(new(EnemyType.Squid1, 0), spawnset.Spawns[0]);
			Assert.AreEqual(new(EnemyType.Squid2, 10), spawnset.Spawns[6]);

			Assert.AreEqual(HandLevel.Level3, spawnset.HandLevel);
			Assert.AreEqual(57, spawnset.AdditionalGems);
			Assert.AreEqual(229, spawnset.TimerStart);
		}

		[TestMethod]
		public void ParseV3_451()
		{
			using FileStream fs = new(Path.Combine("Data", "V3_451"), FileMode.Open);
			Spawnset spawnset = Spawnset.ParseSpawnset(fs);

			Assert.AreEqual(6, spawnset.SpawnVersion);
			Assert.AreEqual(9, spawnset.WorldVersion);
			Assert.AreEqual(38.725f, spawnset.ShrinkStart, 0.001f);
			Assert.AreEqual(20, spawnset.ShrinkEnd, 0.001f);
			Assert.AreEqual(0.025f, spawnset.ShrinkRate, 0.001f);
			Assert.AreEqual(60, spawnset.Brightness, 0.001f);
			Assert.AreEqual(GameMode.Default, spawnset.GameMode);

			Assert.AreEqual(18, spawnset.Spawns.Length);

			Assert.AreEqual(new(EnemyType.Empty, 5), spawnset.Spawns[0]);

			Assert.AreEqual(HandLevel.Level4, spawnset.HandLevel);
			Assert.AreEqual(0, spawnset.AdditionalGems);
			Assert.AreEqual(451, spawnset.TimerStart);
		}

		[TestMethod]
		public void ParseEmpty()
		{
			using FileStream fs = new(Path.Combine("Data", "Empty"), FileMode.Open);
			Spawnset spawnset = Spawnset.ParseSpawnset(fs);

			Assert.AreEqual(6, spawnset.SpawnVersion);
			Assert.AreEqual(9, spawnset.WorldVersion);
			Assert.AreEqual(50, spawnset.ShrinkStart, 0.001f);
			Assert.AreEqual(20, spawnset.ShrinkEnd, 0.001f);
			Assert.AreEqual(0.025f, spawnset.ShrinkRate, 0.001f);
			Assert.AreEqual(60, spawnset.Brightness, 0.001f);
			Assert.AreEqual(GameMode.Default, spawnset.GameMode);

			Assert.AreEqual(0, spawnset.Spawns.Length);

			Assert.AreEqual(HandLevel.Level1, spawnset.HandLevel);
			Assert.AreEqual(0, spawnset.AdditionalGems);
			Assert.AreEqual(0, spawnset.TimerStart);
		}

		[TestMethod]
		public void ParseScanner()
		{
			using FileStream fs = new(Path.Combine("Data", "Scanner"), FileMode.Open);
			Spawnset spawnset = Spawnset.ParseSpawnset(fs);

			Assert.AreEqual(6, spawnset.SpawnVersion);
			Assert.AreEqual(9, spawnset.WorldVersion);
			Assert.AreEqual(26, spawnset.ShrinkStart, 0.001f);
			Assert.AreEqual(15, spawnset.ShrinkEnd, 0.001f);
			Assert.AreEqual(0.025f, spawnset.ShrinkRate, 0.001f);
			Assert.AreEqual(60, spawnset.Brightness, 0.001f);
			Assert.AreEqual(GameMode.Default, spawnset.GameMode);

			Assert.AreEqual(125, spawnset.Spawns.Length);

			Assert.AreEqual(HandLevel.Level4, spawnset.HandLevel);
			Assert.AreEqual(30, spawnset.AdditionalGems);
			Assert.AreEqual(0, spawnset.TimerStart);
		}
	}
}
