namespace DevilDaggersInfo.Test.Web.BlazorWasm.Server;

[TestClass]
public class IntegerArrayCompressorTests
{
	[TestMethod]
	public void TestGetMaxBitCount()
	{
		Assert.ThrowsException<ArgumentOutOfRangeException>(() => IntegerArrayCompressor.GetBitCount(-1));
		Assert.AreEqual(0, IntegerArrayCompressor.GetBitCount(0));
		Assert.AreEqual(1, IntegerArrayCompressor.GetBitCount(1));
		Assert.AreEqual(2, IntegerArrayCompressor.GetBitCount(2));
		Assert.AreEqual(2, IntegerArrayCompressor.GetBitCount(3));
		Assert.AreEqual(3, IntegerArrayCompressor.GetBitCount(4));
		Assert.AreEqual(3, IntegerArrayCompressor.GetBitCount(5));
		Assert.AreEqual(3, IntegerArrayCompressor.GetBitCount(6));
		Assert.AreEqual(3, IntegerArrayCompressor.GetBitCount(7));
		Assert.AreEqual(4, IntegerArrayCompressor.GetBitCount(8));
		Assert.AreEqual(4, IntegerArrayCompressor.GetBitCount(9));
		Assert.AreEqual(4, IntegerArrayCompressor.GetBitCount(15));
		Assert.AreEqual(5, IntegerArrayCompressor.GetBitCount(16));
		Assert.AreEqual(5, IntegerArrayCompressor.GetBitCount(31));
		Assert.AreEqual(8, IntegerArrayCompressor.GetBitCount(byte.MaxValue));
		Assert.AreEqual(9, IntegerArrayCompressor.GetBitCount(byte.MaxValue + 1));
		Assert.AreEqual(15, IntegerArrayCompressor.GetBitCount(short.MaxValue));
		Assert.AreEqual(16, IntegerArrayCompressor.GetBitCount(ushort.MaxValue));
		Assert.AreEqual(17, IntegerArrayCompressor.GetBitCount(ushort.MaxValue + 1));
		Assert.AreEqual(31, IntegerArrayCompressor.GetBitCount(int.MaxValue));
	}

	[TestMethod]
	public void Test1BitNumbers()
	{
		const byte bitCount = 1;
		bool[] binary0 = new[] { false };
		bool[] binary1 = new[] { true };

		TestUtils.AssertArrayContentsEqual(binary0, IntegerArrayCompressor.GetBitsFromValue(0, bitCount));
		TestUtils.AssertArrayContentsEqual(binary1, IntegerArrayCompressor.GetBitsFromValue(1, bitCount));

		Assert.AreEqual(0, IntegerArrayCompressor.GetValueFromBits(binary0));
		Assert.AreEqual(1, IntegerArrayCompressor.GetValueFromBits(binary1));
	}

	[TestMethod]
	public void Test2BitNumbers()
	{
		const byte bitCount = 2;
		bool[] binary0 = new[] { false, false };
		bool[] binary1 = new[] { false, true };
		bool[] binary2 = new[] { true, false };
		bool[] binary3 = new[] { true, true };

		TestUtils.AssertArrayContentsEqual(binary0, IntegerArrayCompressor.GetBitsFromValue(0, bitCount));
		TestUtils.AssertArrayContentsEqual(binary1, IntegerArrayCompressor.GetBitsFromValue(1, bitCount));
		TestUtils.AssertArrayContentsEqual(binary2, IntegerArrayCompressor.GetBitsFromValue(2, bitCount));
		TestUtils.AssertArrayContentsEqual(binary3, IntegerArrayCompressor.GetBitsFromValue(3, bitCount));

		Assert.AreEqual(0, IntegerArrayCompressor.GetValueFromBits(binary0));
		Assert.AreEqual(1, IntegerArrayCompressor.GetValueFromBits(binary1));
		Assert.AreEqual(2, IntegerArrayCompressor.GetValueFromBits(binary2));
		Assert.AreEqual(3, IntegerArrayCompressor.GetValueFromBits(binary3));
	}

	[TestMethod]
	public void Test3BitNumbers()
	{
		const byte bitCount = 3;
		bool[] binary0 = new[] { false, false, false };
		bool[] binary1 = new[] { false, false, true };
		bool[] binary2 = new[] { false, true, false };
		bool[] binary3 = new[] { false, true, true };
		bool[] binary4 = new[] { true, false, false };
		bool[] binary5 = new[] { true, false, true };
		bool[] binary6 = new[] { true, true, false };
		bool[] binary7 = new[] { true, true, true };

		TestUtils.AssertArrayContentsEqual(binary0, IntegerArrayCompressor.GetBitsFromValue(0, bitCount));
		TestUtils.AssertArrayContentsEqual(binary1, IntegerArrayCompressor.GetBitsFromValue(1, bitCount));
		TestUtils.AssertArrayContentsEqual(binary2, IntegerArrayCompressor.GetBitsFromValue(2, bitCount));
		TestUtils.AssertArrayContentsEqual(binary3, IntegerArrayCompressor.GetBitsFromValue(3, bitCount));
		TestUtils.AssertArrayContentsEqual(binary4, IntegerArrayCompressor.GetBitsFromValue(4, bitCount));
		TestUtils.AssertArrayContentsEqual(binary5, IntegerArrayCompressor.GetBitsFromValue(5, bitCount));
		TestUtils.AssertArrayContentsEqual(binary6, IntegerArrayCompressor.GetBitsFromValue(6, bitCount));
		TestUtils.AssertArrayContentsEqual(binary7, IntegerArrayCompressor.GetBitsFromValue(7, bitCount));

		Assert.AreEqual(0, IntegerArrayCompressor.GetValueFromBits(binary0));
		Assert.AreEqual(1, IntegerArrayCompressor.GetValueFromBits(binary1));
		Assert.AreEqual(2, IntegerArrayCompressor.GetValueFromBits(binary2));
		Assert.AreEqual(3, IntegerArrayCompressor.GetValueFromBits(binary3));
		Assert.AreEqual(4, IntegerArrayCompressor.GetValueFromBits(binary4));
		Assert.AreEqual(5, IntegerArrayCompressor.GetValueFromBits(binary5));
		Assert.AreEqual(6, IntegerArrayCompressor.GetValueFromBits(binary6));
		Assert.AreEqual(7, IntegerArrayCompressor.GetValueFromBits(binary7));
	}

	[TestMethod]
	public void Test8BitNumbers()
	{
		const byte bitCount = 8;
		bool[] binary0 = new[] { false, false, false, false, false, false, false, false };
		bool[] binary16 = new[] { false, false, false, true, false, false, false, false };
		bool[] binary72 = new[] { false, true, false, false, true, false, false, false };
		bool[] binary75 = new[] { false, true, false, false, true, false, true, true };
		bool[] binary255 = new[] { true, true, true, true, true, true, true, true };

		TestUtils.AssertArrayContentsEqual(binary0, IntegerArrayCompressor.GetBitsFromValue(0, bitCount));
		TestUtils.AssertArrayContentsEqual(binary16, IntegerArrayCompressor.GetBitsFromValue(16, bitCount));
		TestUtils.AssertArrayContentsEqual(binary72, IntegerArrayCompressor.GetBitsFromValue(72, bitCount));
		TestUtils.AssertArrayContentsEqual(binary75, IntegerArrayCompressor.GetBitsFromValue(75, bitCount));
		TestUtils.AssertArrayContentsEqual(binary255, IntegerArrayCompressor.GetBitsFromValue(255, bitCount));

		Assert.AreEqual(0, IntegerArrayCompressor.GetValueFromBits(binary0));
		Assert.AreEqual(16, IntegerArrayCompressor.GetValueFromBits(binary16));
		Assert.AreEqual(72, IntegerArrayCompressor.GetValueFromBits(binary72));
		Assert.AreEqual(75, IntegerArrayCompressor.GetValueFromBits(binary75));
		Assert.AreEqual(255, IntegerArrayCompressor.GetValueFromBits(binary255));
	}

	[TestMethod]
	public void Test9BitNumbers()
	{
		const byte bitCount = 9;
		bool[] binary0 = new[] { false, false, false, false, false, false, false, false, false };
		bool[] binary16 = new[] { false, false, false, false, true, false, false, false, false };
		bool[] binary72 = new[] { false, false, true, false, false, true, false, false, false };
		bool[] binary75 = new[] { false, false, true, false, false, true, false, true, true };
		bool[] binary255 = new[] { false, true, true, true, true, true, true, true, true };
		bool[] binary511 = new[] { true, true, true, true, true, true, true, true, true };

		TestUtils.AssertArrayContentsEqual(binary0, IntegerArrayCompressor.GetBitsFromValue(0, bitCount));
		TestUtils.AssertArrayContentsEqual(binary16, IntegerArrayCompressor.GetBitsFromValue(16, bitCount));
		TestUtils.AssertArrayContentsEqual(binary72, IntegerArrayCompressor.GetBitsFromValue(72, bitCount));
		TestUtils.AssertArrayContentsEqual(binary75, IntegerArrayCompressor.GetBitsFromValue(75, bitCount));
		TestUtils.AssertArrayContentsEqual(binary255, IntegerArrayCompressor.GetBitsFromValue(255, bitCount));
		TestUtils.AssertArrayContentsEqual(binary511, IntegerArrayCompressor.GetBitsFromValue(511, bitCount));

		Assert.AreEqual(0, IntegerArrayCompressor.GetValueFromBits(binary0));
		Assert.AreEqual(16, IntegerArrayCompressor.GetValueFromBits(binary16));
		Assert.AreEqual(72, IntegerArrayCompressor.GetValueFromBits(binary72));
		Assert.AreEqual(75, IntegerArrayCompressor.GetValueFromBits(binary75));
		Assert.AreEqual(255, IntegerArrayCompressor.GetValueFromBits(binary255));
		Assert.AreEqual(511, IntegerArrayCompressor.GetValueFromBits(binary511));
	}

	[TestMethod]
	public void Test16BitNumbers()
	{
		const byte bitCount = 16;
		bool[] binary0 = new[] { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
		bool[] binary16 = new[] { false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false };
		bool[] binary72 = new[] { false, false, false, false, false, false, false, false, false, true, false, false, true, false, false, false };
		bool[] binary75 = new[] { false, false, false, false, false, false, false, false, false, true, false, false, true, false, true, true };
		bool[] binary255 = new[] { false, false, false, false, false, false, false, false, true, true, true, true, true, true, true, true };
		bool[] binary511 = new[] { false, false, false, false, false, false, false, true, true, true, true, true, true, true, true, true };
		bool[] binary32767 = new[] { false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true };
		bool[] binary65535 = new[] { true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true };

		TestUtils.AssertArrayContentsEqual(binary0, IntegerArrayCompressor.GetBitsFromValue(0, bitCount));
		TestUtils.AssertArrayContentsEqual(binary16, IntegerArrayCompressor.GetBitsFromValue(16, bitCount));
		TestUtils.AssertArrayContentsEqual(binary72, IntegerArrayCompressor.GetBitsFromValue(72, bitCount));
		TestUtils.AssertArrayContentsEqual(binary75, IntegerArrayCompressor.GetBitsFromValue(75, bitCount));
		TestUtils.AssertArrayContentsEqual(binary255, IntegerArrayCompressor.GetBitsFromValue(255, bitCount));
		TestUtils.AssertArrayContentsEqual(binary511, IntegerArrayCompressor.GetBitsFromValue(511, bitCount));
		TestUtils.AssertArrayContentsEqual(binary32767, IntegerArrayCompressor.GetBitsFromValue(32767, bitCount));
		TestUtils.AssertArrayContentsEqual(binary65535, IntegerArrayCompressor.GetBitsFromValue(65535, bitCount));

		Assert.AreEqual(0, IntegerArrayCompressor.GetValueFromBits(binary0));
		Assert.AreEqual(16, IntegerArrayCompressor.GetValueFromBits(binary16));
		Assert.AreEqual(72, IntegerArrayCompressor.GetValueFromBits(binary72));
		Assert.AreEqual(75, IntegerArrayCompressor.GetValueFromBits(binary75));
		Assert.AreEqual(255, IntegerArrayCompressor.GetValueFromBits(binary255));
		Assert.AreEqual(511, IntegerArrayCompressor.GetValueFromBits(binary511));
		Assert.AreEqual(32767, IntegerArrayCompressor.GetValueFromBits(binary32767));
		Assert.AreEqual(65535, IntegerArrayCompressor.GetValueFromBits(binary65535));
	}

	[TestMethod]
	public void Test1BitCompression()
		=> TestCompression(new[] { 0, 0, 0, 1, 0, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 0, 1, 0, 1, 1, 0 });

	[TestMethod]
	public void Test2BitCompression()
		=> TestCompression(new[] { 0, 0, 0, 1, 3, 1, 2 });

	[TestMethod]
	public void Test3BitCompression()
		=> TestCompression(new[] { 7, 3, 4, 1, 0, 5, 5, 5, 5, 0, 4 });

	[TestMethod]
	public void Test4BitCompression()
		=> TestCompression(new[] { 7, 3, 4, 1, 9, 14, 15, 1, 13, 9, 4, 11 });

	[TestMethod]
	public void Test8BitCompression()
		=> TestCompression(new[] { 9, 4, 11, 255, 19, 39, 192, 85, 19, 4, 85 });

	[TestMethod]
	public void Test9BitCompression()
		=> TestCompression(new[] { 9, 4, 11, 255, 19, 39, 192, 85, 19, 4, 85, 499 });

	[TestMethod]
	public void Test11BitCompression()
		=> TestCompression(new[] { 9, 4, 11, 255, 19, 39, 192, 85, 19, 4, 85, 499, 2000 });

	[TestMethod]
	public void Test15BitCompression()
		=> TestCompression(new[] { 9, 4, 11, 255, 19, 39, 192, 85, 19, 4, 85, 499, 32000 });

	[TestMethod]
	public void Test16BitCompression()
		=> TestCompression(new[] { 9, 4, 11, 255, 19, 39, 192, 85, 19, 4, 85, 499, 64000 });

	[AssertionMethod]
	public static void TestCompression(int[] values)
	{
		int expectedBitCount = IntegerArrayCompressor.GetBitCount(values.Max());
		int expectedCompressedByteLength = (values.Length * expectedBitCount - 1) / 8 + 1;

		byte[] compressedData = IntegerArrayCompressor.CompressData(values);
		Assert.AreEqual(expectedBitCount, compressedData[0]);
		Assert.AreEqual(expectedCompressedByteLength, compressedData.Length - 1);

		int[] extractedData = IntegerArrayCompressor.ExtractData(compressedData);
		TestUtils.AssertArrayContentsEqual(values, extractedData[..values.Length]);
	}
}
