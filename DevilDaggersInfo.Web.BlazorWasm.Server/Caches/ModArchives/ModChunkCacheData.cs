using DevilDaggersCore.Mods;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Caches.ModArchives;

public class ModChunkCacheData
{
	public ModChunkCacheData(string name, uint size, AssetType assetType, bool isProhibited)
	{
		Name = name;
		Size = size;
		AssetType = assetType;
		IsProhibited = isProhibited;
	}

	public string Name { get; }
	public uint Size { get; }
	public AssetType AssetType { get; }
	public bool IsProhibited { get; }
}
