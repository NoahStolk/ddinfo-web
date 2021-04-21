using DevilDaggersCore.Mods;

namespace DevilDaggersWebsite.Caches.ModArchive
{
	public class ModChunkCacheData
	{
		public ModChunkCacheData(string name, uint size, AssetType assetType, bool isProhibited)
		{
			Name = name;
			Size = size;
			ModAssetType = assetType;
			IsProhibited = isProhibited;
		}

		public string Name { get; }
		public uint Size { get; }
		public AssetType ModAssetType { get; }
		public bool IsProhibited { get; }
	}
}
