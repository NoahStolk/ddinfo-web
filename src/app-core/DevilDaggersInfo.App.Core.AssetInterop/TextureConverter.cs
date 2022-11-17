using Warp.NET.Content;

namespace DevilDaggersInfo.App.Core.AssetInterop;

public static class TextureConverter
{
	// TODO: Move duplicate code to Core.Mod.
	public static Texture ToWarpTexture(byte[] ddTextureBuffer)
	{
		const ushort expectedHeader = 16401;
		const int headerSize = 11;
		using MemoryStream ms = new(ddTextureBuffer);
		using BinaryReader br = new(ms);
		ushort header = br.ReadUInt16();
		if (header != expectedHeader)
			throw new($"Invalid texture header. Should be {expectedHeader} but got {header}.");

		int width = br.ReadInt32();
		int height = br.ReadInt32();
		if (width < 0 || height < 0)
			throw new($"Texture dimensions cannot be negative ({width}x{height}).");

		_ = br.ReadByte(); // Mipmap count

		int minimumSize = width * height * 4 + headerSize;
		if (ddTextureBuffer.Length < minimumSize)
			throw new($"Invalid texture. Not enough pixel data for complete texture ({ddTextureBuffer.Length:N0} / {minimumSize:N0}).");

		return new(width, height, br.ReadBytes(width * height * 4));
	}
}
