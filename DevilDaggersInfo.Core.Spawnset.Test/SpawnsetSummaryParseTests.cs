using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace DevilDaggersInfo.Core.Spawnset.Test
{
	[TestClass]
	public class SpawnsetSummaryParseTests
	{
		[TestMethod]
		public void ParseSummary_V0()
			=> ParseSummary("V0", 4, 8, GameMode.Default, 57, 18, 275, 30, HandLevel.Level1, 0, 0);

		[TestMethod]
		public void ParseSummary_V1()
			=> ParseSummary("V1", 4, 8, GameMode.Default, 99, 21, 421, 54, HandLevel.Level1, 0, 0);

		[TestMethod]
		public void ParseSummary_V2()
			=> ParseSummary("V2", 4, 9, GameMode.Default, 71, 7, 375, 58, HandLevel.Level1, 0, 0);

		[TestMethod]
		public void ParseSummary_V3()
			=> ParseSummary("V3", 4, 9, GameMode.Default, 90, 17, 451, 56, HandLevel.Level1, 0, 0);

		[TestMethod]
		public void ParseSummary_V3_229()
			=> ParseSummary("V3_229", 6, 9, GameMode.Default, 52, 17, 222, 56, HandLevel.Level3, 57, 229);

		[TestMethod]
		public void ParseSummary_V3_451()
			=> ParseSummary("V3_451", 6, 9, GameMode.Default, 0, 17, null, 56, HandLevel.Level4, 0, 451);

		[TestMethod]
		public void ParseSummary_Empty()
			=> ParseSummary("Empty", 6, 9, GameMode.Default, 0, 0, null, null, HandLevel.Level1, 0, 0);

		[TestMethod]
		public void ParseSummary_Scanner()
			=> ParseSummary("Scanner", 6, 9, GameMode.Default, 62, 62, 16, 21, HandLevel.Level4, 30, 0);

		[AssertionMethod]
		private static SpawnsetSummary ParseSummary(string fileName, int expectedSpawnVersion, int expectedWorldVersion, GameMode expectedGameMode, int expectedNonLoopSpawnCount, int expectedLoopSpawnCount, float? expectedNonLoopLength, float? expectedLoopLength, HandLevel expectedHandLevel, int expectedAdditionalGems, float expectedTimerStart)
		{
			using FileStream fs = new(Path.Combine("Data", fileName), FileMode.Open);
			SpawnsetSummary spawnsetSummary = Spawnset.ParseSpawnsetSummary(fs);

			Assert.AreEqual(expectedSpawnVersion, spawnsetSummary.SpawnVersion);
			Assert.AreEqual(expectedWorldVersion, spawnsetSummary.WorldVersion);
			Assert.AreEqual(expectedGameMode, spawnsetSummary.GameMode);

			Assert.AreEqual(expectedNonLoopSpawnCount, spawnsetSummary.NonLoopSpawnCount);
			Assert.AreEqual(expectedLoopSpawnCount, spawnsetSummary.LoopSpawnCount);

			if (expectedNonLoopLength.HasValue)
			{
				Assert.IsNotNull(spawnsetSummary.NonLoopLength);
				Assert.AreEqual(expectedNonLoopLength.Value, spawnsetSummary.NonLoopLength.Value, 0.001f);
			}
			else
			{
				Assert.IsNull(spawnsetSummary.NonLoopLength);
			}

			if (expectedLoopLength.HasValue)
			{
				Assert.IsNotNull(spawnsetSummary.LoopLength);
				Assert.AreEqual(expectedLoopLength.Value, spawnsetSummary.LoopLength.Value, 0.001f);
			}
			else
			{
				Assert.IsNull(spawnsetSummary.LoopLength);
			}

			Assert.AreEqual(expectedHandLevel, spawnsetSummary.HandLevel);
			Assert.AreEqual(expectedAdditionalGems, spawnsetSummary.AdditionalGems);
			Assert.AreEqual(expectedTimerStart, spawnsetSummary.TimerStart);

			return spawnsetSummary;
		}
	}
}
