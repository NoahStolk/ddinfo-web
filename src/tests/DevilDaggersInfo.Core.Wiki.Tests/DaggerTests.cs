using DevilDaggersInfo.Types.Core.Wiki;

namespace DevilDaggersInfo.Core.Wiki.Tests;

[TestClass]
public class DaggerTests
{
	[TestMethod]
	public void TestLeviathanDagger()
	{
		const int seconds = 1000;

		Assert.AreEqual(DaggersV3_2.Leviathan, Daggers.GetDaggerFromSeconds(GameVersion.V3_2, seconds));
		Assert.AreEqual(DaggersV3_1.Leviathan, Daggers.GetDaggerFromSeconds(GameVersion.V3_1, seconds));
	}

	[TestMethod]
	public void TestDevilDagger()
	{
		const double secondsLastV3Next = 999.9999;
		const int secondsLastV3 = 1000;
		const int secondsFirst = 500;

		Assert.AreEqual(DaggersV3_2.Devil, Daggers.GetDaggerFromSeconds(GameVersion.V3_2, secondsLastV3Next));
		Assert.AreEqual(DaggersV3_2.Devil, Daggers.GetDaggerFromSeconds(GameVersion.V3_2, secondsFirst));
		Assert.AreEqual(DaggersV3_1.Devil, Daggers.GetDaggerFromSeconds(GameVersion.V3_1, secondsLastV3Next));
		Assert.AreEqual(DaggersV3_1.Devil, Daggers.GetDaggerFromSeconds(GameVersion.V3_1, secondsFirst));
		Assert.AreEqual(DaggersV3_0.Devil, Daggers.GetDaggerFromSeconds(GameVersion.V3_0, secondsLastV3));
		Assert.AreEqual(DaggersV3_0.Devil, Daggers.GetDaggerFromSeconds(GameVersion.V3_0, secondsFirst));
		Assert.AreEqual(DaggersV2_0.Devil, Daggers.GetDaggerFromSeconds(GameVersion.V2_0, secondsLastV3));
		Assert.AreEqual(DaggersV2_0.Devil, Daggers.GetDaggerFromSeconds(GameVersion.V2_0, secondsFirst));
		Assert.AreEqual(DaggersV1_0.Devil, Daggers.GetDaggerFromSeconds(GameVersion.V1_0, secondsLastV3));
		Assert.AreEqual(DaggersV1_0.Devil, Daggers.GetDaggerFromSeconds(GameVersion.V1_0, secondsFirst));
	}

	[TestMethod]
	public void TestGoldenDagger()
	{
		const double secondsLast = 499.9999;
		const int secondsFirst = 250;

		Assert.AreEqual(DaggersV3_2.Golden, Daggers.GetDaggerFromSeconds(GameVersion.V3_2, secondsLast));
		Assert.AreEqual(DaggersV3_2.Golden, Daggers.GetDaggerFromSeconds(GameVersion.V3_2, secondsFirst));
		Assert.AreEqual(DaggersV3_1.Golden, Daggers.GetDaggerFromSeconds(GameVersion.V3_1, secondsLast));
		Assert.AreEqual(DaggersV3_1.Golden, Daggers.GetDaggerFromSeconds(GameVersion.V3_1, secondsFirst));
		Assert.AreEqual(DaggersV3_0.Golden, Daggers.GetDaggerFromSeconds(GameVersion.V3_0, secondsLast));
		Assert.AreEqual(DaggersV3_0.Golden, Daggers.GetDaggerFromSeconds(GameVersion.V3_0, secondsFirst));
		Assert.AreEqual(DaggersV2_0.Golden, Daggers.GetDaggerFromSeconds(GameVersion.V2_0, secondsLast));
		Assert.AreEqual(DaggersV2_0.Golden, Daggers.GetDaggerFromSeconds(GameVersion.V2_0, secondsFirst));
		Assert.AreEqual(DaggersV1_0.Golden, Daggers.GetDaggerFromSeconds(GameVersion.V1_0, secondsLast));
		Assert.AreEqual(DaggersV1_0.Golden, Daggers.GetDaggerFromSeconds(GameVersion.V1_0, secondsFirst));
	}

	[TestMethod]
	public void TestSilverDagger()
	{
		const double secondsLast = 249.9999;
		const int secondsFirst = 120;

		Assert.AreEqual(DaggersV3_2.Silver, Daggers.GetDaggerFromSeconds(GameVersion.V3_2, secondsLast));
		Assert.AreEqual(DaggersV3_2.Silver, Daggers.GetDaggerFromSeconds(GameVersion.V3_2, secondsFirst));
		Assert.AreEqual(DaggersV3_1.Silver, Daggers.GetDaggerFromSeconds(GameVersion.V3_1, secondsLast));
		Assert.AreEqual(DaggersV3_1.Silver, Daggers.GetDaggerFromSeconds(GameVersion.V3_1, secondsFirst));
		Assert.AreEqual(DaggersV3_0.Silver, Daggers.GetDaggerFromSeconds(GameVersion.V3_0, secondsLast));
		Assert.AreEqual(DaggersV3_0.Silver, Daggers.GetDaggerFromSeconds(GameVersion.V3_0, secondsFirst));
		Assert.AreEqual(DaggersV2_0.Silver, Daggers.GetDaggerFromSeconds(GameVersion.V2_0, secondsLast));
		Assert.AreEqual(DaggersV2_0.Silver, Daggers.GetDaggerFromSeconds(GameVersion.V2_0, secondsFirst));
		Assert.AreEqual(DaggersV1_0.Silver, Daggers.GetDaggerFromSeconds(GameVersion.V1_0, secondsLast));
		Assert.AreEqual(DaggersV1_0.Silver, Daggers.GetDaggerFromSeconds(GameVersion.V1_0, secondsFirst));
	}

	[TestMethod]
	public void TestBronzeDagger()
	{
		const double secondsLast = 119.9999;
		const int secondsFirst = 60;

		Assert.AreEqual(DaggersV3_2.Bronze, Daggers.GetDaggerFromSeconds(GameVersion.V3_2, secondsLast));
		Assert.AreEqual(DaggersV3_2.Bronze, Daggers.GetDaggerFromSeconds(GameVersion.V3_2, secondsFirst));
		Assert.AreEqual(DaggersV3_1.Bronze, Daggers.GetDaggerFromSeconds(GameVersion.V3_1, secondsLast));
		Assert.AreEqual(DaggersV3_1.Bronze, Daggers.GetDaggerFromSeconds(GameVersion.V3_1, secondsFirst));
		Assert.AreEqual(DaggersV3_0.Bronze, Daggers.GetDaggerFromSeconds(GameVersion.V3_0, secondsLast));
		Assert.AreEqual(DaggersV3_0.Bronze, Daggers.GetDaggerFromSeconds(GameVersion.V3_0, secondsFirst));
		Assert.AreEqual(DaggersV2_0.Bronze, Daggers.GetDaggerFromSeconds(GameVersion.V2_0, secondsLast));
		Assert.AreEqual(DaggersV2_0.Bronze, Daggers.GetDaggerFromSeconds(GameVersion.V2_0, secondsFirst));
		Assert.AreEqual(DaggersV1_0.Bronze, Daggers.GetDaggerFromSeconds(GameVersion.V1_0, secondsLast));
		Assert.AreEqual(DaggersV1_0.Bronze, Daggers.GetDaggerFromSeconds(GameVersion.V1_0, secondsFirst));
	}

	[TestMethod]
	public void TestDefaultDagger()
	{
		const double secondsLast = 59.9999;
		const int secondsFirst = 0;

		Assert.AreEqual(DaggersV3_2.Default, Daggers.GetDaggerFromSeconds(GameVersion.V3_2, secondsLast));
		Assert.AreEqual(DaggersV3_2.Default, Daggers.GetDaggerFromSeconds(GameVersion.V3_2, secondsFirst));
		Assert.AreEqual(DaggersV3_1.Default, Daggers.GetDaggerFromSeconds(GameVersion.V3_1, secondsLast));
		Assert.AreEqual(DaggersV3_1.Default, Daggers.GetDaggerFromSeconds(GameVersion.V3_1, secondsFirst));
		Assert.AreEqual(DaggersV3_0.Default, Daggers.GetDaggerFromSeconds(GameVersion.V3_0, secondsLast));
		Assert.AreEqual(DaggersV3_0.Default, Daggers.GetDaggerFromSeconds(GameVersion.V3_0, secondsFirst));
		Assert.AreEqual(DaggersV2_0.Default, Daggers.GetDaggerFromSeconds(GameVersion.V2_0, secondsLast));
		Assert.AreEqual(DaggersV2_0.Default, Daggers.GetDaggerFromSeconds(GameVersion.V2_0, secondsFirst));
		Assert.AreEqual(DaggersV1_0.Default, Daggers.GetDaggerFromSeconds(GameVersion.V1_0, secondsLast));
		Assert.AreEqual(DaggersV1_0.Default, Daggers.GetDaggerFromSeconds(GameVersion.V1_0, secondsFirst));
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
