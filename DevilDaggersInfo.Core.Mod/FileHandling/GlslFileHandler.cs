namespace DevilDaggersInfo.Core.Mod.FileHandling;

internal sealed class GlslFileHandler : IFileHandler
{
	private static readonly Lazy<GlslFileHandler> _lazy = new(() => new());

	private GlslFileHandler()
	{
	}

	public static GlslFileHandler Instance => _lazy.Value;

	public int HeaderSize => 12;

	public byte[] Compile(byte[] buffer)
	{
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

		return vertexBuffer.Concat(fragmentBuffer).ToArray();
	}
}
