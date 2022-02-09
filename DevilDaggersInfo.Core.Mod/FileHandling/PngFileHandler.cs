using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace DevilDaggersInfo.Core.Mod.FileHandling;

internal sealed class PngFileHandler : IFileHandler
{
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

		bw.Write((ushort)16401);
		bw.Write(image.Width);
		bw.Write(image.Height);
		bw.Write(mipmapCount);

		image.Mutate(ipc => ipc.Flip(FlipMode.Vertical));

		for (int i = 0; i < mipmapCount; i++)
		{
			if (i != 0)
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
		int width = BitConverter.ToInt32(buffer, 2);
		int height = BitConverter.ToInt32(buffer, 6);

		using Image<Rgba32> image = Image.LoadPixelData<Rgba32>(buffer[HeaderSize..], width, height);
		image.Mutate(ipc => ipc.Flip(FlipMode.Vertical));

		using MemoryStream ms = new();
		image.SaveAsPng(ms);
		return ms.ToArray();
	}
}
