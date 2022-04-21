namespace DevilDaggersInfo.Core.Asset;

public static class AssetContainer
{
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
