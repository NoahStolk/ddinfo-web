using System.Numerics;

namespace DevilDaggersInfo.Core.Spawnset.Tests;

// TODO: Refactor and add separate tests for calculating sections. Probably need to store all the expected spawnset data somewhere else.
[TestClass]
public class SpawnsetParseTests
{
	[TestMethod]
	public void Parse_V0()
	{
		SpawnsetBinary spawnset = Parse("V0", 4, 8, 50, 20, 0.025f, 60, GameMode.Survival, default, 400, 250, 120, 60, 82, HandLevel.Level1, 0, 0, new(57, 275), new(18, 30));
		Assert.AreEqual(new(EnemyType.Squid1, 3), spawnset.Spawns[0]);
		Assert.AreEqual(new(EnemyType.Empty, 6), spawnset.Spawns[1]);
	}

	[TestMethod]
	public void Parse_V1()
	{
		SpawnsetBinary spawnset = Parse("V1", 4, 8, 50, 20, 0.025f, 60, GameMode.Survival, default, 400, 250, 120, 60, 130, HandLevel.Level1, 0, 0, new(99, 421), new(21, 54));
		Assert.AreEqual(new(EnemyType.Squid1, 3), spawnset.Spawns[0]);
		Assert.AreEqual(new(EnemyType.Empty, 6), spawnset.Spawns[1]);
	}

	[TestMethod]
	public void Parse_V2()
	{
		SpawnsetBinary spawnset = Parse("V2", 4, 9, 50, 20, 0.025f, 60, GameMode.Survival, default, 500, 250, 120, 60, 87, HandLevel.Level1, 0, 0, new(71, 375), new(7, 58));
		Assert.AreEqual(new(EnemyType.Squid1, 3), spawnset.Spawns[0]);
		Assert.AreEqual(new(EnemyType.Empty, 6), spawnset.Spawns[1]);
	}

	[TestMethod]
	public void Parse_V3()
	{
		SpawnsetBinary spawnset = Parse("V3", 4, 9, 50, 20, 0.025f, 60, GameMode.Survival, default, 500, 250, 120, 60, 118, HandLevel.Level1, 0, 0, new(90, 451), new(17, 56));
		Assert.AreEqual(new(EnemyType.Squid1, 3), spawnset.Spawns[0]);
		Assert.AreEqual(new(EnemyType.Empty, 6), spawnset.Spawns[1]);
	}

	[TestMethod]
	public void Parse_V3_229()
	{
		SpawnsetBinary spawnset = Parse("V3_229", 6, 9, 44.275f, 20, 0.025f, 60, GameMode.Survival, default, 500, 250, 120, 60, 75, HandLevel.Level3, 57, 229, new(52, 222), new(17, 56));
		Assert.AreEqual(new(EnemyType.Squid1, 0), spawnset.Spawns[0]);
		Assert.AreEqual(new(EnemyType.Squid2, 10), spawnset.Spawns[6]);
	}

	[TestMethod]
	public void Parse_V3_451()
	{
		SpawnsetBinary spawnset = Parse("V3_451", 6, 9, 38.725f, 20, 0.025f, 60, GameMode.Survival, default, 500, 250, 120, 60, 18, HandLevel.Level4, 0, 451, new(0, null), new(17, 56));
		Assert.AreEqual(new(EnemyType.Empty, 5), spawnset.Spawns[0]);
	}

	[TestMethod]
	public void Parse_Empty()
	{
		Parse("Empty", 6, 9, 50, 20, 0.025f, 60, GameMode.Survival, default, 500, 250, 120, 60, 0, HandLevel.Level1, 0, 0, new(0, null), new(0, null));
	}

	[TestMethod]
	public void Parse_Scanner()
	{
		SpawnsetBinary spawnset = Parse("Scanner", 6, 9, 26, 15, 0.025f, 60, GameMode.Survival, default, 500, 250, 120, 60, 125, HandLevel.Level4, 30, 0, new(62, 16), new(62, 21));
		Assert.AreEqual(new(EnemyType.Squid2, 0), spawnset.Spawns[0]);
		Assert.AreEqual(new(EnemyType.Spider1, 5), spawnset.Spawns[30]);
	}

	[TestMethod]
	public void Parse_EmptySpawn()
	{
		Parse("EmptySpawn", 6, 9, 50, 20, 0.025f, 60, GameMode.Survival, default, 500, 250, 120, 60, 1, HandLevel.Level1, 0, 0, new(0, null), new(0, null));
	}

	[TestMethod]
	public void Parse_NoEndLoop()
	{
		Parse("NoEndLoop", 6, 9, 50, 20, 0.025f, 60, GameMode.Survival, default, 500, 250, 120, 60, 3, HandLevel.Level1, 0, 0, new(2, 2), new(0, null));
	}

	[TestMethod]
	public void Parse_TimeAttack()
	{
		Parse("TimeAttack", 6, 9, 50, 20, 0.025f, 60, GameMode.TimeAttack, default, 500, 250, 120, 60, 1, HandLevel.Level1, 0, 0, new(1, 1), new(0, null));
	}

	[TestMethod]
	public void Parse_Metathrone()
	{
		Parse("Metathrone", 5, 9, 6105.9f, 27, 11.5f, 180, GameMode.Survival, default, 500, 250, 120, 60, 164, HandLevel.Level1, 0, 0, new(134, 691.6f), new(9, 42.4f));
	}

	[AssertionMethod]
	private static SpawnsetBinary Parse(
		string fileName,
		int expectedSpawnVersion,
		int expectedWorldVersion,
		float expectedShrinkStart,
		float expectedShrinkEnd,
		float expectedShrinkRate,
		float expectedBrightness,
		GameMode expectedGameMode,
		Vector2 expectedRaceDaggerPosition,
		int expectedUnusedDevilTime,
		int expectedUnusedGoldenTime,
		int expectedUnusedSilverTime,
		int expectedUnusedBronzeTime,
		int expectedSpawnCount,
		HandLevel expectedHandLevel,
		int expectedAdditionalGems,
		float expectedTimerStart,
		SpawnSectionInfo expectedPreLoopSection,
		SpawnSectionInfo expectedLoopSection)
	{
		SpawnsetBinary spawnset = SpawnsetBinary.Parse(File.ReadAllBytes(Path.Combine("Resources", fileName)));

		Assert.AreEqual(expectedSpawnVersion, spawnset.SpawnVersion);
		Assert.AreEqual(expectedWorldVersion, spawnset.WorldVersion);
		Assert.AreEqual(expectedShrinkStart, spawnset.ShrinkStart, 0.001f);
		Assert.AreEqual(expectedShrinkEnd, spawnset.ShrinkEnd, 0.001f);
		Assert.AreEqual(expectedShrinkRate, spawnset.ShrinkRate, 0.001f);
		Assert.AreEqual(expectedBrightness, spawnset.Brightness, 0.001f);
		Assert.AreEqual(expectedGameMode, spawnset.GameMode);

		Assert.AreEqual(expectedRaceDaggerPosition, spawnset.RaceDaggerPosition);
		Assert.AreEqual(expectedUnusedDevilTime, spawnset.UnusedDevilTime);
		Assert.AreEqual(expectedUnusedGoldenTime, spawnset.UnusedGoldenTime);
		Assert.AreEqual(expectedUnusedSilverTime, spawnset.UnusedSilverTime);
		Assert.AreEqual(expectedUnusedBronzeTime, spawnset.UnusedBronzeTime);
		Assert.AreEqual(expectedSpawnCount, spawnset.Spawns.Length);

		Assert.AreEqual(expectedHandLevel, spawnset.HandLevel);
		Assert.AreEqual(expectedAdditionalGems, spawnset.AdditionalGems);
		Assert.AreEqual(expectedTimerStart, spawnset.TimerStart);

		(SpawnSectionInfo PreLoopSection, SpawnSectionInfo LoopSection) sections = spawnset.CalculateSections();
		Assert.AreEqual(expectedPreLoopSection, sections.PreLoopSection);
		Assert.AreEqual(expectedLoopSection, sections.LoopSection);

		return spawnset;
	}
}
