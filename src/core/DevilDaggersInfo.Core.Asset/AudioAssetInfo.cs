namespace DevilDaggersInfo.Core.Asset;

public class AudioAssetInfo : AssetInfo
{
	public AudioAssetInfo(string assetName, bool isProhibited, float defaultLoudness, bool presentInDefaultLoudness)
		: base(assetName, isProhibited)
	{
		DefaultLoudness = defaultLoudness;
		PresentInDefaultLoudness = presentInDefaultLoudness;
	}

	public float DefaultLoudness { get; }
	public bool PresentInDefaultLoudness { get; }

	public override AssetType AssetType => AssetType.Audio;
}
