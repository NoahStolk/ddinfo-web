namespace DevilDaggersInfo.Core.Mod.FileHandling;

public static class AssetConverter
{
	public static byte[] Compile(string inputPath, ModBinaryChunk chunk) => chunk.AssetType switch
	{
		AssetType.Audio or AssetType.ModelBinding => File.ReadAllBytes(inputPath),
		AssetType.Model => ObjFileHandler.Instance.Compile(File.ReadAllBytes(inputPath)),
		AssetType.Shader => GlslFileHandler.Instance.Compile(File.ReadAllBytes(inputPath)),
		AssetType.Texture => PngFileHandler.Instance.Compile(File.ReadAllBytes(inputPath)),
		_ => throw new NotSupportedException($"Compile asset of type '{chunk.AssetType}' is not supported."),
	};

	public static byte[] Extract(ModBinaryChunk chunk, byte[] buffer) => chunk.AssetType switch
	{
		AssetType.Audio or AssetType.ModelBinding => buffer,
		AssetType.Model => ObjFileHandler.Instance.Extract(buffer),
		AssetType.Shader => GlslFileHandler.Instance.Extract(buffer),
		AssetType.Texture => PngFileHandler.Instance.Extract(buffer),
		_ => throw new NotSupportedException($"Extracting asset of type '{chunk.AssetType}' is not supported."),
	};
}
