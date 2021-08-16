namespace DevilDaggersInfo.Core.Wiki.Test;

[TestClass]
public class DaggerTests
{
	[TestMethod]
	public void TestLeviathanDagger()
	{
		const int seconds = 1000;
		const int tenthsOfMilliseconds = 1000_0000;

		Assert.AreEqual(Daggers.Leviathan, Daggers.GetDaggerFromSeconds(GameVersion.V3_1, seconds));
		Assert.AreEqual(Daggers.Leviathan, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_1, tenthsOfMilliseconds));
	}

	[TestMethod]
	public void TestDevilDagger()
	{
		const int secondsLastV31 = 999;
		const int tenthsOfMillisecondsLastV31 = 999_9999;

		const int secondsLastV3 = 1000;
		const int tenthsOfMillisecondsLastV3 = 1000_0000;

		const int secondsFirst = 500;
		const int tenthsOfMillisecondsFirst = 500_0000;

		Assert.AreEqual(Daggers.Devil, Daggers.GetDaggerFromSeconds(GameVersion.V3_1, secondsLastV31));
		Assert.AreEqual(Daggers.Devil, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_1, tenthsOfMillisecondsLastV31));

		Assert.AreEqual(Daggers.Devil, Daggers.GetDaggerFromSeconds(GameVersion.V3_1, secondsFirst));
		Assert.AreEqual(Daggers.Devil, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_1, tenthsOfMillisecondsFirst));

		Assert.AreEqual(Daggers.Devil, Daggers.GetDaggerFromSeconds(GameVersion.V3_0, secondsLastV3));
		Assert.AreEqual(Daggers.Devil, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_0, tenthsOfMillisecondsLastV3));

		Assert.AreEqual(Daggers.Devil, Daggers.GetDaggerFromSeconds(GameVersion.V3_0, secondsFirst));
		Assert.AreEqual(Daggers.Devil, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_0, tenthsOfMillisecondsFirst));

		Assert.AreEqual(Daggers.Devil, Daggers.GetDaggerFromSeconds(GameVersion.V2_0, secondsLastV3));
		Assert.AreEqual(Daggers.Devil, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V2_0, tenthsOfMillisecondsLastV3));

		Assert.AreEqual(Daggers.Devil, Daggers.GetDaggerFromSeconds(GameVersion.V2_0, secondsFirst));
		Assert.AreEqual(Daggers.Devil, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V2_0, tenthsOfMillisecondsFirst));

		Assert.AreEqual(Daggers.Devil, Daggers.GetDaggerFromSeconds(GameVersion.V1_0, secondsLastV3));
		Assert.AreEqual(Daggers.Devil, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V1_0, tenthsOfMillisecondsLastV3));

		Assert.AreEqual(Daggers.Devil, Daggers.GetDaggerFromSeconds(GameVersion.V1_0, secondsFirst));
		Assert.AreEqual(Daggers.Devil, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V1_0, tenthsOfMillisecondsFirst));
	}

	[TestMethod]
	public void TestGoldenDagger()
	{
		const int secondsLast = 499;
		const int tenthsOfMillisecondsLast = 499_9999;

		const int secondsFirst = 250;
		const int tenthsOfMillisecondsFirst = 250_0000;

		Assert.AreEqual(Daggers.Golden, Daggers.GetDaggerFromSeconds(GameVersion.V3_1, secondsLast));
		Assert.AreEqual(Daggers.Golden, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_1, tenthsOfMillisecondsLast));

		Assert.AreEqual(Daggers.Golden, Daggers.GetDaggerFromSeconds(GameVersion.V3_1, secondsFirst));
		Assert.AreEqual(Daggers.Golden, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_1, tenthsOfMillisecondsFirst));

		Assert.AreEqual(Daggers.Golden, Daggers.GetDaggerFromSeconds(GameVersion.V3_0, secondsLast));
		Assert.AreEqual(Daggers.Golden, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_0, tenthsOfMillisecondsLast));

		Assert.AreEqual(Daggers.Golden, Daggers.GetDaggerFromSeconds(GameVersion.V3_0, secondsFirst));
		Assert.AreEqual(Daggers.Golden, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_0, tenthsOfMillisecondsFirst));

		Assert.AreEqual(Daggers.Golden, Daggers.GetDaggerFromSeconds(GameVersion.V2_0, secondsLast));
		Assert.AreEqual(Daggers.Golden, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V2_0, tenthsOfMillisecondsLast));

		Assert.AreEqual(Daggers.Golden, Daggers.GetDaggerFromSeconds(GameVersion.V2_0, secondsFirst));
		Assert.AreEqual(Daggers.Golden, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V2_0, tenthsOfMillisecondsFirst));

		Assert.AreEqual(Daggers.Golden, Daggers.GetDaggerFromSeconds(GameVersion.V1_0, secondsLast));
		Assert.AreEqual(Daggers.Golden, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V1_0, tenthsOfMillisecondsLast));

		Assert.AreEqual(Daggers.Golden, Daggers.GetDaggerFromSeconds(GameVersion.V1_0, secondsFirst));
		Assert.AreEqual(Daggers.Golden, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V1_0, tenthsOfMillisecondsFirst));
	}

	[TestMethod]
	public void TestSilverDagger()
	{
		const int secondsLast = 249;
		const int tenthsOfMillisecondsLast = 249_9999;

		const int secondsFirst = 120;
		const int tenthsOfMillisecondsFirst = 120_0000;

		Assert.AreEqual(Daggers.Silver, Daggers.GetDaggerFromSeconds(GameVersion.V3_1, secondsLast));
		Assert.AreEqual(Daggers.Silver, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_1, tenthsOfMillisecondsLast));

		Assert.AreEqual(Daggers.Silver, Daggers.GetDaggerFromSeconds(GameVersion.V3_1, secondsFirst));
		Assert.AreEqual(Daggers.Silver, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_1, tenthsOfMillisecondsFirst));

		Assert.AreEqual(Daggers.Silver, Daggers.GetDaggerFromSeconds(GameVersion.V3_0, secondsLast));
		Assert.AreEqual(Daggers.Silver, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_0, tenthsOfMillisecondsLast));

		Assert.AreEqual(Daggers.Silver, Daggers.GetDaggerFromSeconds(GameVersion.V3_0, secondsFirst));
		Assert.AreEqual(Daggers.Silver, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_0, tenthsOfMillisecondsFirst));

		Assert.AreEqual(Daggers.Silver, Daggers.GetDaggerFromSeconds(GameVersion.V2_0, secondsLast));
		Assert.AreEqual(Daggers.Silver, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V2_0, tenthsOfMillisecondsLast));

		Assert.AreEqual(Daggers.Silver, Daggers.GetDaggerFromSeconds(GameVersion.V2_0, secondsFirst));
		Assert.AreEqual(Daggers.Silver, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V2_0, tenthsOfMillisecondsFirst));

		Assert.AreEqual(Daggers.Silver, Daggers.GetDaggerFromSeconds(GameVersion.V1_0, secondsLast));
		Assert.AreEqual(Daggers.Silver, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V1_0, tenthsOfMillisecondsLast));

		Assert.AreEqual(Daggers.Silver, Daggers.GetDaggerFromSeconds(GameVersion.V1_0, secondsFirst));
		Assert.AreEqual(Daggers.Silver, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V1_0, tenthsOfMillisecondsFirst));
	}

	[TestMethod]
	public void TestBronzeDagger()
	{
		const int secondsLast = 119;
		const int tenthsOfMillisecondsLast = 119_9999;

		const int secondsFirst = 60;
		const int tenthsOfMillisecondsFirst = 60_0000;

		Assert.AreEqual(Daggers.Bronze, Daggers.GetDaggerFromSeconds(GameVersion.V3_1, secondsLast));
		Assert.AreEqual(Daggers.Bronze, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_1, tenthsOfMillisecondsLast));

		Assert.AreEqual(Daggers.Bronze, Daggers.GetDaggerFromSeconds(GameVersion.V3_1, secondsFirst));
		Assert.AreEqual(Daggers.Bronze, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_1, tenthsOfMillisecondsFirst));

		Assert.AreEqual(Daggers.Bronze, Daggers.GetDaggerFromSeconds(GameVersion.V3_0, secondsLast));
		Assert.AreEqual(Daggers.Bronze, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_0, tenthsOfMillisecondsLast));

		Assert.AreEqual(Daggers.Bronze, Daggers.GetDaggerFromSeconds(GameVersion.V3_0, secondsFirst));
		Assert.AreEqual(Daggers.Bronze, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_0, tenthsOfMillisecondsFirst));

		Assert.AreEqual(Daggers.Bronze, Daggers.GetDaggerFromSeconds(GameVersion.V2_0, secondsLast));
		Assert.AreEqual(Daggers.Bronze, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V2_0, tenthsOfMillisecondsLast));

		Assert.AreEqual(Daggers.Bronze, Daggers.GetDaggerFromSeconds(GameVersion.V2_0, secondsFirst));
		Assert.AreEqual(Daggers.Bronze, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V2_0, tenthsOfMillisecondsFirst));

		Assert.AreEqual(Daggers.Bronze, Daggers.GetDaggerFromSeconds(GameVersion.V1_0, secondsLast));
		Assert.AreEqual(Daggers.Bronze, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V1_0, tenthsOfMillisecondsLast));

		Assert.AreEqual(Daggers.Bronze, Daggers.GetDaggerFromSeconds(GameVersion.V1_0, secondsFirst));
		Assert.AreEqual(Daggers.Bronze, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V1_0, tenthsOfMillisecondsFirst));
	}

	[TestMethod]
	public void TestDefaultDagger()
	{
		const int secondsLast = 59;
		const int tenthsOfMillisecondsLast = 59_9999;

		const int secondsFirst = 0;
		const int tenthsOfMillisecondsFirst = 0;

		Assert.AreEqual(Daggers.Default, Daggers.GetDaggerFromSeconds(GameVersion.V3_1, secondsLast));
		Assert.AreEqual(Daggers.Default, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_1, tenthsOfMillisecondsLast));

		Assert.AreEqual(Daggers.Default, Daggers.GetDaggerFromSeconds(GameVersion.V3_1, secondsFirst));
		Assert.AreEqual(Daggers.Default, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_1, tenthsOfMillisecondsFirst));

		Assert.AreEqual(Daggers.Default, Daggers.GetDaggerFromSeconds(GameVersion.V3_0, secondsLast));
		Assert.AreEqual(Daggers.Default, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_0, tenthsOfMillisecondsLast));

		Assert.AreEqual(Daggers.Default, Daggers.GetDaggerFromSeconds(GameVersion.V3_0, secondsFirst));
		Assert.AreEqual(Daggers.Default, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V3_0, tenthsOfMillisecondsFirst));

		Assert.AreEqual(Daggers.Default, Daggers.GetDaggerFromSeconds(GameVersion.V2_0, secondsLast));
		Assert.AreEqual(Daggers.Default, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V2_0, tenthsOfMillisecondsLast));

		Assert.AreEqual(Daggers.Default, Daggers.GetDaggerFromSeconds(GameVersion.V2_0, secondsFirst));
		Assert.AreEqual(Daggers.Default, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V2_0, tenthsOfMillisecondsFirst));

		Assert.AreEqual(Daggers.Default, Daggers.GetDaggerFromSeconds(GameVersion.V1_0, secondsLast));
		Assert.AreEqual(Daggers.Default, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V1_0, tenthsOfMillisecondsLast));

		Assert.AreEqual(Daggers.Default, Daggers.GetDaggerFromSeconds(GameVersion.V1_0, secondsFirst));
		Assert.AreEqual(Daggers.Default, Daggers.GetDaggerFromTenthsOfMilliseconds(GameVersion.V1_0, tenthsOfMillisecondsFirst));
	}

	[TestMethod]
	public void TestOutOfRange()
	{
		Assert.ThrowsException<ArgumentOutOfRangeException>(() => Daggers.GetDaggerFromSeconds(GameVersion.V1_0, -1));
		Assert.ThrowsException<ArgumentOutOfRangeException>(() => Daggers.GetDaggerFromSeconds(GameVersion.V2_0, -1));
		Assert.ThrowsException<ArgumentOutOfRangeException>(() => Daggers.GetDaggerFromSeconds(GameVersion.V3_0, -1));
		Assert.ThrowsException<ArgumentOutOfRangeException>(() => Daggers.GetDaggerFromSeconds(GameVersion.V3_1, -1));
	}
}
