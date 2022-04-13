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
}
