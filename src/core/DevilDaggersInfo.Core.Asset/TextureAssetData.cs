namespace DevilDaggersInfo.Core.Asset;

public class TextureAssetData : AssetData
{
	public TextureAssetData(string assetName, bool isProhibited, int defaultWidth, int defaultHeight, bool isTextureForMesh, string? objectBinding)
		: base(assetName, isProhibited)
	{
		DefaultWidth = defaultWidth;
		DefaultHeight = defaultHeight;
		IsTextureForMesh = isTextureForMesh;
		ObjectBinding = objectBinding;
	}

	public int DefaultWidth { get; }
	public int DefaultHeight { get; }
	public bool IsTextureForMesh { get; }
	public string? ObjectBinding { get; }

	public override AssetType AssetType => AssetType.Texture;
}
