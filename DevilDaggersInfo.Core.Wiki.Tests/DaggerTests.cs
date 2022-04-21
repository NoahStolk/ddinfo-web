namespace DevilDaggersInfo.Test.Core.Wiki;

[TestClass]
public class DaggerTests
{
	[TestMethod]
	public void TestLeviathanDagger()
	{
		const int seconds = 1000;
		const int tenthsOfMilliseconds = 1000_0000;

		Assert.AreEqual(DaggersV3_2.Leviathan, Daggers.GetDaggerFromSeconds(GameVersion.V3_2, seconds));
		Assert.AreEqual(DaggersV3_2.Leviathan, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_2, tenthsOfMilliseconds));

		Assert.AreEqual(DaggersV3_1.Leviathan, Daggers.GetDaggerFromSeconds(GameVersion.V3_1, seconds));
		Assert.AreEqual(DaggersV3_1.Leviathan, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_1, tenthsOfMilliseconds));
	}

	[TestMethod]
	public void TestDevilDagger()
	{
		const int secondsLastV3Next = 999;
		const int tenthsOfMillisecondsLastV3Next = 999_9999;

		const int secondsLastV3 = 1000;
		const int tenthsOfMillisecondsLastV3 = 1000_0000;

		const int secondsFirst = 500;
		const int tenthsOfMillisecondsFirst = 500_0000;

		Assert.AreEqual(DaggersV3_2.Devil, Daggers.GetDaggerFromSeconds(GameVersion.V3_2, secondsLastV3Next));
		Assert.AreEqual(DaggersV3_2.Devil, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_2, tenthsOfMillisecondsLastV3Next));

		Assert.AreEqual(DaggersV3_2.Devil, Daggers.GetDaggerFromSeconds(GameVersion.V3_2, secondsFirst));
		Assert.AreEqual(DaggersV3_2.Devil, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_2, tenthsOfMillisecondsFirst));

		Assert.AreEqual(DaggersV3_1.Devil, Daggers.GetDaggerFromSeconds(GameVersion.V3_1, secondsLastV3Next));
		Assert.AreEqual(DaggersV3_1.Devil, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_1, tenthsOfMillisecondsLastV3Next));

		Assert.AreEqual(DaggersV3_1.Devil, Daggers.GetDaggerFromSeconds(GameVersion.V3_1, secondsFirst));
		Assert.AreEqual(DaggersV3_1.Devil, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_1, tenthsOfMillisecondsFirst));

		Assert.AreEqual(DaggersV3_0.Devil, Daggers.GetDaggerFromSeconds(GameVersion.V3_0, secondsLastV3));
		Assert.AreEqual(DaggersV3_0.Devil, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_0, tenthsOfMillisecondsLastV3));

		Assert.AreEqual(DaggersV3_0.Devil, Daggers.GetDaggerFromSeconds(GameVersion.V3_0, secondsFirst));
		Assert.AreEqual(DaggersV3_0.Devil, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_0, tenthsOfMillisecondsFirst));

		Assert.AreEqual(DaggersV2_0.Devil, Daggers.GetDaggerFromSeconds(GameVersion.V2_0, secondsLastV3));
		Assert.AreEqual(DaggersV2_0.Devil, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V2_0, tenthsOfMillisecondsLastV3));

		Assert.AreEqual(DaggersV2_0.Devil, Daggers.GetDaggerFromSeconds(GameVersion.V2_0, secondsFirst));
		Assert.AreEqual(DaggersV2_0.Devil, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V2_0, tenthsOfMillisecondsFirst));

		Assert.AreEqual(DaggersV1_0.Devil, Daggers.GetDaggerFromSeconds(GameVersion.V1_0, secondsLastV3));
		Assert.AreEqual(DaggersV1_0.Devil, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V1_0, tenthsOfMillisecondsLastV3));

		Assert.AreEqual(DaggersV1_0.Devil, Daggers.GetDaggerFromSeconds(GameVersion.V1_0, secondsFirst));
		Assert.AreEqual(DaggersV1_0.Devil, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V1_0, tenthsOfMillisecondsFirst));
	}

	[TestMethod]
	public void TestGoldenDagger()
	{
		const int secondsLast = 499;
		const int tenthsOfMillisecondsLast = 499_9999;

		const int secondsFirst = 250;
		const int tenthsOfMillisecondsFirst = 250_0000;

		Assert.AreEqual(DaggersV3_2.Golden, Daggers.GetDaggerFromSeconds(GameVersion.V3_2, secondsLast));
		Assert.AreEqual(DaggersV3_2.Golden, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_2, tenthsOfMillisecondsLast));

		Assert.AreEqual(DaggersV3_2.Golden, Daggers.GetDaggerFromSeconds(GameVersion.V3_2, secondsFirst));
		Assert.AreEqual(DaggersV3_2.Golden, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_2, tenthsOfMillisecondsFirst));

		Assert.AreEqual(DaggersV3_1.Golden, Daggers.GetDaggerFromSeconds(GameVersion.V3_1, secondsLast));
		Assert.AreEqual(DaggersV3_1.Golden, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_1, tenthsOfMillisecondsLast));

		Assert.AreEqual(DaggersV3_1.Golden, Daggers.GetDaggerFromSeconds(GameVersion.V3_1, secondsFirst));
		Assert.AreEqual(DaggersV3_1.Golden, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_1, tenthsOfMillisecondsFirst));

		Assert.AreEqual(DaggersV3_0.Golden, Daggers.GetDaggerFromSeconds(GameVersion.V3_0, secondsLast));
		Assert.AreEqual(DaggersV3_0.Golden, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_0, tenthsOfMillisecondsLast));

		Assert.AreEqual(DaggersV3_0.Golden, Daggers.GetDaggerFromSeconds(GameVersion.V3_0, secondsFirst));
		Assert.AreEqual(DaggersV3_0.Golden, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_0, tenthsOfMillisecondsFirst));

		Assert.AreEqual(DaggersV2_0.Golden, Daggers.GetDaggerFromSeconds(GameVersion.V2_0, secondsLast));
		Assert.AreEqual(DaggersV2_0.Golden, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V2_0, tenthsOfMillisecondsLast));

		Assert.AreEqual(DaggersV2_0.Golden, Daggers.GetDaggerFromSeconds(GameVersion.V2_0, secondsFirst));
		Assert.AreEqual(DaggersV2_0.Golden, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V2_0, tenthsOfMillisecondsFirst));

		Assert.AreEqual(DaggersV1_0.Golden, Daggers.GetDaggerFromSeconds(GameVersion.V1_0, secondsLast));
		Assert.AreEqual(DaggersV1_0.Golden, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V1_0, tenthsOfMillisecondsLast));

		Assert.AreEqual(DaggersV1_0.Golden, Daggers.GetDaggerFromSeconds(GameVersion.V1_0, secondsFirst));
		Assert.AreEqual(DaggersV1_0.Golden, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V1_0, tenthsOfMillisecondsFirst));
	}

	[TestMethod]
	public void TestSilverDagger()
	{
		const int secondsLast = 249;
		const int tenthsOfMillisecondsLast = 249_9999;

		const int secondsFirst = 120;
		const int tenthsOfMillisecondsFirst = 120_0000;

		Assert.AreEqual(DaggersV3_2.Silver, Daggers.GetDaggerFromSeconds(GameVersion.V3_2, secondsLast));
		Assert.AreEqual(DaggersV3_2.Silver, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_2, tenthsOfMillisecondsLast));

		Assert.AreEqual(DaggersV3_2.Silver, Daggers.GetDaggerFromSeconds(GameVersion.V3_2, secondsFirst));
		Assert.AreEqual(DaggersV3_2.Silver, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_2, tenthsOfMillisecondsFirst));

		Assert.AreEqual(DaggersV3_1.Silver, Daggers.GetDaggerFromSeconds(GameVersion.V3_1, secondsLast));
		Assert.AreEqual(DaggersV3_1.Silver, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_1, tenthsOfMillisecondsLast));

		Assert.AreEqual(DaggersV3_1.Silver, Daggers.GetDaggerFromSeconds(GameVersion.V3_1, secondsFirst));
		Assert.AreEqual(DaggersV3_1.Silver, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_1, tenthsOfMillisecondsFirst));

		Assert.AreEqual(DaggersV3_0.Silver, Daggers.GetDaggerFromSeconds(GameVersion.V3_0, secondsLast));
		Assert.AreEqual(DaggersV3_0.Silver, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_0, tenthsOfMillisecondsLast));

		Assert.AreEqual(DaggersV3_0.Silver, Daggers.GetDaggerFromSeconds(GameVersion.V3_0, secondsFirst));
		Assert.AreEqual(DaggersV3_0.Silver, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_0, tenthsOfMillisecondsFirst));

		Assert.AreEqual(DaggersV2_0.Silver, Daggers.GetDaggerFromSeconds(GameVersion.V2_0, secondsLast));
		Assert.AreEqual(DaggersV2_0.Silver, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V2_0, tenthsOfMillisecondsLast));

		Assert.AreEqual(DaggersV2_0.Silver, Daggers.GetDaggerFromSeconds(GameVersion.V2_0, secondsFirst));
		Assert.AreEqual(DaggersV2_0.Silver, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V2_0, tenthsOfMillisecondsFirst));

		Assert.AreEqual(DaggersV1_0.Silver, Daggers.GetDaggerFromSeconds(GameVersion.V1_0, secondsLast));
		Assert.AreEqual(DaggersV1_0.Silver, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V1_0, tenthsOfMillisecondsLast));

		Assert.AreEqual(DaggersV1_0.Silver, Daggers.GetDaggerFromSeconds(GameVersion.V1_0, secondsFirst));
		Assert.AreEqual(DaggersV1_0.Silver, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V1_0, tenthsOfMillisecondsFirst));
	}

	[TestMethod]
	public void TestBronzeDagger()
	{
		const int secondsLast = 119;
		const int tenthsOfMillisecondsLast = 119_9999;

		const int secondsFirst = 60;
		const int tenthsOfMillisecondsFirst = 60_0000;

		Assert.AreEqual(DaggersV3_2.Bronze, Daggers.GetDaggerFromSeconds(GameVersion.V3_2, secondsLast));
		Assert.AreEqual(DaggersV3_2.Bronze, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_2, tenthsOfMillisecondsLast));

		Assert.AreEqual(DaggersV3_2.Bronze, Daggers.GetDaggerFromSeconds(GameVersion.V3_2, secondsFirst));
		Assert.AreEqual(DaggersV3_2.Bronze, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_2, tenthsOfMillisecondsFirst));

		Assert.AreEqual(DaggersV3_1.Bronze, Daggers.GetDaggerFromSeconds(GameVersion.V3_1, secondsLast));
		Assert.AreEqual(DaggersV3_1.Bronze, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_1, tenthsOfMillisecondsLast));

		Assert.AreEqual(DaggersV3_1.Bronze, Daggers.GetDaggerFromSeconds(GameVersion.V3_1, secondsFirst));
		Assert.AreEqual(DaggersV3_1.Bronze, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_1, tenthsOfMillisecondsFirst));

		Assert.AreEqual(DaggersV3_0.Bronze, Daggers.GetDaggerFromSeconds(GameVersion.V3_0, secondsLast));
		Assert.AreEqual(DaggersV3_0.Bronze, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_0, tenthsOfMillisecondsLast));

		Assert.AreEqual(DaggersV3_0.Bronze, Daggers.GetDaggerFromSeconds(GameVersion.V3_0, secondsFirst));
		Assert.AreEqual(DaggersV3_0.Bronze, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_0, tenthsOfMillisecondsFirst));

		Assert.AreEqual(DaggersV2_0.Bronze, Daggers.GetDaggerFromSeconds(GameVersion.V2_0, secondsLast));
		Assert.AreEqual(DaggersV2_0.Bronze, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V2_0, tenthsOfMillisecondsLast));

		Assert.AreEqual(DaggersV2_0.Bronze, Daggers.GetDaggerFromSeconds(GameVersion.V2_0, secondsFirst));
		Assert.AreEqual(DaggersV2_0.Bronze, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V2_0, tenthsOfMillisecondsFirst));

		Assert.AreEqual(DaggersV1_0.Bronze, Daggers.GetDaggerFromSeconds(GameVersion.V1_0, secondsLast));
		Assert.AreEqual(DaggersV1_0.Bronze, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V1_0, tenthsOfMillisecondsLast));

		Assert.AreEqual(DaggersV1_0.Bronze, Daggers.GetDaggerFromSeconds(GameVersion.V1_0, secondsFirst));
		Assert.AreEqual(DaggersV1_0.Bronze, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V1_0, tenthsOfMillisecondsFirst));
	}

	[TestMethod]
	public void TestDefaultDagger()
	{
		const int secondsLast = 59;
		const int tenthsOfMillisecondsLast = 59_9999;

		const int secondsFirst = 0;
		const int tenthsOfMillisecondsFirst = 0;

		Assert.AreEqual(DaggersV3_2.Default, Daggers.GetDaggerFromSeconds(GameVersion.V3_2, secondsLast));
		Assert.AreEqual(DaggersV3_2.Default, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_2, tenthsOfMillisecondsLast));

		Assert.AreEqual(DaggersV3_2.Default, Daggers.GetDaggerFromSeconds(GameVersion.V3_2, secondsFirst));
		Assert.AreEqual(DaggersV3_2.Default, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_2, tenthsOfMillisecondsFirst));

		Assert.AreEqual(DaggersV3_1.Default, Daggers.GetDaggerFromSeconds(GameVersion.V3_1, secondsLast));
		Assert.AreEqual(DaggersV3_1.Default, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_1, tenthsOfMillisecondsLast));

		Assert.AreEqual(DaggersV3_1.Default, Daggers.GetDaggerFromSeconds(GameVersion.V3_1, secondsFirst));
		Assert.AreEqual(DaggersV3_1.Default, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_1, tenthsOfMillisecondsFirst));

		Assert.AreEqual(DaggersV3_0.Default, Daggers.GetDaggerFromSeconds(GameVersion.V3_0, secondsLast));
		Assert.AreEqual(DaggersV3_0.Default, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_0, tenthsOfMillisecondsLast));

		Assert.AreEqual(DaggersV3_0.Default, Daggers.GetDaggerFromSeconds(GameVersion.V3_0, secondsFirst));
		Assert.AreEqual(DaggersV3_0.Default, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_0, tenthsOfMillisecondsFirst));

		Assert.AreEqual(DaggersV2_0.Default, Daggers.GetDaggerFromSeconds(GameVersion.V2_0, secondsLast));
		Assert.AreEqual(DaggersV2_0.Default, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V2_0, tenthsOfMillisecondsLast));

		Assert.AreEqual(DaggersV2_0.Default, Daggers.GetDaggerFromSeconds(GameVersion.V2_0, secondsFirst));
		Assert.AreEqual(DaggersV2_0.Default, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V2_0, tenthsOfMillisecondsFirst));

		Assert.AreEqual(DaggersV1_0.Default, Daggers.GetDaggerFromSeconds(GameVersion.V1_0, secondsLast));
		Assert.AreEqual(DaggersV1_0.Default, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V1_0, tenthsOfMillisecondsLast));

		Assert.AreEqual(DaggersV1_0.Default, Daggers.GetDaggerFromSeconds(GameVersion.V1_0, secondsFirst));
		Assert.AreEqual(DaggersV1_0.Default, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V1_0, tenthsOfMillisecondsFirst));
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
