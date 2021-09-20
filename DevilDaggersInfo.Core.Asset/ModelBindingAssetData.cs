namespace DevilDaggersInfo.Core.Asset;

public class ModelBindingAssetData : AssetData
{
	public ModelBindingAssetData(string assetName, bool isProhibited)
		: base(assetName, isProhibited)
	{
	}

	public override AssetType AssetType => AssetType.ModelBinding;
}
