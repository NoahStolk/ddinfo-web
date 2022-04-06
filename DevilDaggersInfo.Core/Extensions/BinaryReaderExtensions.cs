namespace DevilDaggersInfo.Core.Extensions;

public static class BinaryReaderExtensions
{
	public static string ReadNullTerminatedString(this BinaryReader binaryReader)
	{
		StringBuilder sb = new();
		while (true)
		{
			byte b = binaryReader.ReadByte();
			if (b == 0x00)
				break;
			sb.Append((char)b);
		}

		return sb.ToString();
	}

	public static string ReadFixedLengthString(this BinaryReader binaryReader, int length)
		=> ReadFixedLengthString(binaryReader, length, Encoding.Default);

	public static string ReadFixedLengthString(this BinaryReader binaryReader, int length, Encoding encoding)
	{
		if (length <= 0)
			throw new ArgumentOutOfRangeException(nameof(length));

		return encoding.GetString(binaryReader.ReadBytes(length));
	}
}
