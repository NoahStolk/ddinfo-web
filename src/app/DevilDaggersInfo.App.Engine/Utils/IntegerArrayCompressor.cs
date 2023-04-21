namespace Warp.NET.Utils;

internal static class IntegerArrayCompressor
{
	/// <summary>
	/// <para>
	/// Compresses the <paramref name="data"/> array of values into a compact byte array.
	/// Only the required amount of bits is used. For example, if none of the values exceed 15, we can use 4-bit integers.
	/// </para>
	/// <para>
	/// When using an amount of bits that is not divisible by 8, the last byte might be "incomplete" and the remaining bits within that byte will be set to 0.
	/// </para>
	/// <para>
	/// When all the bytes are 0, the returned array will be empty.
	/// </para>
	/// </summary>
	public static byte[] CompressData(byte[] data)
	{
		if (data.Length == 0 || data.All(i => i <= 0))
			return Array.Empty<byte>();

		byte bitCount = GetBitCount(data.Max());

		BitArray bitArray = new(new[] { bitCount })
		{
			Length = 8 + data.Length * bitCount,
		};

		for (int i = 0; i < data.Length; i++)
		{
			bool[] bits = GetBitsFromValue(data[i], bitCount);

			for (int j = 0; j < bitCount; j++)
				bitArray.Set(8 + i * bitCount + j, bits[j]);
		}

		return bitArray.ToBytes();
	}

	/// <summary>
	/// Extracts a compressed <paramref name="data"/> array to the original array of values.
	/// Due to possible remaining bits, there might be additional empty values at the end of the extracted array.
	/// </summary>
	public static byte[] ExtractData(byte[] data)
	{
		if (data.Length == 0)
			return Array.Empty<byte>();

		byte bitCount = data[0];
		BitArray bitArray = new(data[1..]);

		byte[] values = new byte[bitArray.Length / bitCount];
		for (int i = 0; i < values.Length; i++)
		{
			bool[] bits = new bool[bitCount];
			for (int j = 0; j < bitCount; j++)
				bits[j] = bitArray.Get(i * bitCount + j);

			values[i] = GetValueFromBits(bits);
		}

		return values;
	}

	/// <summary>
	/// Returns the amount of bits required to store this <paramref name="value"/>.
	/// </summary>
	private static byte GetBitCount(byte value)
		=> (byte)(Math.Log(value, 2) + 1);

	private static bool[] GetBitsFromValue(byte value, byte bitCount)
	{
		if (bitCount > 8)
			throw new ArgumentOutOfRangeException(nameof(bitCount), "Bit count cannot exceed 8.");

		bool[] bits = new bool[bitCount];
		int sig = 1;
		for (int i = bitCount - 1; i >= 0; i--)
		{
			bits[i] = (value & sig) != 0;
			sig <<= 1;
		}

		return bits;
	}

	private static byte GetValueFromBits(bool[] bits)
	{
		if (bits.Length > 8)
			throw new NotSupportedException("Bit count cannot exceed 8.");

		byte value = 0;
		byte sig = 1;
		for (int i = bits.Length - 1; i >= 0; i--)
		{
			if (bits[i])
				value += sig;

			sig <<= 1;
		}

		return value;
	}

	private static byte[] ToBytes(this BitArray bitArray)
	{
		byte[] bytes = new byte[(bitArray.Length - 1) / 8 + 1];
		bitArray.CopyTo(bytes, 0);
		return bytes;
	}
}
