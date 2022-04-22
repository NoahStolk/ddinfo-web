namespace DevilDaggersInfo.Core.Mod.FileHandling;

public static class AssetConverter
{
	public static AssetData Compile(AssetType assetType, byte[] buffer) => assetType switch
	{
		AssetType.Audio or AssetType.ObjectBinding => new(buffer),
		AssetType.Mesh => new(ObjFileHandler.Instance.Compile(buffer)),
		AssetType.Shader => new(GlslFileHandler.Instance.Compile(buffer)),
		AssetType.Texture => new(PngFileHandler.Instance.Compile(buffer)),
		_ => throw new NotSupportedException($"Compiling asset of type '{assetType}' is not supported."),
	};

	public static byte[] Extract(AssetType assetType, AssetData assetData) => assetType switch
	{
		AssetType.Audio or AssetType.ObjectBinding => assetData.Buffer,
		AssetType.Mesh => ObjFileHandler.Instance.Extract(assetData.Buffer),
		AssetType.Shader => GlslFileHandler.Instance.Extract(assetData.Buffer),
		AssetType.Texture => PngFileHandler.Instance.Extract(assetData.Buffer),
		_ => throw new NotSupportedException($"Extracting asset of type '{assetType}' is not supported."),
	};
}
