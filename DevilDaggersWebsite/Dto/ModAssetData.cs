using DevilDaggersCore.Mods;

namespace DevilDaggersWebsite.Dto
{
	public class ModAssetData
	{
		public ModAssetData(string name, uint size, AssetType modAssetType, bool isProhibited)
		{
			Name = name;
			Size = size;
			ModAssetType = modAssetType;
			IsProhibited = isProhibited;
		}

		public string Name { get; }
		public uint Size { get; }
		public AssetType ModAssetType { get; }
		public bool IsProhibited { get; }
	}
}
