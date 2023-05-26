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
		AppVersion a = new(1, 2, 0);
		AppVersion b = new(1, 11, 0);
		AppVersion c = new(1, 2, 0);

		// A
		Assert.IsTrue(a < b);
		Assert.IsTrue(a == c);

		Assert.IsTrue(b > a);
		Assert.IsTrue(c == a);

		// B
		Assert.IsTrue(b > c);

		Assert.IsTrue(c < b);
	}

	[TestMethod]
	public void TestComparison_Build()
	{
		AppVersion a = new(0, 0, 0, 9);
		AppVersion b = new(0, 0, 0, 12);
		AppVersion c = new(0, 0, 0, 12);

		// A
		Assert.IsTrue(a < b);
		Assert.IsTrue(a < c);

		Assert.IsTrue(b > a);
		Assert.IsTrue(c > a);

		// B
		Assert.IsTrue(b == c);

		Assert.IsTrue(c == b);
	}

	[TestMethod]
	public void TestComparison_Alpha()
	{
		AppVersion a = new(1, 0, 0, 0);
		AppVersion b = new(2, 0, 0, 0, true);
		AppVersion c = new(2, 0, 0, 1, true);
		AppVersion d = new(2, 0, 0, 0, false);
		AppVersion e = new(2, 0, 0, 0, true);
		AppVersion f = new(2, 0, 1, 0, true);

		// A
		Assert.IsTrue(a < b);
		Assert.IsTrue(a < c);
		Assert.IsTrue(a < d);
		Assert.IsTrue(a < e);
		Assert.IsTrue(a < f);

		Assert.IsTrue(b > a);
		Assert.IsTrue(c > a);
		Assert.IsTrue(d > a);
		Assert.IsTrue(e > a);
		Assert.IsTrue(f > a);

		// B
		Assert.IsTrue(b < c);
		Assert.IsTrue(b < d);
		Assert.IsTrue(b == e);
		Assert.IsTrue(b < f);

		Assert.IsTrue(c > b);
		Assert.IsTrue(d > b);
		Assert.IsTrue(e == b);
		Assert.IsTrue(f > b);

		// C
		Assert.IsTrue(c < d);
		Assert.IsTrue(c > e);
		Assert.IsTrue(c < f);

		Assert.IsTrue(d > c);
		Assert.IsTrue(e < c);
		Assert.IsTrue(f > c);

		// D
		Assert.IsTrue(d > e);
		Assert.IsTrue(d < f);

		Assert.IsTrue(e < d);
		Assert.IsTrue(f > d);

		// E
		Assert.IsTrue(e < f);

		Assert.IsTrue(f > e);
	}
}
