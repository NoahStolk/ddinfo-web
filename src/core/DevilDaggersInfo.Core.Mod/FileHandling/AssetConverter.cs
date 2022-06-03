using DevilDaggersInfo.Common.Exceptions;

namespace DevilDaggersInfo.Core.Mod.FileHandling;

public static class AssetConverter
{
	public static AssetData Compile(AssetType assetType, byte[] buffer) => assetType switch
	{
		AssetType.Audio or AssetType.ObjectBinding or AssetType.Shader => new(buffer),
		AssetType.Mesh => new(ObjFileHandler.Instance.Compile(buffer)),
		AssetType.Texture => new(PngFileHandler.Instance.Compile(buffer)),
		_ => throw new InvalidEnumConversionException(assetType),
	};

	public static byte[] Extract(AssetType assetType, AssetData assetData) => assetType switch
	{
		AssetType.Audio or AssetType.ObjectBinding or AssetType.Shader => assetData.Buffer,
		AssetType.Mesh => ObjFileHandler.Instance.Extract(assetData.Buffer),
		AssetType.Texture => PngFileHandler.Instance.Extract(assetData.Buffer),
		_ => throw new InvalidEnumConversionException(assetType),
	};
}
