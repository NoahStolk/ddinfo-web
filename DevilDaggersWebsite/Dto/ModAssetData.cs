using DevilDaggersCore.Mods;

namespace DevilDaggersWebsite.Dto
{
	public class ModAssetData
	{
		public ModAssetData(string name, AssetType modAssetType, bool isProhibited)
		{
			Name = name;
			ModAssetType = modAssetType;
			IsProhibited = isProhibited;
		}

		public string Name { get; }
		public AssetType ModAssetType { get; }
		public bool IsProhibited { get; }
	}
}
