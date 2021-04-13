namespace DevilDaggersWebsite.Dto
{
	public class ModAssetData
	{
		public ModAssetData(string name, ModAssetType modAssetType)
		{
			Name = name;
			ModAssetType = modAssetType;
		}

		public string Name { get; }
		public ModAssetType ModAssetType { get; }
	}
}
