namespace DevilDaggersInfo.Core.Asset;

public class ObjectBindingAssetData : AssetData
{
	public ObjectBindingAssetData(string assetName, bool isProhibited)
		: base(assetName, isProhibited)
	{
	}

	public override AssetType AssetType => AssetType.ObjectBinding;
}
