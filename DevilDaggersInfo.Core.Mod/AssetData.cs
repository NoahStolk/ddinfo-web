namespace DevilDaggersInfo.Core.Mod;

public class AssetData
{
	public AssetData(byte[] buffer)
	{
		Buffer = buffer;
	}

	public byte[] Buffer { get; }
}
