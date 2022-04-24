namespace DevilDaggersInfo.Core.Asset;

public static class AssetContainer
{
	public static List<AssetData> GetAll(AssetType assetType) => assetType switch
	{
		AssetType.Audio => AudioAudio.All.Cast<AssetData>().ToList(),
		AssetType.ObjectBinding => DdObjectBindings.All.Cast<AssetData>().ToList(),
		AssetType.Mesh => DdMeshes.All.Cast<AssetData>().ToList(),
		AssetType.Shader => DdShaders.All.Cast<AssetData>().ToList(),
		AssetType.Texture => DdTextures.All.Cast<AssetData>().ToList(),
		_ => throw new NotSupportedException($"Asset type '{assetType}' is not supported."),
	};

	public static bool GetIsProhibited(AssetType assetType, string assetName) => assetType switch
	{
		AssetType.Audio => AudioAudio.All.Find(a => a.AssetName == assetName)?.IsProhibited ?? false,
		AssetType.ObjectBinding => DdObjectBindings.All.Find(a => a.AssetName == assetName)?.IsProhibited ?? false,
		AssetType.Mesh => DdMeshes.All.Find(a => a.AssetName == assetName)?.IsProhibited ?? false,
		AssetType.Shader => DdShaders.All.Find(a => a.AssetName == assetName)?.IsProhibited ?? false,
		AssetType.Texture => DdTextures.All.Find(a => a.AssetName == assetName)?.IsProhibited ?? false,
		_ => throw new NotSupportedException($"Asset type '{assetType}' is not supported."),
	};
}
