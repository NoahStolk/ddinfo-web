namespace DevilDaggersInfo.Core.Spawnset.Test;

[TestClass]
public class SpawnsetSummaryParseTests
{
	[TestMethod]
	public void ParseSummary_V0()
		=> ParseSummary("V0", 4, 8, GameMode.Default, new(57, 275), new(18, 30), HandLevel.Level1, 0, 0);

	[TestMethod]
	public void ParseSummary_V1()
		=> ParseSummary("V1", 4, 8, GameMode.Default, new(99, 421), new(21, 54), HandLevel.Level1, 0, 0);

	[TestMethod]
	public void ParseSummary_V2()
		=> ParseSummary("V2", 4, 9, GameMode.Default, new(71, 375), new(7, 58), HandLevel.Level1, 0, 0);

	[TestMethod]
	public void ParseSummary_V3()
		=> ParseSummary("V3", 4, 9, GameMode.Default, new(90, 451), new(17, 56), HandLevel.Level1, 0, 0);

	[TestMethod]
	public void ParseSummary_V3_229()
		=> ParseSummary("V3_229", 6, 9, GameMode.Default, new(52, 222), new(17, 56), HandLevel.Level3, 57, 229);

	[TestMethod]
	public void ParseSummary_V3_451()
		=> ParseSummary("V3_451", 6, 9, GameMode.Default, new(0, null), new(17, 56), HandLevel.Level4, 0, 451);

	[TestMethod]
	public void ParseSummary_Empty()
		=> ParseSummary("Empty", 6, 9, GameMode.Default, new(0, null), new(0, null), HandLevel.Level1, 0, 0);

	[TestMethod]
	public void ParseSummary_Scanner()
		=> ParseSummary("Scanner", 6, 9, GameMode.Default, new(62, 16), new(62, 21), HandLevel.Level4, 30, 0);

	[AssertionMethod]
	private static SpawnsetSummary ParseSummary(
		string fileName,
		int expectedSpawnVersion,
		int expectedWorldVersion,
		GameMode expectedGameMode,
		SpawnSectionInfo expectedPreLoopSection,
		SpawnSectionInfo expectedLoopSection,
		HandLevel expectedHandLevel,
		int expectedAdditionalGems,
		float expectedTimerStart)
	{
		using FileStream fs = new(Path.Combine("Data", fileName), FileMode.Open);
		SpawnsetSummary spawnsetSummary = SpawnsetSummary.Parse(fs);

		Assert.AreEqual(expectedSpawnVersion, spawnsetSummary.SpawnVersion);
		Assert.AreEqual(expectedWorldVersion, spawnsetSummary.WorldVersion);
		Assert.AreEqual(expectedGameMode, spawnsetSummary.GameMode);

		Assert.AreEqual(expectedPreLoopSection, spawnsetSummary.PreLoopSection);
		Assert.AreEqual(expectedLoopSection, spawnsetSummary.LoopSection);

		Assert.AreEqual(expectedHandLevel, spawnsetSummary.HandLevel);
		Assert.AreEqual(expectedAdditionalGems, spawnsetSummary.AdditionalGems);
		Assert.AreEqual(expectedTimerStart, spawnsetSummary.TimerStart);

		return spawnsetSummary;
	}
}
