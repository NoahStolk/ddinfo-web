namespace DevilDaggersInfo.Core.Wiki.Test;

[TestClass]
public class DaggerTests
{
	[TestMethod]
	public void TestLeviathanDagger()
	{
		const int seconds = 1000;

		Assert.AreEqual(Daggers.Leviathan, Daggers.GetDaggerFromSeconds(GameVersion.V3_2, seconds));
		Assert.AreEqual(Daggers.Leviathan, Daggers.GetDaggerFromSeconds(GameVersion.V3_1, seconds));
	}

	[TestMethod]
	public void TestDevilDagger()
	{
		const double secondsLastV3Next = 999.9999;
		const int secondsLastV3 = 1000;
		const int secondsFirst = 500;

		Assert.AreEqual(Daggers.Devil, Daggers.GetDaggerFromSeconds(GameVersion.V3_2, secondsLastV3Next));
		Assert.AreEqual(Daggers.Devil, Daggers.GetDaggerFromSeconds(GameVersion.V3_2, secondsFirst));
		Assert.AreEqual(Daggers.Devil, Daggers.GetDaggerFromSeconds(GameVersion.V3_1, secondsLastV3Next));
		Assert.AreEqual(Daggers.Devil, Daggers.GetDaggerFromSeconds(GameVersion.V3_1, secondsFirst));
		Assert.AreEqual(Daggers.Devil, Daggers.GetDaggerFromSeconds(GameVersion.V3_0, secondsLastV3));
		Assert.AreEqual(Daggers.Devil, Daggers.GetDaggerFromSeconds(GameVersion.V3_0, secondsFirst));
		Assert.AreEqual(Daggers.Devil, Daggers.GetDaggerFromSeconds(GameVersion.V2_0, secondsLastV3));
		Assert.AreEqual(Daggers.Devil, Daggers.GetDaggerFromSeconds(GameVersion.V2_0, secondsFirst));
		Assert.AreEqual(Daggers.Devil, Daggers.GetDaggerFromSeconds(GameVersion.V1_0, secondsLastV3));
		Assert.AreEqual(Daggers.Devil, Daggers.GetDaggerFromSeconds(GameVersion.V1_0, secondsFirst));
	}

	[TestMethod]
	public void TestGoldenDagger()
	{
		const double secondsLast = 499.9999;
		const int secondsFirst = 250;

		Assert.AreEqual(Daggers.Golden, Daggers.GetDaggerFromSeconds(GameVersion.V3_2, secondsLast));
		Assert.AreEqual(Daggers.Golden, Daggers.GetDaggerFromSeconds(GameVersion.V3_2, secondsFirst));
		Assert.AreEqual(Daggers.Golden, Daggers.GetDaggerFromSeconds(GameVersion.V3_1, secondsLast));
		Assert.AreEqual(Daggers.Golden, Daggers.GetDaggerFromSeconds(GameVersion.V3_1, secondsFirst));
		Assert.AreEqual(Daggers.Golden, Daggers.GetDaggerFromSeconds(GameVersion.V3_0, secondsLast));
		Assert.AreEqual(Daggers.Golden, Daggers.GetDaggerFromSeconds(GameVersion.V3_0, secondsFirst));
		Assert.AreEqual(Daggers.Golden, Daggers.GetDaggerFromSeconds(GameVersion.V2_0, secondsLast));
		Assert.AreEqual(Daggers.Golden, Daggers.GetDaggerFromSeconds(GameVersion.V2_0, secondsFirst));
		Assert.AreEqual(Daggers.Golden, Daggers.GetDaggerFromSeconds(GameVersion.V1_0, secondsLast));
		Assert.AreEqual(Daggers.Golden, Daggers.GetDaggerFromSeconds(GameVersion.V1_0, secondsFirst));
	}

	[TestMethod]
	public void TestSilverDagger()
	{
		const double secondsLast = 249.9999;
		const int secondsFirst = 120;

		Assert.AreEqual(Daggers.Silver, Daggers.GetDaggerFromSeconds(GameVersion.V3_2, secondsLast));
		Assert.AreEqual(Daggers.Silver, Daggers.GetDaggerFromSeconds(GameVersion.V3_2, secondsFirst));
		Assert.AreEqual(Daggers.Silver, Daggers.GetDaggerFromSeconds(GameVersion.V3_1, secondsLast));
		Assert.AreEqual(Daggers.Silver, Daggers.GetDaggerFromSeconds(GameVersion.V3_1, secondsFirst));
		Assert.AreEqual(Daggers.Silver, Daggers.GetDaggerFromSeconds(GameVersion.V3_0, secondsLast));
		Assert.AreEqual(Daggers.Silver, Daggers.GetDaggerFromSeconds(GameVersion.V3_0, secondsFirst));
		Assert.AreEqual(Daggers.Silver, Daggers.GetDaggerFromSeconds(GameVersion.V2_0, secondsLast));
		Assert.AreEqual(Daggers.Silver, Daggers.GetDaggerFromSeconds(GameVersion.V2_0, secondsFirst));
		Assert.AreEqual(Daggers.Silver, Daggers.GetDaggerFromSeconds(GameVersion.V1_0, secondsLast));
		Assert.AreEqual(Daggers.Silver, Daggers.GetDaggerFromSeconds(GameVersion.V1_0, secondsFirst));
	}

	[TestMethod]
	public void TestBronzeDagger()
	{
		const double secondsLast = 119.9999;
		const int secondsFirst = 60;

		Assert.AreEqual(Daggers.Bronze, Daggers.GetDaggerFromSeconds(GameVersion.V3_2, secondsLast));
		Assert.AreEqual(Daggers.Bronze, Daggers.GetDaggerFromSeconds(GameVersion.V3_2, secondsFirst));
		Assert.AreEqual(Daggers.Bronze, Daggers.GetDaggerFromSeconds(GameVersion.V3_1, secondsLast));
		Assert.AreEqual(Daggers.Bronze, Daggers.GetDaggerFromSeconds(GameVersion.V3_1, secondsFirst));
		Assert.AreEqual(Daggers.Bronze, Daggers.GetDaggerFromSeconds(GameVersion.V3_0, secondsLast));
		Assert.AreEqual(Daggers.Bronze, Daggers.GetDaggerFromSeconds(GameVersion.V3_0, secondsFirst));
		Assert.AreEqual(Daggers.Bronze, Daggers.GetDaggerFromSeconds(GameVersion.V2_0, secondsLast));
		Assert.AreEqual(Daggers.Bronze, Daggers.GetDaggerFromSeconds(GameVersion.V2_0, secondsFirst));
		Assert.AreEqual(Daggers.Bronze, Daggers.GetDaggerFromSeconds(GameVersion.V1_0, secondsLast));
		Assert.AreEqual(Daggers.Bronze, Daggers.GetDaggerFromSeconds(GameVersion.V1_0, secondsFirst));
	}

	[TestMethod]
	public void TestDefaultDagger()
	{
		const double secondsLast = 59.9999;
		const int secondsFirst = 0;

		Assert.AreEqual(Daggers.Default, Daggers.GetDaggerFromSeconds(GameVersion.V3_2, secondsLast));
		Assert.AreEqual(Daggers.Default, Daggers.GetDaggerFromSeconds(GameVersion.V3_2, secondsFirst));
		Assert.AreEqual(Daggers.Default, Daggers.GetDaggerFromSeconds(GameVersion.V3_1, secondsLast));
		Assert.AreEqual(Daggers.Default, Daggers.GetDaggerFromSeconds(GameVersion.V3_1, secondsFirst));
		Assert.AreEqual(Daggers.Default, Daggers.GetDaggerFromSeconds(GameVersion.V3_0, secondsLast));
		Assert.AreEqual(Daggers.Default, Daggers.GetDaggerFromSeconds(GameVersion.V3_0, secondsFirst));
		Assert.AreEqual(Daggers.Default, Daggers.GetDaggerFromSeconds(GameVersion.V2_0, secondsLast));
		Assert.AreEqual(Daggers.Default, Daggers.GetDaggerFromSeconds(GameVersion.V2_0, secondsFirst));
		Assert.AreEqual(Daggers.Default, Daggers.GetDaggerFromSeconds(GameVersion.V1_0, secondsLast));
		Assert.AreEqual(Daggers.Default, Daggers.GetDaggerFromSeconds(GameVersion.V1_0, secondsFirst));
	}

	[TestMethod]
	public void TestOutOfRange()
	{
		Assert.ThrowsException<ArgumentOutOfRangeException>(() => Daggers.GetDaggerFromSeconds(GameVersion.V1_0, -1));
		Assert.ThrowsException<ArgumentOutOfRangeException>(() => Daggers.GetDaggerFromSeconds(GameVersion.V2_0, -1));
		Assert.ThrowsException<ArgumentOutOfRangeException>(() => Daggers.GetDaggerFromSeconds(GameVersion.V3_0, -1));
		Assert.ThrowsException<ArgumentOutOfRangeException>(() => Daggers.GetDaggerFromSeconds(GameVersion.V3_1, -1));
		Assert.ThrowsException<ArgumentOutOfRangeException>(() => Daggers.GetDaggerFromSeconds(GameVersion.V3_2, -1));
	}
}
