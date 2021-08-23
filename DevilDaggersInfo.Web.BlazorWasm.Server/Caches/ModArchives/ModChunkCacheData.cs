using DevilDaggersInfo.Core.Asset.Enums;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Caches.ModArchives;

public class ModChunkCacheData
{
	public ModChunkCacheData(string name, int size, AssetType assetType, bool isProhibited)
	{
		Name = name;
		Size = size;
		AssetType = assetType;
		IsProhibited = isProhibited;
	}

	public string Name { get; }
	public int Size { get; }
	public AssetType AssetType { get; }
	public bool IsProhibited { get; }
}
