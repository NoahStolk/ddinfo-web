namespace DevilDaggersInfo.Core.Extensions;

public static class ByteArrayExtensions
{
	public static string ReadNullTerminatedString(this byte[] buffer, int offset)
		=> buffer.ReadNullTerminatedString(offset, Encoding.UTF8);

	public static string ReadNullTerminatedString(this byte[] buffer, int offset, Encoding encoding)
	{
		if (offset < 0 || offset >= buffer.Length)
			throw new ArgumentOutOfRangeException(nameof(offset), "Offset was out of bounds of the array.");

		int indexOfNextNullTerminator = Array.IndexOf(buffer, (byte)0, offset);
		if (indexOfNextNullTerminator == -1)
			throw new ArgumentOutOfRangeException(nameof(offset), $"Null terminator not observed in buffer with length '{buffer.Length}' starting from offset '{offset}'.");

		return encoding.GetString(buffer[offset..indexOfNextNullTerminator]);
	}
}
