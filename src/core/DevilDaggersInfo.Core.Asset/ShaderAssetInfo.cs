using DevilDaggersInfo.Types.Core.Assets;

namespace DevilDaggersInfo.Core.Asset;

public class ShaderAssetInfo : AssetInfo
{
	public ShaderAssetInfo(string assetName, bool isProhibited)
		: base(assetName, isProhibited)
	{
	}

	public override AssetType AssetType => AssetType.Shader;
}
