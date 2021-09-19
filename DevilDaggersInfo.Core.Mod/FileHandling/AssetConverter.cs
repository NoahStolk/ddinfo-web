namespace DevilDaggersInfo.Core.Mod.FileHandling;

public static class AssetConverter
{
	public static byte[] Compile(ModBinaryChunk chunk, byte[] buffer) => chunk.AssetType switch
	{
		AssetType.Audio or AssetType.ModelBinding => buffer,
		AssetType.Model => ObjFileHandler.Instance.Compile(buffer),
		AssetType.Shader => GlslFileHandler.Instance.Compile(buffer),
		AssetType.Texture => PngFileHandler.Instance.Compile(buffer),
		_ => throw new NotSupportedException($"Compiling asset of type '{chunk.AssetType}' is not supported."),
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
