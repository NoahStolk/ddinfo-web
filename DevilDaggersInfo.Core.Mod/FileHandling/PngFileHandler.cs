using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace DevilDaggersInfo.Core.Mod.FileHandling;

public sealed class PngFileHandler : IFileHandler
{
	private static readonly Lazy<PngFileHandler> _lazy = new(() => new());

	private PngFileHandler()
	{
	}

	public static PngFileHandler Instance => _lazy.Value;

	public int HeaderSize => 11;

	public byte[] Compile(byte[] buffer)
	{
		using Image<Rgba32> image = Image.Load(buffer);
		using MemoryStream ms = new();
		using BinaryWriter bw = new(ms);
		bw.Write(image.Width);
		bw.Write(image.Height);
		for (int i = 0; i < image.Width; i++)
		{
			Span<Rgba32> span = image.GetPixelRowSpan(i);
			for (int j = 0; j < span.Length; j++)
			{
				Rgba32 color = span[j];
				bw.Write(color.A);
				bw.Write(color.G);
				bw.Write(color.B);
				bw.Write(color.R);
			}
		}

		return ms.ToArray();
	}

	public byte[] Extract(byte[] buffer)
	{
		int width = BitConverter.ToInt32(buffer, 2);
		int height = BitConverter.ToInt32(buffer, 6);

		using Image<Rgba32> image = Image.LoadPixelData<Rgba32>(buffer[HeaderSize..], width, height);
		using MemoryStream ms = new();
		image.SaveAsPng(ms);
		return ms.ToArray();
	}
}
