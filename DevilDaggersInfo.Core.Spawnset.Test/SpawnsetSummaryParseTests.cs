using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace DevilDaggersInfo.Core.Spawnset.Test
{
	[TestClass]
	public class SpawnsetSummaryParseTests
	{
		[TestMethod]
		public void ParseSummary_V0()
		{
			using FileStream fs = new(Path.Combine("Data", "V0"), FileMode.Open);
			SpawnsetSummary spawnset = Spawnset.ParseSpawnsetSummary(fs);

			Assert.AreEqual(4, spawnset.SpawnVersion);
			Assert.AreEqual(8, spawnset.WorldVersion);
			Assert.AreEqual(GameMode.Default, spawnset.GameMode);
			Assert.AreEqual(57, spawnset.NonLoopSpawnCount);
			Assert.AreEqual(18, spawnset.LoopSpawnCount);
			Assert.IsNotNull(spawnset.NonLoopLength);
			Assert.IsNotNull(spawnset.LoopLength);
			Assert.AreEqual(275, spawnset.NonLoopLength.Value, 0.001f);
			Assert.AreEqual(30, spawnset.LoopLength.Value, 0.001f);
			Assert.AreEqual(HandLevel.Level1, spawnset.HandLevel);
			Assert.AreEqual(0, spawnset.AdditionalGems);
			Assert.AreEqual(0, spawnset.TimerStart);
		}

		[TestMethod]
		public void ParseSummary_V1()
		{
			using FileStream fs = new(Path.Combine("Data", "V1"), FileMode.Open);
			SpawnsetSummary spawnset = Spawnset.ParseSpawnsetSummary(fs);

			Assert.AreEqual(4, spawnset.SpawnVersion);
			Assert.AreEqual(8, spawnset.WorldVersion);
			Assert.AreEqual(GameMode.Default, spawnset.GameMode);
			Assert.AreEqual(99, spawnset.NonLoopSpawnCount);
			Assert.AreEqual(21, spawnset.LoopSpawnCount);
			Assert.IsNotNull(spawnset.NonLoopLength);
			Assert.IsNotNull(spawnset.LoopLength);
			Assert.AreEqual(421, spawnset.NonLoopLength.Value, 0.001f);
			Assert.AreEqual(54, spawnset.LoopLength.Value, 0.001f);
			Assert.AreEqual(HandLevel.Level1, spawnset.HandLevel);
			Assert.AreEqual(0, spawnset.AdditionalGems);
			Assert.AreEqual(0, spawnset.TimerStart);
		}

		[TestMethod]
		public void ParseSummary_V2()
		{
			using FileStream fs = new(Path.Combine("Data", "V2"), FileMode.Open);
			SpawnsetSummary spawnset = Spawnset.ParseSpawnsetSummary(fs);

			Assert.AreEqual(4, spawnset.SpawnVersion);
			Assert.AreEqual(9, spawnset.WorldVersion);
			Assert.AreEqual(GameMode.Default, spawnset.GameMode);
			Assert.AreEqual(71, spawnset.NonLoopSpawnCount);
			Assert.AreEqual(7, spawnset.LoopSpawnCount);
			Assert.IsNotNull(spawnset.NonLoopLength);
			Assert.IsNotNull(spawnset.LoopLength);
			Assert.AreEqual(375, spawnset.NonLoopLength.Value, 0.001f);
			Assert.AreEqual(58, spawnset.LoopLength.Value, 0.001f);
			Assert.AreEqual(HandLevel.Level1, spawnset.HandLevel);
			Assert.AreEqual(0, spawnset.AdditionalGems);
			Assert.AreEqual(0, spawnset.TimerStart);
		}

		[TestMethod]
		public void ParseSummary_V3()
		{
			using FileStream fs = new(Path.Combine("Data", "V3"), FileMode.Open);
			SpawnsetSummary spawnset = Spawnset.ParseSpawnsetSummary(fs);

			Assert.AreEqual(4, spawnset.SpawnVersion);
			Assert.AreEqual(9, spawnset.WorldVersion);
			Assert.AreEqual(GameMode.Default, spawnset.GameMode);
			Assert.AreEqual(90, spawnset.NonLoopSpawnCount);
			Assert.AreEqual(17, spawnset.LoopSpawnCount);
			Assert.IsNotNull(spawnset.NonLoopLength);
			Assert.IsNotNull(spawnset.LoopLength);
			Assert.AreEqual(451, spawnset.NonLoopLength.Value, 0.001f);
			Assert.AreEqual(56, spawnset.LoopLength.Value, 0.001f);
			Assert.AreEqual(HandLevel.Level1, spawnset.HandLevel);
			Assert.AreEqual(0, spawnset.AdditionalGems);
			Assert.AreEqual(0, spawnset.TimerStart);
		}

		[TestMethod]
		public void ParseSummary_V3_229()
		{
			using FileStream fs = new(Path.Combine("Data", "V3_229"), FileMode.Open);
			SpawnsetSummary spawnset = Spawnset.ParseSpawnsetSummary(fs);

			Assert.AreEqual(6, spawnset.SpawnVersion);
			Assert.AreEqual(9, spawnset.WorldVersion);
			Assert.AreEqual(GameMode.Default, spawnset.GameMode);
			Assert.AreEqual(52, spawnset.NonLoopSpawnCount);
			Assert.AreEqual(17, spawnset.LoopSpawnCount);
			Assert.IsNotNull(spawnset.NonLoopLength);
			Assert.IsNotNull(spawnset.LoopLength);
			Assert.AreEqual(222, spawnset.NonLoopLength.Value, 0.001f);
			Assert.AreEqual(56, spawnset.LoopLength.Value, 0.001f);
			Assert.AreEqual(HandLevel.Level3, spawnset.HandLevel);
			Assert.AreEqual(57, spawnset.AdditionalGems);
			Assert.AreEqual(229, spawnset.TimerStart);
		}

		[TestMethod]
		public void ParseSummary_V3_451()
		{
			using FileStream fs = new(Path.Combine("Data", "V3_451"), FileMode.Open);
			SpawnsetSummary spawnset = Spawnset.ParseSpawnsetSummary(fs);

			Assert.AreEqual(6, spawnset.SpawnVersion);
			Assert.AreEqual(9, spawnset.WorldVersion);
			Assert.AreEqual(GameMode.Default, spawnset.GameMode);
			Assert.AreEqual(0, spawnset.NonLoopSpawnCount);
			Assert.AreEqual(17, spawnset.LoopSpawnCount);
			Assert.IsNull(spawnset.NonLoopLength);
			Assert.IsNotNull(spawnset.LoopLength);
			Assert.AreEqual(56, spawnset.LoopLength.Value, 0.001f);
			Assert.AreEqual(HandLevel.Level4, spawnset.HandLevel);
			Assert.AreEqual(0, spawnset.AdditionalGems);
			Assert.AreEqual(451, spawnset.TimerStart);
		}

		[TestMethod]
		public void ParseSummary_Empty()
		{
			using FileStream fs = new(Path.Combine("Data", "Empty"), FileMode.Open);
			SpawnsetSummary spawnset = Spawnset.ParseSpawnsetSummary(fs);

			Assert.AreEqual(6, spawnset.SpawnVersion);
			Assert.AreEqual(9, spawnset.WorldVersion);
			Assert.AreEqual(GameMode.Default, spawnset.GameMode);
			Assert.AreEqual(0, spawnset.NonLoopSpawnCount);
			Assert.AreEqual(0, spawnset.LoopSpawnCount);
			Assert.IsNull(spawnset.NonLoopLength);
			Assert.IsNull(spawnset.LoopLength);
			Assert.AreEqual(HandLevel.Level1, spawnset.HandLevel);
			Assert.AreEqual(0, spawnset.AdditionalGems);
			Assert.AreEqual(0, spawnset.TimerStart);
		}

		[TestMethod]
		public void ParseSummary_Scanner()
		{
			using FileStream fs = new(Path.Combine("Data", "Scanner"), FileMode.Open);
			SpawnsetSummary spawnset = Spawnset.ParseSpawnsetSummary(fs);

			Assert.AreEqual(6, spawnset.SpawnVersion);
			Assert.AreEqual(9, spawnset.WorldVersion);
			Assert.AreEqual(GameMode.Default, spawnset.GameMode);
			Assert.AreEqual(62, spawnset.NonLoopSpawnCount);
			Assert.AreEqual(62, spawnset.LoopSpawnCount);
			Assert.IsNotNull(spawnset.NonLoopLength);
			Assert.IsNotNull(spawnset.LoopLength);
			Assert.AreEqual(16, spawnset.NonLoopLength.Value, 0.001f);
			Assert.AreEqual(21, spawnset.LoopLength.Value, 0.001f);
			Assert.AreEqual(HandLevel.Level4, spawnset.HandLevel);
			Assert.AreEqual(30, spawnset.AdditionalGems);
			Assert.AreEqual(0, spawnset.TimerStart);
		}
	}
}
