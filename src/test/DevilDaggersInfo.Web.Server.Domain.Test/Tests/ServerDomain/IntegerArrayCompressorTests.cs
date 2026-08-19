using DevilDaggersInfo.Web.Server.Domain.Utils;

namespace DevilDaggersInfo.Web.Server.Domain.Test.Tests.ServerDomain;

internal sealed class IntegerArrayCompressorTests
{
	[Test]
	public async Task TestGetMaxBitCount()
	{
		await Assert.That(() => IntegerArrayCompressor.GetBitCount(-1)).Throws<ArgumentOutOfRangeException>();
		await Assert.That((int)IntegerArrayCompressor.GetBitCount(0)).IsEqualTo(0);
		await Assert.That((int)IntegerArrayCompressor.GetBitCount(1)).IsEqualTo(1);
		await Assert.That((int)IntegerArrayCompressor.GetBitCount(2)).IsEqualTo(2);
		await Assert.That((int)IntegerArrayCompressor.GetBitCount(3)).IsEqualTo(2);
		await Assert.That((int)IntegerArrayCompressor.GetBitCount(4)).IsEqualTo(3);
		await Assert.That((int)IntegerArrayCompressor.GetBitCount(5)).IsEqualTo(3);
		await Assert.That((int)IntegerArrayCompressor.GetBitCount(6)).IsEqualTo(3);
		await Assert.That((int)IntegerArrayCompressor.GetBitCount(7)).IsEqualTo(3);
		await Assert.That((int)IntegerArrayCompressor.GetBitCount(8)).IsEqualTo(4);
		await Assert.That((int)IntegerArrayCompressor.GetBitCount(9)).IsEqualTo(4);
		await Assert.That((int)IntegerArrayCompressor.GetBitCount(15)).IsEqualTo(4);
		await Assert.That((int)IntegerArrayCompressor.GetBitCount(16)).IsEqualTo(5);
		await Assert.That((int)IntegerArrayCompressor.GetBitCount(31)).IsEqualTo(5);
		await Assert.That((int)IntegerArrayCompressor.GetBitCount(byte.MaxValue)).IsEqualTo(8);
		await Assert.That((int)IntegerArrayCompressor.GetBitCount(byte.MaxValue + 1)).IsEqualTo(9);
		await Assert.That((int)IntegerArrayCompressor.GetBitCount(short.MaxValue)).IsEqualTo(15);
		await Assert.That((int)IntegerArrayCompressor.GetBitCount(ushort.MaxValue)).IsEqualTo(16);
		await Assert.That((int)IntegerArrayCompressor.GetBitCount(ushort.MaxValue + 1)).IsEqualTo(17);
		await Assert.That((int)IntegerArrayCompressor.GetBitCount(int.MaxValue)).IsEqualTo(31);
	}

	[Test]
	public async Task Test1BitNumbers()
	{
		const byte bitCount = 1;
		bool[] binary0 = [false];
		bool[] binary1 = [true];

		await Assert.That(IntegerArrayCompressor.GetBitsFromValue(0, bitCount)).IsEquivalentTo(binary0, CollectionOrdering.Matching);
		await Assert.That(IntegerArrayCompressor.GetBitsFromValue(1, bitCount)).IsEquivalentTo(binary1, CollectionOrdering.Matching);

		await Assert.That(IntegerArrayCompressor.GetValueFromBits(binary0)).IsEqualTo(0);
		await Assert.That(IntegerArrayCompressor.GetValueFromBits(binary1)).IsEqualTo(1);
	}

	[Test]
	public async Task Test2BitNumbers()
	{
		const byte bitCount = 2;
		bool[] binary0 = [false, false];
		bool[] binary1 = [false, true];
		bool[] binary2 = [true, false];
		bool[] binary3 = [true, true];

		await Assert.That(IntegerArrayCompressor.GetBitsFromValue(0, bitCount)).IsEquivalentTo(binary0, CollectionOrdering.Matching);
		await Assert.That(IntegerArrayCompressor.GetBitsFromValue(1, bitCount)).IsEquivalentTo(binary1, CollectionOrdering.Matching);
		await Assert.That(IntegerArrayCompressor.GetBitsFromValue(2, bitCount)).IsEquivalentTo(binary2, CollectionOrdering.Matching);
		await Assert.That(IntegerArrayCompressor.GetBitsFromValue(3, bitCount)).IsEquivalentTo(binary3, CollectionOrdering.Matching);

		await Assert.That(IntegerArrayCompressor.GetValueFromBits(binary0)).IsEqualTo(0);
		await Assert.That(IntegerArrayCompressor.GetValueFromBits(binary1)).IsEqualTo(1);
		await Assert.That(IntegerArrayCompressor.GetValueFromBits(binary2)).IsEqualTo(2);
		await Assert.That(IntegerArrayCompressor.GetValueFromBits(binary3)).IsEqualTo(3);
	}

	[Test]
	public async Task Test3BitNumbers()
	{
		const byte bitCount = 3;
		bool[] binary0 = [false, false, false];
		bool[] binary1 = [false, false, true];
		bool[] binary2 = [false, true, false];
		bool[] binary3 = [false, true, true];
		bool[] binary4 = [true, false, false];
		bool[] binary5 = [true, false, true];
		bool[] binary6 = [true, true, false];
		bool[] binary7 = [true, true, true];

		await Assert.That(IntegerArrayCompressor.GetBitsFromValue(0, bitCount)).IsEquivalentTo(binary0, CollectionOrdering.Matching);
		await Assert.That(IntegerArrayCompressor.GetBitsFromValue(1, bitCount)).IsEquivalentTo(binary1, CollectionOrdering.Matching);
		await Assert.That(IntegerArrayCompressor.GetBitsFromValue(2, bitCount)).IsEquivalentTo(binary2, CollectionOrdering.Matching);
		await Assert.That(IntegerArrayCompressor.GetBitsFromValue(3, bitCount)).IsEquivalentTo(binary3, CollectionOrdering.Matching);
		await Assert.That(IntegerArrayCompressor.GetBitsFromValue(4, bitCount)).IsEquivalentTo(binary4, CollectionOrdering.Matching);
		await Assert.That(IntegerArrayCompressor.GetBitsFromValue(5, bitCount)).IsEquivalentTo(binary5, CollectionOrdering.Matching);
		await Assert.That(IntegerArrayCompressor.GetBitsFromValue(6, bitCount)).IsEquivalentTo(binary6, CollectionOrdering.Matching);
		await Assert.That(IntegerArrayCompressor.GetBitsFromValue(7, bitCount)).IsEquivalentTo(binary7, CollectionOrdering.Matching);

		await Assert.That(IntegerArrayCompressor.GetValueFromBits(binary0)).IsEqualTo(0);
		await Assert.That(IntegerArrayCompressor.GetValueFromBits(binary1)).IsEqualTo(1);
		await Assert.That(IntegerArrayCompressor.GetValueFromBits(binary2)).IsEqualTo(2);
		await Assert.That(IntegerArrayCompressor.GetValueFromBits(binary3)).IsEqualTo(3);
		await Assert.That(IntegerArrayCompressor.GetValueFromBits(binary4)).IsEqualTo(4);
		await Assert.That(IntegerArrayCompressor.GetValueFromBits(binary5)).IsEqualTo(5);
		await Assert.That(IntegerArrayCompressor.GetValueFromBits(binary6)).IsEqualTo(6);
		await Assert.That(IntegerArrayCompressor.GetValueFromBits(binary7)).IsEqualTo(7);
	}

	[Test]
	public async Task Test8BitNumbers()
	{
		const byte bitCount = 8;
		bool[] binary0 = [false, false, false, false, false, false, false, false];
		bool[] binary16 = [false, false, false, true, false, false, false, false];
		bool[] binary72 = [false, true, false, false, true, false, false, false];
		bool[] binary75 = [false, true, false, false, true, false, true, true];
		bool[] binary255 = [true, true, true, true, true, true, true, true];

		await Assert.That(IntegerArrayCompressor.GetBitsFromValue(0, bitCount)).IsEquivalentTo(binary0, CollectionOrdering.Matching);
		await Assert.That(IntegerArrayCompressor.GetBitsFromValue(16, bitCount)).IsEquivalentTo(binary16, CollectionOrdering.Matching);
		await Assert.That(IntegerArrayCompressor.GetBitsFromValue(72, bitCount)).IsEquivalentTo(binary72, CollectionOrdering.Matching);
		await Assert.That(IntegerArrayCompressor.GetBitsFromValue(75, bitCount)).IsEquivalentTo(binary75, CollectionOrdering.Matching);
		await Assert.That(IntegerArrayCompressor.GetBitsFromValue(255, bitCount)).IsEquivalentTo(binary255, CollectionOrdering.Matching);

		await Assert.That(IntegerArrayCompressor.GetValueFromBits(binary0)).IsEqualTo(0);
		await Assert.That(IntegerArrayCompressor.GetValueFromBits(binary16)).IsEqualTo(16);
		await Assert.That(IntegerArrayCompressor.GetValueFromBits(binary72)).IsEqualTo(72);
		await Assert.That(IntegerArrayCompressor.GetValueFromBits(binary75)).IsEqualTo(75);
		await Assert.That(IntegerArrayCompressor.GetValueFromBits(binary255)).IsEqualTo(255);
	}

	[Test]
	public async Task Test9BitNumbers()
	{
		const byte bitCount = 9;
		bool[] binary0 = [false, false, false, false, false, false, false, false, false];
		bool[] binary16 = [false, false, false, false, true, false, false, false, false];
		bool[] binary72 = [false, false, true, false, false, true, false, false, false];
		bool[] binary75 = [false, false, true, false, false, true, false, true, true];
		bool[] binary255 = [false, true, true, true, true, true, true, true, true];
		bool[] binary511 = [true, true, true, true, true, true, true, true, true];

		await Assert.That(IntegerArrayCompressor.GetBitsFromValue(0, bitCount)).IsEquivalentTo(binary0, CollectionOrdering.Matching);
		await Assert.That(IntegerArrayCompressor.GetBitsFromValue(16, bitCount)).IsEquivalentTo(binary16, CollectionOrdering.Matching);
		await Assert.That(IntegerArrayCompressor.GetBitsFromValue(72, bitCount)).IsEquivalentTo(binary72, CollectionOrdering.Matching);
		await Assert.That(IntegerArrayCompressor.GetBitsFromValue(75, bitCount)).IsEquivalentTo(binary75, CollectionOrdering.Matching);
		await Assert.That(IntegerArrayCompressor.GetBitsFromValue(255, bitCount)).IsEquivalentTo(binary255, CollectionOrdering.Matching);
		await Assert.That(IntegerArrayCompressor.GetBitsFromValue(511, bitCount)).IsEquivalentTo(binary511, CollectionOrdering.Matching);

		await Assert.That(IntegerArrayCompressor.GetValueFromBits(binary0)).IsEqualTo(0);
		await Assert.That(IntegerArrayCompressor.GetValueFromBits(binary16)).IsEqualTo(16);
		await Assert.That(IntegerArrayCompressor.GetValueFromBits(binary72)).IsEqualTo(72);
		await Assert.That(IntegerArrayCompressor.GetValueFromBits(binary75)).IsEqualTo(75);
		await Assert.That(IntegerArrayCompressor.GetValueFromBits(binary255)).IsEqualTo(255);
		await Assert.That(IntegerArrayCompressor.GetValueFromBits(binary511)).IsEqualTo(511);
	}

	[Test]
	public async Task Test16BitNumbers()
	{
		const byte bitCount = 16;
		bool[] binary0 = [false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false];
		bool[] binary16 = [false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false];
		bool[] binary72 = [false, false, false, false, false, false, false, false, false, true, false, false, true, false, false, false];
		bool[] binary75 = [false, false, false, false, false, false, false, false, false, true, false, false, true, false, true, true];
		bool[] binary255 = [false, false, false, false, false, false, false, false, true, true, true, true, true, true, true, true];
		bool[] binary511 = [false, false, false, false, false, false, false, true, true, true, true, true, true, true, true, true];
		bool[] binary32767 = [false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true];
		bool[] binary65535 = [true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true];

		await Assert.That(IntegerArrayCompressor.GetBitsFromValue(0, bitCount)).IsEquivalentTo(binary0, CollectionOrdering.Matching);
		await Assert.That(IntegerArrayCompressor.GetBitsFromValue(16, bitCount)).IsEquivalentTo(binary16, CollectionOrdering.Matching);
		await Assert.That(IntegerArrayCompressor.GetBitsFromValue(72, bitCount)).IsEquivalentTo(binary72, CollectionOrdering.Matching);
		await Assert.That(IntegerArrayCompressor.GetBitsFromValue(75, bitCount)).IsEquivalentTo(binary75, CollectionOrdering.Matching);
		await Assert.That(IntegerArrayCompressor.GetBitsFromValue(255, bitCount)).IsEquivalentTo(binary255, CollectionOrdering.Matching);
		await Assert.That(IntegerArrayCompressor.GetBitsFromValue(511, bitCount)).IsEquivalentTo(binary511, CollectionOrdering.Matching);
		await Assert.That(IntegerArrayCompressor.GetBitsFromValue(32767, bitCount)).IsEquivalentTo(binary32767, CollectionOrdering.Matching);
		await Assert.That(IntegerArrayCompressor.GetBitsFromValue(65535, bitCount)).IsEquivalentTo(binary65535, CollectionOrdering.Matching);

		await Assert.That(IntegerArrayCompressor.GetValueFromBits(binary0)).IsEqualTo(0);
		await Assert.That(IntegerArrayCompressor.GetValueFromBits(binary16)).IsEqualTo(16);
		await Assert.That(IntegerArrayCompressor.GetValueFromBits(binary72)).IsEqualTo(72);
		await Assert.That(IntegerArrayCompressor.GetValueFromBits(binary75)).IsEqualTo(75);
		await Assert.That(IntegerArrayCompressor.GetValueFromBits(binary255)).IsEqualTo(255);
		await Assert.That(IntegerArrayCompressor.GetValueFromBits(binary511)).IsEqualTo(511);
		await Assert.That(IntegerArrayCompressor.GetValueFromBits(binary32767)).IsEqualTo(32767);
		await Assert.That(IntegerArrayCompressor.GetValueFromBits(binary65535)).IsEqualTo(65535);
	}

	[Test]
	public async Task Test1BitCompression()
	{
		await TestCompressionAsync([0, 0, 0, 1, 0, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 0, 1, 0, 1, 1, 0]);
	}

	[Test]
	public async Task Test2BitCompression()
	{
		await TestCompressionAsync([0, 0, 0, 1, 3, 1, 2]);
	}

	[Test]
	public async Task Test3BitCompression()
	{
		await TestCompressionAsync([7, 3, 4, 1, 0, 5, 5, 5, 5, 0, 4]);
	}

	[Test]
	public async Task Test4BitCompression()
	{
		await TestCompressionAsync([7, 3, 4, 1, 9, 14, 15, 1, 13, 9, 4, 11]);
	}

	[Test]
	public async Task Test8BitCompression()
	{
		await TestCompressionAsync([9, 4, 11, 255, 19, 39, 192, 85, 19, 4, 85]);
	}

	[Test]
	public async Task Test9BitCompression()
	{
		await TestCompressionAsync([9, 4, 11, 255, 19, 39, 192, 85, 19, 4, 85, 499]);
	}

	[Test]
	public async Task Test11BitCompression()
	{
		await TestCompressionAsync([9, 4, 11, 255, 19, 39, 192, 85, 19, 4, 85, 499, 2000]);
	}

	[Test]
	public async Task Test15BitCompression()
	{
		await TestCompressionAsync([9, 4, 11, 255, 19, 39, 192, 85, 19, 4, 85, 499, 32000]);
	}

	[Test]
	public async Task Test16BitCompression()
	{
		await TestCompressionAsync([9, 4, 11, 255, 19, 39, 192, 85, 19, 4, 85, 499, 64000]);
	}

	[AssertionMethod]
	private static async Task TestCompressionAsync(int[] values)
	{
		int expectedBitCount = IntegerArrayCompressor.GetBitCount(values.Max());
		int expectedCompressedByteLength = (values.Length * expectedBitCount - 1) / 8 + 1;

		byte[] compressedData = IntegerArrayCompressor.CompressData(values);
		await Assert.That((int)compressedData[0]).IsEqualTo(expectedBitCount);
		await Assert.That(compressedData.Length - 1).IsEqualTo(expectedCompressedByteLength);

		int[] extractedData = IntegerArrayCompressor.ExtractData(compressedData);
		await Assert.That(extractedData[..values.Length]).IsEquivalentTo(values, CollectionOrdering.Matching);
	}
}
