using System.Diagnostics;

namespace DevilDaggersInfo.Core.Asset;

public static class AssetContainer
{
	public static List<AssetInfo> GetAll(AssetType assetType) => assetType switch
	{
		AssetType.Audio => AudioAudio2.All.Cast<AssetInfo>().ToList(),
		AssetType.ObjectBinding => DdObjectBindings2.All.Cast<AssetInfo>().ToList(),
		AssetType.Mesh => DdMeshes2.All.Cast<AssetInfo>().ToList(),
		AssetType.Shader => DdShaders2.All.Cast<AssetInfo>().ToList(),
		AssetType.Texture => DdTextures2.All.Cast<AssetInfo>().ToList(),
		_ => throw new UnreachableException(),
	};

	public static bool GetIsProhibited(AssetType assetType, string assetName) => assetType switch
	{
		AssetType.Audio => AudioAudio2.All.FirstOrDefault(a => a.AssetName == assetName)?.IsProhibited ?? false,
		AssetType.ObjectBinding => DdObjectBindings2.All.FirstOrDefault(a => a.AssetName == assetName)?.IsProhibited ?? false,
		AssetType.Mesh => DdMeshes2.All.FirstOrDefault(a => a.AssetName == assetName)?.IsProhibited ?? false,
		AssetType.Shader => DdShaders2.All.FirstOrDefault(a => a.AssetName == assetName)?.IsProhibited ?? false,
		AssetType.Texture => DdTextures2.All.FirstOrDefault(a => a.AssetName == assetName)?.IsProhibited ?? false,
		_ => throw new UnreachableException(),
	};
}
