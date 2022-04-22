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
		using MemoryStream ms = new(buffer);
		using BinaryReader br = new(ms);

		int nameLength = br.ReadInt32();
		int vertexSize = br.ReadInt32();
		int fragmentSize = br.ReadInt32();
		_ = br.ReadBytes(nameLength); // Name
		byte[] vertexBuffer = br.ReadBytes(vertexSize);
		byte[] fragmentBuffer = br.ReadBytes(fragmentSize);

		// TODO: Return as separate buffers.
		return vertexBuffer.Concat(fragmentBuffer).ToArray();
	}
}
