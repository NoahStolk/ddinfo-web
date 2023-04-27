using System.Diagnostics;

namespace DevilDaggersInfo.Core.Asset.Extensions;

public static class AssetTypeExtensions
{
	public static string GetFileExtension(this AssetType assetType) => assetType switch
	{
		AssetType.Audio => ".wav",
		AssetType.Mesh => ".obj",
		AssetType.ObjectBinding => ".txt",
		AssetType.Shader => throw new NotSupportedException($"Asset type '{AssetType.Shader}' has multiple file extensions."),
		AssetType.Texture => ".png",
		_ => throw new UnreachableException(),
	};

	public static string ToDisplayString(this AssetType assetType) => assetType switch
	{
		AssetType.Audio => "Audio",
		AssetType.Mesh => "Mesh",
		AssetType.ObjectBinding => "Object Binding",
		AssetType.Shader => "Shader",
		AssetType.Texture => "Texture",
		_ => throw new UnreachableException(),
	};

	public static AssetType? GetAssetType(this ushort type) => type switch
	{
		0x01 => AssetType.Mesh,
		0x02 => AssetType.Texture,
		0x10 => AssetType.Shader,
		0x20 => AssetType.Audio,
		0x80 => AssetType.ObjectBinding,
		_ => null,
	};
}
