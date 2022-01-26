namespace DevilDaggersInfo.Test;

public static class TestUtils
{
	public const string ResourcePath = "Resources";

	[AssertionMethod]
	public static void AssertArrayContentsEqual<T>(T[] expected, T[] actual)
	{
		Assert.AreEqual(expected.Length, actual.Length);
		for (int i = 0; i < expected.Length; i++)
			Assert.AreEqual(expected[i], actual[i]);
	}
}
