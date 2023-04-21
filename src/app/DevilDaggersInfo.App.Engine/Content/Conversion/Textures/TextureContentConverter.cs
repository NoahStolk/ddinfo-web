using Warp.NET.Parsers.Texture;

namespace Warp.NET.Content.Conversion.Textures;

internal sealed class TextureContentConverter : IContentConverter<TextureBinary>
{
	public static TextureBinary Construct(string inputPath)
	{
		TextureData textureData = TgaParser.Parse(File.ReadAllBytes(inputPath));
		return new(textureData.Width, textureData.Height, textureData.ColorData);
	}
}
