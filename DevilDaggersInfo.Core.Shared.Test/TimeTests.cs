namespace DevilDaggersInfo.Core.Shared.Test;

[TestClass]
public class TimeTests
{
	[TestMethod]
	public void TestTime()
	{
		Assert.AreEqual(10001998, 1000.1998.To10thMilliTime());
		Assert.AreEqual(1.0, 10000.ToSecondsTime());
		Assert.AreEqual(1234567890.1234, 12345678901234UL.ToSecondsTime());
	}
}
