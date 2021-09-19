namespace DevilDaggersInfo.Core.Mod.FileHandling;

public sealed class GlslFileHandler : IFileHandler
{
	private static readonly Lazy<GlslFileHandler> _lazy = new(() => new());

	private GlslFileHandler()
	{
	}

	public static GlslFileHandler Instance => _lazy.Value;

	public int HeaderSize => 12;

	public byte[] Compile(byte[] buffer)
	{
		// TODO: Validate if file contains both "// Vert" and "// Frag" (or without space -- case insensitive).
		throw new NotImplementedException();
	}

	public byte[] Extract(byte[] buffer)
	{
		int nameLength = BitConverter.ToInt32(buffer, 0);
		int vertexSize = BitConverter.ToInt32(buffer, 4);
		int fragmentSize = BitConverter.ToInt32(buffer, 8);

		byte[] vertexBuffer = new byte[vertexSize];
		Buffer.BlockCopy(buffer, nameLength + HeaderSize, vertexBuffer, 0, vertexSize);

		byte[] fragmentBuffer = new byte[fragmentSize];
		Buffer.BlockCopy(buffer, nameLength + HeaderSize + vertexSize, fragmentBuffer, 0, fragmentSize);

		// TODO: Add comments at beginning of buffers in case of old shader binaries that don't start with // Vert or // Frag.
		return vertexBuffer.Concat(fragmentBuffer).ToArray();
	}
}
