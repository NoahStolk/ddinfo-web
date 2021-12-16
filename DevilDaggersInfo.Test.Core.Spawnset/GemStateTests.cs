namespace DevilDaggersInfo.Test.Core.Spawnset;

[TestClass]
public class GemStateTests
{
	[TestMethod]
	public void TestGemStateAdd()
	{
		GemState gemState = new(HandLevel.Level1, 0, 0);
		gemState = gemState.Add(5);
		AssertGemState(gemState, HandLevel.Level1, 5);

		gemState = gemState.Add(5);
		AssertGemState(gemState, HandLevel.Level2, 10);

		gemState = gemState.Add(55);
		AssertGemState(gemState, HandLevel.Level2, 65);

		gemState = gemState.Add(10);
		AssertGemState(gemState, HandLevel.Level3, 5);

		gemState = gemState.Add(200);
		AssertGemState(gemState, HandLevel.Level4, 55);

		gemState = new(HandLevel.Level1, 0, 0);
		gemState = gemState.Add(75);
		AssertGemState(gemState, HandLevel.Level3, 5);
	}

	[AssertionMethod]
	private static void AssertGemState(GemState gemState, HandLevel expectedHandLevel, int expectedValue)
	{
		Assert.AreEqual(expectedHandLevel, gemState.HandLevel);
		Assert.AreEqual(expectedValue, gemState.Value);
	}
}
