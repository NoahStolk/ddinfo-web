namespace DevilDaggersInfo.Core.Asset;

public class ShaderAssetData : AssetData
{
	public ShaderAssetData(string assetName, bool isProhibited)
		: base(assetName, isProhibited)
	{
	}

	public override AssetType AssetType => AssetType.Shader;
}
