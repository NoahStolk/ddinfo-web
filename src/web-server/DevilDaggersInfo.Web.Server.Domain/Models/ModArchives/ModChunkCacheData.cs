using DevilDaggersInfo.Types.Core;

namespace DevilDaggersInfo.Web.Server.Domain.Models.ModArchives;

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
