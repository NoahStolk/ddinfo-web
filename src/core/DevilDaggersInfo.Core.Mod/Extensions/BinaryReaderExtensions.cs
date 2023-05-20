namespace DevilDaggersInfo.Core.Mod.Extensions;

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

	public static Vertex ReadVertex(this BinaryReader br)
	{
		Vector3 position = new(
			x: br.ReadSingle(),
			y: br.ReadSingle(),
			z: br.ReadSingle());
		Vector3 normal = new(
			x: br.ReadSingle(),
			y: br.ReadSingle(),
			z: br.ReadSingle());
		Vector2 texCoord = new(
			x: br.ReadSingle(),
			y: br.ReadSingle());
		return new(position, normal, texCoord);
	}
}
