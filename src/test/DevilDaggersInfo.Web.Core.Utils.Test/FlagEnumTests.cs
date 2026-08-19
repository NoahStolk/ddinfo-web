namespace DevilDaggersInfo.Web.Core.Utils.Test;

internal sealed class FlagEnumTests
{
	[Flags]
	private enum TestFlag
	{
		None = 0,
		A = 1,
		B = 2,
		C = 4,
		D = 8,
		E = 16,
		F = 32,
		G = 64,
		H = 128,
		I = 256,
		J = 512,
		K = 1024,
		L = 2048,
		M = 4096,
		N = 8192,
	}

	[Test]
	public async Task TestFlagEnumToList()
	{
		TestFlag tf = TestFlag.A | TestFlag.B | TestFlag.C;
		List<int> list = [.. tf.AsEnumerable()];
		await Assert.That(list.Count).IsEqualTo(3);
		await Assert.That(list.Contains((int)TestFlag.None)).IsFalse();
		await Assert.That(list.Contains((int)TestFlag.A)).IsTrue();
		await Assert.That(list.Contains((int)TestFlag.B)).IsTrue();
		await Assert.That(list.Contains((int)TestFlag.C)).IsTrue();
		await Assert.That(list.Contains((int)TestFlag.D)).IsFalse();
		await Assert.That(list.Contains((int)TestFlag.E)).IsFalse();
		await Assert.That(list.Contains((int)TestFlag.F)).IsFalse();
		await Assert.That(list.Contains((int)TestFlag.G)).IsFalse();
		await Assert.That(list.Contains((int)TestFlag.H)).IsFalse();
		await Assert.That(list.Contains((int)TestFlag.I)).IsFalse();
		await Assert.That(list.Contains((int)TestFlag.J)).IsFalse();
		await Assert.That(list.Contains((int)TestFlag.K)).IsFalse();
		await Assert.That(list.Contains((int)TestFlag.L)).IsFalse();
		await Assert.That(list.Contains((int)TestFlag.M)).IsFalse();
		await Assert.That(list.Contains((int)TestFlag.N)).IsFalse();

		tf = TestFlag.A | TestFlag.B | TestFlag.K;
		list = [.. tf.AsEnumerable()];
		await Assert.That(list.Count).IsEqualTo(3);
		await Assert.That(list.Contains((int)TestFlag.None)).IsFalse();
		await Assert.That(list.Contains((int)TestFlag.A)).IsTrue();
		await Assert.That(list.Contains((int)TestFlag.B)).IsTrue();
		await Assert.That(list.Contains((int)TestFlag.C)).IsFalse();
		await Assert.That(list.Contains((int)TestFlag.D)).IsFalse();
		await Assert.That(list.Contains((int)TestFlag.E)).IsFalse();
		await Assert.That(list.Contains((int)TestFlag.F)).IsFalse();
		await Assert.That(list.Contains((int)TestFlag.G)).IsFalse();
		await Assert.That(list.Contains((int)TestFlag.H)).IsFalse();
		await Assert.That(list.Contains((int)TestFlag.I)).IsFalse();
		await Assert.That(list.Contains((int)TestFlag.J)).IsFalse();
		await Assert.That(list.Contains((int)TestFlag.K)).IsTrue();
		await Assert.That(list.Contains((int)TestFlag.L)).IsFalse();
		await Assert.That(list.Contains((int)TestFlag.M)).IsFalse();
		await Assert.That(list.Contains((int)TestFlag.N)).IsFalse();

		tf = TestFlag.D | TestFlag.M | TestFlag.N;
		list = [.. tf.AsEnumerable()];
		await Assert.That(list.Count).IsEqualTo(3);
		await Assert.That(list.Contains((int)TestFlag.None)).IsFalse();
		await Assert.That(list.Contains((int)TestFlag.A)).IsFalse();
		await Assert.That(list.Contains((int)TestFlag.B)).IsFalse();
		await Assert.That(list.Contains((int)TestFlag.C)).IsFalse();
		await Assert.That(list.Contains((int)TestFlag.D)).IsTrue();
		await Assert.That(list.Contains((int)TestFlag.E)).IsFalse();
		await Assert.That(list.Contains((int)TestFlag.F)).IsFalse();
		await Assert.That(list.Contains((int)TestFlag.G)).IsFalse();
		await Assert.That(list.Contains((int)TestFlag.H)).IsFalse();
		await Assert.That(list.Contains((int)TestFlag.I)).IsFalse();
		await Assert.That(list.Contains((int)TestFlag.J)).IsFalse();
		await Assert.That(list.Contains((int)TestFlag.K)).IsFalse();
		await Assert.That(list.Contains((int)TestFlag.L)).IsFalse();
		await Assert.That(list.Contains((int)TestFlag.M)).IsTrue();
		await Assert.That(list.Contains((int)TestFlag.N)).IsTrue();
	}

	[Test]
	public async Task TestListToFlagEnum()
	{
		List<int> list = [1, 2, 4, 8];
		await Assert.That(list.ToFlagEnum<TestFlag>()).IsEqualTo(TestFlag.A | TestFlag.B | TestFlag.C | TestFlag.D);
	}
}
