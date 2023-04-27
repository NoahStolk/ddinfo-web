namespace DevilDaggersInfo.Core.Asset;

public abstract class AssetInfo
{
	protected AssetInfo(string assetName, bool isProhibited)
	{
		AssetName = assetName;
		IsProhibited = isProhibited;
	}

	public string AssetName { get; }
	public bool IsProhibited { get; }

	public abstract AssetType AssetType { get; }
}
