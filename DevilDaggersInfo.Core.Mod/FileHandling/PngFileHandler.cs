using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;
using System.Runtime.InteropServices;
using System.Xml.Linq;

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
		throw new NotImplementedException();
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
