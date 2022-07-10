using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DevilDaggersInfo.Core.Versioning.Tests;

[TestClass]
public class AppVersionTests
{
	[DataTestMethod]
	[DataRow("v1.2.0.0")]
	[DataRow("1..0.0")]
	[DataRow("1.2.0")]
	[DataRow("a.1.3.4")]
	public void TestParse_Invalid(string version)
	{
		Assert.IsFalse(AppVersion.TryParse(version, out _));
	}

	[DataTestMethod]
	[DataRow("1.2.0.0", 1, 2, 0, false, 0)]
	[DataRow("0.2.0.0", 0, 2, 0, false, 0)]
	[DataRow("3.0.0-alpha.0", 3, 0, 0, true, 0)]
	[DataRow("3.0.0-alpha.1", 3, 0, 0, true, 1)]
	public void TestParse_Valid(string version, int major, int minor, int patch, bool isAlpha, int build)
	{
		AppVersion parsed = AppVersion.Parse(version);
		Assert.AreEqual(major, parsed.Major);
		Assert.AreEqual(minor, parsed.Minor);
		Assert.AreEqual(patch, parsed.Patch);
		Assert.AreEqual(isAlpha, parsed.IsAlpha);
		Assert.AreEqual(build, parsed.Build);
	}

	[TestMethod]
	public void TestComparison_Default()
	{
		AppVersion a = new(1, 0, 0);
		AppVersion b = new(1, 1, 0);
		AppVersion c = new(1, 0, 0);

		Assert.IsTrue(a < b);
		Assert.IsTrue(b > a);

		Assert.IsTrue(a == c);
		Assert.IsTrue(c == a);

		Assert.IsTrue(c < b);
		Assert.IsTrue(b > c);
	}

	[TestMethod]
	public void TestComparison_Build()
	{
		AppVersion a = new(0, 0, 0, 0);
		AppVersion b = new(0, 0, 0, 1);
		AppVersion c = new(0, 0, 0, 1);

		Assert.IsTrue(a < b);
		Assert.IsTrue(b > a);

		Assert.IsTrue(a < c);
		Assert.IsTrue(c > a);

		Assert.IsTrue(c == b);
		Assert.IsTrue(b == c);
	}
}
