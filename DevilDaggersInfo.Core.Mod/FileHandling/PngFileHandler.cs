using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace DevilDaggersInfo.Core.Mod.FileHandling;

internal sealed class PngFileHandler : IFileHandler
{
	private const ushort _header = 16401;

	private static readonly Lazy<PngFileHandler> _lazy = new(() => new());

	private PngFileHandler()
	{
	}

	public static PngFileHandler Instance => _lazy.Value;

	public int HeaderSize => 11;

	public byte[] Compile(byte[] buffer)
	{
		using Image<Rgba32> image = Image.Load<Rgba32>(buffer);
		using MemoryStream ms = new();
		using BinaryWriter bw = new(ms);

		byte mipmapCount = MipmapUtils.GetMipmapCount(image.Width, image.Height);

		bw.Write(_header);
		bw.Write(image.Width);
		bw.Write(image.Height);
		bw.Write(mipmapCount);

		image.Mutate(ipc => ipc.Flip(FlipMode.Vertical));

		for (int i = 0; i < mipmapCount; i++)
		{
			if (i > 0)
				image.Mutate(ipc => ipc.Resize(image.Width / 2, image.Height / 2));

			image.ProcessPixelRows(pixelAccessor =>
			{
				for (int y = 0; y < pixelAccessor.Height; y++)
				{
					Span<Rgba32> row = pixelAccessor.GetRowSpan(y);
					for (int x = 0; x < row.Length; x++)
					{
						Rgba32 color = row[x];
						bw.Write(color.R);
						bw.Write(color.G);
						bw.Write(color.B);
						bw.Write(color.A);
					}
				}
			});
		}

		return ms.ToArray();
	}

	public byte[] Extract(byte[] buffer)
	{
		using MemoryStream ms = new(buffer);
		using BinaryReader br = new(ms);
		ushort header = br.ReadUInt16();
		if (header != _header)
			throw new InvalidModBinaryException($"Invalid texture header. Should be {header} but got {_header}.");

		int width = br.ReadInt32();
		int height = br.ReadInt32();
		if (width < 0 || height < 0)
			throw new InvalidModBinaryException($"Texture dimensions cannot be negative ({width}x{height}).");

		_ = br.ReadByte(); // Mipmap count

		int minimumSize = width * height * 4 + HeaderSize;
		if (buffer.Length < minimumSize)
			throw new InvalidModBinaryException($"Invalid texture. Not enough pixel data for complete texture ({buffer.Length:N0} / {minimumSize:N0}).");

		using Image<Rgba32> image = Image.LoadPixelData<Rgba32>(br.ReadBytes(width * height * 4), width, height);
		image.Mutate(ipc => ipc.Flip(FlipMode.Vertical));

		using MemoryStream outputStream = new();
		image.SaveAsPng(outputStream);
		return outputStream.ToArray();
	}
}
