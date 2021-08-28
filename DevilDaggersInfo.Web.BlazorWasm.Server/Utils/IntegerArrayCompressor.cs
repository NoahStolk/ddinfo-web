namespace DevilDaggersInfo.Web.BlazorWasm.Server.Utils;

public static class IntegerArrayCompressor
{
	/// <summary>
	/// Compresses the <paramref name="data"/> array of values into a compact byte array.
	/// Only the required amount of bits is used. For example, if no values go above 15, we can use 4-bit integers.
	/// When using an amount of bits that is not divisible by 8, the last byte might be "incomplete" and the remaining bits will be set to 0.
	/// </summary>
	public static byte[] CompressData(int[] data)
	{
		if (data.Length == 0 || data.All(i => i <= 0))
			return Array.Empty<byte>();

		byte bitCount = GetBitCount(data.Max());
		BitArray bitArray = new(new byte[] { bitCount });
		bitArray.Length = 8 + data.Length * bitCount;

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
	public static int[] ExtractData(byte[] data)
	{
		byte bitCount = data[0];
		BitArray bitArray = new(data[1..]);

		int[] values = new int[bitArray.Length / bitCount];
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
	public static byte GetBitCount(int value)
	{
		if (value < 0)
			throw new ArgumentOutOfRangeException(nameof(value), "Negative values are not supported.");

		return (byte)(Math.Log(value, 2) + 1);
	}

	public static bool[] GetBitsFromValue(int value, byte bitCount)
	{
		if (bitCount > 31)
			throw new ArgumentOutOfRangeException(nameof(bitCount), "Bit count cannot exceed 31 (max bits for signed integer).");

		bool[] bits = new bool[bitCount];
		int sig = 1;
		for (int i = bitCount - 1; i >= 0; i--)
		{
			bits[i] = (value & sig) != 0;
			sig <<= 1;
		}

		return bits;
	}

	public static int GetValueFromBits(bool[] bits)
	{
		if (bits.Length > 31)
			throw new NotSupportedException("Bit count cannot exceed 31 (max bits for signed integer).");

		int value = 0;
		int sig = 1;
		for (int i = bits.Length - 1; i >= 0; i--)
		{
			if (bits[i])
				value += sig;

			sig <<= 1;
		}

		return value;
	}

	public static byte[] ToBytes(this BitArray bitArray)
	{
		byte[] bytes = new byte[(bitArray.Length - 1) / 8 + 1];
		bitArray.CopyTo(bytes, 0);
		return bytes;
	}
}
