using DevilDaggersWebsite.BlazorWasm.Shared.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.BlazorWasm.Shared.Tests
{
	[TestClass]
	public class FlagEnumTests
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

		[TestMethod]
		public void TestFlagEnumToList()
		{
			TestFlag tf = TestFlag.A | TestFlag.B | TestFlag.C;
			List<int> list = tf.AsEnumerable().ToList();
			Assert.AreEqual(3, list.Count);
			Assert.IsFalse(list.Contains((int)TestFlag.None));
			Assert.IsTrue(list.Contains((int)TestFlag.A));
			Assert.IsTrue(list.Contains((int)TestFlag.B));
			Assert.IsTrue(list.Contains((int)TestFlag.C));
			Assert.IsFalse(list.Contains((int)TestFlag.D));
			Assert.IsFalse(list.Contains((int)TestFlag.E));
			Assert.IsFalse(list.Contains((int)TestFlag.F));
			Assert.IsFalse(list.Contains((int)TestFlag.G));
			Assert.IsFalse(list.Contains((int)TestFlag.H));
			Assert.IsFalse(list.Contains((int)TestFlag.I));
			Assert.IsFalse(list.Contains((int)TestFlag.J));
			Assert.IsFalse(list.Contains((int)TestFlag.K));
			Assert.IsFalse(list.Contains((int)TestFlag.L));
			Assert.IsFalse(list.Contains((int)TestFlag.M));
			Assert.IsFalse(list.Contains((int)TestFlag.N));

			tf = TestFlag.A | TestFlag.B | TestFlag.K;
			list = tf.AsEnumerable().ToList();
			Assert.AreEqual(3, list.Count);
			Assert.IsFalse(list.Contains((int)TestFlag.None));
			Assert.IsTrue(list.Contains((int)TestFlag.A));
			Assert.IsTrue(list.Contains((int)TestFlag.B));
			Assert.IsFalse(list.Contains((int)TestFlag.C));
			Assert.IsFalse(list.Contains((int)TestFlag.D));
			Assert.IsFalse(list.Contains((int)TestFlag.E));
			Assert.IsFalse(list.Contains((int)TestFlag.F));
			Assert.IsFalse(list.Contains((int)TestFlag.G));
			Assert.IsFalse(list.Contains((int)TestFlag.H));
			Assert.IsFalse(list.Contains((int)TestFlag.I));
			Assert.IsFalse(list.Contains((int)TestFlag.J));
			Assert.IsTrue(list.Contains((int)TestFlag.K));
			Assert.IsFalse(list.Contains((int)TestFlag.L));
			Assert.IsFalse(list.Contains((int)TestFlag.M));
			Assert.IsFalse(list.Contains((int)TestFlag.N));

			tf = TestFlag.D | TestFlag.M | TestFlag.N;
			list = tf.AsEnumerable().ToList();
			Assert.AreEqual(3, list.Count);
			Assert.IsFalse(list.Contains((int)TestFlag.None));
			Assert.IsFalse(list.Contains((int)TestFlag.A));
			Assert.IsFalse(list.Contains((int)TestFlag.B));
			Assert.IsFalse(list.Contains((int)TestFlag.C));
			Assert.IsTrue(list.Contains((int)TestFlag.D));
			Assert.IsFalse(list.Contains((int)TestFlag.E));
			Assert.IsFalse(list.Contains((int)TestFlag.F));
			Assert.IsFalse(list.Contains((int)TestFlag.G));
			Assert.IsFalse(list.Contains((int)TestFlag.H));
			Assert.IsFalse(list.Contains((int)TestFlag.I));
			Assert.IsFalse(list.Contains((int)TestFlag.J));
			Assert.IsFalse(list.Contains((int)TestFlag.K));
			Assert.IsFalse(list.Contains((int)TestFlag.L));
			Assert.IsTrue(list.Contains((int)TestFlag.M));
			Assert.IsTrue(list.Contains((int)TestFlag.N));
		}

		[TestMethod]
		public void TestListToFlagEnum()
		{
			List<int> list = new() { 1, 2, 4, 8 };
			Assert.AreEqual(TestFlag.A | TestFlag.B | TestFlag.C | TestFlag.D, list.ToFlagEnum<TestFlag>());
		}
	}
}
