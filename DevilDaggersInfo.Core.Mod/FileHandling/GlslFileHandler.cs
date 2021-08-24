namespace DevilDaggersInfo.Core.Mod.FileHandling;

public sealed class GlslFileHandler : IFileHandler
{
	private static readonly Lazy<GlslFileHandler> _lazy = new(() => new());

	private GlslFileHandler()
	{
	}

	public static GlslFileHandler Instance => _lazy.Value;

	public int HeaderSize => 12;

	public byte[] ToBinary(byte[] fileBuffer)
	{
		// TODO: Validate if file contains both "// Vert" and "// Frag" (or without space -- case insensitive).
		throw new NotImplementedException();
	}

	public byte[] ToFile(byte[] binaryBuffer)
	{
		int nameLength = BitConverter.ToInt32(binaryBuffer, 0);
		int vertexSize = BitConverter.ToInt32(binaryBuffer, 4);
		int fragmentSize = BitConverter.ToInt32(binaryBuffer, 8);

		byte[] vertexBuffer = new byte[vertexSize];
		Buffer.BlockCopy(binaryBuffer, nameLength + HeaderSize, vertexBuffer, 0, vertexSize);

		byte[] fragmentBuffer = new byte[fragmentSize];
		Buffer.BlockCopy(binaryBuffer, nameLength + HeaderSize + vertexSize, fragmentBuffer, 0, fragmentSize);

		// TODO: Add comments at beginning of buffers in case of old shader binaries that don't start with // Vert or // Frag.
		return vertexBuffer.Concat(fragmentBuffer).ToArray();
	}
}
