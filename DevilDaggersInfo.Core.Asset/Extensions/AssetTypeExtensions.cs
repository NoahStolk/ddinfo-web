namespace DevilDaggersInfo.Core.Asset.Extensions;

public static class AssetTypeExtensions
{
	public static string GetFileExtension(this AssetType assetType) => assetType switch
	{
		AssetType.Audio => ".wav",
		AssetType.Model => ".obj",
		AssetType.ModelBinding => ".txt",
		AssetType.Shader => ".glsl",
		AssetType.Texture => ".png",
		_ => throw new NotSupportedException($"Asset type '{assetType}' is not supported."),
	};
}
