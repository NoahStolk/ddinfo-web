using DevilDaggersInfo.Web.Server.Domain.Utils;

namespace DevilDaggersInfo.Web.Server.Domain.Tests;

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
		bool[] binary0 = { false };
		bool[] binary1 = { true };

		CollectionAssert.AreEqual(binary0, IntegerArrayCompressor.GetBitsFromValue(0, bitCount));
		CollectionAssert.AreEqual(binary1, IntegerArrayCompressor.GetBitsFromValue(1, bitCount));

		Assert.AreEqual(0, IntegerArrayCompressor.GetValueFromBits(binary0));
		Assert.AreEqual(1, IntegerArrayCompressor.GetValueFromBits(binary1));
	}

	[TestMethod]
	public void Test2BitNumbers()
	{
		const byte bitCount = 2;
		bool[] binary0 = { false, false };
		bool[] binary1 = { false, true };
		bool[] binary2 = { true, false };
		bool[] binary3 = { true, true };

		CollectionAssert.AreEqual(binary0, IntegerArrayCompressor.GetBitsFromValue(0, bitCount));
		CollectionAssert.AreEqual(binary1, IntegerArrayCompressor.GetBitsFromValue(1, bitCount));
		CollectionAssert.AreEqual(binary2, IntegerArrayCompressor.GetBitsFromValue(2, bitCount));
		CollectionAssert.AreEqual(binary3, IntegerArrayCompressor.GetBitsFromValue(3, bitCount));

		Assert.AreEqual(0, IntegerArrayCompressor.GetValueFromBits(binary0));
		Assert.AreEqual(1, IntegerArrayCompressor.GetValueFromBits(binary1));
		Assert.AreEqual(2, IntegerArrayCompressor.GetValueFromBits(binary2));
		Assert.AreEqual(3, IntegerArrayCompressor.GetValueFromBits(binary3));
	}

	[TestMethod]
	public void Test3BitNumbers()
	{
		const byte bitCount = 3;
		bool[] binary0 = { false, false, false };
		bool[] binary1 = { false, false, true };
		bool[] binary2 = { false, true, false };
		bool[] binary3 = { false, true, true };
		bool[] binary4 = { true, false, false };
		bool[] binary5 = { true, false, true };
		bool[] binary6 = { true, true, false };
		bool[] binary7 = { true, true, true };

		CollectionAssert.AreEqual(binary0, IntegerArrayCompressor.GetBitsFromValue(0, bitCount));
		CollectionAssert.AreEqual(binary1, IntegerArrayCompressor.GetBitsFromValue(1, bitCount));
		CollectionAssert.AreEqual(binary2, IntegerArrayCompressor.GetBitsFromValue(2, bitCount));
		CollectionAssert.AreEqual(binary3, IntegerArrayCompressor.GetBitsFromValue(3, bitCount));
		CollectionAssert.AreEqual(binary4, IntegerArrayCompressor.GetBitsFromValue(4, bitCount));
		CollectionAssert.AreEqual(binary5, IntegerArrayCompressor.GetBitsFromValue(5, bitCount));
		CollectionAssert.AreEqual(binary6, IntegerArrayCompressor.GetBitsFromValue(6, bitCount));
		CollectionAssert.AreEqual(binary7, IntegerArrayCompressor.GetBitsFromValue(7, bitCount));

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
		bool[] binary0 = { false, false, false, false, false, false, false, false };
		bool[] binary16 = { false, false, false, true, false, false, false, false };
		bool[] binary72 = { false, true, false, false, true, false, false, false };
		bool[] binary75 = { false, true, false, false, true, false, true, true };
		bool[] binary255 = { true, true, true, true, true, true, true, true };

		CollectionAssert.AreEqual(binary0, IntegerArrayCompressor.GetBitsFromValue(0, bitCount));
		CollectionAssert.AreEqual(binary16, IntegerArrayCompressor.GetBitsFromValue(16, bitCount));
		CollectionAssert.AreEqual(binary72, IntegerArrayCompressor.GetBitsFromValue(72, bitCount));
		CollectionAssert.AreEqual(binary75, IntegerArrayCompressor.GetBitsFromValue(75, bitCount));
		CollectionAssert.AreEqual(binary255, IntegerArrayCompressor.GetBitsFromValue(255, bitCount));

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
		bool[] binary0 = { false, false, false, false, false, false, false, false, false };
		bool[] binary16 = { false, false, false, false, true, false, false, false, false };
		bool[] binary72 = { false, false, true, false, false, true, false, false, false };
		bool[] binary75 = { false, false, true, false, false, true, false, true, true };
		bool[] binary255 = { false, true, true, true, true, true, true, true, true };
		bool[] binary511 = { true, true, true, true, true, true, true, true, true };

		CollectionAssert.AreEqual(binary0, IntegerArrayCompressor.GetBitsFromValue(0, bitCount));
		CollectionAssert.AreEqual(binary16, IntegerArrayCompressor.GetBitsFromValue(16, bitCount));
		CollectionAssert.AreEqual(binary72, IntegerArrayCompressor.GetBitsFromValue(72, bitCount));
		CollectionAssert.AreEqual(binary75, IntegerArrayCompressor.GetBitsFromValue(75, bitCount));
		CollectionAssert.AreEqual(binary255, IntegerArrayCompressor.GetBitsFromValue(255, bitCount));
		CollectionAssert.AreEqual(binary511, IntegerArrayCompressor.GetBitsFromValue(511, bitCount));

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
		bool[] binary0 = { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
		bool[] binary16 = { false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false };
		bool[] binary72 = { false, false, false, false, false, false, false, false, false, true, false, false, true, false, false, false };
		bool[] binary75 = { false, false, false, false, false, false, false, false, false, true, false, false, true, false, true, true };
		bool[] binary255 = { false, false, false, false, false, false, false, false, true, true, true, true, true, true, true, true };
		bool[] binary511 = { false, false, false, false, false, false, false, true, true, true, true, true, true, true, true, true };
		bool[] binary32767 = { false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true };
		bool[] binary65535 = { true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true };

		CollectionAssert.AreEqual(binary0, IntegerArrayCompressor.GetBitsFromValue(0, bitCount));
		CollectionAssert.AreEqual(binary16, IntegerArrayCompressor.GetBitsFromValue(16, bitCount));
		CollectionAssert.AreEqual(binary72, IntegerArrayCompressor.GetBitsFromValue(72, bitCount));
		CollectionAssert.AreEqual(binary75, IntegerArrayCompressor.GetBitsFromValue(75, bitCount));
		CollectionAssert.AreEqual(binary255, IntegerArrayCompressor.GetBitsFromValue(255, bitCount));
		CollectionAssert.AreEqual(binary511, IntegerArrayCompressor.GetBitsFromValue(511, bitCount));
		CollectionAssert.AreEqual(binary32767, IntegerArrayCompressor.GetBitsFromValue(32767, bitCount));
		CollectionAssert.AreEqual(binary65535, IntegerArrayCompressor.GetBitsFromValue(65535, bitCount));

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
		CollectionAssert.AreEqual(values, extractedData[..values.Length]);
	}
}
