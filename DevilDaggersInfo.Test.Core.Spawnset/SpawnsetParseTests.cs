namespace DevilDaggersInfo.Test.Core.Spawnset;

[TestClass]
public class SpawnsetParseTests
{
	[TestMethod]
	public void Parse_V0()
	{
		SpawnsetBinary spawnset = Parse("V0", 4, 8, 50, 20, 0.025f, 60, GameMode.Default, 82, HandLevel.Level1, 0, 0);
		Assert.AreEqual(new(EnemyType.Squid1, 3), spawnset.Spawns[0]);
		Assert.AreEqual(new(EnemyType.Empty, 6), spawnset.Spawns[1]);
	}

	[TestMethod]
	public void Parse_V1()
	{
		SpawnsetBinary spawnset = Parse("V1", 4, 8, 50, 20, 0.025f, 60, GameMode.Default, 130, HandLevel.Level1, 0, 0);
		Assert.AreEqual(new(EnemyType.Squid1, 3), spawnset.Spawns[0]);
		Assert.AreEqual(new(EnemyType.Empty, 6), spawnset.Spawns[1]);
	}

	[TestMethod]
	public void Parse_V2()
	{
		SpawnsetBinary spawnset = Parse("V2", 4, 9, 50, 20, 0.025f, 60, GameMode.Default, 87, HandLevel.Level1, 0, 0);
		Assert.AreEqual(new(EnemyType.Squid1, 3), spawnset.Spawns[0]);
		Assert.AreEqual(new(EnemyType.Empty, 6), spawnset.Spawns[1]);
	}

	[TestMethod]
	public void Parse_V3()
	{
		SpawnsetBinary spawnset = Parse("V3", 4, 9, 50, 20, 0.025f, 60, GameMode.Default, 118, HandLevel.Level1, 0, 0);
		Assert.AreEqual(new(EnemyType.Squid1, 3), spawnset.Spawns[0]);
		Assert.AreEqual(new(EnemyType.Empty, 6), spawnset.Spawns[1]);
	}

	[TestMethod]
	public void Parse_V3_229()
	{
		SpawnsetBinary spawnset = Parse("V3_229", 6, 9, 44.275f, 20, 0.025f, 60, GameMode.Default, 75, HandLevel.Level3, 57, 229);
		Assert.AreEqual(new(EnemyType.Squid1, 0), spawnset.Spawns[0]);
		Assert.AreEqual(new(EnemyType.Squid2, 10), spawnset.Spawns[6]);
	}

	[TestMethod]
	public void Parse_V3_451()
	{
		SpawnsetBinary spawnset = Parse("V3_451", 6, 9, 38.725f, 20, 0.025f, 60, GameMode.Default, 18, HandLevel.Level4, 0, 451);
		Assert.AreEqual(new(EnemyType.Empty, 5), spawnset.Spawns[0]);
	}

	[TestMethod]
	public void Parse_Empty()
	{
		Parse("Empty", 6, 9, 50, 20, 0.025f, 60, GameMode.Default, 0, HandLevel.Level1, 0, 0);
	}

	[TestMethod]
	public void Parse_Scanner()
	{
		SpawnsetBinary spawnset = Parse("Scanner", 6, 9, 26, 15, 0.025f, 60, GameMode.Default, 125, HandLevel.Level4, 30, 0);
		Assert.AreEqual(new(EnemyType.Squid2, 0), spawnset.Spawns[0]);
		Assert.AreEqual(new(EnemyType.Spider1, 5), spawnset.Spawns[30]);
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
		int expectedSpawnCount,
		HandLevel expectedHandLevel,
		int expectedAdditionalGems,
		float expectedTimerStart)
	{
		SpawnsetBinary spawnset = SpawnsetBinary.Parse(File.ReadAllBytes(Path.Combine(TestUtils.ResourcePath, fileName)));

		Assert.AreEqual(expectedSpawnVersion, spawnset.SpawnVersion);
		Assert.AreEqual(expectedWorldVersion, spawnset.WorldVersion);
		Assert.AreEqual(expectedShrinkStart, spawnset.ShrinkStart, 0.001f);
		Assert.AreEqual(expectedShrinkEnd, spawnset.ShrinkEnd, 0.001f);
		Assert.AreEqual(expectedShrinkRate, spawnset.ShrinkRate, 0.001f);
		Assert.AreEqual(expectedBrightness, spawnset.Brightness, 0.001f);
		Assert.AreEqual(expectedGameMode, spawnset.GameMode);

		Assert.AreEqual(expectedSpawnCount, spawnset.Spawns.Length);

		Assert.AreEqual(expectedHandLevel, spawnset.HandLevel);
		Assert.AreEqual(expectedAdditionalGems, spawnset.AdditionalGems);
		Assert.AreEqual(expectedTimerStart, spawnset.TimerStart);

		return spawnset;
	}
}
