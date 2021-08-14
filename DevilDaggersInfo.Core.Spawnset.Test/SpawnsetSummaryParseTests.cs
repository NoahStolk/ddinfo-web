using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace DevilDaggersInfo.Core.Spawnset.Test
{
	[TestClass]
	public class SpawnsetSummaryParseTests
	{
		[TestMethod]
		public void ParseV0Summary()
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
		public void ParseV1Summary()
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
		public void ParseV2Summary()
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
		public void ParseV3Summary()
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
	}
}
