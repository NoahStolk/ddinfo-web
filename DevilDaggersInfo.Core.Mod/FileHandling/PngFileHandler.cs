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

	public byte[] ToBinary(byte[] fileBuffer)
	{
		using Image<Rgba32> image = Image.Load(fileBuffer);
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

	public byte[] ToFile(byte[] binaryBuffer)
	{
		int width = BitConverter.ToInt32(binaryBuffer, 2);
		int height = BitConverter.ToInt32(binaryBuffer, 6);

		using Image<Rgba32> image = Image.LoadPixelData<Rgba32>(binaryBuffer[HeaderSize..], width, height);
		using MemoryStream ms = new();
		image.SaveAsPng(ms);
		return ms.ToArray();
	}
}
